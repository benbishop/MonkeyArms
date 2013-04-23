using System;

namespace MonkeyArms
{
	public class InjectAttribute : System.Attribute { }

	public class Actor
	{
		public Actor ()
		{
			InjectPropsFromDI ();

		}

		void InjectPropsFromDI ()
		{
			var myPropertyInfo = this.GetType ().GetProperties ();
			//looping through props
			foreach (var propertyInfo in myPropertyInfo) {
				//looping through attributes
				var propType = this.GetType ().GetProperty (propertyInfo.Name).DeclaringType;
				var attrs = this.GetType ().GetProperty (propertyInfo.Name).GetCustomAttributes (false);
				foreach (System.Attribute attr in attrs) {
					//if prop has an inject acttribute
					if (attr is InjectAttribute) {
						//retrieve injectable from DI
						var mi = typeof(DI).GetMethod ("Get", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
						var gmi = mi.MakeGenericMethod (propertyInfo.PropertyType);
						//assign the retrieved item from DI to this
						propertyInfo.SetValue (this, gmi.Invoke (null, null));
					}
				}
			}
		}
	}
}

