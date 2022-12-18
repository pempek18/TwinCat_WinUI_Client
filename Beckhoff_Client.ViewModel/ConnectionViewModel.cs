using Beckhoff_Client.Common.DataProvider;
using Beckhoff_Client.Common.Model;
using Beckhoff_Client.DataAccess;
using Beckhoff_Client.ViewModel.Command;
using System.Net;
using TwinCAT.Ads;

namespace Beckhoff_Client.ViewModel
{
    public class ConnectionViewModel : ViewModelBase
    {
        private readonly tcConnection _tcConnection;
        private readonly IVariablesDataProvider _variablesDataProvider;

        public ConnectionViewModel(tcConnection tcConnection, IVariablesDataProvider variablesDataProvider)
        {
            _tcConnection = tcConnection;
            _variablesDataProvider = variablesDataProvider;
            SaveCommand = new DelegateCommand(Connect, () => CanConnect);
        }

        public DelegateCommand SaveCommand { get; }

        public string RemoteIp
        {
            get => _tcConnection._remoteIp.ToString();
            set
            {
                _tcConnection._remoteIp = IPAddress.Parse( value );
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string RemoteNetId 
        { 
            get => _tcConnection._remoteNetId.ToString(); 
            set
            {
                _tcConnection._remoteNetId = AmsNetId.Parse(value);
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string RemoteRouteName
        {
            get => _tcConnection._remoteRouteName;
            set
            {
                _tcConnection._remoteRouteName = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string LocalNetId
        {
            get => _tcConnection._localNetId.ToString();
            set
            {
                _tcConnection._localNetId = AmsNetId.Parse(value);
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public bool CanConnect => !string.IsNullOrEmpty(RemoteIp) &&
                                  !string.IsNullOrEmpty(RemoteNetId) &&
                                  !string.IsNullOrEmpty(RemoteRouteName) &&
                                  !string.IsNullOrEmpty(LocalNetId);

        public void Connect()
        {
            _variablesDataProvider.SetConnection(_tcConnection);
        }
    }
}
