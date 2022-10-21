using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EveIndustrialSpreadsheet.AppraisalRequestPack {
    [Serializable()]
    class AppraisalRequestItem {
        public string name { get; set; }
       // int type_id;

        public AppraisalRequestItem(string name) {
            this.name = name;
            //this.type_id = type_id;
        }
    }
}
