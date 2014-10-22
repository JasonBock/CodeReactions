using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public abstract class SimulatedKeylogger
		: Keylogger
	{
		private static string content = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG.";
		private Task keyGenerator;
		private CancellationToken cancel;
		private CancellationTokenSource source;

		protected SimulatedKeylogger()
			: base()
		{
			this.source = new CancellationTokenSource();
			this.cancel = source.Token;

			this.keyGenerator = Task.Factory.StartNew(() =>
			{
				var index = 0;
				var random = new Random();

				while (!this.cancel.IsCancellationRequested)
				{
					Task.Delay(random.Next(150, 225)).Wait();
					this.HandleKey((Keys)SimulatedKeylogger.content[index]);
					index = (index + 1) % content.Length;
				}
			});
		}

		public override void Dispose()
		{
			this.source.Cancel();
			this.keyGenerator.Wait();
		}
	}
}
