using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared.FedExRequestResponse.ValidateAddress
{
    public partial class FedExValidateAddressRequest
    {
        [JsonProperty("addressesToValidate")]
        public List<AddressToValidate> AddressesToValidate { get; set; }
    }

    public partial class AddressToValidate
    {
        [JsonProperty("address")]
        public Address Address { get; set; }
    }

    public partial class Address
    {
        [JsonProperty("streetLines")]
        public List<string> StreetLines { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; } = "US";
    }
}
