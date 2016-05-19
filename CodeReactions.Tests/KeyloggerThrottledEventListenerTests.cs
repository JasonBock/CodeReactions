using System;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;
using ReactiveUI.Testing;
using Xunit;

namespace CodeReactions.Tests
{
	public sealed class KeyloggerThrottledEventListenerTests
	{
		[Fact]
		public void Listen()
		{
			var source = new KeyloggerSource();

			new TestScheduler().With(scheduler =>
			{
				using (var listener = new KeyloggerThrottledEventListener(source, scheduler))
				{
					source.PressKeys(new[] { 'a', 'b', 'c' });
					scheduler.AdvanceByMs(5000);
					Assert.Equal("abc", listener.LatestKeys);
				}
			});
		}

		[Fact]
		public void ListenWithDefaultScheduler()
		{
			var source = new KeyloggerSource();

			using (var listener = new KeyloggerThrottledEventListener(source, Scheduler.Default))
			{
				source.PressKeys(new[] { 'a', 'b', 'c' });
				Assert.Equal("abc", listener.LatestKeys);
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
