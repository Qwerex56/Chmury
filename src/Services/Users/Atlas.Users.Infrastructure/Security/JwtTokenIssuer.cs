using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Atlas.Kit.Infrastructure.Security;
using Atlas.Users.Application.Abstractions;
using Atlas.Users.Application.Common;
using Atlas.Users.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Atlas.Users.Infrastructure.Security;

internal sealed class JwtTokenIssuer(IOptions<JwtOptions> options, TimeProvider clock) : ITokenIssuer
{
    public AuthTokenView Issue(User user)
    {
        var jwt = options.Value;
        var expiresAt = clock.GetUtcNow().AddMinutes(jwt.LifetimeMinutes);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            ],
            notBefore: clock.GetUtcNow().UtcDateTime,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        return new AuthTokenView(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, UserView.From(user));
    }
}
