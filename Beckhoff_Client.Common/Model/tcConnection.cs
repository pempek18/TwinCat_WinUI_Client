using System.Net;
using TwinCAT.Ads;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.Common.Model
{
    public class tcConnection
    {
        public string _remoteRouteName;
        public AmsNetId _localNetId;
        public IPAddress _remoteIp;
        public AmsNetId _remoteNetId;
        private string _user;
        private string _password;

        public tcConnection()
        {
            if (string.IsNullOrEmpty(_remoteRouteName))
            {
                _remoteRouteName = "VM";
            }
            if (_localNetId == null)
            {
                _localNetId = new AmsNetId("192.168.58.1.1.1");
            }
            if (_remoteIp == null)
            {
                _remoteIp = IPAddress.Parse("192.168.58.129");
            }
            if (_remoteNetId == null)
            {
                _remoteNetId = new AmsNetId("169.254.13.29.1.1");
            }
            if (string.IsNullOrEmpty(_user))
            {
                _user = "Administrator";
            }
            if (string.IsNullOrEmpty(_password))
            {
                _password = "1";
            }

        }

        public string remoteRouteName
        {
            get => _remoteRouteName;
            set 
            { 
                _remoteRouteName = value; 
            }
        }
        public string localNetId 
        { 
            get => _localNetId.ToString();
            set
            {
               _localNetId = new AmsNetId(value);
            }
        }
        public string remoteIp
        {
            get => _remoteIp.ToString();
            set
            {
                _remoteIp = IPAddress.Parse(value);
            }
        }
        public string remoteNetId
        {
            get => _remoteNetId.ToString();
            set
            {
                _remoteNetId =  new AmsNetId(value);
            }
        }

        public string User { get => _user; set => _user = value; }
        public string Password { get => _password; set => _password = value; }
    }
}
