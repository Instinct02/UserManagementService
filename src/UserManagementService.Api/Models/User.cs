using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Api.Models;

public class User
{
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; }
    [StringLength(50)]
    public string LastName { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [StringLength(50)]
    public string Password { get; set; }
    [StringLength(200)]
    public string Salt { get; set; }
    [StringLength(50)]
    public string PhoneNumber { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }

    // ✅ Add these for refresh token support
    [StringLength(200)]
    public string? RefreshToken { get; set; }
    [StringLength(200)]
    public DateTime? RefreshTokenExpiryTime { get; set; }
}