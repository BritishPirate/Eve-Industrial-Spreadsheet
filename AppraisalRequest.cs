using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EveIndustrialSpreadsheet.AppraisalRequestPack.EvepraisalRequest;

namespace EveIndustrialSpreadsheet {
    internal interface AppraisalRequest {
        public AppraisalRequest newRequest(Market market, EvepraisalRequestItem[] items);
        public AppraisalRequest newRequest(Market market, List<EvepraisalRequestItem> items);
    }
}
