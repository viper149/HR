using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Rnd;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_YS_INDENT_MASTER_Repository : BaseRepository<F_YS_INDENT_MASTER>, IF_YS_INDENT_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_YS_INDENT_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<F_YS_INDENT_MASTER>> GetAllIndentMasterAsync()
        {
            try
            {
                var result =  await DenimDbContext.F_YS_INDENT_MASTER
                    .Include(c=>c.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation)
                    .Include(c=>c.INDSL.ORDERNO_SNavigation.RND_SAMPLE_INFO_DYEING)
                    .ThenInclude(c=>c.PL_SAMPLE_PROG_SETUP)
                    .Select(e => new F_YS_INDENT_MASTER
                    {
                        INDNO = e.INDNO,
                        EncryptedId = _protector.Protect(e.INDID.ToString()),
                        INDDATE = e.INDDATE,
                        INDSLID = e.INDSLID,
                        OPT1 = e.INDSL.ORDER_NONavigation!=null?e.INDSL.ORDER_NONavigation.SO.SO_NO:e.INDSL.ORDERNO_SNavigation.SDRF_NO,
                        OPT2 = e.INDSL.ORDER_NONavigation!=null?e.INDSL.ORDER_NONavigation.SO.STYLE.FABCODENavigation.STYLE_NAME:"",
                        OPT3 = e.INDSL.INDENT_SL_NO,
                        OPT4 = e.INDSL.INDSLNO,
                        REMARKS = e.REMARKS,
                        IsRevised = e.INDSL.STATUS,
                        INDSL = new RND_PURCHASE_REQUISITION_MASTER
                        {
                            REMARKS = e.INDSL.REMARKS
                        }
                    }).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> GetLastIndNo()
        {
            try
            {
                var fYsIndentMaster = await DenimDbContext.F_YS_INDENT_MASTER
                    .Select(e => new F_YS_INDENT_MASTER
                    {
                        INDID = e.INDID
                    })
                    .OrderByDescending(e => e.INDID).FirstOrDefaultAsync();

                return fYsIndentMaster != null && fYsIndentMaster.INDID != 0 ? fYsIndentMaster.INDID + 1 : 400;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<F_YS_INDENT_MASTER> GetIndentByINDSLID(int indslid)
        {
            try
            {
                return await DenimDbContext.F_YS_INDENT_MASTER.FirstOrDefaultAsync(e => e.INDSLID.Equals(indslid));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertAndGetIdAsync(F_YS_INDENT_MASTER ysIndentMaster)
        {
            try
            {
                await DenimDbContext.F_YS_INDENT_MASTER.AddAsync(ysIndentMaster);
                await SaveChanges();
                return ysIndentMaster.INDID;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public async Task<RndYarnRequisitionViewModel> GetInitObjects(RndYarnRequisitionViewModel rndYarnRequisitionViewModel)
        {
            rndYarnRequisitionViewModel.YarnforList = await DenimDbContext.YARNFOR.ToListAsync();

            rndYarnRequisitionViewModel.RndPurchaseRequisitionMasterList = await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                .Include(c=>c.ORDER_NONavigation.SO)
                .Include(c=>c.ORDERNO_SNavigation)
                .Select(e => new RND_PURCHASE_REQUISITION_MASTER
                {
                    INDSLID = e.INDSLID,
                    STATUS = e.STATUS,
                    OPT2 = $"{e.INDSLNO ?? e.INDENT_SL_NO} - {(e.ORDER_NONavigation==null?e.ORDERNO_SNavigation.SDRF_NO:e.ORDER_NONavigation.SO.SO_NO)}"

                }).Where(e => e.STATUS.Equals("0") && !DenimDbContext.F_YS_INDENT_MASTER.Any(o=>o.INDSLID.Equals(e.INDSLID)))
                .ToListAsync();

            var fYsIndentMaster = await DenimDbContext.F_YS_INDENT_MASTER
                .Select(e => new F_YS_INDENT_MASTER
                {
                    INDID = e.INDID
                }).OrderByDescending(e => e.INDID).FirstOrDefaultAsync();

            //rndYarnRequisitionViewModel.BasYarnCountInfos = await _denimDbContext.BAS_YARN_COUNTINFO
            //    .Where(e => !string.IsNullOrEmpty(e.COUNTNAME))
            //    .Select(e =>
            //    new BAS_YARN_COUNTINFO
            //    {
            //        COUNTID = e.COUNTID,
            //        COUNTNAME = e.COUNTNAME

            //    }).OrderByDescending(e => e.COUNTNAME).ToListAsync();

            var indId = fYsIndentMaster != null && fYsIndentMaster.INDID != 0 ? fYsIndentMaster.INDID + 1 : 4000;

            rndYarnRequisitionViewModel.FysIndentMaster = new F_YS_INDENT_MASTER
            {
                INDDATE = DateTime.Now,
                INDNO = indId.ToString()
            };

            return rndYarnRequisitionViewModel;
        }

        public async Task<RndYarnRequisitionViewModel> GetInitObjectsForYarnDetails(RndYarnRequisitionViewModel requisitionViewModel)
        {
            foreach (var item in requisitionViewModel.FysIndentDetailList)
            {
                item.BASCOUNTINFO = await DenimDbContext.BAS_YARN_COUNTINFO.FirstOrDefaultAsync(e => e.COUNTID.Equals(item.PRODID ?? 0));
                item.SEC = await DenimDbContext.F_BAS_SECTION.FirstOrDefaultAsync(e => e.SECID.Equals(item.SECID));
                item.LOT = await DenimDbContext.BAS_YARN_LOTINFO.FirstOrDefaultAsync(e => e.LOTID.Equals(item.PREV_LOTID));
                item.RAWNavigation = await DenimDbContext.F_YS_RAW_PER.FirstOrDefaultAsync(e => e.RAWID.Equals(item.RAW));
                item.SLUB_CODENavigation = await DenimDbContext.F_YS_SLUB_CODE.FirstOrDefaultAsync(e => e.ID.Equals(item.SLUB_CODE));
                item.YARN_FORNavigation = await DenimDbContext.YARNFOR.FirstOrDefaultAsync(e => e.YARNID.Equals(item.YARN_FOR));
                item.YARN_FROMNavigation = await DenimDbContext.YARNFROM.FirstOrDefaultAsync(e => e.YFID.Equals(item.YARN_FROM));
                item.FBasUnits = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));
            }

            return requisitionViewModel;
        }


        public async Task<RndYarnRequisitionViewModel> GetIndentCountListByINDSLID(RndYarnRequisitionViewModel requisitionViewModel)
        {
            try
            {
                requisitionViewModel.FysIndentDetailList = await DenimDbContext.F_YS_INDENT_DETAILS
                    .Include(c=>c.RAWNavigation)
                    .Include(c=>c.SLUB_CODENavigation)
                    .Include(c=>c.BASCOUNTINFO)
                    .Where(c => c.INDSLID.Equals(requisitionViewModel.FysIndentMaster.INDSLID)).ToListAsync();

                foreach (var item in requisitionViewModel.FysIndentDetailList)
                {
                    item.CNSMP_AMOUNT = item.ORDER_QTY;
                }

                return requisitionViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndYarnRequisitionViewModel> GetOtherDetails(string id)
        {

            try
            {
                var rndPurchaseRequisitionMaster = await DenimDbContext.RND_PURCHASE_REQUISITION_MASTER
                    .Include(e => e.ORDER_NONavigation)
                    .ThenInclude(e => e.SO)
                    .ThenInclude(e => e.PIMASTER)
                    .Where(e => e.INDSLID.Equals(int.Parse(id)))
                    .FirstOrDefaultAsync();

                return new RndYarnRequisitionViewModel
                {
                    FysIndentDetails = new F_YS_INDENT_DETAILS
                    {
                        RND_PURCHASE_REQUISITION_MASTER = rndPurchaseRequisitionMaster
                    }
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<RndYarnRequisitionViewModel> FindByIdIncludeAllAsync(int indId)
        {
            try
            {
                var rndYarnRequisitionViewModel = await DenimDbContext.F_YS_INDENT_MASTER
                    .Include(e => e.INDSL.F_YS_INDENT_DETAILS)
                    .ThenInclude(c=>c.RAWNavigation)
                    .Include(e => e.INDSL.F_YS_INDENT_DETAILS)
                    .ThenInclude(e => e.SEC)
                    .ThenInclude(e => e.F_YS_INDENT_DETAILS)
                    .ThenInclude(e => e.BASCOUNTINFO)
                    .Include(e => e.F_YS_INDENT_DETAILS)
                    .ThenInclude(e => e.FBasUnits)
                    .Include(e => e.F_YS_INDENT_DETAILS)
                    .ThenInclude(e => e.LOT)
                    .Where(e => e.INDID.Equals(indId))
                    .Select(e => new RndYarnRequisitionViewModel
                    {
                        FysIndentMaster = new F_YS_INDENT_MASTER
                        {
                            INDID = e.INDID,
                            EncryptedId = _protector.Protect(e.INDID.ToString()),
                            INDNO = e.INDNO,
                            INDSLID = e.INDSLID,
                            INDDATE = e.INDDATE,
                            REMARKS = e.REMARKS,
                            OPT5 = e.INDSL.INDSLNO,
                            OPT3 = e.INDSL.INDSLNO
                        },
                        FysIndentDetailList = e.INDSL.F_YS_INDENT_DETAILS.Select(f => new F_YS_INDENT_DETAILS
                        {
                            TRNSID = f.TRNSID,
                            PRODID = f.PRODID,
                            INDSLID = f.INDSLID,
                            INDID = f.INDID,
                            SECID = f.SECID,
                            UNIT = f.UNIT,
                            RAW = f.RAW,
                            SLUB_CODE = f.SLUB_CODE,
                            STOCK_AMOUNT = f.STOCK_AMOUNT,
                            ORDER_QTY = f.ORDER_QTY,
                            CNSMP_AMOUNT = f.CNSMP_AMOUNT,
                            LAST_INDENT_NO = f.LAST_INDENT_NO,
                            PREV_LOTID = f.PREV_LOTID,
                            TRNSDATE = f.TRNSDATE,
                            REMARKS = f.REMARKS,
                            ETR = f.ETR,
                            BASCOUNTINFO = f.BASCOUNTINFO,
                            SEC = f.SEC,
                            FBasUnits = f.FBasUnits,
                            YARN_FORNavigation = f.YARN_FORNavigation,
                            RAWNavigation = f.RAWNavigation,
                            LOT = f.LOT
                        }).ToList()
                    }).FirstOrDefaultAsync();

                return rndYarnRequisitionViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
