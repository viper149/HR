using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.Receive
{
    public class CreateFSampleGarmentRcvMViewModel
    {
        public CreateFSampleGarmentRcvMViewModel()
        {
            FSampleGarmentRcvDs = new List<F_SAMPLE_GARMENT_RCV_D>();
            FSampleItemDetailses = new List<F_SAMPLE_ITEM_DETAILS>();
            RndFabricinfos = new List<RND_FABRICINFO>();
            BasColors = new List<BAS_COLOR>();
            FSampleLocations = new List<F_SAMPLE_LOCATION>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
            FHrEmployees = new List<F_HRD_EMPLOYEE>();
            FBasSections = new List<F_BAS_SECTION>();
        }

        public F_SAMPLE_GARMENT_RCV_M FSampleGarmentRcvM { get; set; }
        public F_SAMPLE_GARMENT_RCV_D FSampleGarmentRcvD { get; set; }
        public List<F_SAMPLE_GARMENT_RCV_D> FSampleGarmentRcvDs { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_SAMPLE_ITEM_DETAILS> FSampleItemDetailses { get; set; }
        public List<RND_FABRICINFO> RndFabricinfos { get; set; }
        public List<BAS_COLOR> BasColors { get; set; }
        public List<F_SAMPLE_LOCATION> FSampleLocations { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
        [Display(Name = "Production Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ProductionDate { get; set; }
    }
}
