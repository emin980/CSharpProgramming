namespace HangarDesk.Final.Domain;
internal sealed class AuthenticatedUser
{
    public int Id { get; init; }
    public string Username { get; init; }
    public string DisplayName { get; init; }
    public string Role { get; init; }
}
internal sealed class UserCredential
{
    public int Id { get; init; }
    public string Username { get; init; }
    public string DisplayName { get; init; }
    public byte[] PasswordHash { get; init; }
    public byte[] PasswordSalt { get; init; }
    public string Role { get; init; }
    public bool IsActive { get; init; }
}
internal static class Roles
{
    public const string Administrator = "Administrator";
    public const string Technician = "Technician";
    public const string Viewer = "Viewer";
}
