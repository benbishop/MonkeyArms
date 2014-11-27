using System;

namespace MonkeyArms
{
    public abstract class Command : IInjectingTarget
    {
        public event EventHandler Released = delegate { };

        public bool Detained { get; private set; }

        protected Command()
        {
            Detained = false;
        }

        public abstract void Execute(InvokerArgs args);

        protected void Detain()
        {
            Detained = true;
        }

        protected void Release()
        {
            Detained = false;
            Released(this, new EventArgs());
        }
    }
}