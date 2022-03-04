using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Cardano Wallet API - Address.
        /// </summary>
        // ===========================================================================================
        public class Address
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves list of addresses of a given Wallet ID both used and unused.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.AddressList List(String walletID)
            {
                Data.AddressList addressList = new Data.AddressList();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/addresses"));

                var t = Task.Run(() => client.Get());
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    addressList.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    addressList.addresses = JsonConvert.DeserializeObject<Data.Address[]>(t.Result);
                    addressList.error = error;
                }

                return addressList;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves list of addresses of a given Wallet ID, with option if used or unused.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.AddressList List(String walletID, String state)
            {
                Data.AddressList addressList = new Data.AddressList();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/addresses"));
                String query = "state=" + state;

                var t = Task.Run(() => client.Get(query));
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    addressList.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    addressList.addresses = JsonConvert.DeserializeObject<Data.Address[]>(t.Result);
                    addressList.error = error;
                }

                return addressList;
            }
        }
    }
}