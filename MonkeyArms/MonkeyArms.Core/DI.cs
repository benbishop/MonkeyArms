using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class MustBeSingletonAttribute : Attribute
	{

	}

	public static class DI
	{

		private static readonly Dictionary<Type, object> SingletonInstances = new Dictionary<Type, object>();
		private static readonly Dictionary<Type, Type> InterfaceMappings = new Dictionary<Type, Type>();

		public static void MapSingleton<TSingleton>()
            where TSingleton : class
		{
			SingletonInstances[typeof(TSingleton)] = Activator.CreateInstance(typeof(TSingleton));

		}

		public static void MapInterfaceSingleton<TInterface, TImplementation>()
            where TImplementation : class, TInterface
            where TInterface : class
		{
			MapClassToInterface<TImplementation, TInterface>();
			MapInstanceToSingleton<TInterface>(Get<TImplementation>());
		}

		public static void UnMapSingleton<TSingleton>()
            where TSingleton : class
		{
			if (SingletonInstances.ContainsKey(typeof(TSingleton)))
			{
				SingletonInstances.Remove(typeof(TSingleton));
			}


		}

		public static void MapInstanceToSingleton<TSingleton>(object instance)
		{
			SingletonInstances[typeof(TSingleton)] = instance;

		}

		private static bool IsTypeSingleton(Type typeToTest)
		{
			return SingletonInstances.ContainsKey(typeToTest);
		}

		public static void UnMapInstanceFromSingleton<TSingleton>()
		{
			if (SingletonInstances.ContainsKey(typeof(TSingleton)))
			{
				SingletonInstances.Remove(typeof(TSingleton));
			}

		}

		public static void MapClassToInterface<TImplementation, TInterface>()
            where TImplementation : class, TInterface
            where TInterface : class
		{
			InterfaceMappings[typeof(TInterface)] = typeof(TImplementation);

		}

		public static TInvoker MapCommandToInvoker<TCommand, TInvoker>()
            where TCommand : Command
            where TInvoker : Invoker
		{
			if (!IsTypeSingleton(typeof(TInvoker)))
			{
				MapSingleton<TInvoker>();
			}

			var invoker = Get<TInvoker>();
			invoker.AddCommand<TCommand>();

			return invoker;
		}
		/*
		 * Mediator mappings
		 */
		private static readonly Dictionary<Type, Type> ClassMediatorMappings = new Dictionary<Type, Type>();
		private static readonly Dictionary<IMediatorTarget, Mediator> MediatorAssignments =
			new Dictionary<IMediatorTarget, Mediator>();

		public static void MapMediatorToClass<TMediator, TClass>()
            where TMediator : Mediator
            where TClass : IMediatorTarget
		{
			ClassMediatorMappings[typeof(TClass)] = typeof(TMediator);
		}

		public static Mediator RequestMediator(IMediatorTarget target)
		{
			if (target == null)
			{
				throw (new ArgumentException("Null cannot be passed for IMediatorTarget"));
			}

			if (MediatorAssignments.ContainsKey(target))
			{
				throw (new ArgumentException("Target already has Mediator assigned to it."));
			}

			var targetType = target.GetType();

			//if we don't have this class type specifically mapped
			if (!ClassMediatorMappings.ContainsKey(targetType))
			{
				//checking to see if this target has a super class that has a mediator assigned to it
				foreach (Type classType in ClassMediatorMappings.Keys)
				{
					//TODO: See if this would be better targetType.BaseType
					var interfaces = targetType.GetInterfaces();
					var interfaceFound = false;
					foreach (var interfaceType in interfaces)
					{
						if (interfaceType == classType)
						{
							interfaceFound = true;
						}
					}
					if (targetType.IsSubclassOf(classType) || interfaceFound)
					{
						return CreateMediator(target, classType);
					}
				}
				//if still nothing we blow an exception
				throw (new ArgumentException(
					"Target type does not have mediator type mapped for it. Invoke MapMediatorToClass first."));
			}

			return CreateMediator(target, targetType);
		}

		private static Mediator CreateMediator(IMediatorTarget target, Type targetType)
		{
			var m = (Mediator)Activator.CreateInstance(ClassMediatorMappings[targetType], target);
			DIUtil.InjectProps(m);
			MediatorAssignments[target] = m;
			m.Register();
			return m;
		}

		public static void DestroyMediator(IMediatorTarget target)
		{
			if (MediatorAssignments.ContainsKey(target))
			{
				MediatorAssignments[target].Unregister();
				MediatorAssignments.Remove(target);
			}
		}
		/*
		 * Standard Get method
		 */
		public static TGet Get<TGet>()
            where TGet : class
		{
			var t = typeof(TGet);

			if (MustBeSingleton(t) && !IsTypeSingleton(t))
			{
				throw (new ArgumentException("Target type must be mapped as singleton: " + t.FullName));
			}

			//TODO: Look into inspecting constructor arguments

			if (SingletonInstances.ContainsKey(typeof(TGet)))
			{
				return SingletonInstances[typeof(TGet)] as TGet;
			}

       

			if (InterfaceMappings.ContainsKey(typeof(TGet)))
			{
				return Activator.CreateInstance(InterfaceMappings[typeof(TGet)]) as TGet;
			}


			return Activator.CreateInstance(typeof(TGet)) as TGet;


		}

		private static bool MustBeSingleton(Type t)
		{
			// Get instance of the attribute.
			MustBeSingletonAttribute MustAttribute =
				(MustBeSingletonAttribute)Attribute.GetCustomAttribute(t, typeof(MustBeSingletonAttribute));

			return MustAttribute != null;
		}
	}
}