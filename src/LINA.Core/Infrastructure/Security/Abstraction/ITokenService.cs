using LINA.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LINA.Core.Infrastructure.Security.Abstraction
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user);
    }
}
