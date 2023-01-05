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
        public static async Task<string> getAppraisal(string appraisalID) {
            // Set the URL and create the HTTP client
            string url = "https://evepraisal.com/a/" + appraisalID + ".json";
            using HttpClient client = createClient();

            //Get the reply from the client's request
            string content = await client.GetStringAsync(url);

            // Print out the variable
            return content;
        }


        public static async Task<string> newAppraisal(EvepraisalRequest ar, bool persist = false) {
            // Set the URL and create the HTTP client (Persist is used to determine if the requests needs to stay existing)
            string url = "https://evepraisal.com/appraisal/structured.json" + "?persist=" + (persist ? "yes" : "no");
            using HttpClient client = createClient();

            // Format the data so that I can easily send it later
            StringContent data = new StringContent(ar.toJson(), Encoding.UTF8, "application/json");

            // Send the post request and save the response to a variable
            HttpResponseMessage response = await client.PostAsync(url, data);
                
            // Read the response as a string and save the string to a variable
            string result = response.Content.ReadAsStringAsync().Result;

            // Print out the variable
            return result;
        }


        private static HttpClient createClient() {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", ".NET/6.0 (+Discord: SirPirate#0001 || GitHub: https://github.com/BritishPirate)");
            return client;
        }
        
    }
}
