using TinyIoC;
using System;

namespace MonkeyArms
{
	public static class DI
	{
		public static TinyIoCContainer Injector = new TinyIoCContainer ();


		public static void MapSingleton<TSingleton> ()
			where TSingleton :class
		{
			Injector.Register<TSingleton> ().AsSingleton ();


		}

		
		public static void MapClassToInterface<TInterface, TImplementation> ()
			where TInterface : class
			where TImplementation : class, TInterface
			{
				Injector.Register<TInterface, TImplementation> ();

				
			}
		
		public static TGet Get<TGet> ()
			where TGet : class
		{
			return Injector.Resolve<TGet> ();
		}
	}
}

