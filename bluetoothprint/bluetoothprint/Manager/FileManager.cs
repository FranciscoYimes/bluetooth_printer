using System;
using bluetoothprint.Interfaces;
using Xamarin.Forms;

namespace bluetoothprint.Manager
{
    public static class FileManager
    {
        public static string GetDefaultPrint()
        {
            try
            {
                var res = DependencyService.Get<ISaveAndLoad>().LoadText("QPOS.DefaultPrint.txt");
                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static void SetDefaultPrint(string defPrint)
        {
            DependencyService.Get<ISaveAndLoad>().SaveText("QPOS.DefaultPrint.txt", defPrint);
        }
    }
}
