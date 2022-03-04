using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Cardano Wallet API - Wallet.
        /// </summary>
        // ===========================================================================================
        public class Wallet
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Gets the Available, Rewards or Total Balance in ADA of the given Wallet ID. 
            /// The Available Balance is the amount that can be spent unconditionally, while the 
            /// Total Balance is the combined amounts including the Available balance, Rewards and 
            /// amounts from pending transactions. If an error is encountered, such as invalid 
            /// Cardano Wallet ID or a network error, this will always return zero balance.
            /// Always check validity of the wallet by using the Get module which also contains all 
            /// the balances.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Decimal Balance(String walletID, Data.Enumerations.Balances balances)
            {
                Data.Wallet wallet = Get(walletID);

                if (wallet.error.code != Client.successFlag)
                    return 0;
                else
                {
                    Decimal lovelace = 1000000;
                    Decimal balance = 
                        (balances == Data.Enumerations.Balances.Available ?
                            wallet.balance.available.quantity:
                        (balances == Data.Enumerations.Balances.Rewards ? 
                            wallet.balance.reward.quantity:
                            wallet.balance.total.quantity));

                    return balance / lovelace;
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates a Cardano wallet with the given options.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Wallet Create(Data.WalletOptions walletOptions)
            {
                Data.Wallet wallet = new Data.Wallet();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets"));
                String jsonPayload = JsonConvert.SerializeObject(walletOptions);

                var t = Task.Run(() => client.Post(jsonPayload));
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    wallet.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    wallet = JsonConvert.DeserializeObject<Data.Wallet>(t.Result);
                    wallet.error = error;
                }

                return wallet;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Deletes a Cardano wallet with the given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Error Delete(String walletID)
            {
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID));

                var t = Task.Run(() => client.Delete());
                t.Wait();

                // Check for error

                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);
                }
                else
                {
                    error.code = Client.successFlag;
                    error.message = "";
                }

                return error;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Gets the Cardano wallet details with the given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Wallet Get(String walletID)
            {
                Data.Wallet wallet = new Data.Wallet();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID));

                var t = Task.Run(() => client.Get());
                t.Wait();

                // Check for error

                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    wallet.error = error;
                }
                else
                {
                    error.code = Client.successFlag;
                    error.message = "";

                    wallet = JsonConvert.DeserializeObject<Data.Wallet>(t.Result);
                    wallet.error = error;
                }

                return wallet;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Gets the list of known Cardano wallets, ordered from oldest to newest.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.WalletList List()
            {
                Data.WalletList walletList = new Data.WalletList();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/"));

                var t = Task.Run(() => client.Get());
                t.Wait();

                // Check for error

                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    walletList.error = error;
                }
                else
                {
                    error.code = Client.successFlag;
                    error.message = "";

                    walletList.wallets = JsonConvert.DeserializeObject<Data.Wallet[]>(t.Result);
                    walletList.error = error;
                }

                return walletList;
            }
        }
    }
}