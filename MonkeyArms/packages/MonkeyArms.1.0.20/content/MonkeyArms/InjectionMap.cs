using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class InjectionMap
	{
		public delegate object ValueDelegate ();

		protected Dictionary<Type, ValueDelegate> InvokerMap = new Dictionary<Type, ValueDelegate> ();

		public InjectionMap ()
		{
	
		}

		public void Add <TValueType> (ValueDelegate par) where TValueType:class
		{
			InvokerMap [typeof(TValueType)] = par;
		}

		public object Get (Type valueType)
		{

			return InvokerMap [valueType] ();
		}

		public bool HasTypeMapped (Type typeToCheck)
		{
			return InvokerMap.ContainsKey (typeToCheck);
		}
	}
}

