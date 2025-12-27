public class AuthResponseDto
{
    public Guid UserId { get; }
    public string UserName { get; }
    public string Email { get; }
    public IEnumerable<string> Roles { get; }
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public AuthResponseDto(
        Guid userId,
        string userName,
        string email,
        IEnumerable<string> roles,
        string accessToken,
        string refreshToken)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
        Roles = roles;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
