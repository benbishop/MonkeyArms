using System;

namespace MonkeyArms
{
	public abstract class Mediator:IInjectingTarget
	{


		public Mediator (IMediatorTarget target)
		{


		}


		public abstract void Register();

		public abstract void Unregister ();


	}

	public interface IMediatorTarget{

	}
}

