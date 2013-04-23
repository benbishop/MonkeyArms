using TinyIoC;
using System;

namespace MonkeyArms
{
	public static class DI
	{
		private static TinyIoCContainer Injector = new TinyIoCContainer ();

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

		public static void MapCommandToInvoker<TCommand, TInvoker> ()
			where TCommand : Command, new()
			where TInvoker : Invoker
		{
		

			MapSingleton<TInvoker> ();
			var invoker = DI.Get<TInvoker> ();
			var command = new TCommand ();
			invoker.AddCommand (command);


		}

		public static TGet Get<TGet> ()
			where TGet : class
		{
			return Injector.Resolve<TGet> ();
		}
	}
}

