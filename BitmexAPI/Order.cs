using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BitmexAPI
{

    public partial class OrderPOSTRequestParams : QueryJsonParams
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("side")]
        public string Side { get; set; }
        [JsonProperty("orderQty")]
        public decimal? OrderQty { get; set; }
        [JsonProperty("price")]
        public decimal? Price { get; set; }
        [JsonProperty("displayQty")]
        public decimal? DisplayQty { get; set; }
        [JsonProperty("stopPx")]
        public decimal? StopPx { get; set; }
        [JsonProperty("clOrdID")]
        public string ClOrdID { get; set; }
        [JsonProperty("clOrdLinkID")]
        public string ClOrdLinkID { get; set; }
        [JsonProperty("pegOffsetValue")]
        public decimal? PegOffsetValue { get; set; }
        [JsonProperty("pegPriceType")]
        public string PegPriceType { get; set; }
        [JsonProperty("ordType")]
        public string OrdType { get; set; }
        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }
        [JsonProperty("execInst")]
        public string ExecInst { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }

        public static OrderPOSTRequestParams CreateSimpleMarket(string symbol, decimal quantity, OrderSide side)
        {
            return new OrderPOSTRequestParams
            {
                Symbol = symbol,
                Side = Enum.GetName(typeof(OrderSide), side),
                OrderQty = quantity,
                OrdType = Enum.GetName(typeof(OrderType), OrderType.Market),
            };
        }
    }

    public enum OrderSide
    {
        Buy = 1,
        Sell = 2
    }

    public enum OrderType
    {
        Market = 1,
        Limit = 2,
        Stop = 3,
    }

    public interface IJsonQueryParams
    {
        string ToJson();
    }

    public abstract class QueryJsonParams : IJsonQueryParams
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [DebuggerDisplay("{Symbol} {Side} {OrderQty}")]
    public class OrderDto
    {
        [JsonProperty("orderID")]
        public string OrderId { get; set; }

        [JsonProperty("clOrdID")]
        public string ClOrdId { get; set; }

        [JsonProperty("clOrdLinkID")]
        public string ClOrdLinkId { get; set; }

        [JsonProperty("account")]
        public long? Account { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }

        [JsonProperty("orderQty")]
        public decimal? OrderQty { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("displayQty")]
        public decimal? DisplayQty { get; set; }

        [JsonProperty("stopPx")]
        public decimal? StopPx { get; set; }

        [JsonProperty("pegOffsetValue")]
        public decimal? PegOffsetValue { get; set; }

        [JsonProperty("pegPriceType")]
        public string PegPriceType { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("settlCurrency")]
        public string SettlCurrency { get; set; }

        [JsonProperty("ordType")]
        public string OrdType { get; set; }

        [JsonProperty("timeInForce")]
        public string TimeInForce { get; set; }

        [JsonProperty("execInst")]
        public string ExecInst { get; set; }

        [JsonProperty("exDestination")]
        public string ExDestination { get; set; }

        [JsonProperty("ordStatus")]
        public string OrdStatus { get; set; }

        [JsonProperty("triggered")]
        public string Triggered { get; set; }

        [JsonProperty("workingIndicator")]
        public bool WorkingIndicator { get; set; }

        [JsonProperty("ordRejReason")]
        public string OrdRejReason { get; set; }

        [JsonProperty("leavesQty")]
        public decimal? LeavesQty { get; set; }

        [JsonProperty("cumQty")]
        public decimal CumQty { get; set; }

        [JsonProperty("avgPx")]
        public decimal? AvgPx { get; set; }

        [JsonProperty("multiLegReportingType")]
        public string MultiLegReportingType { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("transactTime")]
        public System.DateTimeOffset TransactTime { get; set; }

        [JsonProperty("timestamp")]
        public System.DateTimeOffset Timestamp { get; set; }
    }
}
