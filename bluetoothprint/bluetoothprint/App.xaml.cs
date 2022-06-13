using System;
using bluetoothprint.Services;
using Prism;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bluetoothprint
{
    public partial class App : Application
    {
        public App(IBluetoothService initializer)
        {
            InitializeComponent();
            MainPage = new MainPage(initializer);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
