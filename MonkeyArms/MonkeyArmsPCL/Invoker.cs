using System;
using System.Collections.Generic;

namespace MonkeyArms
{

	public class Invoker
	{
		public virtual event EventHandler Invoked = delegate {};

		protected List<Type> CommandTypes = new List<Type>();

		private List<Command> detainedCommands = new List<Command>();

		public Invoker ()
		{
		}



		public void AddCommand<TCommand>()
			where TCommand:class
		{
			if (!CommandTypes.Contains (typeof(TCommand))) {
				CommandTypes.Add (typeof(TCommand));
			}
		}

		public void RemoveCommand(Type command)
		{
			if (CommandTypes.Contains (command)) {
				CommandTypes.Remove (command);
			}
		}

		public virtual void Invoke(InvokerArgs args = null)
		{
			foreach (Type command in CommandTypes) {
				Command c = (Command)Activator.CreateInstance (command);
				DIUtil.InjectProps (c);

				if(c.Detained){
					c.Released += HandleCommandRelease;
					detainedCommands.Add(c);
				}

				c.Execute (args);
			}

			Invoked(this, args);
		}

		protected void HandleCommandRelease (object sender, EventArgs e)
		{
			var command = sender as Command;
			command.Released -= HandleCommandRelease;
			if(detainedCommands.Contains(command)){
				detainedCommands.Remove(command);
			}
		}
	}

	public class InvokerArgs:EventArgs
	{
		public InvokerArgs()
		{

		}
	}



}

