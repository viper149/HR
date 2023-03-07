using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_FABRIC_RETURN_RECEIVE_Repository : BaseRepository<F_FS_FABRIC_RETURN_RECEIVE>,
        IF_FS_FABRIC_RETURN_RECEIVE
    {

        private readonly IDataProtector _protector;

        public SQLF_FS_FABRIC_RETURN_RECEIVE_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }



        public async Task<IEnumerable<F_FS_FABRIC_RETURN_RECEIVE>> GetFFsFabricReturnReceive()
        {
            return await DenimDbContext.F_FS_FABRIC_RETURN_RECEIVE

                .Include(e => e.BUYER_)
                .Include(e => e.DO_NONavigation)
                .Include(e => e.FABCODENavigation)
                .Include(e => e.PI)
                .Select(d => new F_FS_FABRIC_RETURN_RECEIVE()
                {
                    RCVID = d.RCVID,
                    EncryptedId = _protector.Protect(d.RCVID.ToString()),
                    RCVDATE = d.RCVDATE,
                    DC_NO = d.DC_NO,
                    ROLL_QTY = d.ROLL_QTY,
                    QTY_YDS = d.QTY_YDS,
                    REMARKS = d.REMARKS,
                    CREATED_BY = d.CREATED_BY,
                    CREATED_AT = d.CREATED_AT,
                    UPDATED_BY = d.UPDATED_BY,
                    BUYER_ = new BAS_BUYERINFO()
                    {
                        BUYER_NAME = d.BUYER_.BUYER_NAME
                    },

                    DO_NONavigation = new ACC_EXPORT_DOMASTER()
                     {
                         DONO = d.DO_NONavigation.DONO
                    },

                    FABCODENavigation = new RND_FABRICINFO()
                    {
                        STYLE_NAME = d.FABCODENavigation.STYLE_NAME
                    },
                    PI = new COM_EX_PIMASTER()
                    {
                        PINO = d.PI.PINO
                    }

                }).ToListAsync();
    }

        public async Task<FFsFabricReturnReceiveViewModel> GetInitObjByAsync(FFsFabricReturnReceiveViewModel ffsfabricViewModel)
        {
            ffsfabricViewModel.BuyerList = await DenimDbContext.BAS_BUYERINFO
                .Select(d => new BAS_BUYERINFO
                {
                    BUYERID = d.BUYERID,
                    BUYER_NAME = d.BUYER_NAME
                }).OrderBy(d => d.BUYER_NAME).ToListAsync();

            ffsfabricViewModel.FabList = await DenimDbContext.RND_FABRICINFO
                .Select(d => new RND_FABRICINFO
                {
                    FABCODE = d.FABCODE,
                    STYLE_NAME = d.STYLE_NAME
                }).OrderBy(d => d.STYLE_NAME).ToListAsync();


            ffsfabricViewModel.DoList = await DenimDbContext.ACC_EXPORT_DOMASTER
               .Select(d => new ACC_EXPORT_DOMASTER
               {
                   TRNSID = d.TRNSID,
                   DONO = d.DONO
               }).OrderBy(d => d.DONO).ToListAsync();
            
            ffsfabricViewModel.PiList = await DenimDbContext.COM_EX_PIMASTER
               .Select(d => new COM_EX_PIMASTER
               {
                   PIID = d.PIID,
                   PINO = d.PINO
               }).OrderBy(d => d.PINO).ToListAsync();


            return ffsfabricViewModel;
        }


        public async Task<IEnumerable<COM_EX_PIMASTER>> GetStyleByPi(int pi)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_PIMASTER
                    .Include(c => c.COM_EX_PI_DETAILS)
                    .ThenInclude(c => c.STYLE)
                    .ThenInclude(c => c.FABCODENavigation)
                    .Where(d => d.PIID.Equals(pi))
                    .ToListAsync();


                return result;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

    }
}
