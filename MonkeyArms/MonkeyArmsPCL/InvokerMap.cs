using System;
using System.Collections.Generic;

namespace MonkeyArms
{
	public class InvokerMap
	{
		protected Dictionary<Invoker, List<EventHandler>> Map = new Dictionary<Invoker, List<EventHandler>>(); 

		public InvokerMap ()
		{
		}

		public bool IsInvokerMapped (Invoker targetInvoker)
		{
			return Map.ContainsKey (targetInvoker);
		}

		public bool InvokerHasHandler (Invoker targetInvoker, EventHandler handlerFunction)
		{
			return (IsInvokerMapped(targetInvoker) && Map [targetInvoker].Contains (handlerFunction));
		}

		public void Add (Invoker targetInvoker, EventHandler handlerFunction)
		{
			if (!IsInvokerMapped (targetInvoker)) {
				Map [targetInvoker] = new List<EventHandler> ();
			}
			if (!InvokerHasHandler (targetInvoker, handlerFunction)) {
				Map [targetInvoker].Add (handlerFunction);
				targetInvoker.Invoked += handlerFunction;
			}
		}

		public void Remove(Invoker targetInvoker, EventHandler handlerFunction)
		{

			if (Map.ContainsKey (targetInvoker) && Map [targetInvoker].Contains (handlerFunction)) {
				Map [targetInvoker].Remove (handlerFunction);
				targetInvoker.Invoked -= handlerFunction;
			}
		}

		public void RemoveAll()
		{
			foreach (var keyValuePair in Map) {
				foreach (var eventHandler in keyValuePair.Value) {
					keyValuePair.Key.Invoked -= eventHandler;
				}
				keyValuePair.Value.RemoveAll (e => e is EventHandler);
			}
		}

		public int GetInvokerHandlerCount(Invoker targetInvoker)
		{
			if (Map.ContainsKey (targetInvoker)) {
				return Map [targetInvoker].Count;
			}
			return 0;
		}
	}
}

