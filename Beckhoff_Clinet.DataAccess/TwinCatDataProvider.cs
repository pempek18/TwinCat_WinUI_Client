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

        public IEnumerable<Variables> GetVariables()
        {
            List<Variables> variables = new List<Variables>();
            // Load all Symbols + DataTypes
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            ISymbolCollection<ISymbol> allSymbols = loader.Symbols;
            Symbol symbol;
            foreach (ISymbol val in allSymbols)
            {
                if (val.Category != DataTypeCategory.Struct)
                {
                    Variables variable = new Variables();
                    symbol = (Symbol)loader.Symbols["." + val.InstanceName];
                    variable.Name = val.InstanceName;
                    variable.Value = symbol.ReadValue().ToString();
                    Debug.WriteLine("Name : " + variable.Name + " " + variable.Value);
                    variables.Add(variable);
                }
            }
            return variables;

        }
        public Variables GetVariableValue(Variables variable)
        {
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            Symbol symbol = (Symbol)loader.Symbols["." + variable.Name];
            variable.Value = symbol.ReadValue().ToString();
            return variable;
        }
        public void SetVatiable(Variables var)
        {
            ISymbolLoader loader = SymbolLoaderFactory.Create(ads, SymbolLoaderSettings.Default);
            Symbol symbol;
            symbol = (Symbol)loader.Symbols["." + var.Name];
            symbol.WriteValue(var.Value);
        }
    }
}
