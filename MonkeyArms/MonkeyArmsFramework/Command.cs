using System;

namespace MonkeyArms
{
	public class Command:Actor
	{
		//TODO: Figure out if we need Detain/Release mechanism for Async operations

		public Command ():base()
		{

		}

		public virtual void Execute(InvokerArgs args)
		{

		}
	}
}

