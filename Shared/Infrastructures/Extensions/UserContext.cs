using Stl.Fusion;
using System.Security.Claims;

namespace Shared.Infrastructures
{
	public class UserContext
	{
		public IEnumerable<Claim> UserClaims = new List<Claim>();
		public Session Session = Session.Default;
	}
}
