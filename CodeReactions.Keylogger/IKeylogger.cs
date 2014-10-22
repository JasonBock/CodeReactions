using System.Windows.Forms;

namespace CodeReactions.Keylogger
{
	public interface IKeylogger
	{
		void HandleKey(Keys key);
	}
}
