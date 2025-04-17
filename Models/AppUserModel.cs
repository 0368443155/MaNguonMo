using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
	public class AppUserModel : IdentityUser
	{
		public string Occupation {  get; set; }
	}
}
