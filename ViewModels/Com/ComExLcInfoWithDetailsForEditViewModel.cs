using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DenimERP.ViewModels.Com
{
    public class ComExLcInfoWithDetailsForEditViewModel
    {
        public ComExLcInfoWithDetailsForEditViewModel()
        {
            bAS_BEN_BANK_MASTERs = new List<BAS_BEN_BANK_MASTER>();
            bAS_BUYER_BANK_MASTERs = new List<BAS_BUYER_BANK_MASTER>();
            bAS_BUYER_BANK_MASTERs = new List<BAS_BUYER_BANK_MASTER>();
            bAS_BUYERINFOs = new List<BAS_BUYERINFO>();
            bAS_TEAMINFOs = new List<BAS_TEAMINFO>();
            cOM_EX_PIMASTERs = new List<COM_EX_PIMASTER>();
            comExPIViewModels = new List<ComExPIViewModel>();
            comExPIViewModelsToDisplay = new List<ComExPIViewModel>();
            cOM_EX_LCDETAILs = new List<COM_EX_LCDETAILS>();
            comExPIViewModel = new ComExPIViewModel();
            ComTenors = new List<COM_TENOR>();
        }

        public COM_EX_LCINFO cOM_EX_LCINFO { get; set; }
        public ComExPIViewModel comExPIViewModel { get; set; }
        public List<ComExPIViewModel> comExPIViewModels { get; set; }
        public List<ComExPIViewModel> comExPIViewModelsToDisplay { get; set; }
        public List<BAS_BEN_BANK_MASTER> bAS_BEN_BANK_MASTERs { get; set; }
        public List<BAS_BUYER_BANK_MASTER> bAS_BUYER_BANK_MASTERs { get; set; }
        public List<BAS_BUYERINFO> bAS_BUYERINFOs { get; set; }
        public List<BAS_TEAMINFO> bAS_TEAMINFOs { get; set; }
        public List<COM_EX_PIMASTER> cOM_EX_PIMASTERs { get; set; }
        public List<COM_EX_LCDETAILS> cOM_EX_LCDETAILs { get; set; }
        public List<COM_TENOR> ComTenors { get; set; }

        public SelectList Currency { get; set; }
        public SelectList Status { get; set; }

        [Display(Name = "Upload UD File")]
        public IFormFile UDFILEUPLOAD { get; set; }
        [Display(Name = "Upload UP File")]
        public IFormFile UPFILEUPLOAD { get; set; }
        [Display(Name = "Upload Cost Sheet File")]
        public IFormFile COSTSHEETFILEUPLOAD { get; set; }
        [Display(Name = "Upload Proforma Invoice")]
        public IFormFile PiFile { get; set; }
        [Display(Name = "Upload Master LC File")]
        public IFormFile MLCFILEUPLOAD { get; set; }
        [Display(Name = "Upload LC File")]
        public IFormFile LCFILEUPLOAD { get; set; }
    }
}
