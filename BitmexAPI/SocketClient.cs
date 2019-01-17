using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocket4Net;

namespace BitmexAPI
{
    class BitmexClient
    {
        private WebSocket _websocketClient;
        private const string SocketUrl = "wss://testnet.bitmex.com/realtime";
        private ExpiresTimeProvider _expiresTimeProvider;
        private SignatureProvider _signatureProvider;
        private const string _secret = "_fzjFhDzQLrizBrWZl7QxK7wBVn-a2HKIrhVQ7baAgmVvzN8";
        private const string _key = "jgfHziVHF4Z46pp7hpqJjTGU";

        public void Setup()
        {
            _expiresTimeProvider = new ExpiresTimeProvider();
            _signatureProvider = new SignatureProvider();

            _websocketClient = new WebSocket(SocketUrl);

            _websocketClient.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(websocketClient_Error);
            _websocketClient.Opened += new EventHandler(websocketClient_Opened);
            _websocketClient.MessageReceived += new EventHandler<MessageReceivedEventArgs>(websocketClient_MessageReceived);
        }

        public void Start()
        {
            _websocketClient.Open();

            var ish = _websocketClient.Handshaked;
        }

        public void AuthorizeSocket()
        {
            var expiresTime = _expiresTimeProvider.Get();
            var signatureString = _signatureProvider.CreateSignature(_secret, $"GET/realtime{expiresTime}");
            var message = new SocketAuthorizationMessage(_key, expiresTime, signatureString);
            Send(message);
        }

        public void Subscribe(string topic)
        {
            var expiresTime = _expiresTimeProvider.Get();
            var signatureString = _signatureProvider.CreateSignature(_secret, $"GET/realtime{expiresTime}");
            var message = new SocketSubscriptionMessage(topic);
            Send(message);
        }

        public void Send<TMessage>(TMessage message)
            where TMessage : SocketMessage
        {
            var json = JsonConvert.SerializeObject(message);
            Console.WriteLine("Sending message: \n" + json);
            _websocketClient.Send(json);
        }

        private void websocketClient_Opened(object sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Client successfully connected.");
            Console.WriteLine();

            AuthorizeSocket();
            Subscribe("privateNotifications");
            Subscribe("wallet");
            Subscribe("affiliate");
            Subscribe("order");
            Subscribe("margin");
            Subscribe("position");
            Subscribe("transact");
        }

        private void websocketClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("Message Received. Server answered: " + e.Message);
        }

        private void websocketClient_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.GetType() + ": " + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);

            if (e.Exception.InnerException != null)
            {
                Console.WriteLine(e.Exception.InnerException.GetType());
            }

            return;
        }
    }
}
