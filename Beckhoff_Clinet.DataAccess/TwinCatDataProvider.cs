using Beckhoff_Client.Common.Model;
using Beckhoff_Client.Common.DataProvider;
using System;
using System.Collections.Generic;
using System.Net;
using TwinCAT.Ads;
using TwinCAT.Ads.TcpRouter;
using System.Threading;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;
using TwinCAT;
using System.Diagnostics;

namespace Beckhoff_Client.DataAccess
{
    public class TwinCatDataProvider : IVariablesDataProvider
    {
        AdsClient ads = new AdsClient();
        public bool SetConnection(tcConnection ConnectionData)
        {
            AmsTcpIpRouter _router = new AmsTcpIpRouter(ConnectionData._localNetId);
            CancellationToken cancel = new CancellationToken();
            try
            {
                _router.AddRoute(new Route(ConnectionData._remoteRouteName, ConnectionData._remoteNetId, new IPAddress[] { ConnectionData._remoteIp }));
                _router.StartAsync(cancel);

                ads.Connect(ConnectionData._remoteNetId, 851);

                return ads.IsConnected;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Symbol> GetVariables()
        {
            List<Symbol> Symbols = new List<Symbol>();
            // Load all Symbols + DataTypes
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            ISymbolCollection<ISymbol> allSymbols = loader.Symbols;
            foreach (ISymbol val in allSymbols)
            {
                if (val.Category != DataTypeCategory.Struct && val.Category != DataTypeCategory.Enum )
                {
                    Symbol symbol = (Symbol)loader.Symbols["." + val.InstanceName];
                    Debug.WriteLine("Name : " + symbol.InstanceName + " " + symbol.ReadValue().ToString());
                    Symbols.Add(symbol);
                }
            }
            return Symbols;

        }
        public Symbol GetVariableValue(Symbol symbol)
        {
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            Symbol _symbol = (Symbol)loader.Symbols[symbol.InstanceName];
            return _symbol;
        }
        public void SetVariable(Symbol symbol)
        {
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            Symbol _symbol = (Symbol)loader.Symbols[symbol.InstanceName];
            _symbol.WriteValue(symbol);
        }
    }
}
