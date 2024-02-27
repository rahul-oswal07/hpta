using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Common.Models
{
    public class OTPConfig
    {
        public string Secret { get; set; }

        public int ValidityInMinutes { get; set; }
    }
}
