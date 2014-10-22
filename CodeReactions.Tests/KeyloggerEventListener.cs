using System;
using System.Reactive.Linq;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerEventListener
		: IDisposable
	{
		private KeyloggerSource source;
		private IDisposable subscription;

		public KeyloggerEventListener(KeyloggerSource source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

			this.source = source;
			this.subscription = Observable.FromEventPattern<KeysPressedEventArgs>(this.source, "KeysPressed")
				.Subscribe(pattern =>
				{
					this.LatestKeys = new string(pattern.EventArgs.Message.Keys);
				});
		}

		public string LatestKeys { get; private set; }

		public void Dispose()
		{
			this.subscription.Dispose();
		}
	}
}
