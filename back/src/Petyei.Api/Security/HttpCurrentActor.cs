using System.Security.Claims;
using Petyei.Application.Common;

namespace Petyei.Api.Security;

/// <summary>Reads the account identifier from the authenticated principal of the current request.</summary>
internal sealed class HttpCurrentActor(IHttpContextAccessor accessor) : ICurrentActor
{
    public Guid? AccountId =>
        Guid.TryParse(accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;
}
