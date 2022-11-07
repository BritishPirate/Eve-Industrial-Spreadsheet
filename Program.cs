using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using EveIndustrialSpreadsheet;
using static EveIndustrialSpreadsheet.AppraisalRequestPack.AppraisalRequest;
using EveIndustrialSpreadsheet.AppraisalRequestPack;
using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Books.v1;
using Google.Apis.Books.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Microsoft.Win32.SafeHandles;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using static EveIndustrialSpreadsheet.AppraisalPack.AppraisalRoot;
using EveIndustrialSpreadsheet.AppraisalPack;

internal class Program {
    private static SpreadSheetManager spreadsheetManager;
    public async static Task Main(string[] args) {
        try {
            spreadsheetManager = new SpreadSheetManager();
        }
        catch(AggregateException ex) {
            foreach(var e in ex.InnerExceptions) {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }


        string itemName = spreadsheetManager.getColumn("main", 'A').Result[0];
        Appraisal appraisal = await newAppraisal(itemName);
        string amarrBuy = appraisal.items[0].prices.buy.max.ToString();
        await spreadsheetManager.updatePriceDetails(appraisal, "main", "B1:D1");
    }

    private async static Task<Appraisal> newAppraisal(string itemName) {
        List<AppraisalRequestItem> appraisalRequestItems = new List<AppraisalRequestItem>();
        appraisalRequestItems.Add(new AppraisalRequestItem(itemName));
        AppraisalRequest appraisalRequest = new AppraisalRequest(Market.Amarr, appraisalRequestItems);
        string newReturn = await EvepraisalAPIManager.newAppraisal(appraisalRequest);
        return AppraisalRoot.fromJson(newReturn);
    }

    /*
    private async static Task<string> getItemToAppraise() {
        Spreadsheet spreadsheet = await googleApi.getSheet("Main!A1", true);
        string item = spreadsheet.Sheets[0].Data[0].RowData[0].Values[0].EffectiveValue.StringValue;
        return item;
    } // */

    /*
    private async static Task updateSpreadsheetValue(String data) {
        IList<IList<Object>> temp = new List<IList<Object>>();
        temp.Add(new List<Object>() { data });

        ValueRange ValueRange = new ValueRange();
        ValueRange.Range = "Main!B1";
        ValueRange.Values = temp;

        var response = await googleApi.updateValues(ValueRange);
    } // */

    /*
 *     public static async void Main(string[] args) {
    try {
        new GoogleApiManager().authenticateApp().Wait();
    }
    catch(AggregateException ex) {
        foreach(var e in ex.InnerExceptions) {
            Console.WriteLine("ERROR: " + e.Message);
        }
    }

    Spreadsheet spreadsheet = await googleApi.getSheet("Main!A1:A5", true);
    Console.WriteLine(JsonConvert.SerializeObject(spreadsheet));

    #region TempCommentedCode
    /*
    string getReturn = await EvepraisalAPIManager.getAppraisal("coyaw");
    Console.WriteLine(getReturn);

    List<AppraisalRequestItem> appraisalRequestItems = new List<AppraisalRequestItem>();
    appraisalRequestItems.Add(new AppraisalRequestItem("Rifter"));
    AppraisalRequest appraisalRequest = new AppraisalRequest(Market.Amarr, appraisalRequestItems);
    string newReturn = await EvepraisalAPIManager.newAppraisal(appraisalRequest);
    Console.WriteLine(newReturn);

    //*/

}