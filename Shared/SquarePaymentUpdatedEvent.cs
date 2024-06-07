using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LouiseTieDyeStore.Shared
{
    public partial class SquarePaymentUpdatedEvent
    {
        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("event_id")]
        public Guid EventId { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public Object Object { get; set; }
    }

    public partial class Object
    {
        [JsonProperty("payment")]
        public Payment Payment { get; set; }
    }

    public partial class Payment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("amount_money")]
        public Money AmountMoney { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("delay_duration")]
        public string DelayDuration { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        [JsonProperty("card_details")]
        public CardDetails CardDetails { get; set; }

        [JsonProperty("location_id")]
        public string LocationId { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("risk_evaluation")]
        public RiskEvaluation RiskEvaluation { get; set; }

        [JsonProperty("total_money")]
        public Money TotalMoney { get; set; }

        [JsonProperty("approved_money")]
        public Money ApprovedMoney { get; set; }

        [JsonProperty("receipt_number")]
        public string ReceiptNumber { get; set; }

        [JsonProperty("receipt_url")]
        public Uri ReceiptUrl { get; set; }

        [JsonProperty("delay_action")]
        public string DelayAction { get; set; }

        [JsonProperty("delayed_until")]
        public DateTimeOffset DelayedUntil { get; set; }

        [JsonProperty("version_token")]
        public string VersionToken { get; set; }
    }

    public partial class Money
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class CardDetails
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("entry_method")]
        public string EntryMethod { get; set; }

        [JsonProperty("cvv_status")]
        public string CvvStatus { get; set; }

        [JsonProperty("avs_status")]
        public string AvsStatus { get; set; }

        [JsonProperty("statement_description")]
        public string StatementDescription { get; set; }

        [JsonProperty("card_payment_timeline")]
        public CardPaymentTimeline CardPaymentTimeline { get; set; }
    }

    public partial class Card
    {
        [JsonProperty("card_brand")]
        public string CardBrand { get; set; }

        [JsonProperty("last_4")]
        public long Last4 { get; set; }

        [JsonProperty("exp_month")]
        public long ExpMonth { get; set; }

        [JsonProperty("exp_year")]
        public long ExpYear { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("prepaid_type")]
        public string PrepaidType { get; set; }

        [JsonProperty("bin")]
        public long Bin { get; set; }
    }

    public partial class CardPaymentTimeline
    {
        [JsonProperty("authorized_at")]
        public DateTimeOffset AuthorizedAt { get; set; }

        [JsonProperty("captured_at")]
        public DateTimeOffset CapturedAt { get; set; }
    }

    public partial class RiskEvaluation
    {
        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("risk_level")]
        public string RiskLevel { get; set; }
    }
}
