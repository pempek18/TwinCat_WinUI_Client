using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beckhoff_Client.Model;

namespace Beckhoff_Client.Common.ConnectionProvider
{
    public interface I_tcConnection
    {
        bool GetConnection(tcConnection ConnectionData);
        void GetVariables();
        void SetVatiable();
    }
}
