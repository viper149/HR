using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;
using Microsoft.AspNetCore.Http;

namespace DenimERP.ViewModels.Com.InvoiceImport
{
    public class ComImpInvoiceInfoCreateViewModel
    {
        public ComImpInvoiceInfoCreateViewModel()
        {
            ComImpInvoiceinfo = new COM_IMP_INVOICEINFO();
            ComImpInvdetails = new COM_IMP_INVDETAILS();
            ComImpInvdetailses = new List<COM_IMP_INVDETAILS>();
            ComImpLcinformations = new List<COM_IMP_LCINFORMATION>();
            BasTransportinfos = new List<BAS_TRANSPORTINFO>();
            BasProductinfos = new List<BAS_PRODUCTINFO>();
            FBasUnitses = new List<F_BAS_UNITS>();
            ComContainers = new List<COM_CONTAINER>();
            FChemStoreProductinfos = new List<F_CHEM_STORE_PRODUCTINFO>();
            BasYarnLotinfos = new List<BAS_YARN_LOTINFO>();
            CnFList = new List<COM_IMP_CNFINFO>();
        }

        public COM_IMP_INVOICEINFO ComImpInvoiceinfo { get; set; }
        public COM_IMP_INVDETAILS ComImpInvdetails { get; set; }
        public List<BAS_TRANSPORTINFO> BasTransportinfos { get; set; }
        public List<COM_IMP_INVDETAILS> ComImpInvdetailses { get; set; }
        public List<COM_IMP_LCINFORMATION> ComImpLcinformations { get; set; }
        public List<COM_IMP_DEL_STATUS> ComImpDelStatuses { get; set; }
        public List<BAS_PRODUCTINFO> BasProductinfos { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        public List<COM_CONTAINER> ComContainers { get; set; }
        public List<F_CHEM_STORE_PRODUCTINFO> FChemStoreProductinfos { get; set; }
        public List<BAS_YARN_LOTINFO> BasYarnLotinfos { get; set; }
        public List<COM_IMP_CNFINFO> CnFList { get; set; }


        [Display(Name = "Upload Invoice File")]
        public IFormFile INVPATH { get; set; }
        [Display(Name = "Upload B/L File")]
        public IFormFile BLPATH { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
