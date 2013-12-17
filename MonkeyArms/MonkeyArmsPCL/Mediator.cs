using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public abstract class Mediator:IInjectingTarget
	{

		protected InvokerMap InvokerMap = new InvokerMap();

		public Mediator (IMediatorTarget target)
		{


		}


		public abstract void Register();

		public virtual void Unregister ()
		{
			InvokerMap.RemoveAll ();

		}

		public int GetInvokerMapHandlerCount(Invoker invoker)
		{
			return InvokerMap.GetInvokerHandlerCount (invoker);
		}





	}

	public interface IMediatorTarget{

	}
}

