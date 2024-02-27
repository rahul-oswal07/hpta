using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Services.Contracts
{
    public interface IHashingService
    {
        string GenerateHash(string password, KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256, int iterationCount = 10000, int saltSize = 16);

        bool VerifyHash(string password, string passwordHash);
    }
}
