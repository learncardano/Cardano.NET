using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cardano.NET
{
    public partial class WalletAPI
    {
        // ===========================================================================================
        // Class library for using HttpClient in posting requests to a pre-assigned URL.
        // ===========================================================================================
        public partial class Client
        {
            private static String url = "";
            public static String errorFlag = "!<ERROR>";
            public static String successFlag = "000";

            public static String URL
            {
                get { return url; }
                set { url = value; }
            }

            public Client(String URL)
            {
                url = URL;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates an asynchronous DELETE request using HttpClient.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public async Task<String> Delete()
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.DeleteAsync(URL);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    /*
                     * Return an array of:
                     * 0 - Client.errorFlag
                     * 1 - Server Status Code
                     * 2 - API Error Code and Message
                     * 3 - Payload or Query parameter
                     */

                    return errorFlag + "|" + response.StatusCode.ToString() + "|" +
                        await response.Content.ReadAsStringAsync() + "|" + "<no payload>";
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates an asynchronous GET request using HttpClient.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public async Task<String> Get()
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(URL);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    /*
                     * Return an array of:
                     * 0 - Client.errorFlag
                     * 1 - Server Status Code
                     * 2 - API Error Code and Message
                     * 3 - Payload or Query parameter
                     */

                    return errorFlag + "|" + response.StatusCode.ToString() + "|" +
                        await response.Content.ReadAsStringAsync() + "|" + "<no payload>";
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates an asynchronous GET request with query parameter using HttpClient.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public async Task<String> Get(String query)
            {
                HttpClient client = new HttpClient();
                UriBuilder uriBuilder = new UriBuilder(URL);
                uriBuilder.Query = query;

                HttpResponseMessage response = await client.GetAsync(uriBuilder.Uri);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    /*
                     * Return an array of:
                     * 0 - Client.errorFlag
                     * 1 - Server Status Code
                     * 2 - API Error Code and Message
                     * 3 - Payload or Query parameter
                     */

                    return errorFlag + "|" + response.StatusCode.ToString() + "|" +
                        await response.Content.ReadAsStringAsync() + "|" + query;
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates an asynchronous POST request using HttpClient with a Json formatted parameter.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public async Task<String> Post(String jsonPayload)
            {
                HttpContent payload = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(URL, payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    /*
                     * Return an array of:
                     * 0 - Client.errorFlag
                     * 1 - Server Status Code
                     * 2 - API Error Code and Message
                     * 3 - Payload or Query parameter
                     */

                    return errorFlag + "|" + response.StatusCode.ToString() + "|" +
                        await response.Content.ReadAsStringAsync() + "|" + jsonPayload;
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Creates an asynchronous PUT request using HttpClient with a Json formatted parameter.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public async Task<String> Put(String jsonPayload)
            {
                HttpContent payload = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PutAsync(URL, payload);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                {
                    /*
                     * Return an array of:
                     * 0 - Client.errorFlag
                     * 1 - Server Status Code
                     * 2 - API Error Code and Message
                     * 3 - Payload or Query parameter
                     */

                    return errorFlag + "|" + response.StatusCode.ToString() + "|" +
                        await response.Content.ReadAsStringAsync() + "|" + jsonPayload;
                }
            }
        }
    }
}