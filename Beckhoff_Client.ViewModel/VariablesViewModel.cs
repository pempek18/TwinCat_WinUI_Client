using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.Common.Model;
using Beckhoff_Client.DataAccess;
using Beckhoff_Client.ViewModel.Command;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.ViewModel
{
    public class VariablesViewModel : ViewModelBase
    {
        private readonly Variables _variables;
        private readonly IVariablesDataProvider _variablesDataProvider;

        public VariablesViewModel(Variables variables, IVariablesDataProvider variablesDataProvider)
        {
            _variables = variables;
            _variablesDataProvider = variablesDataProvider;
            SaveCommand = new DelegateCommand(ReadVariables, () => CanRead);
        }

        public string Name 
        { 
            get => _variables.Name; 
        }
        public string Value
        {
            get => _variables.Value;
            set
            {
                _variables.Value = value;
            }
        }

        public DelegateCommand SaveCommand { get; }

        public bool CanRead => true;

        public void ReadVariables()
        {

        }
    }
}
