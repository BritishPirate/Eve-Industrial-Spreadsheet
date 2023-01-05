using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace EveIndustrialSpreadsheet.AppraisalRequestPack
{
    struct EvepraisalRequest {
        #region attributes
        public Market? market_name { get; set; }
        public List<EvepraisalRequestItem> items { get; set; }
        #endregion

        #region constructors
        public EvepraisalRequest() {
            market_name = null;
            items = new List<EvepraisalRequestItem>();
        }

        public EvepraisalRequest(Market market, List<EvepraisalRequestItem> items) {
            this.market_name = market;
            this.items = items;
        }

        public EvepraisalRequest(Market market, EvepraisalRequestItem[] items) {
            this.market_name = market;
            this.items = items.OfType<EvepraisalRequestItem>().ToList();
        }

        public EvepraisalRequest(Market market, List<string> items) {
            this.market_name = market;
            var list = new List<EvepraisalRequestItem>();
            foreach(string item in items) {
                list.Add(new EvepraisalRequestItem(item));
            }
            this.items = list;
        }
        public EvepraisalRequest(Market market, List<string> items, List<int> itemQuantities) {
            this.market_name = market;
            var list = new List<EvepraisalRequestItem>();
            for(int i = 0; i < items.Count; i++) {
                list.Add(new EvepraisalRequestItem(items[i], itemQuantities[i]));
            }
            this.items = list;
        }

        public EvepraisalRequest(Market market, string[] items) : this(market, items.OfType<string>().ToList()) {
            
        }

        #endregion

        #region methods
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
        #endregion

        #region internal classes
        [Serializable()]
        internal struct EvepraisalRequestItem {
            #region attributes
            public string name { get; set; }
            public int quantity { get; set; }
            #endregion

            #region constructors
            public EvepraisalRequestItem(string name, int quantity = 0) {
                this.name = name;
                this.quantity = quantity;
            }
            #endregion
        }
        #endregion
    }
}
