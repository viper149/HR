using System;
using System.ComponentModel.DataAnnotations;

namespace DenimERP.Models
{
    public partial class F_PR_FN_CHEMICAL_CONSUMPTION
    {

        public int TRNSID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TRNSDATE { get; set; }
        public int? FPMID { get; set; }
        public int? CHEM_PROD_ID { get; set; }
        public double? STOCK_QTY { get; set; }
        public string RECIPE { get; set; }
        public double? SUG_QTY { get; set; }
        public double? QTY { get; set; }
        public string REMARKS { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_AT { get; set; }

        public F_CHEM_STORE_PRODUCTINFO CHEM_PROD_ { get; set; }
        public F_PR_FINISHING_MACHINE_PREPARATION FPM { get; set; }
    }
}
