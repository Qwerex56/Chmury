using Atlas.Users.Application.Common;
using Atlas.Users.Domain;

namespace Atlas.Users.Application.Abstractions;

public interface ITokenIssuer
{
    AuthTokenView Issue(User user);
}
