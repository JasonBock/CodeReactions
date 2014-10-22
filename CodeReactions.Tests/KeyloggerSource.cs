using System;
using CodeReactions.Messages;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerSource
	{
		public event EventHandler<KeysPressedEventArgs> KeysPressed;

		public void PressKeys(char[] keys)
		{
			var @event = this.KeysPressed;

			if (@event != null)
			{
				@event(this, new KeysPressedEventArgs(new KeyloggerMessage { Keys = keys }));
			}
		}
	}
}
