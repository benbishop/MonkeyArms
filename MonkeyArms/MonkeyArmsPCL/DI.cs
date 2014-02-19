using System;
using System.Collections.Generic;
using TinyIoC;

namespace MonkeyArms
{
    public static class DI
    {
        private static readonly TinyIoCContainer Injector = new TinyIoCContainer();

        private static readonly Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        private static readonly List<Type> Singletons = new List<Type>();

        public static void MapSingleton<TSingleton>()
            where TSingleton : class
        {
            Injector.Register<TSingleton>().AsSingleton();
            Singletons.Add(typeof(TSingleton));
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
            Injector.Register<TSingleton>().AsMultiInstance();
            Singletons.Remove(typeof(TSingleton));
        }

        public static void MapInstanceToSingleton<TSingleton>(object instance)
        {
            Instances[typeof(TSingleton)] = instance;
            if (!IsTypeSingleton(typeof(TSingleton)))
            {
                Singletons.Add(typeof(TSingleton));
            }
        }

        private static bool IsTypeSingleton(Type typeToTest)
        {
            return Singletons.Contains(typeToTest);
        }

        public static void UnMapInstanceFromSingleton<TSingleton>()
        {
            if (Instances.ContainsKey(typeof(TSingleton)))
            {
                Instances.Remove(typeof(TSingleton));
            }
        }

        public static void MapClassToInterface<TImplementation, TInterface>()
            where TImplementation : class, TInterface
            where TInterface : class
        {
            Injector.Register<TInterface, TImplementation>().AsMultiInstance();
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
            //TODO: Look into inspecting constructor arguments
            var t = typeof(TGet);
            if (Instances.ContainsKey(typeof(TGet)))
            {
                return Instances[typeof(TGet)] as TGet;
            }

            if (!Injector.CanResolve<TGet>())
            {
                throw (new ArgumentException("Target type cannot be resolved: " + t.FullName));
            }

            return Injector.Resolve<TGet>();
        }
    }
}