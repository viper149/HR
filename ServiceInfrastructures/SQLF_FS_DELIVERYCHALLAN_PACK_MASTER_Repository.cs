using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Data;
using DenimERP.Models;
using DenimERP.Security;
using DenimERP.ServiceInfrastructures.BaseInfrastructures;
using DenimERP.ServiceInterfaces;
using DenimERP.ViewModels.Factory.Fabric_Store;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLF_FS_DELIVERYCHALLAN_PACK_MASTER_Repository : BaseRepository<F_FS_DELIVERYCHALLAN_PACK_MASTER>, IF_FS_DELIVERYCHALLAN_PACK_MASTER
    {
        private readonly IDataProtector _protector;

        public SQLF_FS_DELIVERYCHALLAN_PACK_MASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings) : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<dynamic>> GetChallanListAsync()
        {
            var result = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                    .Include(c => c.DO)
                    .ThenInclude(c => c.ComExLcInfo)
                    .Include(c => c.PI)
                    .Include(c => c.SO_NONavigation)
                    .ThenInclude(c => c.STYLE)
                    .ThenInclude(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Include(c => c.VEHICLENONavigation)
                    .Include(c => c.DELIVERY_TYPENavigation)
                    .Include(c => c.AUDITBYNavigation)
                    .Select(c => new
                    {
                        Do = c.ISSUE_TYPE == 1 && c.DO != null ? c.DO.DONO : "",
                        D_CHALLANDATE = c.D_CHALLANDATE,
                        D_CHALLANID = c.D_CHALLANID,
                        PI = c.ISSUE_TYPE == 1 && c.PI != null ? c.PI.PINO : "",
                        StyleName = c.ISSUE_TYPE == 1 && c.SO_NONavigation != null ? c.SO_NONavigation.STYLE.FABCODENavigation.STYLE_NAME : "",
                        So_No = c.ISSUE_TYPE == 1 && c.SO_NONavigation != null ? c.SO_NONavigation.SO_NO : "",
                        VNUMBER = c.ISSUE_TYPE == 1 && c.VEHICLENONavigation != null ? c.VEHICLENONavigation.VNUMBER : "",
                        DEL_TYPE = c.DELIVERY_TYPENavigation.DEL_TYPE,
                        DC_NO = c.DC_NO,
                        GP_NO = c.GP_NO,
                        LOCKNO = c.LOCKNO,
                        AUDITBY = c.AUDITBYNavigation != null ? c.AUDITBYNavigation.FIRST_NAME : "",
                        REMARKS = c.REMARKS,
                        EncryptedId = _protector.Protect(c.D_CHALLANID.ToString())
                    }).OrderByDescending(c => c.D_CHALLANID).ToListAsync();

            return result;
        }

        public async Task<List<F_FS_DELIVERYCHALLAN_PACK_MASTER>> GetAllByIssueType()
        {
            try
            {
                var result = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                    .Where(c => c.DELIVERY_TYPE == 5)
                    .OrderByDescending(c => c.D_CHALLANID)
                    .ToListAsync();

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FabDeliveryChallanViewModel> GetInitObjects(FabDeliveryChallanViewModel fabDeliveryChallanViewModel)
        {
            fabDeliveryChallanViewModel.DoLocalList = await DenimDbContext.ACC_LOCAL_DOMASTER.Select(c => new ACC_LOCAL_DOMASTER
            {
                TRNSID = c.TRNSID,
                DONO = c.DONO,
            }).ToListAsync();

            fabDeliveryChallanViewModel.ScList = await DenimDbContext.COM_EX_SCINFO.Select(c => new COM_EX_SCINFO
            {
                SCID = c.SCID,
                SCNO = c.SCNO
            }).ToListAsync();

            fabDeliveryChallanViewModel.DoMasters = await DenimDbContext.ACC_EXPORT_DOMASTER
                .Where(d=> d.COMMENTS != null && !d.IS_CANCELLED)
                .Select(c => new ACC_EXPORT_DOMASTER
                {
                    TRNSID = c.TRNSID,
                    DONO = c.DONO
                }).ToListAsync();

            fabDeliveryChallanViewModel.PiMasters = await DenimDbContext.COM_EX_PIMASTER
                .Select(c => new COM_EX_PIMASTER
                {
                    PIID = c.PIID,
                    PINO = c.PINO
                })
                .ToListAsync();

            fabDeliveryChallanViewModel.PiDetails = await DenimDbContext.COM_EX_PI_DETAILS
                .Include(c => c.STYLE.FABCODENavigation)
                .Select(c => new COM_EX_PI_DETAILS
                {
                    TRNSID = c.TRNSID,
                    SO_NO = $"{c.STYLE.FABCODENavigation.STYLE_NAME} ({c.SO_NO})"
                })
                .ToListAsync();
            fabDeliveryChallanViewModel.FBasVehicleInfos = await DenimDbContext.F_BAS_VEHICLE_INFO.ToListAsync();
            fabDeliveryChallanViewModel.BasBuyerInfos = await DenimDbContext.BAS_BUYERINFO.ToListAsync();
            fabDeliveryChallanViewModel.FBasDeliveryTypes = await DenimDbContext.F_BAS_DELIVERY_TYPE.Where(c => c.ID.Equals(3) || c.ID.Equals(4) || c.ID.Equals(5)).ToListAsync();
            fabDeliveryChallanViewModel.FFsIssueTypes = await DenimDbContext.F_FS_ISSUE_TYPE.ToListAsync();


            var FHrEmployees = await DenimDbContext.F_HR_EMP_OFFICIALINFO
                .Include(c => c.EMP)
                .Where(c => c.SECID.Equals(136) && !c.OPN2.Equals("Y"))
                .ToListAsync();

            fabDeliveryChallanViewModel.FHrEmployees = FHrEmployees.Select(c => new F_HRD_EMPLOYEE
            {
                EMPID = c.EMP.EMPID,
                FIRST_NAME = c.EMP.FIRST_NAME + " " + c.EMP.LAST_NAME + '-' + c.EMP.EMPNO
            }).ToList();

            //fabDeliveryChallanViewModel.FFsFabricRcvDetailsList = await _denimDbContext.F_FS_FABRIC_RCV_DETAILS
            //    .Include(c => c.ROLL_)
            //    .Where(c => _denimDbContext.F_FS_FABRIC_CLEARANCE_DETAILS.Any(d=>d.ROLL_ID.Equals(c.ROLL_ID) && d.STATUS.Equals(1)) && c.BALANCE_QTY > 0)
            //    .ToListAsync();

            return fabDeliveryChallanViewModel;
        }

        public async Task<int> InsertAndGetIdAsync(F_FS_DELIVERYCHALLAN_PACK_MASTER fFsDeliveryChallanPackMaster)
        {
            try
            {
                await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER.AddAsync(fFsDeliveryChallanPackMaster);
                await SaveChanges();
                return fFsDeliveryChallanPackMaster.D_CHALLANID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public FabDeliveryChallanViewModel FindAllByIdAsync(int id)
        {
            try
            {
                var fabDeliveryChallanViewModel = DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .ThenInclude(c => c.ROLL)
                    .Include(c => c.DO)
                    .Where(c => c.D_CHALLANID.Equals(id))
                    .Select(c => new FabDeliveryChallanViewModel
                    {
                        FFsDeliveryChallanPackMaster = c,
                        FFsDeliverychallanPackDetailsList = c.F_FS_DELIVERYCHALLAN_PACK_DETAILS.ToList()
                    }).FirstOrDefault();

                return fabDeliveryChallanViewModel;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> GetDoBalance(int doId)
        {
            try
            {
                var result = await DenimDbContext.ACC_EXPORT_DOMASTER
                    .Include(c => c.ACC_EXPORT_DODETAILS)
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .ThenInclude(c => c.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .ThenInclude(c => c.SO_NONavigation)
                    .Where(c => c.TRNSID.Equals(doId)).FirstOrDefaultAsync();


                var doOracleDelivery = await DenimDbContext.F_FS_DO_BALANCE_FROM_ORACLE
                    .Where(c => c.DO_REF.Equals(result.DONO)).SumAsync(c => c.ORACLE_DELIVERY);

                double doQty;
                if (result.F_FS_DELIVERYCHALLAN_PACK_MASTER.Count != 0)
                {
                    doQty = result.ACC_EXPORT_DODETAILS.Sum(item => result.F_FS_DELIVERYCHALLAN_PACK_MASTER.Sum(i => item.PIID == i.PIID && item.STYLEID == i.SO_NONavigation.STYLEID
                        ? decimal.ToDouble(item.QTY ?? 0) : 0));
                }
                else
                {
                    doQty = result.ACC_EXPORT_DODETAILS.Sum(c => decimal.ToDouble(c.QTY ?? 0));
                }

                doQty -= (doOracleDelivery ?? 0);

                var issue = result.ACC_EXPORT_DODETAILS.Sum(item => result.F_FS_DELIVERYCHALLAN_PACK_MASTER.Sum(i => i.PIID == item.PIID && i.SO_NONavigation.STYLEID == item.STYLEID ? i.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Sum(p => (p.LENGTH1 ?? 0.0) + (p.LENGTH2 ?? 0.0)) : 0));
                var balance = doQty - issue;
                return balance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<dynamic> GetPiBalance(int piId, int trnsId)
        {
            //&& c.COM_EX_PI_DETAILS.Select(x => x.TRNSID).Equals(30677)
            try
            {
                var result = await DenimDbContext.COM_EX_PIMASTER
                    .Include(c => c.COM_EX_PI_DETAILS)
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .ThenInclude(c => c.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .ThenInclude(x => x.ROLL.FABCODENavigation)
                    .Where(c => c.PIID.Equals(piId)).FirstOrDefaultAsync();

                if (result.COM_EX_PI_DETAILS.FirstOrDefault()?.UNIT == 6)
                {
                    var x = result.PI_QTY / 0.9144;
                    x = (double?)Math.Ceiling((decimal)x);
                    result.PI_QTY = x;
                }

                var piDelivery = result.COM_EX_PI_DETAILS.Sum(x =>
                    x.F_FS_DELIVERYCHALLAN_PACK_MASTER.Sum(x =>
                        x.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Sum(x => x.ROLL.QTY_YARDS)));

                var piQtyAsSoNO = result.COM_EX_PI_DETAILS.Where(x => x.TRNSID.Equals(trnsId)).Select(x => x.QTY).FirstOrDefault();

                var piBalance = piQtyAsSoNO - piDelivery;
                var piQty = result.PI_QTY;
                var issue = 0.0;

                foreach (var item in result.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                {
                    issue = item.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Sum(c => c.LENGTH1 + c.LENGTH2) ?? 0;
                }

                var balance = piQty - issue;
                // return balance ?? 0;

                var piDetailsList = new
                {
                    blnce = balance,
                    pidel = piDelivery,
                    piqtyasso = piQtyAsSoNO,
                    pibalance = piBalance
                };
                return piDetailsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<double> GetSoBalance(int soId)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_PI_DETAILS
                    .Include(c => c.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                    .ThenInclude(c => c.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .Where(c => c.TRNSID.Equals(soId)).FirstOrDefaultAsync();


                if (result.UNIT == 6)
                {
                    var x = result.QTY / 0.9144;
                    x = (double?)Math.Ceiling((decimal)x);
                    result.QTY = (int?)x;
                }

                var soQty = result.QTY;
                var issue = 0.0;

                foreach (var item in result.F_FS_DELIVERYCHALLAN_PACK_MASTER)
                {
                    issue = item.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Sum(c => c.LENGTH1 + c.LENGTH2) ?? 0;
                }
                var balance = soQty - issue;
                return balance ?? 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<F_FS_DELIVERYCHALLAN_PACK_MASTER> GetDoWisePackingRollList(int doId)
        {
            try
            {
                var x = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                    .Include(d => d.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .ThenInclude(d => d.ROLL.PO_NONavigation)
                    .Include(d => d.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .ThenInclude(d => d.ROLL.SO_NONavigation.STYLE)
                    .Include(d => d.F_FS_DELIVERYCHALLAN_PACK_DETAILS)
                    .ThenInclude(d => d.ROLL.SO_NONavigation.F_BAS_UNITS)
                    .Where(d => d.DOID.Equals(doId))
                    .Select(d => new F_FS_DELIVERYCHALLAN_PACK_MASTER
                    {
                        F_FS_DELIVERYCHALLAN_PACK_DETAILS = d.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Select(f => new F_FS_DELIVERYCHALLAN_PACK_DETAILS
                        {
                            D_CHALLANID = f.D_CHALLANID,
                            ROLL = new F_FS_FABRIC_RCV_DETAILS
                            {
                                Qty = d.F_FS_DELIVERYCHALLAN_PACK_DETAILS.Sum(c => (c.LENGTH1 ?? 0) + (c.LENGTH2 ?? 0)),
                                PO_NONavigation = new COM_EX_PIMASTER
                                {
                                    PINO = f.ROLL.PO_NONavigation.PINO
                                },
                                SO_NONavigation = new COM_EX_PI_DETAILS
                                {
                                    QTY = f.ROLL.SO_NONavigation.QTY,
                                    STYLE = new COM_EX_FABSTYLE
                                    {
                                        STYLENAME = f.ROLL.SO_NONavigation.STYLE.STYLENAME
                                    },
                                    F_BAS_UNITS = new F_BAS_UNITS
                                    {
                                        UNAME = f.ROLL.SO_NONavigation.F_BAS_UNITS.UNAME
                                    }
                                }
                            }
                        }
                        ).ToList()
                    }).FirstOrDefaultAsync();


                var lsit = x.F_FS_DELIVERYCHALLAN_PACK_DETAILS.GroupBy(x => new { x.D_CHALLANID, x.ROLL.SO_NONavigation.STYLE.STYLENAME }).Select(x => x.First());

                x.FFsDeliveryChallanPackDetailsList = lsit.ToList();

                return x;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<dynamic> GetChallanNo(int delType)
        {
            try
            {
                if (delType is 3 or 4)
                {
                    var challan = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                        .Where(c => c.DELIVERY_TYPE.Equals(delType))
                        .OrderByDescending(c => int.Parse(c.DC_NO ?? "0"))
                        .Select(c => c.DC_NO)
                        .FirstOrDefaultAsync();

                    var gatePass = await DenimDbContext.F_FS_DELIVERYCHALLAN_PACK_MASTER
                        .Where(c => c.DELIVERY_TYPE.Equals(delType))
                        .OrderByDescending(c => int.Parse(c.GP_NO ?? "0"))
                        .Select(c => c.GP_NO)
                        .FirstOrDefaultAsync();

                    var packingList = new
                    {
                        challan = int.Parse(challan) + 1,
                        gatePass = int.Parse(gatePass) + 1
                    };

                    return packingList;
                }

                return null;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
