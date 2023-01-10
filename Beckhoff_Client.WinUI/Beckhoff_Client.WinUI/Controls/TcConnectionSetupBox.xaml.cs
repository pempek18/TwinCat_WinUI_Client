using Beckhoff_Client.Common.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.WinUI.Controls
{
    [ContentProperty(Name = nameof(tcConn))]
    public sealed partial class TcConnectionSetupBox : UserControl
    {
        public TcConnectionSetupBox()
        {
            this.InitializeComponent();
        }



        public tcConnection tcConn
        {
            get { return (tcConnection)GetValue(tcConnProp); }
            set { SetValue(tcConnProp, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tcConnProp =
            DependencyProperty.Register("tcConn", typeof(tcConnection), typeof(TcConnectionSetupBox), new PropertyMetadata(null));


    }
}
