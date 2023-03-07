using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class AccExportDoMasterViewModel
    {
        public AccExportDoMasterViewModel()
        {
            cOM_EX_LCINFOs = new List<COM_EX_LCINFO>();
            cOM_EX_FABSTYLEs = new List<COM_EX_FABSTYLE>();
            aCC_EXPORT_DODETAILSList = new List<ACC_EXPORT_DODETAILS>();
            cOM_EX_PIMASTERs = new List<COM_EX_PIMASTER>();
            ComExPiDetailses = new List<COM_EX_PI_DETAILS>();

            ACC_EXPORT_DOMASTER = new ACC_EXPORT_DOMASTER
            {
                TRNSDATE = DateTime.Now
            };
            ACC_EXPORT_DODETAILS = new ACC_EXPORT_DODETAILS
            {
                TRNSDATE = DateTime.Now
            };
        }

        public ACC_EXPORT_DOMASTER ACC_EXPORT_DOMASTER { get; set; }
        public ACC_EXPORT_DODETAILS ACC_EXPORT_DODETAILS { get; set; }
        public COM_EX_LCINFO COM_EX_LCINFO { get; set; }
        public COM_EX_PIMASTER ComExPimaster { get; set; }

        public List<COM_EX_LCINFO> cOM_EX_LCINFOs { get; set; }
        public List<COM_EX_FABSTYLE> cOM_EX_FABSTYLEs { get; set; }
        public List<ACC_EXPORT_DODETAILS> aCC_EXPORT_DODETAILSList { get; set; }
        public List<ACC_EXPORT_DODETAILS> aCC_EXPORT_DODETAILs { get; set; }
        public List<COM_EX_PIMASTER> cOM_EX_PIMASTERs { get; set; }
        public List<COM_EX_PI_DETAILS> ComExPiDetailses { get; set; }

        public int RemoveIndex { get; set; }
        public bool IsDelete { get; set; }
    }
}
