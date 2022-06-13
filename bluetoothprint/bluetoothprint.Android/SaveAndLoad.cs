using System;
using System.Diagnostics;
using System.IO;
using bluetoothprint.Droid;
using bluetoothprint.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad))]
namespace bluetoothprint.Droid
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public string LoadText(string filename)
        {
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                return System.IO.File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }
        }

        public void SaveText(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.WriteAllText(filePath, text);
        }
    }
}
