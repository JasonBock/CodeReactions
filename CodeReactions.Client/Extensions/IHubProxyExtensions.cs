using System;
using System.Reactive.Linq;
using Microsoft.AspNet.SignalR.Client;

namespace CodeReactions.Client.Extensions
{
	public static class IHubProxyExtensions
	{
		public static IObservable<T> ObserveAs<T>(this IHubProxy @this, string eventName)
		{
			return from item in @this.Observe(eventName)
					 let m = item[0].ToObject<T>()
					 select m;
		}
	}
}
