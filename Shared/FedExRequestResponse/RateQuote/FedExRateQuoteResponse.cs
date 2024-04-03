using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared.FedExRequestResponse.RateQuote
{
    public partial class FedExRateQuoteResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("output")]
        public Output Output { get; set; }
    }

    public partial class Output
    {
        [JsonIgnore]
        [JsonProperty("alerts")]
        public List<Alert> Alerts { get; set; }

        [JsonProperty("rateReplyDetails")]
        public List<RateReplyDetail> RateReplyDetails { get; set; }

        [JsonIgnore]
        [JsonProperty("quoteDate")]
        public DateTimeOffset QuoteDate { get; set; }

        [JsonIgnore]
        [JsonProperty("encoded")]
        public bool Encoded { get; set; }
    }

    public partial class Alert
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("alertType")]
        public string AlertType { get; set; }
    }

    public partial class RateReplyDetail
    {
        [JsonProperty("serviceType")]
        public string ServiceType { get; set; }

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        [JsonProperty("packagingType")]
        public string PackagingType { get; set; }

        [JsonProperty("ratedShipmentDetails")]
        public List<RatedShipmentDetail> RatedShipmentDetails { get; set; }

        [JsonIgnore]
        [JsonProperty("operationalDetail")]
        public OperationalDetail OperationalDetail { get; set; }

        [JsonIgnore]
        [JsonProperty("signatureOptionType")]
        public string SignatureOptionType { get; set; }

        [JsonIgnore]
        [JsonProperty("serviceDescription")]
        public ServiceDescription ServiceDescription { get; set; }
    }

    public partial class OperationalDetail
    {
        [JsonProperty("ineligibleForMoneyBackGuarantee")]
        public bool IneligibleForMoneyBackGuarantee { get; set; }

        [JsonProperty("astraDescription")]
        public string AstraDescription { get; set; }

        [JsonProperty("airportId")]
        public string AirportId { get; set; }

        [JsonProperty("serviceCode")]
        public int ServiceCode { get; set; }
    }

    public partial class RatedShipmentDetail
    {
        [JsonProperty("rateType")]
        public string RateType { get; set; }

        [JsonProperty("ratedWeightMethod")]
        public string RatedWeightMethod { get; set; }

        [JsonProperty("totalDiscounts")]
        public double TotalDiscounts { get; set; }

        [JsonProperty("totalBaseCharge")]
        public double TotalBaseCharge { get; set; }

        [JsonProperty("totalNetCharge")]
        public double TotalNetCharge { get; set; }

        [JsonProperty("totalNetFedExCharge")]
        public double TotalNetFedExCharge { get; set; }

        [JsonIgnore]
        [JsonProperty("shipmentRateDetail")]
        public ShipmentRateDetail ShipmentRateDetail { get; set; }

        [JsonIgnore]
        [JsonProperty("ratedPackages")]
        public List<RatedPackage> RatedPackages { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class RatedPackage
    {
        [JsonProperty("groupNumber")]
        public int GroupNumber { get; set; }

        [JsonProperty("effectiveNetDiscount")]
        public double EffectiveNetDiscount { get; set; }

        [JsonProperty("packageRateDetail")]
        public PackageRateDetail PackageRateDetail { get; set; }

        [JsonProperty("sequenceNumber")]
        public int SequenceNumber { get; set; }
    }

    public partial class PackageRateDetail
    {
        [JsonProperty("rateType")]
        public string RateType { get; set; }

        [JsonProperty("ratedWeightMethod")]
        public string RatedWeightMethod { get; set; }

        [JsonProperty("baseCharge")]
        public double BaseCharge { get; set; }

        [JsonProperty("netFreight")]
        public double NetFreight { get; set; }

        [JsonProperty("totalSurcharges")]
        public double TotalSurcharges { get; set; }

        [JsonProperty("netFedExCharge")]
        public double NetFedExCharge { get; set; }

        [JsonProperty("totalTaxes")]
        public double TotalTaxes { get; set; }

        [JsonProperty("netCharge")]
        public double NetCharge { get; set; }

        [JsonProperty("totalRebates")]
        public double TotalRebates { get; set; }

        [JsonProperty("billingWeight")]
        public BillingWeight BillingWeight { get; set; }

        [JsonProperty("totalFreightDiscounts")]
        public double TotalFreightDiscounts { get; set; }

        [JsonProperty("surcharges")]
        public List<Surcharge> Surcharges { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class BillingWeight
    {
        [JsonProperty("units")]
        public string Units { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

    public partial class Surcharge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }

    public partial class ShipmentRateDetail
    {
        [JsonProperty("rateZone")]
        public int RateZone { get; set; }

        [JsonProperty("dimDivisor")]
        public int DimDivisor { get; set; }

        [JsonProperty("fuelSurchargePercent")]
        public double FuelSurchargePercent { get; set; }

        [JsonProperty("totalSurcharges")]
        public double TotalSurcharges { get; set; }

        [JsonProperty("totalFreightDiscount")]
        public double TotalFreightDiscount { get; set; }

        [JsonProperty("surCharges")]
        public List<Surcharge> SurCharges { get; set; }

        [JsonProperty("totalBillingWeight")]
        public BillingWeight TotalBillingWeight { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class ServiceDescription
    {
        [JsonProperty("serviceId")]
        public string ServiceId { get; set; }

        [JsonProperty("serviceType")]
        public string ServiceType { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("names")]
        public List<Name> Names { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("astraDescription")]
        public string AstraDescription { get; set; }
    }

    public partial class Name
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }















    //public partial class FedExRateQuoteResponse
    //{
    //    [JsonProperty("transactionId")]
    //    public string TransactionId { get; set; }

    //    [JsonProperty("output")]
    //    public Output Output { get; set; }
    //}

    //public partial class Output
    //{
    //    [JsonProperty("alerts")]
    //    public List<Alert> Alerts { get; set; }

    //    [JsonProperty("rateReplyDetails")]
    //    public List<RateReplyDetail> RateReplyDetails { get; set; }

    //    [JsonProperty("quoteDate")]
    //    public DateTimeOffset QuoteDate { get; set; }

    //    [JsonProperty("encoded")]
    //    public bool Encoded { get; set; }
    //}

    //public partial class Alert
    //{
    //    [JsonProperty("code")]
    //    public string Code { get; set; }

    //    [JsonProperty("message")]
    //    public string Message { get; set; }

    //    [JsonProperty("alertType")]
    //    public string AlertType { get; set; }
    //}

    //public partial class RateReplyDetail
    //{
    //    [JsonProperty("serviceType")]
    //    public string ServiceType { get; set; }

    //    [JsonProperty("serviceName")]
    //    public string ServiceName { get; set; }

    //    [JsonProperty("packagingType")]
    //    public string PackagingType { get; set; }

    //    [JsonProperty("ratedShipmentDetails")]
    //    public List<RatedShipmentDetail> RatedShipmentDetails { get; set; }

    //    [JsonProperty("operationalDetail")]
    //    public OperationalDetail OperationalDetail { get; set; }

    //    [JsonProperty("signatureOptionType")]
    //    public string SignatureOptionType { get; set; }

    //    [JsonProperty("serviceDescription")]
    //    public ServiceDescription ServiceDescription { get; set; }
    //}

    //public partial class OperationalDetail
    //{
    //    [JsonProperty("ineligibleForMoneyBackGuarantee")]
    //    public bool IneligibleForMoneyBackGuarantee { get; set; }

    //    [JsonProperty("astraDescription")]
    //    public string AstraDescription { get; set; }

    //    [JsonProperty("airportId")]
    //    public string AirportId { get; set; }

    //    [JsonProperty("serviceCode")]
    //    public int ServiceCode { get; set; }
    //}

    //public partial class RatedShipmentDetail
    //{
    //    [JsonProperty("rateType")]
    //    public string RateType { get; set; }

    //    [JsonProperty("ratedWeightMethod")]
    //    public string RatedWeightMethod { get; set; }

    //    [JsonProperty("totalDiscounts")]
    //    public double TotalDiscounts { get; set; }

    //    [JsonProperty("totalBaseCharge")]
    //    public double TotalBaseCharge { get; set; }

    //    [JsonProperty("totalNetCharge")]
    //    public double TotalNetCharge { get; set; }

    //    [JsonProperty("totalNetFedExCharge")]
    //    public double TotalNetFedExCharge { get; set; }

    //    [JsonProperty("shipmentRateDetail")]
    //    public ShipmentRateDetail ShipmentRateDetail { get; set; }

    //    [JsonProperty("ratedPackages")]
    //    public List<RatedPackage> RatedPackages { get; set; }

    //    [JsonProperty("currency")]
    //    public string Currency { get; set; }
    //}

    //public partial class RatedPackage
    //{
    //    [JsonProperty("groupNumber")]
    //    public int GroupNumber { get; set; }

    //    [JsonProperty("effectiveNetDiscount")]
    //    public int EffectiveNetDiscount { get; set; }

    //    [JsonProperty("packageRateDetail")]
    //    public PackageRateDetail PackageRateDetail { get; set; }

    //    [JsonProperty("sequenceNumber")]
    //    public int SequenceNumber { get; set; }
    //}

    //public partial class PackageRateDetail
    //{
    //    [JsonProperty("rateType")]
    //    public string RateType { get; set; }

    //    [JsonProperty("ratedWeightMethod")]
    //    public string RatedWeightMethod { get; set; }

    //    [JsonProperty("baseCharge")]
    //    public double BaseCharge { get; set; }

    //    [JsonProperty("netFreight")]
    //    public double NetFreight { get; set; }

    //    [JsonProperty("totalSurcharges")]
    //    public double TotalSurcharges { get; set; }

    //    [JsonProperty("netFedExCharge")]
    //    public double NetFedExCharge { get; set; }

    //    [JsonProperty("totalTaxes")]
    //    public int TotalTaxes { get; set; }

    //    [JsonProperty("netCharge")]
    //    public double NetCharge { get; set; }

    //    [JsonProperty("totalRebates")]
    //    public int TotalRebates { get; set; }

    //    [JsonProperty("billingWeight")]
    //    public BillingWeight BillingWeight { get; set; }

    //    [JsonProperty("totalFreightDiscounts")]
    //    public double TotalFreightDiscounts { get; set; }

    //    [JsonProperty("freightDiscounts")]
    //    public List<FreightDiscount> FreightDiscounts { get; set; }

    //    [JsonProperty("surcharges")]
    //    public List<FreightDiscount> Surcharges { get; set; }

    //    [JsonProperty("currency")]
    //    public string Currency { get; set; }
    //}

    //public partial class BillingWeight
    //{
    //    [JsonProperty("units")]
    //    public string Units { get; set; }

    //    [JsonProperty("value")]
    //    public int Value { get; set; }
    //}

    //public partial class FreightDiscount
    //{
    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("description")]
    //    public string Description { get; set; }

    //    [JsonProperty("amount")]
    //    public double Amount { get; set; }

    //    [JsonProperty("percent", NullValueHandling = NullValueHandling.Ignore)]
    //    public int? Percent { get; set; }

    //    [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
    //    public string Level { get; set; }
    //}

    //public partial class ShipmentRateDetail
    //{
    //    [JsonProperty("rateZone")]
    //    public int RateZone { get; set; }

    //    [JsonProperty("dimDivisor")]
    //    public int DimDivisor { get; set; }

    //    [JsonProperty("fuelSurchargePercent")]
    //    public double FuelSurchargePercent { get; set; }

    //    [JsonProperty("totalSurcharges")]
    //    public double TotalSurcharges { get; set; }

    //    [JsonProperty("totalFreightDiscount")]
    //    public double TotalFreightDiscount { get; set; }

    //    [JsonProperty("freightDiscount")]
    //    public List<FreightDiscount> FreightDiscount { get; set; }

    //    [JsonProperty("surCharges")]
    //    public List<FreightDiscount> SurCharges { get; set; }

    //    [JsonProperty("totalBillingWeight")]
    //    public BillingWeight TotalBillingWeight { get; set; }

    //    [JsonProperty("currency")]
    //    public string Currency { get; set; }
    //}

    //public partial class ServiceDescription
    //{
    //    [JsonProperty("serviceId")]
    //    public string ServiceId { get; set; }

    //    [JsonProperty("serviceType")]
    //    public string ServiceType { get; set; }

    //    [JsonProperty("code")]
    //    public int Code { get; set; }

    //    [JsonProperty("names")]
    //    public List<Name> Names { get; set; }

    //    [JsonProperty("description")]
    //    public string Description { get; set; }

    //    [JsonProperty("astraDescription")]
    //    public string AstraDescription { get; set; }
    //}

    //public partial class Name
    //{
    //    [JsonProperty("type")]
    //    public string Type { get; set; }

    //    [JsonProperty("encoding")]
    //    public string Encoding { get; set; }

    //    [JsonProperty("value")]
    //    public string Value { get; set; }
    //}
}

