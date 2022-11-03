using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace EveIndustrialSpreadsheet.AppraisalRequestPack {
    [Serializable()]
    struct AppraisalRequest {
        public Market? market_name { get; set; }
        public List<AppraisalRequestItem> items { get; set; }
            
        public AppraisalRequest() {
            market_name = null;
            items = new List<AppraisalRequestItem>();
        }

        public AppraisalRequest(Market market, List<AppraisalRequestItem> items) {
            this.market_name = market;
            this.items = items;
        }

        public AppraisalRequest(Market market, AppraisalRequestItem[] items) {
            this.market_name = market;
            this.items = items.OfType<AppraisalRequestItem>().ToList();
        }

        public string toJson() {
            var options = new JsonSerializerOptions {
                WriteIndented = true,
                Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
            };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
