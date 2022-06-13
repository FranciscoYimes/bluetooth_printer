using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Android.Bluetooth;
using Android.Graphics;
using bluetoothprint.Droid.Services;
using bluetoothprint.Services;
using bluetoothprint.StaticValues;
using bluetoothprint.Structure;
using Java.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(BluetoothService))]
namespace bluetoothprint.Droid.Services
{
    public class BluetoothService : IBluetoothService
    {

        private BluetoothSocket bluetoothSocket;

        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return btdevice;
            }
        }

        public async Task Print(string deviceName, byte[] buffer)
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                      where bd?.Name == deviceName
                                      select bd).FirstOrDefault();

            try
            {
                var btSocket = device?.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
                await btSocket.ConnectAsync();
                await btSocket.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                btSocket.Close();

                /*using (BluetoothSocket bluetoothSocket = device?.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                {
                    bluetoothSocket?.Connect();
                    //byte[] buffer = Encoding.UTF8.GetBytes(text);
                    bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                    bluetoothSocket.Close();
                }*/
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public async Task getDeviceInfo(string deviceName)
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                      where bd?.Name == deviceName
                                      select bd).FirstOrDefault();
        }

        public async Task Print(string deviceName, Dte dte)
        {
            BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                      where bd?.Name == deviceName
                                      select bd).FirstOrDefault();

            try
            {
                bluetoothSocket = device?.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
                await bluetoothSocket.ConnectAsync();

                if (dte.EnvioBOLETA != null) await PrintBoleta(dte);
                else if (dte.EnvioDTE != null) await PrintFactura(dte);
                else throw new Exception("No DTE data");

                bluetoothSocket.Close();

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private async Task PrintFactura(Dte dte)
        {
            await SetAlignCenter();
            await WriteLine_Bigger("----------------------------------------", 1);
            await WriteLine_Bigger("FACTURA ELECTRONICA", 1);
            await WriteLine_Bigger(dte.EnvioDTE.SetDTE.Caratula.RutEmisor, 1);
            await WriteLine_Bigger("Nº " + dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.IdDoc.Folio, 1);
            await WriteLine_Bigger("----------------------------------------", 1);
            await WriteLine("S.I.I. SANTIAGO");
            await WriteLine_Bigger("\n", 1);

            await Reset();

            await WriteLine_Bold("RAZON SOCIAL EMISOR");
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Emisor.RUTEmisor);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Emisor.RznSoc);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Emisor.GiroEmis);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Emisor.DirOrigen);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Emisor.CmnaOrigen);
            await WriteLine("\n");

            await WriteLine_Bold("RAZON SOCIAL RECEPTOR");
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Receptor.RUTRecep);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Receptor.RznSocRecep);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Receptor.GiroRecep);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Receptor.DirRecep);
            await WriteLine(dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Receptor.CmnaRecep);
            await WriteLine("\n");
            await WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            await WriteLine("\n");
            await Reset();

            await SetAlignCenter();

            await WriteLine("----------------------------------------");

            if (int.Parse(dte.EnvioDTE.SetDTE.Caratula.SubTotDTE.TpoDTE) == (int)TipoDte.FACTURA_AFECTA)
            {
                await WriteLine(String.Format("{0,13} {1,27}", "TOTAL NETO :", "$" + dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Totales.MntNeto));
                await WriteLine(String.Format("{0,13} {1,27}", "IVA :", "$" + dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Totales.IVA));
            }

            await WriteLine_Bigger(String.Format("{0,13} {1,27}", "TOTAL :", "$" + dte.EnvioDTE.SetDTE.DTE.Documento.Encabezado.Totales.MntTotal), 3);
            //await WriteLine_Bigger("Total:     " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntTotal,3);
            await WriteLine("\n");
            //await WriteBytes(BarcodeGenerator.generatePDF147(getDteXML(dte)));
        }

        private async Task PrintBoleta( Dte dte)
        {
            await SetAlignCenter();
            await WriteLine_Bigger("----------------------------------------", 1);
            await WriteLine_Bigger("BOLETA ELECTRONICA", 1);
            await WriteLine_Bigger(dte.EnvioBOLETA.SetDTE.Caratula.RutEmisor, 1);
            await WriteLine_Bigger("Nº " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.IdDoc.Folio, 1);
            await WriteLine_Bigger("----------------------------------------", 1);
            await WriteLine("S.I.I. SANTIAGO");
            await WriteLine_Bigger("\n", 1);
            
            await Reset();
            await SetAlignCenter();
            await WriteLine(dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Emisor.RznSocEmisor);
            await WriteLine(dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Emisor.GiroEmisor);
            await WriteLine(dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Emisor.DirOrigen);
            await WriteLine("Sucursal Nº " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Emisor.CdgSIISucur);
            await WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            await WriteLine("\n");
            await Reset();

            await SetAlignCenter();

            await WriteLine("----------------------------------------");

            if (int.Parse(dte.EnvioBOLETA.SetDTE.Caratula.SubTotDTE.TpoDTE) == (int)TipoDte.BOLETA_AFECTA)
            {
                //String.Format("{0,13} {1,27}", "TOTAL NETO :", "$" + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntNeto);
                await WriteLine(String.Format("{0,13} {1,27}", "TOTAL NETO :", "$" + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntNeto));
                await WriteLine(String.Format("{0,13} {1,27}", "IVA :", "$" + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.IVA));
                //await WriteLine("Total Neto:    " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntNeto);
                //await WriteLine("Total IVA:     " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.IVA);
            }

            await WriteLine_Bigger(String.Format("{0,13} {1,27}", "TOTAL :", "$" + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntTotal),3);
            //await WriteLine_Bigger("Total:     " + dte.EnvioBOLETA.SetDTE.DTE.Documento.Encabezado.Totales.MntTotal,3);
            await WriteLine("\n");
            //await WriteBytes(BarcodeGenerator.generatePDF147(getDteXML(dte)));
        }

        private String getDteXML(Dte dte)
        {
            XmlDocument docdte = new XmlDocument();
            var jsonDte = JsonConvert.SerializeObject(dte.EnvioBOLETA.SetDTE.DTE.Documento.TED);
            var jobj = JObject.Parse(JsonConvert.SerializeObject(dte.EnvioBOLETA.SetDTE.DTE.Documento));
            var jobjTed = new JObject();
            jobjTed.Add("TED", jobj["TED"]);
            var res3 = jobjTed.ToString().Replace("@", "");
            var res2 = JsonConvert.DeserializeXmlNode(res3);
            return res2.InnerXml;
        }

        public async Task WriteLine_Bigger(string text, byte n)
        {
            //big on
            await WriteByte(29);
            await WriteByte(33);
            await WriteByte(n);

            //Sends the text
            await WriteLine(text);

            //big off
            await WriteByte(29);
            await WriteByte(33);
            await WriteByte(0);
        }

        private async Task WriteLine_Big(string text)
        {
            const byte DoubleHeight = 1 << 4;
            const byte DoubleWidth = 1 << 5;
            //const byte Bold = 1 << 3;

            await WriteByte(27);
            await WriteByte(33);
            await WriteByte(DoubleHeight + DoubleWidth);

            //Sends the text
            await WriteLine(text);

            //big off
            await WriteByte(27);
            await WriteByte(33);
            await WriteByte(0);
        }

        private async Task WriteByte(byte textByte)
        {
            var bytes = new byte[] { textByte };
            await bluetoothSocket.OutputStream.WriteAsync(bytes, 0, bytes.Length);
        }

        private async Task WriteBytes(byte[] textBytes)
        {
            await bluetoothSocket.OutputStream.WriteAsync(textBytes, 0, textBytes.Length);
        }

        public async Task WriteToBuffer(string text)
        {
            text = text.Trim('\n').Trim('\r');
            byte[] originalBytes = System.Text.Encoding.UTF8.GetBytes(text);
            await WriteBytes(originalBytes);
        }

        public async Task WriteLine(string text)
        {
            await WriteToBuffer(text);
            await WriteByte(10);
        }

        public async Task Reset()
        {
            await WriteByte(27);
            await WriteByte(64);
        }

        public async Task LineFeed()
        {
            await WriteByte(10);
        }

        public async Task LineFeed(byte lines)
        {
            await WriteByte(27);
            await WriteByte(100);
            await WriteByte(lines);
        }

        public async Task WriteLine_Bold(string text)
        {
            //bold on
            await BoldOn();

            //Sends the text
            await WriteLine(text);

            //bold off
            await BoldOff();

            await LineFeed();
        }
        public async Task BoldOn()
        {
            await WriteByte(27);
            await WriteByte(32);
            await WriteByte(1);
            await WriteByte(27);
            await WriteByte(69);
            await WriteByte(1);
        }

        public async Task BoldOff()
        {
            await WriteByte(27);
            await WriteByte(32);
            await WriteByte(0);
            await WriteByte(27);
            await WriteByte(69);
            await WriteByte(0);
        }
        public async Task SetAlignLeft()
        {
            await WriteByte(27);
            await WriteByte(97);
            await WriteByte(0);
        }

        public async Task SetAlignCenter()
        {
            await WriteByte(27);
            await WriteByte(97);
            await WriteByte(1);
        }

        public async Task SetAlignRight()
        {
            await WriteByte(27);
            await WriteByte(97);
            await WriteByte(2);
        }

        public async Task SetUnderLine(string text)
        {
            //underline on
            await WriteByte(27);
            await WriteByte(45);
            await WriteByte(1);

            //Sends the text
            await WriteLine(text);

            //underline off
            await WriteByte(27);
            await WriteByte(45);
            await WriteByte(0);
        }
        public async Task SetUnderLineOn()
        {
            //underline on
            await WriteByte(27);
            await WriteByte(45);
            await WriteByte(1);
        }
        public async Task SetUnderLineOff()
        {
            //underline off
            await WriteByte(27);
            await WriteByte(45);
            await WriteByte(0);
        }
    }
}
