using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitmexAPI
{
    public abstract class SocketMessage
    {
        [JsonProperty("op")]
        public string Operation { get; }
        [JsonProperty("args")]
        public object[] Arguments { get; }

        protected SocketMessage(OperationType operation, params object[] args)
        {

            Operation = Enum.GetName(typeof(OperationType), operation);
            Arguments = args;
        }
    }

    public enum OperationType
    {
        subscribe,
        unsubscribe,
        authKeyExpires
    }

    public sealed class SocketAuthorizationMessage : SocketMessage
    {
        public SocketAuthorizationMessage(string apiKey, long expiresTime, string sign) : base(OperationType.authKeyExpires, apiKey, expiresTime, sign)
        {

        }
    }

    public sealed class SocketSubscriptionMessage : SocketMessage
    {
        public SocketSubscriptionMessage(params object[] args) : base(OperationType.subscribe, args)
        {
        }
    }
}
