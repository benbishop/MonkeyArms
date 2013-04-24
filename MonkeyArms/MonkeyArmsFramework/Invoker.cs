using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class Invoker
	{
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
		}
	}

	public class InvokerArgs
	{
		public InvokerArgs()
		{

		}
	}


}

