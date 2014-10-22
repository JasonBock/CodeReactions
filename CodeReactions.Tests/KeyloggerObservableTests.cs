using System;
using System.Reactive.Subjects;
using CodeReactions.Messages;
using Xunit;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerObservableTests
	{
		[Fact]
		public void OnNextKeyloggerMessage()
		{
			var subject = new Subject<KeyloggerMessage>();

			var listener = new KeyloggerObservable(subject);
			subject.OnNext(new KeyloggerMessage { Keys = new[] { 'a', 'b', 'c' } });

			Assert.Equal("abc", listener.LatestKeys);
		}

		[Fact]
		public void CreateWhenObservableIsNull()
		{
			Assert.Throws<ArgumentNullException>(() => new KeyloggerObservable(null));
		}
	}
}
