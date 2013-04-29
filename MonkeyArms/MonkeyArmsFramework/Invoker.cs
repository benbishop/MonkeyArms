using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class Invoker
	{
		public event EventHandler Invoked = delegate {};

		protected List<Type> CommandTypes = new List<Type>();

		public Invoker ()
		{
		}

		public void AddCommand<TCommand>()
			where TCommand:class
		{
			if (CommandTypes.IndexOf (typeof(TCommand)) == -1) {
				CommandTypes.Add (typeof(TCommand));
			}
		}

		public void RemoveCommand(Type command)
		{
			if (CommandTypes.IndexOf (command) != -1) {
				CommandTypes.Remove (command);
			}
		}

		public void Invoke(InvokerArgs args = null)
		{
			foreach (Type command in CommandTypes) {
				Command c = (Command)Activator.CreateInstance (command);
				DIUtil.InjectProps (c);
				c.Execute (args);
			}

			Invoked(this, new InvokedEventArgs(args));
		}
	}

	public class InvokerArgs
	{
		public InvokerArgs()
		{

		}
	}

	public class InvokedEventArgs:EventArgs{

		private InvokerArgs invokerArgs;

		public InvokerArgs InvokerArgs {
			get {
				return invokerArgs;
			}
		}

		public InvokedEventArgs(InvokerArgs invokerArgs){
			this.invokerArgs = invokerArgs;
		}
	}


}

