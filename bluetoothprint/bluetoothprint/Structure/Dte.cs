using System;
using Newtonsoft.Json;

namespace bluetoothprint.Structure
{
    public class Dte
    {
        public EnvioDTE EnvioDTE { get; set; }
        public EnvioDTE EnvioBOLETA { get; set; }
        public Dte()
        {
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CAF
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
        public DA DA { get; set; }
        public FRMA FRMA { get; set; }
    }

    public class CanonicalizationMethod
    {
        [JsonProperty("@Algorithm")]
        public string Algorithm { get; set; }
    }

    public class Caratula
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
        public string RutEmisor { get; set; }
        public string RutEnvia { get; set; }
        public string RutReceptor { get; set; }
        public string FchResol { get; set; }
        public string NroResol { get; set; }
        public DateTime TmstFirmaEnv { get; set; }
        public SubTotDTE SubTotDTE { get; set; }
    }

    public class DA
    {
        public string RE { get; set; }
        public string RS { get; set; }
        public string TD { get; set; }
        public RNG RNG { get; set; }
        public string FA { get; set; }
        public RSAPK RSAPK { get; set; }
        public string IDK { get; set; }
    }

    public class DD
    {
        public string RE { get; set; }
        public string TD { get; set; }
        public string F { get; set; }
        public string FE { get; set; }
        public string RR { get; set; }
        public string RSR { get; set; }
        public string MNT { get; set; }
        public string IT1 { get; set; }
        public CAF CAF { get; set; }
        public DateTime TSTED { get; set; }
    }

    public class Detalle
    {
        public string NroLinDet { get; set; }
        public string NmbItem { get; set; }
        public string QtyItem { get; set; }
        public string UnmdItem { get; set; }
        public string PrcItem { get; set; }
        public string MontoItem { get; set; }
    }

    public class DigestMethod
    {
        [JsonProperty("@Algorithm")]
        public string Algorithm { get; set; }
    }

    public class Documento
    {
        [JsonProperty("@ID")]
        public string ID { get; set; }
        public Encabezado Encabezado { get; set; }
        public Detalle Detalle { get; set; }
        public TED TED { get; set; }
        public DateTime TmstFirma { get; set; }
    }

    public class DTE
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public Documento Documento { get; set; }
        public Signature Signature { get; set; }
    }

    public class Emisor
    {
        public string RUTEmisor { get; set; }
        public string RznSoc { get; set; }
        public string RznSocEmisor { get; set; }
        public string GiroEmisor { get; set; }
        public string GiroEmis { get; set; }
        public string Acteco { get; set; }
        public string DirOrigen { get; set; }
        public string CmnaOrigen { get; set; }
        public string CdgSIISucur { get; set; }
    }

    public class Encabezado
    {
        public IdDoc IdDoc { get; set; }
        public Emisor Emisor { get; set; }
        public Receptor Receptor { get; set; }
        public Totales Totales { get; set; }
    }

    public class EnvioDTE
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string XmlnsXsi { get; set; }

        [JsonProperty("@xsi:schemaLocation")]
        public string XsiSchemaLocation { get; set; }
        public SetDTE SetDTE { get; set; }
        public Signature Signature { get; set; }
    }

    public class FRMA
    {
        [JsonProperty("@algoritmo")]
        public string Algoritmo { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class FRMT
    {
        [JsonProperty("@algoritmo")]
        public string Algoritmo { get; set; }

        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class IdDoc
    {
        public string TipoDTE { get; set; }
        public string Folio { get; set; }
        public string FchEmis { get; set; }
        public string MntBruto { get; set; }
        public string TpoTranCompra { get; set; }
        public string TpoTranVenta { get; set; }
        public string FmaPago { get; set; }
        public string MedioPago { get; set; }
    }

    public class KeyInfo
    {
        public KeyValue KeyValue { get; set; }
        public X509Data X509Data { get; set; }
    }

    public class KeyValue
    {
        public RSAKeyValue RSAKeyValue { get; set; }
    }

    public class Receptor
    {
        public string RUTRecep { get; set; }
        public string RznSocRecep { get; set; }
        public string GiroRecep { get; set; }
        public string DirRecep { get; set; }
        public string CmnaRecep { get; set; }
    }

    public class Reference
    {
        [JsonProperty("@URI")]
        public string URI { get; set; }
        public DigestMethod DigestMethod { get; set; }
        public string DigestValue { get; set; }
    }

    public class RNG
    {
        public string D { get; set; }
        public string H { get; set; }
    }

    public class RSAKeyValue
    {
        public string Modulus { get; set; }
        public string Exponent { get; set; }
    }

    public class RSAPK
    {
        public string M { get; set; }
        public string E { get; set; }
    }

    public class SetDTE
    {
        [JsonProperty("@ID")]
        public string ID { get; set; }
        public Caratula Caratula { get; set; }
        public DTE DTE { get; set; }
    }

    public class Signature
    {
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public SignedInfo SignedInfo { get; set; }
        public string SignatureValue { get; set; }
        public KeyInfo KeyInfo { get; set; }
    }

    public class SignatureMethod
    {
        [JsonProperty("@Algorithm")]
        public string Algorithm { get; set; }
    }

    public class SignedInfo
    {
        public CanonicalizationMethod CanonicalizationMethod { get; set; }
        public SignatureMethod SignatureMethod { get; set; }
        public Reference Reference { get; set; }
    }

    public class SubTotDTE
    {
        public string TpoDTE { get; set; }
        public string NroDTE { get; set; }
    }

    public class TED
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
        public DD DD { get; set; }
        public FRMT FRMT { get; set; }
    }

    public class Totales
    {
        public string MntNeto { get; set; }
        public string MntExe { get; set; }
        public string TasaIVA { get; set; }
        public string IVA { get; set; }
        public string MntTotal { get; set; }
    }

    public class X509Data
    {
        public string X509Certificate { get; set; }
    }


}
