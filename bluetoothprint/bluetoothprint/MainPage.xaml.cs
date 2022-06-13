using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using bluetoothprint.Manager;
using bluetoothprint.Services;
using bluetoothprint.StaticValues;
using bluetoothprint.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace bluetoothprint
{
    public partial class MainPage : ContentPage
    {
        private IBluetoothService _blueToothService;
        private Printer _printer;
        private string selectedDevice = null;

        public MainPage(IBluetoothService initializer)
        {
            InitializeComponent();
            _blueToothService = initializer;
            _printer = new Printer(initializer);
            var list = _blueToothService.GetDeviceList();

            List<string> listadoBl = new List<string>();
            foreach (var item in list)
                listadoBl.Add(item);
            deviceList.ItemsSource = listadoBl;
            var defDisp = FileManager.GetDefaultPrint();
            if (!string.IsNullOrEmpty(defDisp))
            {
                deviceList.SelectedItem = defDisp;
                selectedDevice = defDisp;
            }

        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedDevice))
                {
                    var dte = getDte();

                    /*XmlDocument docdte = new XmlDocument();
                    var jsonDte = JsonConvert.SerializeObject(dte.EnvioBOLETA.SetDTE.DTE.Documento.TED);

                    

                    var jobj = JObject.Parse(JsonConvert.SerializeObject(dte.EnvioBOLETA.SetDTE.DTE.Documento));

                    
                    var jobjTed = new JObject();
                    jobjTed.Add("TED", jobj["TED"]);
                    var res3 = jobjTed.ToString().Replace("@", "");
                    var res2 = JsonConvert.DeserializeXmlNode(res3);

                    jobj.Remove("Detalle");
                    */
                    await _printer.PrintDte(dte);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        void deviceList_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            selectedDevice = deviceList.SelectedItem.ToString();
            _printer.MyPrinter = selectedDevice;
            FileManager.SetDefaultPrint(selectedDevice);
        }

        private void BindDeviceList()
        {
            var list = _blueToothService.GetDeviceList();
            deviceList.ItemsSource.Clear();
            foreach (var item in list)
                deviceList.ItemsSource.Add(item);
        }

        private Dte getDte()
        {
            string jsonFileName = "39_BOLETA_AFECTA.xml";

            if(boletaE.IsChecked) jsonFileName = "41_BOLETA_EXENTA.xml";
            else if (facturaA.IsChecked) jsonFileName = "33_FACTURA_AFECTA.xml";
            else if (facturaE.IsChecked) jsonFileName = "34_FACTURA_EXENTA.xml";

            var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{jsonFileName}");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var xml = reader.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                FindTEDXML(doc);


                var jsonString = JsonConvert.SerializeObject(doc);

                

                if (jsonString != null)
                {
                    return JsonConvert.DeserializeObject<Dte>(jsonString);
                }

                

                //Converting JSON Array Objects into generic list    
                //ObjContactList = JsonConvert.DeserializeObject<ContactList>(jsonString);
            }

            return null;
        }

        private void FindTEDXML(XmlDocument doc)
        {
            var root = doc.DocumentElement;
            var res = root.ChildNodes;
            var res2 = res[0].ChildNodes[1].ChildNodes[0].ChildNodes;
            //doc.LoadXml(res2.InnerXml);

            

        }
    }
}
