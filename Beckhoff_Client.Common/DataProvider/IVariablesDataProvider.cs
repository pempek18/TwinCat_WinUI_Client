using System.Collections.Generic;
using Beckhoff_Client.Common.Model;

namespace Beckhoff_Client.Common.DataProvider
{
    public interface IVariablesDataProvider
    {
        bool SetConnection(tcConnection ConnectionData);
        IEnumerable<Variables> GetVariables();
        Variables GetVariableValue(Variables variable);
        void SetVatiable(Variables var);
    }
}
