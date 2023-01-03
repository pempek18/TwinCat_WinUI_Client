using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.Common.Model;
using Beckhoff_Client.DataAccess;
using Beckhoff_Client.ViewModel.Command;
using TwinCAT.Ads.TypeSystem;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.ViewModel
{
    public class VariablesViewModel : ViewModelBase
    {
        private readonly Symbol _symbol;
        private readonly IVariablesDataProvider _variablesDataProvider;
        private string ValueToWrite;
        public VariablesViewModel(Symbol symbol, IVariablesDataProvider variablesDataProvider)
        {
            _symbol = symbol;
            ValueToWrite = _symbol.ReadValue().ToString();
            _variablesDataProvider = variablesDataProvider;
            SaveCommand = new DelegateCommand(Save, () => CanSave);
        }

        public string Name
        {
            get => _symbol.InstanceName;
        }
        public string Value
        {
            get => ValueToWrite;
            set
            {
                if (ValueToWrite != value)
                {
                    ValueToWrite = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CanSave));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand SaveCommand { get; }

        public bool CanSave
        {
            get
            {
                bool CanSaveTemp;
                if (_symbol.GetType().ToString() == "BOOL" )
                {
                    bool.TryParse(ValueToWrite, out CanSaveTemp);
                    return CanSaveTemp;
                } else if (_symbol.DataType.Id == ((int)TwinCAT.Ads.AdsDataTypeId.ADST_INT16) )
                {
                    return false;
                }
                else return false;
            }
        }

        public void Save()
        {
                _symbol.WriteValue(ValueToWrite);
        }
    }
}
