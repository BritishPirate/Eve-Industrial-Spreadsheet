using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveIndustrialSpreadsheet {
    internal interface Appraisal {
        public List<List<float>> prices();
    }
}
