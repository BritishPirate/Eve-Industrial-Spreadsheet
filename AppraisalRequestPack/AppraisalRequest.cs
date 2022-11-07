using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace EveIndustrialSpreadsheet.AppraisalRequestPack
{
    struct AppraisalRequest {
        #region attributes
        public Market? market_name { get; set; }
        public List<AppraisalRequestItem> items { get; set; }
        #endregion

        #region constructors
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
        internal struct AppraisalRequestItem {
            #region attributes
            public string name { get; set; }
            // int type_id;
            #endregion

            #region constructors
            public AppraisalRequestItem(string name) {
                this.name = name;
                //this.type_id = type_id;
            }
            #endregion
        }
        #endregion
    }
}
