using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Beckhoff_Client.WinUI.Controls
{
    public sealed partial class FB_STD_INT1 : UserControl
    {
        public FB_STD_INT1()
        {
            this.InitializeComponent();
        }

        public bool Sensor_0 { get; set; }
        public bool Sensor_1 { get; set; }
        public bool Sensor_2 { get; set; }
        public bool Sensor_3 { get; set; }
        public bool TestDrive { get; set; }
        public string FunName { get; set; }

        private void BtnTestDrive_Clicked(object sender, RoutedEventArgs e)
        {
            if (TestDrive)
            { 
                TestDrive = false;
                BtnTestDrive.Background = 
            }
        }
    }
}
