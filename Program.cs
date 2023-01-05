using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using EveIndustrialSpreadsheet;
using static EveIndustrialSpreadsheet.AppraisalRequestPack.EvepraisalRequest;
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
using static EveIndustrialSpreadsheet.AppraisalPack.EvepraisalRoot;
using EveIndustrialSpreadsheet.AppraisalPack;
using System.Linq;

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


        await appraiseSheet();
    }

    private async static Task appraiseSheet() {
        //Get values from spreadhseet
        var itemNames = await spreadsheetManager.getColumn("main", 'A');
        var temp = await spreadsheetManager.getColumn("main", 'B');
        temp.RemoveAt(0); // Remove the first thing for titles
        var itemQuantities = temp.Select(int.Parse).ToList();
        itemNames.RemoveAt(0); // Remove the first thing for titles
        
        

        //Get appraisal from evepraisal
        var appraisalRequest = new EvepraisalRequest(Market.Amarr, itemNames, itemQuantities);
        string appraisalJson = await EvepraisalAPIManager.newAppraisal(appraisalRequest);
        Appraisal appraisal = EvepraisalRoot.fromJson(appraisalJson);

        //Re-sort Evepraisal's auto sorter >,>
        appraisal = fixEvepraisal((Evepraisal)appraisal, appraisalRequest);

        // Update the sheet
        int start = 2;
        await spreadsheetManager.updatePriceDetails(appraisal, "main", "C" + start + ":F" + (start + appraisal.prices().Count - 1));
    }

    private async static Task<Evepraisal> newAppraisal(string itemName) {
        List<EvepraisalRequestItem> appraisalRequestItems = new List<EvepraisalRequestItem>();
        appraisalRequestItems.Add(new EvepraisalRequestItem(itemName));
        EvepraisalRequest appraisalRequest = new EvepraisalRequest(Market.Amarr, appraisalRequestItems);
        string newReturn = await EvepraisalAPIManager.newAppraisal(appraisalRequest);
        return EvepraisalRoot.fromJson(newReturn);
    }

    private static Appraisal fixEvepraisal(Evepraisal appraisal, EvepraisalRequest request) {
        var ogOrder = request.items;
        var appraisedList = appraisal.items;
        List<Evepraisal.AppraisalItem> newList = new List<Evepraisal.AppraisalItem>();
        foreach(var item in ogOrder) {
            foreach(var appraisedItem in appraisedList) {
                if(appraisedItem.typeName == item.name && appraisedItem.quantity == item.quantity) {
                    newList.Add(appraisedItem);
                    break;
                }
            }
        }
        appraisal.items = newList;
        return appraisal;
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

    List<EvepraisalRequestItem> appraisalRequestItems = new List<EvepraisalRequestItem>();
    appraisalRequestItems.Add(new EvepraisalRequestItem("Rifter"));
    EvepraisalRequest appraisalRequest = new EvepraisalRequest(Market.Amarr, appraisalRequestItems);
    string newReturn = await EvepraisalAPIManager.newAppraisal(appraisalRequest);
    Console.WriteLine(newReturn);

    //*/

}