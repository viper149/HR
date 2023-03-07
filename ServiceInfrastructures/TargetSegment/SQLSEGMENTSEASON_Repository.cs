﻿using DenimERP.Data;
using DenimERP.Models;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces.TargetSegment;

namespace DenimERP.ServiceInfrastructures.TargetSegment
{
    public class SQLSEGMENTSEASON_Repository : BaseRepository<SEGMENTSEASON>, ISEGMENTSEASON
    {
        public SQLSEGMENTSEASON_Repository(DenimDbContext denimDbContext) : base(denimDbContext)
        {
        }
    }
}
