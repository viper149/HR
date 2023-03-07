using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;
using Microsoft.AspNetCore.Http;

namespace DenimERP.ViewModels.Com.InvoiceExport
{
    public class ComExInvoiceMasterCreateViewModel
    {
        public ComExInvoiceMasterCreateViewModel()
        {
            ComExInvdetails = new COM_EX_INVDETAILS();

            ComExLcinfos = new List<COM_EX_LCINFO>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            ComExInvdetailses = new List<COM_EX_INVDETAILS>();
            ComExFabstyles = new List<COM_EX_FABSTYLE>();
            ComExPiDetailses = new List<COM_EX_PI_DETAILS>();
        }

        public COM_EX_INVOICEMASTER ComExInvoicemaster { get; set; }
        public COM_EX_INVDETAILS ComExInvdetails { get; set; }
        public COM_EX_PI_DETAILS ComExPiDetails { get; set; }

        public List<COM_EX_INVDETAILS> ComExInvdetailses { get; set; }
        public List<COM_EX_LCINFO> ComExLcinfos { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public List<COM_EX_FABSTYLE> ComExFabstyles { get; set; }
        public List<COM_EX_PI_DETAILS> ComExPiDetailses { get; set; }

        public double? PreviousValue { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }

        [Display(Name = "Pending Day")]
        public double PENDINGDIFFERENCEDAYS { get; set; }
        [Display(Name = "Difference Days")]
        public double BADIFFERENCE { get; set; }
        [Display(Name = "L/C Tenor")]
        public string BADEFFEREDDAYS { get; set; }
        [Display(Name = "Bank File Upload")]
        public IFormFile BANKREFPATH { get; set; }
        [Display(Name = "Discrepancy File Upload")]
        public IFormFile DISCREPANCYPATH { get; set; }
        [Display(Name = "Payment File Upload")]
        public IFormFile PAYMENTPATH { get; set; }
        [Display(Name = "Acceptance File Upload")]
        public IFormFile BANKACCEPTPATH { get; set; }
    }
}
