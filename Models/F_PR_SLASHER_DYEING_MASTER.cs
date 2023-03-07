using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using DenimERP.Models.BaseModels;

namespace DenimERP.Models
{
    public partial class F_PR_SLASHER_DYEING_MASTER : BaseEntity
    {
        public F_PR_SLASHER_DYEING_MASTER()
        {
            F_PR_SLASHER_CHEM_CONSM = new HashSet<F_PR_SLASHER_CHEM_CONSM>();
            F_PR_SLASHER_DYEING_DETAILS = new HashSet<F_PR_SLASHER_DYEING_DETAILS>();
        }

        public int SLID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        public DateTime? TRNSDATE { get; set; }
        [Required]
        public int? SETID { get; set; }
        public double? BEAM_SPACE { get; set; }
        public double? PICK_UP { get; set; }
        public double? TOTAL_ENDS { get; set; }
        public double? ACTUAL_ENDS { get; set; }
        public string REMARKS { get; set; }
        public bool CLOSE_STATUS { get; set; }
        public string OPT1 { get; set; }
        public string OPT2 { get; set; }
        public string OPT3 { get; set; }


        [NotMapped]
        public string DateFormat
        {
            get
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper();
                return !string.IsNullOrEmpty(dateTimeFormat) ? dateTimeFormat : "DD-MMM-YY";
            }
        }

        public PL_PRODUCTION_SETDISTRIBUTION SET { get; set; }
        public ICollection<F_PR_SLASHER_CHEM_CONSM> F_PR_SLASHER_CHEM_CONSM { get; set; }
        public ICollection<F_PR_SLASHER_DYEING_DETAILS> F_PR_SLASHER_DYEING_DETAILS { get; set; }
    }
}
