using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace EveIndustrialSpreadsheet.AppraisalRequestPack {
    [Serializable()]
    class AppraisalRequest {
        public string market_name { get; set; }
        public List<AppraisalRequestItem> items { get; set; }
            
        public AppraisalRequest() {
            market_name = "";
            items = new List<AppraisalRequestItem>();
        }

        public AppraisalRequest(Market market, List<AppraisalRequestItem> items) {
            this.market_name = market.ToString().ToLower();
            this.items = items;
        }

        public AppraisalRequest(Market market, AppraisalRequestItem[] items) {
            this.market_name = market.ToString().ToLower();
            this.items = items.OfType<AppraisalRequestItem>().ToList();
        }

        public string toJson() {
            return JsonSerializer.Serialize(this);
        }
    }
}
