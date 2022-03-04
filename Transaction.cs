using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Cardano Wallet API - Transaction.
        /// </summary>
        // ===========================================================================================
        public class Transaction
        {
            // ===========================================================================================
            /// <summary>
            /// Class library for building a transaction draft.
            /// </summary>
            // ===========================================================================================
            public class Builder
            {
                public Data.Transaction.Draft Draft;
                public Data.Payments[] Payments;

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Initialize the transaction draft.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public Builder()
                {
                    Draft = new Data.Transaction.Draft();
                }

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Adds a recipient address and amount in ADA to send.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public void Add(String address, Decimal amountADA)
                {
                    Tools tools = new Tools();
                    List<Data.Payments> newPayments = new List<Data.Payments>(); // Use to rebuild Payments
                    Boolean added = false;

                    if (Payments != null && Payments.Length > 0)
                        foreach (Data.Payments p in Payments)
                        {
                            // If recipient exists, update amount
                            if (p.address == address)
                            {
                                // Add the amount to existing recipient
                                p.amount.quantity += tools.ToLovelace(amountADA);

                                // Flag as added so as not to add anymore
                                added = true;
                            }

                            // Add to list
                            newPayments.Add(p);
                        }

                    // If recipient is not yet in the list
                    if (!added)
                    {
                        Data.Denomination denomination = new Data.Denomination();
                        Data.Payments payments = new Data.Payments();

                        // Add the new Recipient
                        denomination.quantity = tools.ToLovelace(amountADA);

                        payments.address = address;
                        payments.amount = denomination;

                        // Add to list
                        newPayments.Add(payments);
                    }

                    // Replace with the new list.
                    Payments = newPayments.ToArray();
                }

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Finalize the transaction draft when estimating fee.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public void Final()
                {
                    Draft.payments = Payments;
                }

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Removes a recipient from the list and deduct the amount of ADA.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public void Remove(String address)
                {
                    if (Payments != null && Payments.Length > 0)
                    {
                        List<Data.Payments> newPayments = new List<Data.Payments>(); // Use to rebuild Payments

                        newPayments = new List<Data.Payments>(Payments);
                        newPayments.Remove(newPayments.Find(i => i.address == address));

                        // Save new list
                        Payments = newPayments.ToArray();
                    }
                }

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Sign transaction draft with the wallet password.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public void Sign(String password)
                {
                    Final(); // Make sure to finalize the draft

                    Draft.passphrase = password;
                }

                // ---------------------------------------------------------------------------------------
                /// <summary>
                /// Computes the total amount of ADA.
                /// </summary>
                // ---------------------------------------------------------------------------------------
                public Decimal Total()
                {
                    Decimal total = 0;

                    if (Payments != null && Payments.Length > 0)
                        foreach (Data.Payments p in Payments)
                            total += p.amount.quantity;

                    return total;
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Cancels a transaction if status is still pending.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Error Cancel(String walletID, String transactionID)
            {
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/transactions/" + transactionID));

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
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";
                }

                return error;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Estimates the fee for a given draft transaction.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Transaction.Fee Estimate(String walletID, Data.Transaction.Draft txDraft)
            {
                Data.Transaction.Fee fee = new Data.Transaction.Fee();
                Data.Error error = new Data.Error();
                
                Client client = new Client(UrlBuilder("wallets/" + walletID + "/payment-fees"));
                String jsonPayload = JsonConvert.SerializeObject(txDraft);

                var t = Task.Run(() => client.Post(jsonPayload));
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    fee.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    fee = JsonConvert.DeserializeObject<Data.Transaction.Fee>(t.Result);
                    fee.error = error;
                }

                return fee;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves transaction details of a given Wallet ID and Transaction ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Transaction.Information Get(String walletID, String transactionID)
            {
                Data.Transaction.Information information = new Data.Transaction.Information();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/transactions/" + transactionID));

                var t = Task.Run(() => client.Get());
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    information.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    information = JsonConvert.DeserializeObject<Data.Transaction.Information>(t.Result);
                    information.error = error;
                }

                return information;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves list of transactions of a given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Transaction.List List(String walletID, DateTime dateStart, DateTime dateEnd, 
                String order, Int32 minWithdrawal)
            {
                Data.Transaction.List transactionList = new Data.Transaction.List();
                Data.Error error = new Data.Error();
                Tools tools = new Tools();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/transactions"));
                String query = 
                    "start=" + tools.ToDateISO8601(dateStart) + 
                    "&end" + tools.ToDateISO8601(dateEnd) + 
                    "&order=" + order +
                    (minWithdrawal >= 1 ? "&minWithdrawal=" + minWithdrawal.ToString() : "");

                var t = Task.Run(() => client.Get(query));
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    transactionList.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    transactionList.information = 
                        JsonConvert.DeserializeObject<Data.Transaction.Information[]>(t.Result);
                    transactionList.error = error;
                }

                return transactionList;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Finalizes and sends the transaction from the wallet.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Transaction.Information Send(String walletID, Data.Transaction.Draft txDraft)
            {
                Data.Transaction.Information information = new Data.Transaction.Information();
                Data.Error error = new Data.Error();

                Client client = new Client(UrlBuilder("wallets/" + walletID + "/transactions"));
                String jsonPayload = JsonConvert.SerializeObject(txDraft);

                var t = Task.Run(() => client.Post(jsonPayload));
                t.Wait();

                // Check for error
                if (t.Result.Length >= Client.errorFlag.Length &&
                    t.Result.Substring(0, Client.errorFlag.Length) == Client.errorFlag)
                {
                    String[] parseError = t.Result.Split('|');
                    error = JsonConvert.DeserializeObject<Data.Error>(parseError[2]);

                    // Save Error to returned object
                    information.error = error;
                }
                else
                {
                    // Indicate success
                    error.code = Client.successFlag;
                    error.message = "";

                    information = JsonConvert.DeserializeObject<Data.Transaction.Information>(t.Result);
                    information.error = error;
                }

                return information;
            }
        }
    }
}