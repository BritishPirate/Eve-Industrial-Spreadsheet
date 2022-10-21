using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using EveIndustrialSpreadsheet;
using EveIndustrialSpreadsheet.AppraisalRequestPack;
using System.Text.Json;
using System.Text.Json.Serialization;
// See https://aka.ms/new-console-template for more information


await EvepraisalAPIManager.getAppraisal("coyaw");

List<AppraisalRequestItem> appraisalRequestItems = new List<AppraisalRequestItem>();
appraisalRequestItems.Add(new AppraisalRequestItem("Rifter"));
AppraisalRequest appraisalRequest = new AppraisalRequest(Market.Amarr, appraisalRequestItems);
await EvepraisalAPIManager.newAppraisal(appraisalRequest);

/*
List<AppraisalRequestItem> appraisalRequestItems = new List<AppraisalRequestItem>();
appraisalRequestItems.Add(new AppraisalRequestItem("Rifter"));
AppraisalRequest appraisalRequest = new AppraisalRequest(Market.Amarr, appraisalRequestItems);
Console.WriteLine(appraisalRequest);
Console.WriteLine(appraisalRequest.toJson() + "\n" + JsonSerializer.Serialize(appraisalRequest));

EvepraisalAPIManager.newAppraisal(appraisalRequest);
*/
