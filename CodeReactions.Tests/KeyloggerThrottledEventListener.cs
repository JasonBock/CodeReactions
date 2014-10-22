using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerThrottledEventListener
		: IDisposable
	{
		private KeyloggerSource source;
		private IDisposable subscription;

		public KeyloggerThrottledEventListener(KeyloggerSource source, IScheduler scheduler)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			if (scheduler == null)
			{
				throw new ArgumentNullException("scheduler");
			}

			this.source = source;
			this.subscription = Observable.FromEventPattern<KeysPressedEventArgs>(this.source, "KeysPressed")
				.Throttle(TimeSpan.FromSeconds(5), scheduler)
				.Subscribe(pattern => this.LatestKeys = new string(pattern.EventArgs.Message.Keys));
		}

		public string LatestKeys { get; private set; }

		public void Dispose()
		{
			this.subscription.Dispose();
		}
	}
}
