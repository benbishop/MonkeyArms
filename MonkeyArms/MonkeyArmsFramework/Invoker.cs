using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class Invoker
	{
		public event EventHandler Invoked = delegate {};

		protected List<Command> Commands = new List<Command>();

		public Invoker ()
		{
		}
		//TODO: Change this to take a Type so Commands are disposed after executing
		public void AddCommand(Command command)
		{
			if (Commands.IndexOf (command) == -1) {
				Commands.Add (command);
			}
		}

		public void RemoveCommand(Command command)
		{
			if (Commands.IndexOf (command) != -1) {
				Commands.Remove (command);
			}
		}

		public void Invoke(InvokerArgs args = null)
		{
			foreach (Command command in Commands) {
				command.Execute (args);
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

