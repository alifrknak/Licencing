using Domain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application;

public class RequestContext(IHttpContextAccessor httpContextAccessor)
{
    /// <summary>
    ///  return one's id who makes current request
    /// </summary>
    /// <returns></returns>
    public Guid GetRequestOwnerId()
    {
        return Guid.Parse(httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == "Sub").Value);
    }

    public UserType GetRequestUserType()
    {
        return Enum.Parse<UserType>(httpContextAccessor.HttpContext.User.Claims.Single(x => x.Type == ClaimTypes.Role).Value);
    }
}