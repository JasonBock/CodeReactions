using Owin;

namespace CodeReactions.Server
{
	public sealed class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			//app.MapSignalR<KeyloggerConnection>("/keylogger/keyspressed");
			app.MapSignalR();
		}
	}
}