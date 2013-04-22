using TinyIoC;

namespace MonkeyArms
{
	public class AppContext
	{
		private TinyIoCContainer Injector;

		public AppContext(){
			Injector = new TinyIoCContainer ();
		}

		public void MapSingleton<TSingleton> ()
			where TSingleton :class
		{
			Injector.Register<TSingleton> ().AsSingleton ();
		}
		
		public void MapClassToInterface<TInterface, TImplementation> ()
			where TInterface : class
			where TImplementation : class, TInterface
			{
				Injector.Register<TInterface, TImplementation> ();
				
			}
		
		public TGet Get<TGet> ()
			where TGet : class
		{
			return Injector.Resolve<TGet> ();
		}
	}
}

