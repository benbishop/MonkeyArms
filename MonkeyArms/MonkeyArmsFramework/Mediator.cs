using System;

namespace MonkeyArms
{
	public class Mediator:Actor
	{


		public Mediator (IMediatorTarget target)
		{


		}


		public virtual void Register()
		{

		}

		public virtual void Unregister()
		{

		}


	}

	public interface IMediatorTarget{

	}
}

