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
            SymbolLoaderSettings settings = new SymbolLoaderSettings(SymbolsLoadMode.VirtualTree);
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, settings);
            ISymbol Positions = (Symbol)loader.Symbols["Positions"];
            ISymbolCollection<ISymbol>  SubPositions = Positions.SubSymbols;
            foreach (ISymbol sym in SubPositions)
            {
                if (sym.DataType.ToString() == "FB_STD_INT1" )
                {
                    Symbol symbol = (Symbol)loader.Symbols["Positions" + "." + sym.InstanceName];
                    Debug.WriteLine("Name : " + symbol.InstanceName );
                    Symbols.Add(symbol);
                }
            }
            return Symbols;

        }
        public string GetSubSymbolsValue(ISymbol symbol, string subSymbolName)
        {
            foreach (ISymbol sym in symbol.SubSymbols)
            {
                if (sym.InstanceName == subSymbolName)
                {
                    ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
                    Symbol _symbol = (Symbol)loader.Symbols[sym.InstancePath];
                    return _symbol.ReadValue().ToString();
                }
            }
            return "";
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
