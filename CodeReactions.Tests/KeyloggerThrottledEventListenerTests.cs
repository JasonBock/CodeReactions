using System;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using Xunit;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerThrottledEventListenerTests
	{
		[Fact]
		public void Listen()
		{
			var source = new KeyloggerSource();
			var scheduler = new TestScheduler();

			using (var listener = new KeyloggerThrottledEventListener(source, scheduler))
			{
				source.PressKeys(new[] { 'a', 'b', 'c' });
				scheduler.AdvanceBy(TimeSpan.TicksPerMillisecond * 5000);
				Assert.Equal("abc", listener.LatestKeys);
			}
		}

		[Fact]
		public void ListenWithDefaultScheduler()
		{
			var source = new KeyloggerSource();

			using (var listener = new KeyloggerThrottledEventListener(source, Scheduler.Default))
			{
				source.PressKeys(new[] { 'a', 'b', 'c' });
				// Technically, if this tests takes longer than 5 seconds, this may not be null :)
				// But that's the point, we don't have the control we want.
				Assert.Null(listener.LatestKeys);
			}
		}

		[Fact]
		public void ListenWhenSourceIsNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new KeyloggerThrottledEventListener(null, new MockScheduler()));
		}

		[Fact]
		public void ListenWhenSchedulerIsNull()
		{
			Assert.Throws<ArgumentNullException>(() =>
				new KeyloggerThrottledEventListener(new KeyloggerSource(), null));
		}
	}

	public sealed class MockScheduler
		: IScheduler
	{
		public DateTimeOffset Now
		{
			get { throw new NotImplementedException(); }
		}

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			throw new NotImplementedException();
		}

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
		{
			throw new NotImplementedException();
		}

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			throw new NotImplementedException();
		}
	}
}
