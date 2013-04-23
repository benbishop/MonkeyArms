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
			where TClass : IMediatorTarget
		{
			ClassMediatorMappings[typeof(TClass)] = typeof(TMediator);
		}

		public static Mediator RequestMediator(IMediatorTarget target)
		{
			var targetType = target.GetType ();

			//if we don't have this class type specifically mapped
			if (!ClassMediatorMappings.ContainsKey (targetType)) {
				//checking to see if this target has a super class that has a mediator assigned to it
				foreach(Type classType in ClassMediatorMappings.Keys){
					if(targetType.IsSubclassOf(classType)){
						return CreateMediator (target, classType);
					}
				}
				//if still nothing we blow an exception
				throw(new ArgumentException ("Target type does not have mediator type mapped for it. Invoke MapMediatorToClass first."));
			}


			return CreateMediator (target, targetType);

		}

		static Mediator CreateMediator (IMediatorTarget target, Type targetType)
		{
			Mediator m = (Mediator)Activator.CreateInstance (ClassMediatorMappings [targetType], target);
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

