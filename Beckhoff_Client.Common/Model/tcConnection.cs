using System.Net;
using TwinCAT.Ads;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.Common.Model
{
    public class tcConnection
    {
        public AmsNetId _remoteNetId { get; set; }
        public IPAddress _remoteIp { get; set; }
        public string _remoteRouteName { get; set; }
        public AmsNetId _localNetId { get; set; }
    }
}
