using System.Collections.Generic;
using Beckhoff_Client.Common.Model;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace Beckhoff_Client.Common.DataProvider
{
    public interface IVariablesDataProvider
    {
        bool SetConnection(tcConnection ConnectionData);
        IEnumerable<Symbol> GetVariables();
        Symbol GetVariableValue(Symbol symbol);
        Symbol GetSubSymbols(Symbol symbol, string subSymbolName);
        //void SetSubSymbolsValue(ISymbol symbol, string subSymbolName);
        void SetVariable(Symbol symbol);
    }
}
