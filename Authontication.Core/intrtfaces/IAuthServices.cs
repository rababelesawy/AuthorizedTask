using Authontication.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authontication.Core.intrtfaces
{
    public interface IAuthServices
    {

        Task<string> CreateTokenAsync(AppUser user);
        Task<string> GenerateRefreshTokenAsync();

        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
