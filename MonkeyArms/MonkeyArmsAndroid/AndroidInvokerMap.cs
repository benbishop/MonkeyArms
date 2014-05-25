using System;
using MonkeyArms;
using Android.App;
using Java.Lang;

namespace MonkeyArms.Android
{
	public class AndroidInvokerMap:InvokerMap
	{
		public AndroidInvokerMap ()
		{
		}

		public override void Add (IInvoker targetInvoker, EventHandler handlerFunction, object handlerHost)
		{
			if (handlerHost is Activity) {
				base.Add (targetInvoker, (object sender, EventArgs e) => ((Activity)handlerHost).RunOnUiThread (() => handlerFunction (sender, e)), handlerHost);
			} else if (handlerHost is Fragment) {
				base.Add (targetInvoker, (object sender, EventArgs e) => ((Fragment)handlerHost).Activity.RunOnUiThread (() => handlerFunction (sender, e)), handlerHost);
			} else {
				base.Add (targetInvoker, handlerFunction, handlerHost);
			}

		}
	}
}

