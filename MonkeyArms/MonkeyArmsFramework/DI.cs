using TinyIoC;
using System.Collections.Generic;
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

		/*
		 * Mediator mappings
		 */

		private static Dictionary<Type, Type> ClassMediatorMappings = new Dictionary<Type,Type>(); 
		private static Dictionary<IMediatorTarget, Mediator> MediatorAssignments = new Dictionary<IMediatorTarget, Mediator>();

		public static void MapMediatorToClass<TMediator, TClass> ()
			where TMediator:Mediator
			where TClass : class
		{
			ClassMediatorMappings[typeof(TClass)] = typeof(TMediator);
		}

		public static Mediator RequestMediator(IMediatorTarget target)
		{
			if (!ClassMediatorMappings.ContainsKey (target.GetType ())) {
				throw(new ArgumentException ("Target type does not have mediator type mapped for it. Invoke MapMediatorToClass first."));
			}

			Mediator m = (Mediator)Activator.CreateInstance(ClassMediatorMappings[target.GetType()], target);
			MediatorAssignments [target] = m;
			m.Register ();
			return m;

		}

		public static void DestroyMediator(IMediatorTarget target)
		{
			MediatorAssignments [target].Unregister ();
			MediatorAssignments.Remove(target);
		}


		/*
		 * Standard Get method
		 */ 
		public static TGet Get<TGet> ()
			where TGet : class
		{
			return Injector.Resolve<TGet> ();
		}
	}
}

