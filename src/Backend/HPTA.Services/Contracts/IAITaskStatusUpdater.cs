using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTA.Services.Contracts
{
    public interface IAITaskStatusUpdater
    {
        void UpdateStatus(string key, bool status);
    }
}
