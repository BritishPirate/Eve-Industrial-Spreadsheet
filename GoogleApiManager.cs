﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;


namespace EveIndustrialSpreadsheet {
    internal class GoogleApiManager {
        // The service that will be used to interact with google sheets
        SheetsService service;

        // The ID of the desired spreadsheet
        string spreadsheetId = "12xbQMm4b2iFCjc1ABP4sVCog_7FL4FD6EyZkBN0PAqM";

        /// <summary>
        /// Authenticates the app with OAuth2 with google using my client ID and Secret
        /// </summary>
        public async Task authenticateApp() {
            // The credentials necessary for authentication.
            UserCredential credential;

            // Uses a new file stream to read the Client Secrets json file which has the needed credentials.
            using(var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read)) {
                
                // Requests the specific necessary scope, in this case it asks for access to all spreadsheets. 
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets }, // TODO: Figure out how to get access to a specific spreadsheet rather than all spreadsheets.
                    "user", CancellationToken.None, new FileDataStore("Spreadsheets.MySpreadsheets"));
            }

            // Setup the service to interact with google sheets
            service = new SheetsService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = "Eve Industrial Spreadsheet"
            });
        }

        /// <summary>
        /// This sends a get request to the google API and returns a spreadsheet with a specific range.
        /// </summary>
        /// <param name="range">The range to be returned. Formatted as spreadsheet formatting. (SheetName!A1:C5)</param>
        /// <param name="includeGridData">Determines whether the data inside the cells will be returned (False by default)</param>
        /// <returns></returns>
        public async Task<Spreadsheet> getSheet(string range, bool includeGridData = false) {
            List<string> ranges = new List<string>();
            ranges.Add(range);
            return await getSheet(ranges, includeGridData);
        }

        /// <summary>
        /// This sends a get request to the google API and returns a spreadsheet with a specific set of ranges.
        /// </summary>
        /// <param name="ranges">The ranges to be returned. Formatted as spreadsheet formatting. (SheetName!A1:C5)</param>
        /// <param name="includeGridData">Determines whether the data inside the cells will be returned (False by default)</param>
        /// <returns></returns>
        public async Task<Spreadsheet> getSheet(List<string> ranges, bool includeGridData = false) {
            var getSheetRequest = service.Spreadsheets.Get(spreadsheetId);
            getSheetRequest.Ranges = ranges;
            getSheetRequest.IncludeGridData = includeGridData;

            // Get a specific spreadsheet
            Spreadsheet sheet = await getSheetRequest.ExecuteAsync();
            return sheet;
        }

        /// <summary>
        /// A function to update the values in the spreadsheet.
        /// </summary>
        /// <param name="data">The new range/values pair to update in the spreadsheet</param>
        /// <param name="valueInputOption">How the input data should be interpreted. Optiosn are: "RAW", "USER_Entered", "INPUT_VALUE_OPTION_UNSPECIFIED" ("RAW" By default)</param>
        /// <returns>The response to the request</returns>
        public async Task<BatchUpdateValuesResponse> updateValues(ValueRange data, string valueInputOption = "RAW") {
            List<ValueRange> dataList = new List<ValueRange>();
            dataList.Add(data);
            return await updateValues(dataList, valueInputOption);
        }

        /// <summary>
        /// A function to update the values in the spreadsheet.
        /// </summary>
        /// <param name="dataList">A list of range/value pairs to be updated in the spreadsheet.</param>
        /// <param name="valueInputOption">How the input data should be interpreted. Optiosn are: "RAW", "USER_Entered", "INPUT_VALUE_OPTION_UNSPECIFIED" ("RAW" By default)</param>
        /// <returns>The response to the request</returns>
        public async Task<BatchUpdateValuesResponse> updateValues(List<ValueRange> dataList, string valueInputOption = "RAW") {

            BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();

            requestBody.ValueInputOption = valueInputOption;
            requestBody.Data = dataList;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = service.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);

            BatchUpdateValuesResponse response = await request.ExecuteAsync();

            return response;
        }
    }
}
