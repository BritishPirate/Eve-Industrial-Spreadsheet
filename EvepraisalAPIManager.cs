using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using EveIndustrialSpreadsheet.AppraisalRequestPack;

namespace EveIndustrialSpreadsheet {
    class EvepraisalAPIManager {

        /// <summary>
        /// Gets an existing appraisal via it's ID
        /// </summary>
        /// <param name="appraisalID">The ID of the desired appraisal</param>
        /// <returns></returns>
        public static async Task getAppraisal(string appraisalID) {
            // Set the URL and create the HTTP client
            string url = "https://evepraisal.com/a/" + appraisalID + ".json";
            using HttpClient client = createClient();

            //Get the reply from the client's request and assign it to a variable
            string content = await client.GetStringAsync(url);

            // Print out the variable
            Console.WriteLine(content);
        }


        public static async Task newAppraisal(AppraisalRequest ar, bool persist = false) {
            // Set the URL and create the HTTP client (Persist is used to determine if the requests needs to stay existing)
            string url = "https://evepraisal.com/appraisal/structured.json" + "?persist=" + (persist ? "yes" : "no");
            using HttpClient client = createClient();

            // Format the data so that I can easily send it later
            StringContent data = new StringContent(ar.toJson(), Encoding.UTF8, "application/json");

            Console.WriteLine(ar.toJson());

            // Send the post request and save the response to a variable
            HttpResponseMessage response = await client.PostAsync(url, data);

            // Read the response as a string and save the string to a variable
            string result = response.Content.ReadAsStringAsync().Result;

            // Print out the variable
            Console.WriteLine(result);
        }


        private static HttpClient createClient() {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET/6.0 (+Discord: SirPirate#0001 || GitHub: https://github.com/BritishPirate)");
            return client;
        }

        private static WebRequest createRequest(string url, bool persist = false) {
            // Create the request based on the URL, and add the parameter for persist (This tells the API whether or not to keep this request alive)
            WebRequest request = WebRequest.Create(url);

            // Sets a header in the request indicating the contact details so that the reciever can contact me when I inevitably fuck up and accidentally abuse their API
            request.Headers["User-Agent"] = "Discord: SirPirate#0001 || GitHub: https://github.com/BritishPirate";
            
            return request;
        }
        
    }
}
