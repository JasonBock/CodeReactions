using System;
using CodeReactions.Messages;

namespace CodeReactions.Tests
{
	public sealed class KeysPressedEventArgs
		: EventArgs
	{
		public KeysPressedEventArgs(KeyloggerMessage message)
			: base()
		{
			this.Message = message;
		}

		public KeyloggerMessage Message { get; private set; }
	}
}
