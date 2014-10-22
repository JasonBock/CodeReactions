using System.Collections.Generic;
using System.Linq;

namespace CodeReactions.Client.Extensions
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<T> In<T>(this IEnumerable<T> @this, IEnumerable<T> comparison)
		{
			return @this.Where(x => comparison.Contains(x));
		}
	}
}
