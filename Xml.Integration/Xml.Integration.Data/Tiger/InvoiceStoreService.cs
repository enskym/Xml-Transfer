using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityObjects;
using Xml.Integration.Data.DataAccess;
using Xml.Integration.Data.Helper;
using Xml.Integration.Data.Models;


namespace Xml.Integration.Data.Tiger
{
    public class InvoiceStoreService : App
    {

        public ResponseMessage CreateSalesInvoice(SalesInvoiceHeader Order)
        {
            try
            {
                var resp = new ResponseMessage();
                var nfi = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = ""
                };

                var cariKodu = LogoDataAccess.GetCariKoduByTax(Order.Customer.TaxID);

                if (Order.Customer.TaxID.ToControl())
                {
                    cariKodu = LogoDataAccess.GetCariKoduByTax(Order.Customer.TaxPersonalID);
                }

                if (cariKodu.ToControl())
                {
                    resp.Message = Order.Customer.TaxID + " vergi nolu logota tanımlı cari hesabı yok.";
                    return resp;
                }

                int firmano =Convert.ToInt32( "FirmNo".GetAppSetting());
                var tiger = TigerInstance(firmano);

                var now = DateTime.Now;

                object myDate = null;
                tiger.PackDate(now.Day, now.Month, now.Year, ref myDate);

                object myTime = null;
                tiger.PackTime(now.Hour, now.Minute, now.Second, ref myTime);

                UnityObjects.Data invoice = tiger.NewDataObject(UnityObjects.DataObjectType.doSalesInvoice);
                invoice.New();  

                invoice.DataFields.FieldByName("TYPE").Value =8;
                invoice.DataFields.FieldByName("NUMBER").Value = "~"; //Order.No;
                invoice.DataFields.FieldByName("DATE").Value = myDate;
                invoice.DataFields.FieldByName("TIME").Value = myTime;
                invoice.DataFields.FieldByName("DOC_NUMBER").Value = Order.No;
                invoice.DataFields.FieldByName("AUXIL_CODE").Value = "";
                invoice.DataFields.FieldByName("DOC_TRACK_NR").Value = Order.ExternalDocumentNo;
                invoice.DataFields.FieldByName("AUTH_CODE").Value = "";

                invoice.DataFields.FieldByName("ARP_CODE").Value = cariKodu;

                invoice.DataFields.FieldByName("PROJECT_CODE").Value = "Logo.CardCode".GetAppSetting();
               
                invoice.DataFields.FieldByName("NOTES1").Value = "";
                invoice.DataFields.FieldByName("NOTES6").Value = Order.OrderType;
                //invoice.DataFields.FieldByName("RC_XRATE").Value = 1;
                invoice.DataFields.FieldByName("PAYMENT_CODE").Value = "Logo.PayplanCode".GetAppSetting();

                invoice.DataFields.FieldByName("SALESMAN_CODE").Value = "";// Order.SalesPersonCode;
                //invoice.DataFields.FieldByName("CURRSEL_TOTALS").Value = 1;

                invoice.DataFields.FieldByName("INTEL_LIST").Value = 0;
                invoice.DataFields.FieldByName("DOC_DATE").Value = Order.OrderDate.TarihCevirXml();
                invoice.DataFields.FieldByName("EDURATION_TYPE").Value = "0";

                invoice.DataFields.FieldByName("EINVOICE").Value = "1";
                invoice.DataFields.FieldByName("SHIPPING_AGENT").Value = "1";
                invoice.DataFields.FieldByName("AUXIL_CODE").Value = Order.LicencePlateNo;

                if (Order.EPlatformType == "E-Fatura")
                {

                }
                else
                {
                   
                }

                Lines detay = invoice.DataFields.FieldByName("TRANSACTIONS").Lines;

                int index = 0;
                

                foreach (SalesInvoiceLineDto item in Order.SalesInvoiceLineJson )
                {
                    if(item.UnitPrice != null)
                    {
                        if((string)(item.UnitPrice) == "0.00")
                        {
                            continue;
                        }
                    }

                    if (detay.AppendLine())
                    {
                        detay[index].FieldByName("TYPE").Value = 4; // Hizmet 4 kart 0 olacak
                        detay[index].FieldByName("MASTER_CODE").Value = "Logo.CardCode".GetAppSetting(); //"Ürün Kodu";
                        detay[index].FieldByName("AUXIL_CODE").Value = item.No;
                        detay[index].FieldByName("DELVRY_CODE").Value = item.No;
                        detay[index].FieldByName("DESCRIPTION").Value = item.No +" " +  item.Description + item.Description2;

                        detay[index].FieldByName("UNIT_CODE").Value = "ADET";// item.UnitofMeasure; // "Ürün Birimi (Adet,KG,Metre gibi)";
                        detay[index].FieldByName("QUANTITY").Value = item.Quantity; // 1;  //"Sayısı";
                        detay[index].FieldByName("PRICE").Value = decimal.Parse(item.UnitPrice.Replace(".",","));

                        detay[index].FieldByName("VAT_RATE").Value = item.VAT;
                        detay[index].FieldByName("PROJECT_CODE").Value = "Logo.CardCode".GetAppSetting();

                        // detay[index].FieldByName("DUE_DATE").Value = ((string)item.ShipmentDate).TarihCevirXml();

                    }

                    index += 1;
                }

                invoice.FillAccCodes();
        
                if (invoice.Post() == true)
                {
                    resp.Status = true;
                    resp.Message = "Aktarım Başarılı.";
                    return resp;
                }
                else
                {
                    if (invoice.ErrorCode != 0)
                    {

                    }
                    else if (invoice.ValidateErrors.Count > 0)
                    {
                        string result = "XML ErrorList:";
                        for (int i = 0; i < invoice.ValidateErrors.Count; i++)
                        {
                            result += "(" + invoice.ValidateErrors[i].ID.ToString() + ") - " + invoice.ValidateErrors[i].Error;
                        }
                        resp.Message = result;
                    }

                    resp.Message = resp.Message + "Logo fiş aktarılamadı";
                    return resp;
                }

                resp.Message = "Başarılı";
                return resp;


            }
            catch (Exception e)
            {
                return new ResponseMessage
                {
                    Message = "Logo tiger lisans sorunu .Sipariş entegrasyon sırasında hata oluştu. " + e.Message,
                    Code = e.Message + " | " + e.StackTrace
                };
            }
        }
    }
}
