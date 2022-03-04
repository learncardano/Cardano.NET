using System;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Cardano Wallet API - Recovery Phrase.
        /// </summary>
        // ===========================================================================================
        public class RecoveryPhrase
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Generates a recovery phrase object with the specified number of words.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.RecoveryPhrase Generate(Int32 wordCount)
            {
                Data.RecoveryPhrase recoveryPhrase = new Data.RecoveryPhrase();
                recoveryPhrase.mnemonic_sentence = Mnemonic(wordCount).Split(' ');

                return recoveryPhrase;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Generates a mnemonic sentence with the specified number of words.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public String Mnemonic(Int32 wordCount)
            {
                CardanoSharp.Wallet.MnemonicService mnemonicService = new CardanoSharp.Wallet.MnemonicService();
                CardanoSharp.Wallet.Models.Keys.Mnemonic mnemonic = mnemonicService.Generate(wordCount);

                return mnemonic.Words;
            }
        }
    }
}