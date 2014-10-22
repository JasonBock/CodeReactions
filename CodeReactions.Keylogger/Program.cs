using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	class Program
	{
		private static readonly char[] pianoKeys = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };

		static void Main(string[] args)
		{
			//Program.UseConsoleKeylogger();
			//Program.UseObservableKeylogger();
			//Program.UseObservableCreate();
			//Program.UseObservableEventPattern();
			//Program.UseObservableEventPatternWithStringName();
			//Program.UseFilteredObservable();
			//Program.UseFilteredTimedObservable();
			//Program.UseExceptionObserverWithOnError();
			//Program.UseExceptionObserverWithRetry();
			Program.UsePublishingKeylogger();
			//Program.UseSimulatedPublishingKeylogger();
		}

		private static void UseConsoleKeylogger()
		{
			using (var keyLogger = new ConsoleKeylogger())
			{
				Application.Run();
			}
		}

		private static void UseObservableKeylogger()
		{
			using (var keyLogger = new ObservableKeylogger())
			{
				keyLogger.Subscribe(new ConsoleObserver());
				Application.Run();
			}
		}

		private static void UseObservableCreate()
		{
			var observable = Observable.Create<char>(
				o =>
				{
					var keylogger = new EventedKeylogger();
					keylogger.KeyLogged += (s, e) => o.OnNext(e.Key);
					return () => keylogger.Dispose();
				});

			using (var subscription = observable.Subscribe(key => Console.Out.WriteLine(key)))
			{
				Application.Run();
			}
		}

		private static void UseObservableEventPattern()
		{
			using (var keylogger = new EventedKeylogger())
			{
				var observable = Observable.FromEventPattern<KeyLoggedEventArgs>(
					h => keylogger.KeyLogged += h,
					h => keylogger.KeyLogged -= h);
				using (var subscription = observable.Subscribe(
					pattern => Console.Out.WriteLine(pattern.EventArgs.Key)))
				{
					Application.Run();
				}
			}
		}

		private static void UseObservableEventPatternWithStringName()
		{
			using (var keylogger = new EventedKeylogger())
			{
				var observable = Observable.FromEventPattern<KeyLoggedEventArgs>(
					keylogger, "KeyLogged");
				using (var subscription = observable.Subscribe(
					pattern => Console.Out.WriteLine(pattern.EventArgs.Key)))
				{
					Application.Run();
				}
			}
		}

		private static void UseFilteredObservable()
		{
			using (var keylogger = new EventedKeylogger())
			{
				using (var subscriber = Observable.FromEventPattern<KeyLoggedEventArgs>(
					keylogger, "KeyLogged")
					.Where(pattern => Program.pianoKeys.Contains(pattern.EventArgs.Key))
					.Subscribe(pattern => Console.Out.WriteLine(pattern.EventArgs.Key)))
				{
					Application.Run();
				}
			}
		}

		private static void UseFilteredTimedObservable()
		{
			using (var keylogger = new EventedKeylogger())
			{
				var observable = Observable.FromEventPattern<KeyLoggedEventArgs>(
					keylogger, "KeyLogged")
					.Where(pattern => Program.pianoKeys.Contains(pattern.EventArgs.Key));
				using (var consoleSubscriber =
					observable.Subscribe(pattern => Console.Out.WriteLine(pattern.EventArgs.Key)))
				{
					using (var first10TimedSubscriber =
						Observable.Using(
							() => new ScopedTime("Piano Keys"), _ => observable)
						.Take(10)
						.Subscribe())
					{
						Application.Run();
					}
				}
			}
		}

		private static void UseExceptionObserverWithOnError()
		{
			using (var keyLogger = new ExceptionKeylogger())
			{
				using (var subscription = keyLogger.Subscribe(
					pattern => Console.Out.WriteLine(pattern),
					new Action<Exception>(exception => Console.Out.WriteLine(exception.Message))))
				{
					Application.Run();
				}
			}
		}

		private static void UseExceptionObserverWithRetry()
		{
			using (var keyLogger = new ExceptionKeylogger())
			{
				using (var subscription = keyLogger.Retry(3).Subscribe(
					pattern => Console.Out.WriteLine(pattern),
					new Action<Exception>(exception => Console.Out.WriteLine(exception.Message))))
				{
					Application.Run();
				}
			}
		}

		private static void UsePublishingKeylogger()
		{
			using (var keyLogger = new PublishingKeylogger())
			{
				Application.Run();
			}
		}

		private static void UseSimulatedPublishingKeylogger()
		{
			using (var keyLogger = new SimulatedPublishingKeylogger())
			{
				Console.ReadLine();
			}
		}
	}
}
