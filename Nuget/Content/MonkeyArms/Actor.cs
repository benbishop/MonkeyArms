using System;

namespace MonkeyArms
{


	public class Actor:IInjectingTarget
	{
		public Actor ()
		{
			InjectPropsFromDI ();

		}

		void InjectPropsFromDI ()
		{
			DIUtil.InjectProps(this);
		}
	}
}

