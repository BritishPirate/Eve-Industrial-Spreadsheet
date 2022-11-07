using Google.Apis.Sheets.v4.Data;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EveIndustrialSpreadsheet.AppraisalPack.AppraisalRoot;

namespace EveIndustrialSpreadsheet {
    internal class SpreadSheetManager {
        #region attributes
        private GoogleApiManager googleApi;
        #endregion

        #region constructors
        public SpreadSheetManager() {
            googleApi = new GoogleApiManager();
        }

        public SpreadSheetManager(GoogleApiManager googleApi) {
            this.googleApi = googleApi;
        }
        #endregion

        public async Task updatePriceDetails(Appraisal appraisal, string sheet, string range) {
            IList<IList<Object>> updatedList = new List<IList<Object>>();
            
            var priceList = new List<Object>();
            var prices = appraisal.items[0].prices;
            priceList.Add((String)prices.all.avg.ToString());
            priceList.Add((String)prices.sell.min.ToString());
            priceList.Add((String)prices.buy.max.ToString());

            updatedList.Add(priceList);

            ValueRange valueRange = new ValueRange();
            valueRange.Range = sheet + "!" + range;
            valueRange.Values = updatedList;

            await googleApi.updateValues(valueRange);
        }

        public async Task<List<string>?> getColumn(string sheetName, char columnNotation) {
            List<List<string>>? values = await googleApi.getValueStrings(sheetName + "!" + columnNotation + ":" + columnNotation);
            if(values == null) return null;
            List<string> column = new List<string>();
            foreach(List<string> row in values) {
                column.Add(row[0]);
            }
            return column;
        }

        public async Task updateSpreadsheetValue(String data) {
            IList<IList<Object>> temp = new List<IList<Object>>();
            temp.Add(new List<Object>() { data });

            ValueRange ValueRange = new ValueRange();
            ValueRange.Range = "Main!B1:D1";
            ValueRange.Values = temp;

            var response = await googleApi.updateValues(ValueRange);
        }
    }
}
