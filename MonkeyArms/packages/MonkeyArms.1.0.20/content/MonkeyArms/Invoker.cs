using System;
using System.Collections.Generic;

namespace MonkeyArms
{
    public class Invoker : IInvoker
    {
        public event EventHandler Invoked = delegate { };

        protected List<Type> CommandTypes = new List<Type>();

        private readonly List<Command> _detainedCommands = new List<Command>();

        public void AddCommand<TCommand>()
            where TCommand : class
        {
            if (!CommandTypes.Contains(typeof(TCommand)))
            {
                CommandTypes.Add(typeof(TCommand));
            }
        }

        public void RemoveCommand(Type command)
        {
            if (CommandTypes.Contains(command))
            {
                CommandTypes.Remove(command);
            }
        }

        public virtual void Invoke(InvokerArgs args = null)
        {
            foreach (Type command in CommandTypes)
            {
                var c = (Command)Activator.CreateInstance(command);
                DIUtil.InjectProps(c);

                if (c.Detained)
                {
                    c.Released += HandleCommandRelease;
                    _detainedCommands.Add(c);
                }

                c.Execute(args);
            }

            if (Invoked != null)
            {
                Invoked(this, args);
            }
        }

        protected void HandleCommandRelease(object sender, EventArgs e)
        {
            var command = sender as Command;
            if (command != null)
            {
                command.Released -= HandleCommandRelease;
                if (_detainedCommands.Contains(command))
                {
                    _detainedCommands.Remove(command);
                }
            }
        }
    }

    public interface IInvoker
    {
        event EventHandler Invoked;

        void Invoke(InvokerArgs args);
    }

    public class InvokerArgs : EventArgs
    {
        public new static InvokerArgs Empty = new InvokerArgs();
    }
}