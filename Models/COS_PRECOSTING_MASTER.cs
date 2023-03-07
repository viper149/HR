using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenimERP.Models
{
    public class COS_PRECOSTING_MASTER
    {
        public COS_PRECOSTING_MASTER()
        {
            RND_PURCHASE_REQUISITION_MASTER = new HashSet<RND_PURCHASE_REQUISITION_MASTER>();
            COS_PRECOSTING_DETAILS = new HashSet<COS_PRECOSTING_DETAILS>();
            COM_EX_PI_DETAILS = new HashSet<COM_EX_PI_DETAILS>();
        }

        [Display(Name = "Pre-Costing Id")]
        public int CSID { get; set; }
        [NotMapped]
        public string EncryptedId { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CSDATE { get; set; }
        [Display(Name = "Fabric Code")]
        public int FABCODE { get; set; }
        [Editable(false)]
        [Display(Name = "Color")]
        public int? COLORCODE { get; set; }
        [Display(Name = "Order Qty")]
        public double? ORDER_QTY { get; set; }
        [Display(Name = "Greige EPI")]
        public double? GREPI { get; set; }
        [Display(Name = "Greige PPI")]
        public double? GRPPI { get; set; }
        [Display(Name = "Finish EPI")]
        public double? FNEPI { get; set; }
        [Display(Name = "Finish PPI")]
        public double? FNPPI { get; set; }
        [Display(Name = "No of Ends")]
        public double? TOTALENDS { get; set; }
        [Display(Name = "Width")]
        public string WIDEC { get; set; }
        [Display(Name = "Weight")]
        public string WGDEC { get; set; }
        [Display(Name = "Weave")]
        public int? WID { get; set; }
        [Display(Name = "Reed Space")]
        public double? REED_SPACE { get; set; }
        [Display(Name = "Finish Route")]
        public string FINISH_ROUTE { get; set; }
        [Display(Name = "Composition")]
        public string COMPOSITION { get; set; }
        [Display(Name = "Certification")]
        public int? CID { get; set; }
        [Display(Name = "Certification Cost")]
        public double? CERTIFICATE_COST { get; set; }
        [Display(Name = "Tenor")]
        public int? TID { get; set; }
        [Display(Name = "Finish Type")]
        public int? FINID { get; set; }
        [Display(Name = "Finish Cost")]
        public double? FINISHCOST { get; set; }
        [Display(Name = "Surplus Qty")]
        public double? SURPLUS_QTY{ get; set; }
        [Display(Name = "Loom Type")]
        public string LOOMTYPE { get; set; }
        [Display(Name = "Loom Speed")]
        public double? LOOMSPEED { get; set; }
        [Display(Name = "Efficiency")]
        public double? EFFICIENCY { get; set; }                           
        [Display(Name = "Commission")]
        public double? COMMISSION { get; set; }                         
        [Display(Name = "Interest")]
        public double? INTEREST { get; set; }                           
        [Display(Name = "Finish Cost")]
        public int? FCID { get; set; }                                    
        [Display(Name = "Standard Cost")]
        public int? SCID { get; set; }                                    
        [Display(Name = "Ad. Monthly Cost")]
        public double? ADJ_MONTHLYCOST { get; set; }                      
        [Display(Name = "Ad. Production")]
        public double? ADJ_PRODUCTION { get; set; }                       
        [Display(Name = "Rate")]
        public double? RATE { get; set; }                                 
        [Display(Name = "Ad. Overhead($)")]
        public double? ADJ_OVERHEAD_USD { get; set; }                     
        [Display(Name = "Option 1")]
        public string OPTION1 { get; set; }                              
        [Display(Name = "Option 2")]
        public string OPTION2 { get; set; }
        [Display(Name = "Option 3")]
        public string OPTION3 { get; set; }
        [Display(Name = "Remarks")]
        public string REMARKS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Created At")]
        public DateTime? CREATED_AT { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Updated At")]
        public DateTime? UPDATED_AT { get; set; }
        [Display(Name = "Author")]
        public string USERID { get; set; }
        [Display(Name = "CFR Cost")]
        public double? CFRCOST { get; set; }
        [Display(Name = "Com. Sample Cost")]
        public double? COMSAMPLECOST { get; set; }
        [Display(Name = "Rejection Rate")]
        public double? REJECTION { get; set; }



        [Display(Name = "Monthly Cost")]
        public double? MONTHLY_COST { get; set; }
        [Display(Name = "Prod.")]
        public double? PROD { get; set; }
        [Display(Name = "Overhead(৳)")]
        public double? OVERHEAD_BDT { get; set; }
        [Display(Name = "Convert Rate")]
        public double? CONV_RATE { get; set; }
        [Display(Name = "Overhead($)")]
        public double? OVERHEAD_USD { get; set; }
        [Display(Name = "Loomspeed")]
        public double? SLOOMSPEED { get; set; }
        [Display(Name = "Efficiency")]
        public double? SEFFICIENCY { get; set; }
        [Display(Name = "Greige PPI")]
        public double? SGRPPI { get; set; }
        [Display(Name = "Contraction")]
        public double? CONTRACTION { get; set; }


        [Display(Name = "Dyeing Cost")]
        public double? DYEINGCOST { get; set; }
        [Display(Name = "UP Charge")]
        public double? UPCHARGE { get; set; }
        [Display(Name = "Printing Cost")]
        public double? PRINTINGCOST { get; set; }
        [Display(Name = "Overhead Cost")]
        public double? OVERHEADCOST { get; set; }
        [Display(Name = "Sample Cost")]
        public double? SAMPLECOST { get; set; }
        [Display(Name = "Sizing Cost")]
        public double? SIZINGCOST { get; set; }
        

        [Display(Name = "Author")]
        [NotMapped]
        public string USERNAME { get; set; }
        [NotMapped]
        [Display(Name = "CS Date")]
        public string CSDATESTRING { get; set; }
        [NotMapped]
        [Display(Name = "Color")]
        public string COLORNAME { get; set; }
        [NotMapped]
        public bool HasAccess { get; set; }

        public COS_STANDARD_CONS CosStandardCons { get; set; }
        public COS_FIXEDCOST CosFixedCost { get; set; }
        public BAS_COLOR Color { get; set; }
        public RND_WEAVE Weave { get; set; }
        public RND_FINISHTYPE FinishType { get; set; }
        public RND_FABRICINFO FABCODENavigation { get; set; }
        public COS_CERTIFICATION_COST C { get; set; }
        public COM_TENOR T { get; set; }

        public ICollection<RND_PURCHASE_REQUISITION_MASTER> RND_PURCHASE_REQUISITION_MASTER { get; set; }
        public ICollection<COS_PRECOSTING_DETAILS> COS_PRECOSTING_DETAILS { get; set; }
        public ICollection<COM_EX_PI_DETAILS> COM_EX_PI_DETAILS { get; set; }

    }
}
