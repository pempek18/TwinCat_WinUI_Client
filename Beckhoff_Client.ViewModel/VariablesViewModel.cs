using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.ViewModel.Command;
using System;
using System.Collections.ObjectModel;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.ViewModel
{
    public class VariablesViewModel : ViewModelBase
    {
        private readonly Symbol _symbol;
        private readonly IVariablesDataProvider _variablesDataProvider;
        private string _Value = null;
        private Int32 _SelectedIndex = -1;
        public ObservableCollection<String> Values { get; } = new();
        bool changeValue = false;
        bool changeValueRadioButton;
        private bool _radioButtonTrue;

        public VariablesViewModel(Symbol symbol, IVariablesDataProvider variablesDataProvider)
        {
            _symbol = symbol;
            _variablesDataProvider = variablesDataProvider;
            SaveCommand = new DelegateCommand(Save, () => CanSave);
            LoadCommand = new DelegateCommand(Load, () => IsValueSelected);
        }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand LoadCommand { get; }

        public string Name
        {
            get => _symbol.InstanceName;
        }
        public string Type
        {
            get => _symbol.TypeName.ToString();
        }
        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (changeValue)
                {
                    value = _Value;
                    changeValue = false;
                    RaisePropertyChanged();
                }
                if (_Value != value)
                {
                    _Value = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CanSave));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public bool IsNumber
        {
            get
            {
                try
                {
                    Int32.Parse(_Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool IsBoolean
        {
            get
            {
                try
                {
                    bool.Parse(_Value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool RadioButtonFalseChecked
        {
            get => !_radioButtonTrue;
            set
            {
                if (changeValueRadioButton)
                {
                    value = !_radioButtonTrue;
                    changeValueRadioButton = false;
                }
            }
        }
        public bool RadioButtonTrueChecked
        {
            get => _radioButtonTrue;
            set
            {
                if (changeValueRadioButton)
                {
                    value = _radioButtonTrue;
                    changeValueRadioButton = false;
                }
                if (_radioButtonTrue != value)
                {
                    _radioButtonTrue = value;
                    _Value = value.ToString();
                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(CanSave));
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }
        public string SelectedValue
        {
            get
            {
                Value = _Value;
                return _Value;
            }
            set
            {
                _Value = value;
                changeValue = true;
                Value = _Value;
                RaisePropertyChanged(nameof(Value));
                RaisePropertyChanged(nameof(IsValueSelected));
                /*Radio buttons*/
                RaisePropertyChanged(nameof(IsBoolean));
                if (IsBoolean)
                {
                    _radioButtonTrue = bool.Parse(_Value);
                    if (_radioButtonTrue)
                    {
                        changeValueRadioButton = true;
                        RadioButtonTrueChecked = true;
                        RaisePropertyChanged(nameof(RadioButtonTrueChecked));
                    }
                    else
                    {
                        changeValueRadioButton = true;
                        RadioButtonFalseChecked = true;
                        RaisePropertyChanged(nameof(RadioButtonFalseChecked));
                    }
                }
                else
                {
                    RaisePropertyChanged(nameof(IsNumber));
                }
            }
        }
        public bool IsValueSelected => !string.IsNullOrEmpty(SelectedValue);

        public Int32 SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
            }
        }

        public bool CanSave
        {
            get
            {
                if (IsBoolean)
                {
                    try
                    {
                        bool.Parse(_Value);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                else if (IsNumber)
                {
                    try
                    {
                        Int32.Parse(_Value);
                        return true;
                    }
                    catch
                    {

                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void Save()
        {
            switch (_symbol.Category)
            {
                case TwinCAT.TypeSystem.DataTypeCategory.None:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Primitive:
                    _symbol.WriteValue(_Value);
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Alias:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Enum:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Array:
                    bool[] tab = new bool[_symbol.Size];
                    for (int i = 0; i < tab.Length; i++)
                    {
                        bool.TryParse(Values[i], out tab[i]);
                    }
                    bool.TryParse(_Value, out tab[_SelectedIndex]);
                    _symbol.WriteValue(tab);
                    Values[_SelectedIndex] = _Value;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Struct:

                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.FunctionBlock:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Program:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Function:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.SubRange:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.String:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Bitset:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Pointer:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Union:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Reference:
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Interface:
                    break;
                default:
                    break;
            }
            Load();
        }

        public void Load()
        {
            Values.Clear();
            switch (_symbol.Category)
            {
                case TwinCAT.TypeSystem.DataTypeCategory.None:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Primitive:
                    if (_Value == null)
                        _Value = _symbol.ReadValue().ToString();
                    Values.Add(_Value);
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Alias:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Enum:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Array:
                    if (_Value == null)
                        _Value = _symbol.ReadValue().ToString();
                    if (_Value == "System.Boolean[]")
                    {
                        bool[] tab = new bool[_symbol.Size];
                        tab = (bool[])_symbol.ReadAnyValue(typeof(bool[]));
                        foreach (var item in tab)
                        {
                            Values.Add(item.ToString());
                        }
                        _Value = Values[0];
                    }
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Struct:
                    _Value = _variablesDataProvider.GetSubSymbolsValue(_symbol, "TestDrive");
                    Values.Add(_Value);
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.FunctionBlock:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Program:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Function:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.SubRange:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.String:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Bitset:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Pointer:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Union:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Reference:
                    _Value = null;
                    break;
                case TwinCAT.TypeSystem.DataTypeCategory.Interface:
                    _Value = null;
                    break;
            }
        }
    }
}
