using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Cardano Wallet API - Network.
        /// </summary>
        // ===========================================================================================
        public class Network
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Query the tip of the blockchain and returns the latest information on the network.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Network Query()
            {
                Client client = new Client(UrlBuilder("network/information"));

                var t = Task.Run(() => client.Get());
                t.Wait();

                Data.Network network = 
                    JsonConvert.DeserializeObject<Data.Network>(t.Result);

                return network;
            }
        }
    }
}