using System;

namespace MonkeyArms
{
	public class Command:IInjectingTarget
	{
		//TODO: Figure out if we need Detain/Release mechanism for Async operations

		public Command ()
		{

		}

		public virtual void Execute(InvokerArgs args)
		{

		}
	}
}

