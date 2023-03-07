using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;
using Microsoft.AspNetCore.Http;

namespace DenimERP.ViewModels.Com
{
    public class ComImpLcInformationForCreateViewModel
    {
        public ComImpLcInformationForCreateViewModel()
        {
            bAS_PRODCATEGORies = new List<BAS_PRODCATEGORY>();
            cOM_IMP_LCDETAILs = new List<COM_IMP_LCDETAILS>();
            bAS_SUPPLIERINFOs = new List<BAS_SUPPLIERINFO>();
            bAS_BEN_BANK_MASTERs = new List<BAS_BEN_BANK_MASTER>();
            bAS_BUYER_BANK_MASTERs = new List<BAS_BUYER_BANK_MASTER>();
            bAS_INSURANCEINFOs = new List<BAS_INSURANCEINFO>();
            cOM_IMP_LCINFORMATION = new COM_IMP_LCINFORMATION();
            cOM_IMP_LCDETAILS = new COM_IMP_LCDETAILS();
            ComTenors = new List<COM_TENOR>();
            ComImpLctypes = new List<COM_IMP_LCTYPE>();
            BasProductinfos = new List<BAS_PRODUCTINFO>();
            FBasUnitses = new List<F_BAS_UNITS>();
        }

        public COM_IMP_LCINFORMATION cOM_IMP_LCINFORMATION { get; set; }
        public COM_IMP_LCDETAILS cOM_IMP_LCDETAILS { get; set; }
        public List<BAS_PRODCATEGORY> bAS_PRODCATEGORies { get; set; }
        public List<COM_IMP_LCDETAILS> cOM_IMP_LCDETAILs { get; set; }
        public List<BAS_SUPPLIERINFO> bAS_SUPPLIERINFOs { get; set; }
        public List<BAS_BEN_BANK_MASTER> bAS_BEN_BANK_MASTERs { get; set; }
        public List<BAS_BUYER_BANK_MASTER> bAS_BUYER_BANK_MASTERs { get; set; }
        public List<BAS_INSURANCEINFO> bAS_INSURANCEINFOs { get; set; }
        public List<COM_TENOR> ComTenors { get; set; }
        public List<COM_IMP_LCTYPE> ComImpLctypes { get; set; }
        public List<BAS_PRODUCTINFO> BasProductinfos { get; set; }
        public COM_EX_LCINFO ComExLcinfo { get; set; }
        public List<COM_EX_LCINFO> ComExLcinfos { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        
        [Display(Name = "Upload L/C File")]
        public IFormFile LCPATH { get; set; }
        [Display(Name = "Upload PI File")]
        public IFormFile PIPATH { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
