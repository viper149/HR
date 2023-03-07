using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DenimERP.Models;
using DenimERP.ServiceInterfaces.BaseInterfaces;
using DenimERP.ViewModels.Marketing;

namespace DenimERP.ServiceInterfaces.Marketing
{
    public interface IMKT_SDRF_INFO : IBaseService<MKT_SDRF_INFO>
    {
        Task<IEnumerable<MKT_SDRF_INFO>> GetMktSdrfAllAsync();
        Task<int> GetSdrfNumberAsync();
        Task<bool> FindBySdrfNoInUseAsync(string sdrfNo);
        Task<bool> GetAvailableDate(DateTime date);
        Task<MktSdrfInfoViewModel> GetInitObjects(MktSdrfInfoViewModel mktSdrfInfoViewModel);
    }
}
