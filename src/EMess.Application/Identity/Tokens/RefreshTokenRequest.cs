namespace EMess.Application.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);