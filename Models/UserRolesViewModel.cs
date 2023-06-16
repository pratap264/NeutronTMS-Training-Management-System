using Microsoft.AspNetCore.Identity;
namespace NeutronTMS.Models;
public class UserRolesViewModel
{
    public string UserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = null!;
}