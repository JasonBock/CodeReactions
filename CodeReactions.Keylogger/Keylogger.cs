using System;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public abstract class Keylogger
		: IDisposable
	{
		protected abstract void HandleKey(Keys key);

		public abstract void Dispose();
	}
}
