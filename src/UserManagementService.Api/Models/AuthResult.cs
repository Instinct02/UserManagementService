namespace UserManagementService.Api.Models;
public class AuthResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string Email { get; set; }
}