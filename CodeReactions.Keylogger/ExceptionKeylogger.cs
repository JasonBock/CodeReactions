using System;
using System.Reactive.Disposables;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public sealed class ExceptionKeylogger
		: Win32Keylogger, IObservable<char>
	{
		private IObserver<char> observer;

		public ExceptionKeylogger()
			: base() { }

		protected override void HandleKey(Keys key)
		{
			if (this.observer != null)
			{
				if (key == Keys.Q)
				{
					this.observer.OnError(new NotSupportedException("Qs are evil!"));
				}
				else
				{
					this.observer.OnNext(Convert.ToChar((int)key));
				}
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
