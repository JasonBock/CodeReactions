using System;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public sealed class EventedKeylogger
		: Win32Keylogger
	{
		public event EventHandler<KeyLoggedEventArgs> KeyLogged;

		public EventedKeylogger()
			: base() { }

		protected override void HandleKey(Keys key)
		{
			var @event = this.KeyLogged;

			if (@event != null)
			{
				@event(this, new KeyLoggedEventArgs(Convert.ToChar((int)key)));
			}
		}
	}
}
