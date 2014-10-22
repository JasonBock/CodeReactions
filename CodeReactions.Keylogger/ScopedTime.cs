using System;
using System.Diagnostics;

namespace CodeReactions.Keylogger
{
	public sealed class ScopedTime
		: IDisposable
	{
		public ScopedTime(string name)
		{
			this.Name = name;
			this.Watch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			this.Watch.Stop();
			Console.Out.WriteLine("{0} - {1}", this.Name, this.Watch.Elapsed);
		}

		public string Name { get; private set; }
		public Stopwatch Watch { get; private set; }
	}
}
