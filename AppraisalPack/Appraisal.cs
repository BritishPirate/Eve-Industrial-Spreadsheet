using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EveIndustrialSpreadsheet.AppraisalRequestPack;

namespace EveIndustrialSpreadsheet.AppraisalPack
{
    struct AppraisalRoot
    {
        #region attributes
        public Appraisal appraisal { get; set; }
        #endregion

        #region constructors
        public AppraisalRoot(Appraisal appraisal)
        {
            this.appraisal = appraisal;
        }
        #endregion

        #region methods
        public static Appraisal fromJson(string json)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
            };
            return JsonSerializer.Deserialize<AppraisalRoot>(json, options)!.appraisal;
        }
        #endregion

        #region internal classes
        /// <summary>
        /// This class exists to unpack the appraisal JSON.
        /// </summary>
        [Serializable()]
        public struct Appraisal
        {
            #region attributes
            public int? created { get; set; }
            public string? kind { get; set; }
            public Market? market_name { get; set; }
            public Totals? totals { get; set; }
            public List<AppraisalItem>? items { get; set; }
            public string? raw { get; set; }
            public string? unparsed { get; set; }
            /// <summary>
            /// The property called private on the Json. Name changed to avoid conflicts
            /// </summary>
            public bool? @private { get; set; }
            public bool? live { get; set; }
            #endregion

            #region constructors
            public Appraisal()
            {
                created = null;
                kind = null;
                market_name = null;
                totals = null;
                items = null;
                raw = null;
                unparsed = null;
                @private = null;
                live = null;
            }

            public Appraisal(int created, string kind, Market marketName, Totals totals, List<AppraisalItem> items, string raw, string unparsed, bool @private, bool live)
            {
                this.created = created;
                this.kind = kind;
                market_name = marketName;
                this.totals = totals;
                this.items = items;
                this.raw = raw;
                this.unparsed = unparsed;
                this.@private = @private;
                this.live = live;
            }
            #endregion

            #region methods
            public string toJson()
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters =
                        {
                        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                    }
                };
                AppraisalRoot root = new AppraisalRoot(this);
                return JsonSerializer.Serialize(root, options);
            }
            #endregion

            #region internal classes
            [Serializable()]
            public struct AppraisalItem
            {
                #region attributes
                public string? name { get; set; }
                public int? typeId { get; set; }
                public string? typeName { get; set; }
                public int? typeVolume { get; set; }
                public int? quantity { get; set; }
                public Prices? prices { get; set; }
                public Meta? meta { get; set; }
                #endregion

                #region constructors
                public AppraisalItem(string name, int typeId, string typeName, int typeVolume, int quantity, Prices prices, Meta meta)
                {
                    this.name = name;
                    this.typeId = typeId;
                    this.typeName = typeName;
                    this.typeVolume = typeVolume;
                    this.quantity = quantity;
                    this.prices = prices;
                    this.meta = meta;
                }
                public AppraisalItem()
                {
                    name = null;
                    typeId = null;
                    typeName = null;
                    typeVolume = null;
                    quantity = null;
                    prices = null;
                    meta = null;
                }
                #endregion

                #region internal classes
                [Serializable()]
                public struct Prices
                {
                    #region attributes
                    public PriceStats? all { get; set; }
                    public PriceStats? buy { get; set; }
                    public PriceStats? sell { get; set; }
                    public DateTime? updated { get; set; }
                    public string? strategy { get; set; }
                    #endregion

                    #region constructors
                    public Prices()
                    {
                        all = null;
                        buy = null;
                        sell = null;
                        updated = null;
                        strategy = null;
                    }

                    public Prices(PriceStats all, PriceStats buy, PriceStats sell, DateTime updated, string strategy)
                    {
                        this.all = all;
                        this.buy = buy;
                        this.sell = sell;
                        this.updated = updated;
                        this.strategy = strategy;
                    }
                    #endregion

                    #region internal classes
                    [Serializable()]
                    public struct PriceStats
                    {
                        #region attributes
                        public float? avg { get; set; }
                        public float? max { get; set; }
                        public float? median { get; set; }
                        public float? min { get; set; }
                        public float? percentile { get; set; }
                        public float? stddev { get; set; }
                        public int? volume { get; set; }
                        public int? orderCount { get; set; }
                        #endregion

                        #region constructors
                        public PriceStats()
                        {
                            avg = null;
                            max = null;
                            median = null;
                            min = null;
                            percentile = null;
                            stddev = null;
                            volume = null;
                            orderCount = null;
                        }

                        public PriceStats(float avg, float max, float median, float min, float percentile, float stddev, int volume, int orderCount)
                        {
                            this.avg = avg;
                            this.max = max;
                            this.median = median;
                            this.min = min;
                            this.percentile = percentile;
                            this.stddev = stddev;
                            this.volume = volume;
                            this.orderCount = orderCount;
                        }
                        #endregion
                    }
                    #endregion
                }

                [Serializable()]
                public struct Meta
                {
                    #region constructors
                    public Meta()
                    {

                    }
                    #endregion
                }
                #endregion
            }

            [Serializable()]
            public struct Totals
            {
                #region attributes
                int? buy { get; set; }
                int? sell { get; set; }
                int? volume { get; set; }
                #endregion

                #region constructors
                public Totals()
                {
                    buy = null;
                    sell = null;
                    volume = null;
                }

                public Totals(int buy, int sell, int volume)
                {
                    this.buy = buy;
                    this.sell = sell;
                    this.volume = volume;
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
