using CodeMode.Token.Service.Configurations;
using CodeMode.Token.Service.Dtos.TokenDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeMode.Token.Service.Services.Abstract
{
    public interface ITokenService<TUser> where TUser : IdentityUser
    {
        Task<CreateTokenDto> CreateTokenAsync(TUser user, List<Claim> claims, CustomTokenOptions options);
        //ClientTokenDto CreateTokenByClient(Client client);
    }
}
