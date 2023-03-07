using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels.SampleGarments.HDispatch
{
    public class CreateHSampleDespatchMViewModel
    {
        public CreateHSampleDespatchMViewModel()
        {
            HSampleDespatchM = new H_SAMPLE_DESPATCH_M();
            BasBrandinfos = new List<BAS_BRANDINFO>();
            BasTeaminfos = new List<BAS_TEAMINFO>();
            HSampleReceivingDs = new List<H_SAMPLE_RECEIVING_D>();
            ExtendHSampleReceivingDViewModels = new List<ExtendHSampleReceivingDViewModel>();
            HSampleDespatchDs = new List<H_SAMPLE_DESPATCH_D>();
            FBasUnitses = new List<F_BAS_UNITS>();
            BasBuyerinfos = new List<BAS_BUYERINFO>();
        }

        public H_SAMPLE_DESPATCH_M HSampleDespatchM { get; set; }
        public H_SAMPLE_DESPATCH_D HSampleDespatchD { get; set; }
        public List<H_SAMPLE_DESPATCH_D> HSampleDespatchDs { get; set; }
        public List<BAS_BRANDINFO> BasBrandinfos { get; set; }
        public List<BAS_TEAMINFO> BasTeaminfos { get; set; }
        public List<H_SAMPLE_RECEIVING_D> HSampleReceivingDs { get; set; }
        public List<ExtendHSampleReceivingDViewModel> ExtendHSampleReceivingDViewModels { get; set; }
        public List<F_BAS_UNITS> FBasUnitses { get; set; }
        public List<BAS_BUYERINFO> BasBuyerinfos { get; set; }
        public int RemoveIndex { get; set; }
        public bool IsDeletable { get; set; }
    }
}
