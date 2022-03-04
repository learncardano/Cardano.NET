using System;
using System.Collections.Generic;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        public class Data
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Enumerations contains static or constant values used in various objects.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class Enumerations
            {
                public enum Balances
                {
                    Available = 0,
                    Rewards = 1,
                    Total = 2
                }

                public class Direction
                {
                    public static String Outgoing = "outgoing";
                    public static String Incoming = "incoming";
                }

                public class Order
                {
                    public static String Ascending = "ascending";
                    public static String Descending = "descending";
                }

                public class States
                {
                    public static String Used = "used";
                    public static String Unused = "unused";
                }

                public class Status
                {
                    public static String Expired = "expired";
                    public static String InLedger = "in_ledger";
                    public static String Pending = "pending";
                    public static String Submitted = "submitted";
                }
                public class Validity
                {
                    public static String Outgoing = "valid";
                    public static String Incoming = "invalid";
                }
            }

            public class Transaction
            { 
                public class Construct
                {
                    public Payments[] payments;
                    public String withdrawal = "self";
                    public Delegations delegations;
                }

                public class Constructed
                {
                    public String transaction;
                    public CoinSelection coin_selection;
                    public Denomination fee;
                }

                public class Draft
                {
                    public String passphrase;
                    public Payments[] payments;
                    public String withdrawal = "self";
                    public Height time_to_live = new Height(500, "second");
                }

                public class Fee
                {
                    public Denomination estimated_min;
                    public Denomination estimated_max;
                    public Denomination[] minimum_coins;
                    public Denomination deposit;
                    public Error error;
                }

                public class Information
                {
                    public String id;
                    public Denomination amount;
                    public Denomination fee;
                    public Denomination deposit_taken;
                    public Denomination deposit_returned;
                    public NodeTip inserted_at; // if: status == in_ledger
                    public NetworkTip expires_at; // if: status == pending OR status == expired
                    public NodeTip pending_since; // if: status == in_ledger
                    public Height depth = new Height(0, "block"); // if: status == in_ledger
                    public String direction; // Enumerations.Direction
                    public CoinInputs[] inputs;
                    public CoinOutputs[] outputs;
                    public CoinCollateral[] collateral;
                    public Withdrawals[] withdrawals;
                    public AssetMint[] mint;
                    public String status; // Enumerations.Status
                    public String script_validity; // Enumerations.Validity
                    public Error error;
                }

                public class List
                {
                    public Information[] information;
                    public Error error;
                }

                public class Sign
                {
                    public String passphrase;
                    public String transaction;
                }

                public class Submit
                {
                    public String transaction;
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Address object contains information of a wallet address.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class Address
            {
                public String id;
                public String state;
                public String[] derivation_path;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// AddressList contains known addresses of a given Wallet Id, ordered from oldest to newest.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class AddressList
            {
                public Address[] addresses;
                public Error error; // Contains the result of the Cardano API on error

            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Error object contains the response details from wallet API on error result.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class Error
            {
                public String code;
                public String message;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Network contains the latest blockchain details in the Cardano network.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class Network
            {
                public NetworkTip network_tip;
                public State sync_progress;
                public NextEpoch next_epoch;
                public String node_era;
                public NodeTip node_tip;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Address object contains information of a wallet address.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class RecoveryPhrase
            {
                public String[] mnemonic_sentence;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Wallet contains the details after creating or extracting a wallet.
            /// </summary>
            // ---------------------------------------------------------------------------------------

            public class Wallet
            {
                public String id;
                public Int32 address_pool_gap;
                public Balance balance;
                public Assets assets;
                public Delegation delegation;
                public String name;
                public PassPhrase passphrase;
                public State state;
                public NodeTip tip;
                public Error error; // Contains the result of the Cardano API on error
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// WalletList contains known wallets, ordered from oldest to newest.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class WalletList
            {
                public Wallet[] wallets;
                public Error error; // Contains the result of the Cardano API on error

            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// WalletOptions contains the parameters when creating a wallet.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class WalletOptions
            {
                public String name;
                public String[] mnemonic_sentence;
                public String passphrase;
                public Int32 address_pool_gap = 20; // Default is 20
            }

            // ---------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------
            // The following are the list of global objects used by different primary objects above.
            // ---------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------

            #region Global Objects

            public class Assets
            {
                public AssetInformation[] available;
                public AssetInformation[] total;
            }

            public class AssetInformation
            {
                public String policy_id;
                public String asset_name;
                public Int32 quantity;
            }

            public class AssetMint
            {
                public String policy_id;
                public String asset_name;
                public String fingerprint;
                public Int32 quantity;
            }

            public class Balance
            {
                public Denomination available;
                public Denomination reward;
                public Denomination total;
            }

            public class CoinCertificates
            {
                public String certificate_type;
                public String pool;
                public String[] reward_account_path = new string[5];
            }

            public class CoinChange
            {
                public String address;
                public Denomination amount;
                public AssetInformation[] assets;
                public String derivation_path;
            }

            public class CoinCollateral
            {
                public String address;
                public Denomination amount;
                public String id;
                public String derivation_path;
                public Int32 index;
            }

            public class CoinInputs
            {
                public String address;
                public Denomination amount;
                public AssetInformation[] assets;
                public String id;
                public String derivation_path;
                public Int32 index;
            }

            public class CoinOutputs
            {
                public String address;
                public Denomination amount;
                public AssetInformation[] assets;
            }

            public class CoinWithdrawals
            {
                public String stake_address;
                public String derivation_path;
                public Denomination amount;
            }

            public class CoinSelection
            {
                public CoinInputs inputs;
                public CoinOutputs outputs;
                public CoinChange change;
                public CoinCollateral collateral;
                public CoinWithdrawals withdrawals;
                public CoinCertificates certificates;
                public Denomination[] deposits_taken;
                public Denomination[] deposits_returned;
                public String metadata;
            }

            public class Delegation
            {
                public DelegationActive active;
                public DelegationNext[] next;
            }

            public class Delegations
            {
                public Join[] join;
            }

            public class DelegationActive
            {
                public String status;
                public String target;
            }

            public class DelegationNext
            {
                public String status;
                public DelegationNextChange changes_at;
            }

            public class DelegationNextChange
            {
                public Int32 epoch_number;
                public String epoch_start_time;
            }

            public class Denomination
            {
                public Int32 quantity;
                public String unit = "lovelace";
            }

            public class Height
            {
                public Int32 quantity;
                public String unit;

                public Height(Int32 Quantity, String Unit)
                {
                    this.quantity = Quantity;
                    this.unit = Unit;
                }
            }

            public class Join
            {
                public String pool;
                public String stake_key_index;
            }

            public class NetworkTip
            {
                public Int32 epoch_number;
                public String time;
                public Int32 absolute_slot_number;
                public Int32 slot_number;
            }

            public class NextEpoch
            {
                public String epoch_start_time;
                public Int32 epoch_number;
            }

            public class NodeTip
            {
                public Height height;
                public Int32 epoch_number;
                public String time;
                public Int32 absolute_slot_number;
                public Int32 slot_number;
            }

            public class Payments
            {
                public String address;
                public Denomination amount;
                public AssetInformation[] assets;
            }

            public class PassPhrase
            {
                public String last_updated_at;
            }

            public class Progress
            {
                public Decimal quantity;
                public String unit;
            }

            public class State
            {
                public String status;
                public Progress progress;
            }

            public class Withdrawals
            {
                public String stake_address;
                public Denomination amount;
            }

            #endregion
        }
    }
}