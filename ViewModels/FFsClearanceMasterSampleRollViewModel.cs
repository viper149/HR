using System;
using System.Collections.Generic;
using DenimERP.Models;

namespace DenimERP.ViewModels
{
    public class FFsClearanceMasterSampleRollViewModel
    {
        public FFsClearanceMasterSampleRollViewModel()
        {
            FFsClearanceMasterSampleRoll = new F_FS_CLEARANCE_MASTER_SAMPLE_ROLL
            {
                MSRDATE = DateTime.Now,
                MAILDATE = DateTime.Now
            };
        }

        public F_FS_CLEARANCE_MASTER_SAMPLE_ROLL FFsClearanceMasterSampleRoll { get; set; }
        public PL_PRODUCTION_SETDISTRIBUTION PlProductionSetdistribution { get; set; }

        public List<PL_PRODUCTION_SETDISTRIBUTION> PlProductionSetdistributionsList { get; set; }
        public List<F_FS_CLEARANCE_ROLL_TYPE> FFsClearanceRollTypeList { get; set; }
        public List<F_FS_CLEARANCE_WASH_TYPE> FFsClearanceWashTypeList { get; set; }
        public List<F_PR_INSPECTION_PROCESS_DETAILS> FPrInspectionProcessDetailseList { get; set; }
    }
}
