using System;

namespace MonkeyArms
{
	public class Mediator
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

