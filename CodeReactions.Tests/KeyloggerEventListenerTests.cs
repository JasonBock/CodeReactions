using System;
using Xunit;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerEventListenerTests
	{
		[Fact]
		public void Listen()
		{
			var source = new KeyloggerSource();

			using (var listener = new KeyloggerEventListener(source))
			{
				source.PressKeys(new[] { 'a', 'b', 'c' });

				Assert.Equal("abc", listener.LatestKeys);
			}
		}

		[Fact]
		public void ListenWhenSourceIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new KeyloggerEventListener(null));
		}
	}
}
