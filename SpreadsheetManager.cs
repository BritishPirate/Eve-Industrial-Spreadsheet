using Google.Apis.Sheets.v4.Data;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EveIndustrialSpreadsheet.AppraisalPack.EvepraisalRoot;

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

        /// <summary>
        /// Places the items from the appraisal list in the sheet
        /// </summary>
        /// <param name="appraisal">The appraisal</param>
        /// <param name="sheet"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public async Task updatePriceDetails(Appraisal appraisal, string sheet, string range) {
            IList<IList<Object>> updatedList = new List<IList<Object>>();

            var priceList = appraisal.prices();

            ValueRange valueRange = new ValueRange();
            valueRange.Range = sheet + "!" + range;
            valueRange.Values = toObjList(priceList);

            await googleApi.updateValues(valueRange);
        }

        public async Task<List<string>?> getColumn(string sheetName, char columnNotation) {
            List<List<string>>? values = await googleApi.getValueStrings(sheetName + "!" + columnNotation + ":" + columnNotation);
            if(values == null) return null;
            List<string> column = new List<string>();
            foreach(List<string> row in values) {
                column.Add(row.Count > 0 ? row[0] : "");
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

        private static IList<IList<Object>> toObjList(List<List<string>> floatList) {
            IList<IList<Object>> ret = new List<IList<Object>>();
            for(int i = 0;i < floatList.Count;i++) {
                var objList = new List<Object>();
                foreach(string s in floatList[i]) {
                    objList.Add(s);
                }
                ret.Add(objList);
            }
            return ret;
        }

        private static IList<IList<Object>> toObjList(List<List<float>> floatList) {
            IList<IList<Object>> ret = new List<IList<Object>>();
            for(int i = 0;i < floatList.Count;i++) {
                var objList = new List<Object>();
                foreach(float f in floatList[i]) {
                    objList.Add(f);
                }
                ret.Add(objList);
            }
            return ret;
        }
    }
}
