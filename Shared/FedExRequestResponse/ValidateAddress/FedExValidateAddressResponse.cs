using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared.FedExRequestResponse.ValidateAddress
{
    public partial class FedExValidateAddressResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("customerTransactionId")]
        public string CustomerTransactionId { get; set; }

        [JsonProperty("output")]
        public Output Output { get; set; }
    }

    public partial class Output
    {
        [JsonProperty("resolvedAddresses")]
        public List<ResolvedAddress> ResolvedAddresses { get; set; }

        [JsonProperty("alerts")]
        public List<Alert> Alerts { get; set; }
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

    public partial class ResolvedAddress
    {
        [JsonProperty("streetLinesToken")]
        public List<string> StreetLinesToken { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateOrProvinceCode")]
        public string StateOrProvinceCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("customerMessage")]
        public List<object> CustomerMessage { get; set; }

        [JsonProperty("cityToken")]
        public List<string> CityToken { get; set; }

        [JsonProperty("postalCodeToken")]
        public PostalCodeToken PostalCodeToken { get; set; }

        [JsonProperty("parsedPostalCode")]
        public ParsedPostalCode ParsedPostalCode { get; set; }

        [JsonProperty("classification")]
        public string Classification { get; set; }

        [JsonProperty("postOfficeBox")]
        public bool PostOfficeBox { get; set; }

        [JsonProperty("normalizedStatusNameDPV")]
        public bool NormalizedStatusNameDpv { get; set; }

        [JsonProperty("standardizedStatusNameMatchSource")]
        public string StandardizedStatusNameMatchSource { get; set; }

        [JsonProperty("resolutionMethodName")]
        public string ResolutionMethodName { get; set; }

        [JsonProperty("ruralRouteHighwayContract")]
        public bool RuralRouteHighwayContract { get; set; }

        [JsonProperty("generalDelivery")]
        public bool GeneralDelivery { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("POBox")]
        public bool PoBox { get; set; }

        [JsonProperty("POBoxOnlyZIP")]
        public bool PoBoxOnlyZip { get; set; }

        [JsonProperty("SplitZip")]
        public bool SplitZip { get; set; }

        [JsonProperty("SuiteRequiredButMissing")]
        public bool SuiteRequiredButMissing { get; set; }

        [JsonProperty("InvalidSuiteNumber")]
        public bool InvalidSuiteNumber { get; set; }

        [JsonProperty("ResolutionInput")]
        public string ResolutionInput { get; set; }

        [JsonProperty("DPV")]
        public bool Dpv { get; set; }

        [JsonProperty("ResolutionMethod")]
        public string ResolutionMethod { get; set; }

        [JsonProperty("DataVintage")]
        public string DataVintage { get; set; }

        [JsonProperty("MatchSource")]
        public string MatchSource { get; set; }

        [JsonProperty("CountrySupported")]
        public bool CountrySupported { get; set; }

        [JsonProperty("ValidlyFormed")]
        public bool ValidlyFormed { get; set; }

        [JsonProperty("Matched")]
        public bool Matched { get; set; }

        [JsonProperty("Resolved")]
        public bool Resolved { get; set; }

        [JsonProperty("Inserted")]
        public bool Inserted { get; set; }

        [JsonProperty("MultiUnitBase")]
        public bool MultiUnitBase { get; set; }

        [JsonProperty("ZIP11Match")]
        public bool Zip11Match { get; set; }

        [JsonProperty("ZIP4Match")]
        public bool Zip4Match { get; set; }

        [JsonProperty("UniqueZIP")]
        public bool UniqueZip { get; set; }

        [JsonProperty("StreetAddress")]
        public bool StreetAddress { get; set; }

        [JsonProperty("RRConversion")]
        public bool RrConversion { get; set; }

        [JsonProperty("ValidMultiUnit")]
        public bool ValidMultiUnit { get; set; }

        [JsonProperty("AddressType")]
        public string AddressType { get; set; }

        [JsonProperty("AddressPrecision")]
        public string AddressPrecision { get; set; }

        [JsonProperty("MultipleMatches")]
        public bool MultipleMatches { get; set; }
    }

    public partial class ParsedPostalCode
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("addOn")]
        public int AddOn { get; set; }

        [JsonProperty("deliveryPoint")]
        public int DeliveryPoint { get; set; }
    }

    public partial class PostalCodeToken
    {
        [JsonProperty("changed")]
        public bool Changed { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
