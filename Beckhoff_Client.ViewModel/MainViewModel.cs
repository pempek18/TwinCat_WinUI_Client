using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.Common.Model;
using Beckhoff_Client.ViewModel.Command;
using System.Collections.ObjectModel;
using System.Net;
using TwinCAT.Ads;

namespace Beckhoff_Client.ViewModel
{
    public class MainViewModel : ViewModelBase 
    {
        //Od C# 9 możemy używać "target-typed new expresion"
        //ObservableCollection to classa która robi to samo co List<>
        //tylko informuje of zmianach w kolekcji
        //public ObservableCollection<ConnectionViewModel> connections { get; } = new();
        public ObservableCollection<VariablesViewModel> Variables{ get; } = new();
        private VariablesViewModel _selectedVariable;
        private tcConnection tcConnection = new tcConnection();
        private readonly IVariablesDataProvider _variablesDataProvider;
        public MainViewModel(IVariablesDataProvider variablesDataProvider)
        {
            _variablesDataProvider = variablesDataProvider;
            LoadCommand = new DelegateCommand(Load);
        }
        public DelegateCommand LoadCommand { get; }
        public VariablesViewModel SelectedVariable
        {
            get => _selectedVariable;
            set
            {
                if (_selectedVariable != value)
                {
                    _selectedVariable = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(IsVariableSelected));
                }
            } 
        }

        public bool IsVariableSelected => SelectedVariable != null;
        //potrzebujemy listy zmiennych a do tego potrzebujemy użyć konstruktor
        public void Load()
        {
            tcConnection._remoteNetId       = new AmsNetId("169.254.13.29.1.1");
            tcConnection._remoteIp          = IPAddress.Parse("192.168.58.129");
            tcConnection._remoteRouteName   = "VM";
            tcConnection._localNetId        = new AmsNetId("192.168.58.1.1.1");

            bool connectionStatus = _variablesDataProvider.SetConnection(tcConnection);
            var variables = _variablesDataProvider.GetVariables();

            Variables.Clear();
            foreach (var item in variables)
            {
                Variables.Add(new VariablesViewModel(item, _variablesDataProvider));
            }
        }
        public string Save()
        {
            Variables variable = new Variables();
            variable.Name = _selectedVariable.Name;
            variable.Value = _selectedVariable.Value;
            _variablesDataProvider.SetVatiable(variable);
            return _variablesDataProvider.GetVariableValue(variable).ToString();
        }
    }
}
