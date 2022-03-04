using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cardano.NET
{
    // ===========================================================================================
    /// <summary>
    /// Class library for accessing Cardano Wallet API. Upon first use or upon page load,
    /// you need to instantiate a WalletAPI object to initialize the BaseURL with the 
    /// url address of the Cardano Wallet web api instance (eg. "http://127.0.0.1:8090/").
    /// </summary>
    // ===========================================================================================
    public partial class WalletAPI
    {
        private static String baseURL { get; set; } = String.Empty;

        public WalletAPI(String BaseURL)
        {
            baseURL = BaseURL + 
                (BaseURL.EndsWith("/") ? "" : "/") +
                (BaseURL.EndsWith("v2/") ? "" : "v2/");
        }

        public WalletAPI(String BaseURL, String WalletFilePath)
        {
            walletFilePath = WalletFilePath;
            baseURL = BaseURL +
                (BaseURL.EndsWith("/") ? "" : "/") +
                (BaseURL.EndsWith("v2/") ? "" : "v2/");
        }

        private static String walletFilePath = "";

        public static String WalletFilePath
        {
            get { return walletFilePath; }
            set { walletFilePath = value; }
        }

        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a final URL from the base URL and the additional parameter set.
        /// </summary>
        // ---------------------------------------------------------------------------------------
        public static String UrlBuilder(String parameter)
        {
            return baseURL += parameter;
        }

        // ---------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the Cardano Wallet EXE file with the given command parameters.
        /// </summary>
        // ---------------------------------------------------------------------------------------
        private static String ExecuteWallet(String Command)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = WalletFilePath + "cardano-wallet.exe",
                Verb = "runas",
                Arguments = "/C " + Command,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var cmd = Process.Start(startInfo);
            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();

            return output;
        }
    }
}