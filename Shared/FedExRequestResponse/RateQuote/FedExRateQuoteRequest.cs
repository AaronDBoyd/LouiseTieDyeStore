using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared.FedExRequestResponse.RateQuote
{
    public partial class FedExRateQuoteRequest
    {
        [JsonProperty("accountNumber")]
        public AccountNumber AccountNumber { get; set; }

        [JsonProperty("requestedShipment")]
        public RequestedShipment RequestedShipment { get; set; }

        [JsonProperty("carrierCodes")]
        public List<string> CarrierCodes { get; set; }
    }

    public partial class AccountNumber
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class RequestedShipment
    {
        [JsonProperty("shipper")]
        public Recipient Shipper { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }

        [JsonProperty("pickupType")]
        public string PickupType { get; set; }

        [JsonProperty("rateRequestType")]
        public List<string> RateRequestType { get; set; }

        [JsonProperty("requestedPackageLineItems")]
        public List<RequestedPackageLineItem> RequestedPackageLineItems { get; set; }
    }

    public partial class Recipient
    {
        [JsonProperty("address")]
        public Address Address { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; } = "US";
    }

    public partial class RequestedPackageLineItem
    {
        [JsonProperty("weight")]
        public Weight Weight { get; set; }
    }

    public partial class Weight
    {
        [JsonProperty("units")]
        public string Units { get; } = "LB"; 

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
