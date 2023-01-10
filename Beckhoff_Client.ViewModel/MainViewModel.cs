using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.Common.Model;
using Beckhoff_Client.ViewModel.Command;
using System.Collections.ObjectModel;
using System.Net;
using TwinCAT.Ads;
using TwinCAT.Ads.TypeSystem;

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
        public tcConnection tcConnection = new tcConnection();
        private readonly IVariablesDataProvider _variablesDataProvider;
        public bool connectionStatus = false;
        
        public MainViewModel(IVariablesDataProvider variablesDataProvider)
        {
            _variablesDataProvider = variablesDataProvider;
            LoadCommand = new DelegateCommand(Load);
        }
        public DelegateCommand LoadCommand { get; }
        public VariablesViewModel SelectedVariable /* Służy tylko do wpisywania w pole tekstowe wartości zmiennej */
        {
            get => _selectedVariable;
            set
            {
                if (_selectedVariable != value && value != null)
                {
                    _selectedVariable = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(IsVariableSelected));
                    _selectedVariable.Load();
                }
            } 
        }

        public bool IsVariableSelected => SelectedVariable != null;
        //potrzebujemy listy zmiennych a do tego potrzebujemy użyć konstruktor
        public void Load()
        {         
            connectionStatus = _variablesDataProvider.SetConnection(tcConnection);

            var variables = _variablesDataProvider.GetVariables();

            Variables.Clear();
            foreach (var item in variables)
            {
                Variables.Add(new VariablesViewModel(item, _variablesDataProvider));
            }
        }
    }
}
