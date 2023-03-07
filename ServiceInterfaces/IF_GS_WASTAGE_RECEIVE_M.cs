﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels;

namespace DenimERP.ServiceInterfaces
{
    public interface IF_GS_WASTAGE_RECEIVE_M : IBaseService<F_GS_WASTAGE_RECEIVE_M>
    {
        Task<IEnumerable<F_GS_WASTAGE_RECEIVE_M>> GetAllFGsWastageReceiveAsync();
        Task<FGsWastageReceiveViewModel> GetInitObjByAsync(FGsWastageReceiveViewModel fGsWastageReceiveViewModel);

        Task<FGsWastageReceiveViewModel> FindByIdIncludeAllAsync(int wrId);
    }
}



