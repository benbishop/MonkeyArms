using System;

namespace MonkeyArms
{
	public class InjectAttribute : System.Attribute { }

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

