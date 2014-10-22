using System.Web.Http;
using CodeReactions.Messages;
using Microsoft.AspNet.SignalR;

namespace CodeReactions.Server.Controllers
{
	public sealed class KeyloggerController
		: ApiController
	{
		// POST api/keylogger
		public void Post([FromBody] KeyloggerMessage value)
		{
			GlobalHost.ConnectionManager.GetHubContext<KeyloggerHub>().Clients.All.KeysPressed(value);
		}
	}
}
