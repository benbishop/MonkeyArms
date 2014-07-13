using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonkeyArms
{
	public class InjectAttribute : Attribute
	{
		public Type Default;
		public string Name;
	}

	public class DIUtil
	{
		public static void InjectProps (IInjectingTarget target, InjectionMap map = null)
		{
			Inject (target.GetType ().GetProperties (), target, map);
			Inject (target.GetType ().GetFields (), target, map);

		}

		static void Inject (MemberInfo[] memberInfoArray, IInjectingTarget target, InjectionMap map)
		{
			foreach (var memberInfo in memberInfoArray) {
				IEnumerable<object> attrs = GetAttributes (target, memberInfo);

				//TODO: Look into adding a warning if an inject prop is private or protected

				foreach (InjectAttribute attr in attrs) {
					if (map == null || !map.HasTypeMapped (GetMemberInfoType (memberInfo))) {


						//We get the reference declaration to DI's Get method
						var mi = typeof(DI).GetMethod ("Get", BindingFlags.Static | BindingFlags.Public);

						//getting DI.Get method so we can invoke it to get the value
						//mi = DI.Get
						//memberInfo = value type we want from DI.Get
						MethodInfo methodInfo = GetMethodInfo (mi, memberInfo);


						//Checking if value was found
						object valueToInject;
						//If this throws an exception there is probably a child Inject prop that is not registered

						try {
							valueToInject = methodInfo.Invoke (null, new object[]{ attr.Name });
						} catch (Exception e) {

							valueToInject = null;
						}

						if (valueToInject == null && attr.Default == null) {
							throw(new ArgumentException ("Inject target type was not found. Did you forget to register it with DI?"));
						} else if (valueToInject == null && (attr as InjectAttribute).Default != null) {
							AssignValueToTarget (target, memberInfo, Activator.CreateInstance ((attr as InjectAttribute).Default));
						} else {
							AssignValueToTarget (target, memberInfo, valueToInject);
						}
					} else {
						var valueToAssign = map.Get (GetMemberInfoType (memberInfo));
						AssignValueToTarget (target, memberInfo, valueToAssign);

					}
				}
			}
		}

		private static Type GetMemberInfoType (MemberInfo info)
		{
			if (info is FieldInfo) {
				return (info as FieldInfo).FieldType;
			}

			if (info is PropertyInfo) {
				return (info as PropertyInfo).PropertyType;
			}
			return null;
		}

		private static void AssignValueToTarget (IInjectingTarget target, MemberInfo memberInfo, object valueToInject)
		{
			if (memberInfo is FieldInfo) {
				(memberInfo as FieldInfo).SetValue (target, valueToInject);
			}
			if (memberInfo is PropertyInfo) {
				(memberInfo as PropertyInfo).SetValue (target, valueToInject, null);
			}
		}

		private static MethodInfo GetMethodInfo (MethodInfo mi, MemberInfo memberInfo)
		{
			if (memberInfo is FieldInfo) {
				return mi.MakeGenericMethod ((memberInfo as FieldInfo).FieldType);
			}
			if (memberInfo is PropertyInfo) {
				return mi.MakeGenericMethod ((memberInfo as PropertyInfo).PropertyType);
			}
			return null;
		}

		private static IEnumerable<object> GetAttributes (IInjectingTarget target, MemberInfo memberInfo)
		{
			if (memberInfo is FieldInfo) {
				return target.GetType ().GetField (memberInfo.Name).GetCustomAttributes (false);
			}
			if (memberInfo is PropertyInfo) {
				return target.GetType ().GetProperty (memberInfo.Name).GetCustomAttributes (false);
			}
			throw (new ArgumentException ("Inject tag was places on a member type injection doesn't support"));
		}
	}

	public interface IInjectingTarget
	{
	}

	public interface IIoCContainer
	{
		TGet Get<TGet> () where TGet : class;
	}
}