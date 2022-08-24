using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml.Integration.Data.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CustLedgerEntry
    {
        public string EntryNo { get; set; }
        public string CustomerNo { get; set; }
        public string PostingDate { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public object CurrencyCode { get; set; }
        public string Amount { get; set; }
        public string OriginalAmountLCY { get; set; }
        public string AmountLCY { get; set; }
        public string SelltoCustomerNo { get; set; }
        public string DueDate { get; set; }
        public string DebitAmountLCY { get; set; }
        public string CreditAmountLCY { get; set; }
        public string DocumentDate { get; set; }
        public object ExternalDocumentNo { get; set; }
        public string AdjustedCurrencyFactor { get; set; }
        public string OriginalCurrencyFactor { get; set; }
        public string OriginalAmount { get; set; }
        public string Reversed { get; set; }
        public string ReversedbyEntryNo { get; set; }
        public string ReversedEntryNo { get; set; }
        public object PaymentMethodCode { get; set; }
        public object LicencePlateNo { get; set; }
        public string SelltoContactNo { get; set; }
        public string BilltoContractNo { get; set; }
        public string Name { get; set; }
        public object Name2 { get; set; }
        public string VoucherNo { get; set; }
    }

    public class CustomerDto
    {
        public string TaxArea { get; set; }
        public string TaxID { get; set; }
        public string TaxPersonalID { get; set; }
    }

    public class RootDto
    {
        [JsonProperty("?xml")]
        public XmlDto Xml { get; set; }
        public Root2 Root { get; set; }
    }

    public class Root2
    {
        public List<SalesInvoiceHeader> SalesInvoiceHeader { get; set; }
        public List<CustLedgerEntry> CustLedgerEntry { get; set; }
    }

    public class SalesInvoiceHeader
    {
        public string SeltoCustomerNo { get; set; }
        public string No { get; set; }
        public string BilltoCustomerNo { get; set; }
        public string BilltoName { get; set; }
        public object BilltoName2 { get; set; }
        public string BilltoAdress { get; set; }
        public string BiltoAdress2 { get; set; }
        public string BilltoCity { get; set; }
        public object BilltoContract { get; set; }
        public object YourReference { get; set; }
        public string ShiptoName { get; set; }
        public object ShiptoName2 { get; set; }
        public string ShiptoAdress { get; set; }
        public string ShiptoAdress2 { get; set; }
        public string ShiptoCity { get; set; }
        public string OrderDate { get; set; }
        public string PostingDate { get; set; }
        public string ShipmentDate { get; set; }
        public string DueDate { get; set; }
        public object CurrecyCode { get; set; }
        public string CurrencyFactor { get; set; }
        public string PricesIncludingVAT { get; set; }
        public string SalesPersonCode { get; set; }
        public string Amount { get; set; }
        public string AmountIncludingVAT { get; set; }
        public string SelltoCustomerName { get; set; }
        public object SelltoCustomerName2 { get; set; }
        public string SelltoAdress { get; set; }
        public string SelltoAdress2 { get; set; }
        public string SelltoCity { get; set; }
        public object SelltoContract { get; set; }
        public string BilltoPostCode { get; set; }
        public string BilltoCounty { get; set; }
        public string BilltoCountryRegionCode { get; set; }
        public string SelltoPostCode { get; set; }
        public string SelltoCountry { get; set; }
        public string SelltoCountryRegionCode { get; set; }
        public string ShiptoPostCode { get; set; }
        public string ShiptoCounty { get; set; }
        public string ShiptoCountryRegionCode { get; set; }
        public string DocumentDate { get; set; }
        public string ExternalDocumentNo { get; set; }
        public string PaymentMethodCode { get; set; }
        public string TaxAreaCode { get; set; }
        public string VATBusPostingGroup { get; set; }
        public string SelltoContractNo { get; set; }
        public string BilltoContractNo { get; set; }
        public string OrderType { get; set; }
        public object RepresentativeCode { get; set; }
        public object Text1 { get; set; }
        public object Text2 { get; set; }
        public object Text3 { get; set; }
        public object Text4 { get; set; }
        public object Text5 { get; set; }
        public object Text6 { get; set; }
        public string Text7 { get; set; }
        public string VehicleNo { get; set; }
        public string LicencePlateNo { get; set; }
        public string VehicleManufacturer { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleVariant { get; set; }
        public string VehicleIDNo { get; set; }
        public object LicencedFrom { get; set; }
        public object FirstLicenceDate { get; set; }
        public object NextGeneralInspection { get; set; }
        public string MileageThisVisit { get; set; }
        public string ServiceCenter { get; set; }
        public string FaxNo { get; set; }
        public string EMail { get; set; }
        public string PhoneNo { get; set; }
        public string MobilePhoneNo { get; set; }
        public string Reverse { get; set; }
        public string EPlatformType { get; set; }
        public CustomerDto Customer { get; set; }
        public dynamic SalesInvoiceLine { get; set; }
        public List<SalesInvoiceLineDto> SalesInvoiceLineJson { get; set; }
    }
    public class SalesInvoiceLineDto
    {
        public string DocumentNo { get; set; }
        public string LineNo { get; set; }
        public string No { get; set; }
        public string LocationCode { get; set; }
        public string ShipmentDate { get; set; }
        public string VAT { get; set; }
        public string Quantity { get; set; }
        public string UnitofMeasure { get; set; }
        public string UnitPrice { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }

    }
    public class XmlDto
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }

        [JsonProperty("@standalone")]
        public string Standalone { get; set; }
    }


}
