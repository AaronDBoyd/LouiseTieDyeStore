﻿using LouiseTieDyeStore.Shared.FedExRequestResponse.AuthToken;
using LouiseTieDyeStore.Shared.FedExRequestResponse.RateQuote;
using LouiseTieDyeStore.Shared.FedExRequestResponse.ValidateAddress;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Policy;

namespace LouiseTieDyeStore.Server.Services.ShippingService
{
    public class FedExShippingService : IShippingService
    {
        private readonly IConfiguration _config;

        public FedExShippingService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> GetAuthToken()
        {
            // TODO: Do I abstract away this client setup?

            string clientId = _config["FedExKeys:ClientId"];
            string clientSecret = _config["FedExKeys:ClientSecret"];

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };
            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://apis-sandbox.fedex.com");

            using var req = new HttpRequestMessage(HttpMethod.Post, "/oauth/token") { Content = new FormUrlEncodedContent(dict) };
            using var res = await client.SendAsync(req);

            if (res != null)
            {
                var tokenObject = JsonConvert.DeserializeObject<FedExAuthTokenResponse>(await res.Content.ReadAsStringAsync());

                var token = tokenObject.AccessToken;

                return token;
            }
            else
            {
                return "Unsuccessful Request";
            }
        }

        public async Task<ServiceResponse<string>> GetShippingRateQuote(ShippingInfoDTO shippingInfo, string authToken)
        {

            var shippingInfoRequest = new FedExRateQuoteRequest
            {
                AccountNumber = new AccountNumber
                {
                    Value = _config["FedExKeys:AccountNumber"],
                },
                RequestedShipment = new RequestedShipment
                {
                    Shipper = new Recipient
                    {
                        Address = new Shared.FedExRequestResponse.RateQuote.Address
                        {
                            PostalCode = int.Parse(_config["Shipping:LocalPostalCode"])
                        }
                    },
                    Recipient = new Recipient
                    {
                        Address = new Shared.FedExRequestResponse.RateQuote.Address
                        {
                            PostalCode = shippingInfo.Zip
                        }
                    },
                    PickupType = "DROPOFF_AT_FEDEX_LOCATION",
                    RateRequestType = new List<string>
                    {
                        "ACCOUNT",
                        "PREFERRED"
                    },
                    RequestedPackageLineItems = new List<RequestedPackageLineItem>
                    {
                        new RequestedPackageLineItem
                        {
                            Weight = new Weight
                            {                               
                                Value = 1
                            }
                        }
                    }
                },
                CarrierCodes = new List<string>
                {
                    "FDXG"
                }
            };

            //Console.WriteLine("!!! ShippingInfo: " + JsonConvert.SerializeObject(shippingInfoRequest));


            // TODO: Create Client for Fedex APIs
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://apis-sandbox.fedex.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);
            client.DefaultRequestHeaders.Add("X-locale", "en_US");

            try
            {

                var result = await client.PostAsJsonAsync("/rate/v1/rates/quotes", shippingInfoRequest);

                //Console.WriteLine("!!!Quote Result: " + await result.Content.ReadAsStringAsync());

                var quoteResponse = JsonConvert.DeserializeObject<FedExRateQuoteResponse>(await result.Content.ReadAsStringAsync());

                double shippingCost = quoteResponse.Output.RateReplyDetails[0].RatedShipmentDetails[0].TotalNetCharge;

                return new ServiceResponse<string>
                {
                    Data = shippingCost.ToString()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<string>> ValidateShippingAddress(ShippingInfoDTO shippingInfo)
        {
            //ShippingInfoDTO shippingInfo = new ShippingInfoDTO
            //{
            //    LineOne = "8321 Cavaricci Ave",
            //    City = "Las Vegas",
            //    Zip = 89129
            //};

            var requestAddress = new FedExValidateAddressRequest
            {
                AddressesToValidate = new List<AddressToValidate>
                {
                    new AddressToValidate
                    {
                        Address = new Shared.FedExRequestResponse.ValidateAddress.Address
                        {
                            StreetLines = new List<string>
                            {
                                shippingInfo.LineOne
                            },
                            City = shippingInfo.City,
                            PostalCode = shippingInfo.Zip.ToString()
                        }
                    }
                }
            };

            Console.WriteLine("!!!Request Address: " + JsonConvert.SerializeObject(requestAddress));


            var authToken = await GetAuthToken();

            

            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://apis-sandbox.fedex.com");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);
            client.DefaultRequestHeaders.Add("X-locale", "en_US");

            try
            {
                // TODO: !!! NEED TO RETEST PRODUCTION API

                var result = await client.PostAsJsonAsync("/address/v1/addresses/resolve", requestAddress);

                //Console.WriteLine("!!!Address Result: " + await result.Content.ReadAsStringAsync());

                var returnString = JsonConvert.DeserializeObject<FedExValidateAddressResponse>(await result.Content.ReadAsStringAsync());

               // Console.WriteLine("!!! Address Object: " + JsonConvert.SerializeObject(returnString));

                return await GetShippingRateQuote(shippingInfo, authToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Invalid Address"
                };
            }          
        }
    }
}