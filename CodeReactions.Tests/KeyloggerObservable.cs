using System;
using CodeReactions.Messages;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerObservable
		: IDisposable
	{
		private IDisposable subscription;

		public KeyloggerObservable(IObservable<KeyloggerMessage> observable)
		{
			if (observable == null)
			{
				throw new ArgumentNullException("observable");
			}

			this.subscription = observable.Subscribe(
				message => this.LatestKeys = new string(message.Keys));
		}

		public string LatestKeys { get; private set; }

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
