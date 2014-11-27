namespace MonkeyArms
{
    public class Actor : IInjectingTarget
    {
        public Actor()
        {
            InjectPropsFromDI();
        }

        private void InjectPropsFromDI()
        {
            DIUtil.InjectProps(this);
        }
    }
}