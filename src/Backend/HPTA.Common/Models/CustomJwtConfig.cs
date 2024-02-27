using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Common.Models
{
    public class CustomJwtConfig
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpiryDurationInMinutes { get; set; } = 15;
    }
}
