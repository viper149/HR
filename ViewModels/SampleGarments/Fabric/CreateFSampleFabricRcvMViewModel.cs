using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.Fabric
{
    public class CreateFSampleFabricRcvMViewModel
    {
        public CreateFSampleFabricRcvMViewModel()
        {
            FSampleFabricRcvDs = new List<F_SAMPLE_FABRIC_RCV_D>();
            FBasSections = new List<F_BAS_SECTION>();
            FHrEmployees = new List<F_HRD_EMPLOYEE>();
            FSampleItemDetailses = new List<F_SAMPLE_ITEM_DETAILS>();
            RndFabricinfos = new List<RND_FABRICINFO>();
            PlProductionSetdistributions = new List<PL_PRODUCTION_SETDISTRIBUTION>();
        }

        public F_SAMPLE_FABRIC_RCV_M FSampleFabricRcvM { get; set; }
        public F_SAMPLE_FABRIC_RCV_D FSampleFabricRcvD { get; set; }

        public List<F_SAMPLE_FABRIC_RCV_D> FSampleFabricRcvDsT { get; set; }
        public List<F_SAMPLE_FABRIC_RCV_D> FSampleFabricRcvDs { get; set; }
        public List<F_BAS_SECTION> FBasSections { get; set; }
        public List<F_HRD_EMPLOYEE> FHrEmployees { get; set; }
        public List<F_SAMPLE_ITEM_DETAILS> FSampleItemDetailses { get; set; }
        public List<RND_FABRICINFO> RndFabricinfos { get; set; }
        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributions { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Production Date")]
        public DateTime? ProductionDate { get; set; }

        public bool IsDeletable { get; set; }
        public int RemoveIndex { get; set; }
    }
}
