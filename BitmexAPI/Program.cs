using System;

namespace BitmexAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            //var bitmexSocketClient = new BitmexClient();
            //bitmexSocketClient.Setup();
            //bitmexSocketClient.Start();
            var bitmexHttpClient = new BitmexHttpClient(); ;
            bitmexHttpClient.PlaceTestOrder();
            Console.ReadLine();
        }
    }
}
