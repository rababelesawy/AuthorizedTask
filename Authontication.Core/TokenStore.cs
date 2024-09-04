using Authontication.Core.Entities;
using System.Collections.Concurrent;

public static class TokenStore
{
    private static readonly ConcurrentDictionary<string, RefreshToken> _refreshTokens = new ConcurrentDictionary<string, RefreshToken>();

    public static void AddToken(string token, string userId, TimeSpan expiration)
    {
        _refreshTokens[token] = new RefreshToken
        {
            Token = token,
            UserId = userId,
            Expiration = DateTime.UtcNow.Add(expiration), // Use the provided TimeSpan
            IsRevoked = false
        };
    }

    public static RefreshToken GetToken(string token)
    {
        _refreshTokens.TryGetValue(token, out var refreshToken);
        return refreshToken;
    }

    public static void RevokeToken(string token)
    {
        if (_refreshTokens.TryGetValue(token, out var refreshToken))
        {
            refreshToken.IsRevoked = true;
            _refreshTokens[token] = refreshToken;
        }
    }
}
