using Microsoft.AspNet.SignalR;

namespace CodeReactions.Server
{
	public sealed class KeyloggerConnection
		: PersistentConnection { }

	public sealed class KeyloggerHub
		: Hub { }
}