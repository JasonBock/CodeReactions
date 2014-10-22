using System;

namespace CodeReactions.Keylogger
{
	public sealed class KeyLoggedEventArgs
		: EventArgs
	{
		public KeyLoggedEventArgs(char key)
			: base()
		{
			this.Key = key;
		}

		public char Key { get; private set; }
	}
}
