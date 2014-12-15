using System;
using MonkeyArms;
using MonoTouch.UIKit;
using System.Diagnostics;

namespace MonkeyArms.IOS
{
	public class IOSInvokerMap:InvokerMap, IInvokerMap
	{
		public IOSInvokerMap()
		{
			Debug.WriteLine("IOSInvokerMap");
		}

		public override void Add(IInvoker targetInvoker, EventHandler handlerFunction, object handlerHost)
		{
			if (handlerHost is UIViewController)
			{

				base.Add(targetInvoker, (object sender, EventArgs e) => ((UIViewController)handlerHost).InvokeOnMainThread(() => handlerFunction(sender, e)), handlerHost);
			}
			else if (handlerHost is UIView)
			{
				base.Add(targetInvoker, (object sender, EventArgs e) => ((UIView)handlerHost).InvokeOnMainThread(() => handlerFunction(sender, e)), handlerHost);

			}
			else
			{
				base.Add(targetInvoker, handlerFunction, handlerHost);
			}

		}
	}
}

