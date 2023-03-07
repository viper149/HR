using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public partial class COM_IMP_LCDETAILS
    {
        public COM_IMP_LCDETAILS()
        {
            F_GEN_S_RECEIVE_MASTER = new HashSet<F_GEN_S_RECEIVE_MASTER>();
        }
        public int TRNSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "Date")]
        public DateTime TRNSDATE { get; set; }
        [Display(Name = "L/C No")]
        public string LCNO { get; set; }
        public int? LC_ID { get; set; }
        [Required(ErrorMessage = "The field {0} can not be empty.")]
        [Display(Name = "PI No")]
        public string PINO { get; set; }
        [Display(Name = "H.S Code")]
        public string HSCODE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "PI Date")]
        public DateTime? PIDATE { get; set; }
        [Display(Name = "PI File")]
        public string PIPATH { get; set; }
        [Display(Name = "Product Name")]
        public int? PRODID { get; set; }
        [Display(Name = "Unit")]
        public int? UNIT { get; set; }
        [Display(Name = "Qty.")]
        public double? QTY { get; set; }
        [Display(Name = "Rate")]
        public double? RATE { get; set; }
        [Display(Name = "Total")]
        public double? TOTAL { get; set; }
        [Display(Name = "Author")]
        public string USRID { get; set; }

        public COM_IMP_LCINFORMATION LC { get; set; }
        public F_BAS_UNITS F_BAS_UNITS { get; set; }
        public BAS_PRODUCTINFO BAS_PRODUCTINFO { get; set; }
        public ICollection<F_GEN_S_RECEIVE_MASTER> F_GEN_S_RECEIVE_MASTER { get; set; }
    }
}
