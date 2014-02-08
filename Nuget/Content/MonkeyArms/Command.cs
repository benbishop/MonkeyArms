using System;

namespace MonkeyArms
{
	public abstract class Command:IInjectingTarget
	{
 
		public event EventHandler Released = delegate{};

		private bool detained = false;
		public bool Detained{
			get{
				return detained;
			}
		}

		public Command ()
		{

		}

		public abstract void Execute (InvokerArgs args);


		protected void Detain()
		{
			detained = true;
		}

		protected void Release()
		{
			detained = false;
			Released(this, new EventArgs());

		}

	}
}

