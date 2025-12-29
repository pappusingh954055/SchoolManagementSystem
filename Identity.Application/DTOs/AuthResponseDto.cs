public class AuthResponseDto
{
    public Guid UserId { get; init; }
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
    public IEnumerable<string> Roles { get; init; } = new List<string>();

    //public AuthResponseDto(
    //    Guid userId,       
    //    IEnumerable<string> roles,
    //    string accessToken,
    //    string refreshToken)
    //{
    //    UserId = userId;    
    //    Roles = roles;
    //    AccessToken = accessToken;
    //    RefreshToken = refreshToken;
    //}
}
