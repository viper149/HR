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
using DenimERP.ViewModels.Basic.YarnCountInfo;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLBAS_YARN_COUNTINFO_Repository : BaseRepository<BAS_YARN_COUNTINFO>, IBAS_YARN_COUNTINFO
    {
        private readonly IDataProtector _protector;

        public SQLBAS_YARN_COUNTINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
            : base(denimDbContext)
        {
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<string> FindCountNameByIdAsync(int? id)
        {
            try
            {
                var countName = await DenimDbContext.BAS_YARN_COUNTINFO.Where(yc => yc.COUNTID == id).Select(e => e.RND_COUNTNAME).ToListAsync();
                return countName.FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<COS_PRECOSTING_DETAILS>> GetCountDetailsByIdAsync(List<COS_PRECOSTING_DETAILS> cosPreCostingDetailsList)
        {
            try
            {
                foreach (var cosPreCostingDetails in cosPreCostingDetailsList)
                {
                    cosPreCostingDetails.BasYarnCountInfo = await DenimDbContext.BAS_YARN_COUNTINFO
                        .Where(yc => yc.COUNTID.Equals(cosPreCostingDetails.COUNTID))
                        .Select(e => new BAS_YARN_COUNTINFO
                        {
                            COUNTNAME = e.COUNTNAME,
                            RND_COUNTNAME = e.RND_COUNTNAME
                        }).FirstOrDefaultAsync();

                    var Type = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.LOT)
                        .Where(c => c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) &&
                                    c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR))
                        .Select(c => c.YARNTYPE)
                        .FirstOrDefaultAsync();
                    string color;
                    if (cosPreCostingDetails.FABCODENavigation.STYLE_NAME.Contains("-CHK-"))
                    {
                        color = await DenimDbContext.RND_FABRIC_COUNTINFO
                            .Include(c => c.LOT)
                            .Where(c => c.COLORCODE!= null && c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) && c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR) && c.COLORCODE.ToString().Equals(cosPreCostingDetails.REMARKS))
                            .Select(c => c.Color.COLOR)
                            .FirstOrDefaultAsync();
                    }
                    else
                    {
                        color = await DenimDbContext.RND_FABRIC_COUNTINFO
                            .Include(c => c.LOT)
                            .Where(c => c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) && c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR))
                            .Select(c => c.Color.COLOR)
                            .FirstOrDefaultAsync();
                    }
                    

                    cosPreCostingDetails.BasYarnCountInfo.RND_COUNTNAME = cosPreCostingDetails.BasYarnCountInfo.RND_COUNTNAME + ' ' + Type + (color != null ? '(' + color + ')' : "");

                    cosPreCostingDetails.Yarnfor = await DenimDbContext.YARNFOR
                        .Where(c => c.YARNID.Equals(cosPreCostingDetails.YARN_FOR)).FirstOrDefaultAsync();

                    //if (cosPreCostingDetails.YPB != null)
                    //{
                    //    cosPreCostingDetails.YPBNavigation = await _denimDbContext.BAS_SUPPLIERINFO
                    //        .Where(c => c.SUPPID.Equals(cosPreCostingDetails.YPB))
                    //        .Select(c => new BAS_SUPPLIERINFO
                    //        {
                    //            SUPPNAME = c.SUPPNAME
                    //        })
                    //        .FirstOrDefaultAsync();
                    //}
                    //else
                    //{
                    //    cosPreCostingDetails.YPBNavigation = new BAS_SUPPLIERINFO();
                    //}
                }
                return cosPreCostingDetailsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BAS_YARN_COUNTINFO>> GetForSelectItemsByAsync()
        {
            return await DenimDbContext.BAS_YARN_COUNTINFO.Select(e => new BAS_YARN_COUNTINFO
            {
                COUNTID = e.COUNTID,
                COUNTNAME = e.COUNTNAME
            }).ToListAsync();
        }

        public async Task<DataTableObject<BAS_YARN_COUNTINFO>> GetForDataTableByAsync(string sortColumn, string sortColumnDirection, string searchValue, string draw, int skip,
            int pageSize)
        {
            try
            {
                var basYarnCountinfos = DenimDbContext.BAS_YARN_COUNTINFO
                    .GroupJoin(DenimDbContext.BAS_COLOR,
                        f1 => f1.COLOR,
                        f2 => f2.COLORCODE,
                        (f1, f2) => new
                        {
                            F1 = f1,
                            F2 = f2.FirstOrDefault()
                        })
                    .GroupJoin(DenimDbContext.BAS_YARN_PARTNO,
                        f3 => f3.F1.PART_ID,
                        f4 => f4.PART_ID,
                        (f3, f4) => new
                        {
                            F3 = f3,
                            F4 = f4.FirstOrDefault()
                        })
                    .GroupJoin(DenimDbContext.BAS_YARN_CATEGORY,
                        f5 => f5.F3.F1.YARN_CAT_ID,
                        f6 => f6.YARN_CAT_ID,
                        (f5, f6) => new
                        {
                            F5 = f5,
                            F6 = f6.FirstOrDefault()
                        })
                    .Select(e => new BAS_YARN_COUNTINFO
                    {
                        EncryptedId = _protector.Protect(e.F5.F3.F1.COUNTID.ToString()),
                        COUNTNAME = e.F5.F3.F1.COUNTNAME,
                        DESCRIPTION = e.F5.F3.F1.DESCRIPTION,
                        UNIT = e.F5.F3.F1.UNIT,
                        REMARKS = e.F5.F3.F1.REMARKS,
                        BAS_COLOR = e.F5.F3.F2 ?? new BAS_COLOR(),
                        PART_ = e.F5.F4 ?? new BAS_YARN_PARTNO(),
                        YARN_CAT_ = e.F6 ?? new BAS_YARN_CATEGORY()
                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    basYarnCountinfos = OrderedResult<BAS_YARN_COUNTINFO>.GetOrderedResult(sortColumnDirection, sortColumn, null, basYarnCountinfos);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    basYarnCountinfos = basYarnCountinfos
                        .Where(m => m.COUNTNAME.ToUpper().Contains(searchValue)
                                    || m.DESCRIPTION != null && m.DESCRIPTION.ToUpper().Contains(searchValue)
                                    || m.UNIT != null && m.UNIT.ToUpper().Contains(searchValue)
                                    || m.BAS_COLOR != null && m.BAS_COLOR.COLOR.ToUpper().Contains(searchValue)
                                    || m.REMARKS != null && m.REMARKS.ToUpper().Contains(searchValue));

                    basYarnCountinfos = OrderedResult<BAS_YARN_COUNTINFO>.GetOrderedResult(sortColumnDirection, sortColumn, null, basYarnCountinfos);
                }

                var recordsTotal = await basYarnCountinfos.CountAsync();

                return new DataTableObject<BAS_YARN_COUNTINFO>(draw, recordsTotal, recordsTotal, await basYarnCountinfos.Skip(skip).Take(pageSize).ToListAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<BAS_YARN_COUNTINFO>> GetCountListByAsync()
        {
            return await DenimDbContext.BAS_YARN_COUNTINFO
                .Include(d => d.BAS_COLOR)
                .Include(d => d.PART_)
                .Include(d => d.YARN_CAT_)
                .Select(d => new BAS_YARN_COUNTINFO
                {
                    COUNTID = d.COUNTID,
                    EncryptedId = _protector.Protect(d.COUNTID.ToString()),
                    COUNTNAME = d.COUNTNAME,
                    RND_COUNTNAME = d.RND_COUNTNAME,
                    DESCRIPTION = d.DESCRIPTION,
                    UNIT = d.UNIT,
                    REMARKS = d.REMARKS,
                    BAS_COLOR = new BAS_COLOR
                    {
                        COLOR = d.BAS_COLOR.COLOR
                    },
                    PART_ = new BAS_YARN_PARTNO
                    {
                        PART_NO = d.PART_.PART_NO
                    },
                    YARN_CAT_ = new BAS_YARN_CATEGORY
                    {
                        CATEGORY_NAME = d.YARN_CAT_.CATEGORY_NAME
                    }
                }).ToListAsync();
        }

        public async Task<List<COS_PRECOSTING_DETAILS>> GetDetailsByIdAsync(List<COS_PRECOSTING_DETAILS> cosPreCostingDetailsList)
        {
            try
            {
                foreach (var cosPreCostingDetails in cosPreCostingDetailsList)
                {
                    cosPreCostingDetails.BasYarnCountInfo = await DenimDbContext.BAS_YARN_COUNTINFO
                        .Where(yc => yc.COUNTID.Equals(cosPreCostingDetails.COUNTID))
                        .Select(e => new BAS_YARN_COUNTINFO
                        {
                            COUNTNAME = e.COUNTNAME,
                            RND_COUNTNAME = e.RND_COUNTNAME
                        }).FirstOrDefaultAsync();

                    var Type = await DenimDbContext.RND_FABRIC_COUNTINFO
                        .Include(c => c.LOT)
                        .Where(c => c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) &&
                                    c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR))
                        .Select(c => c.YARNTYPE)
                        .FirstOrDefaultAsync();
                    string color;
                    if (cosPreCostingDetails.FABCODENavigation.STYLE_NAME.Contains("-CHK-"))
                    {
                        color = await DenimDbContext.RND_FABRIC_COUNTINFO
                            .Include(c => c.LOT)
                            .Where(c => c.COLORCODE != null && c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) && c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR) && c.COLORCODE.ToString().Equals(cosPreCostingDetails.REMARKS))
                            .Select(c => c.Color.COLOR).FirstOrDefaultAsync();
                    }
                    else
                    {
                        color = await DenimDbContext.RND_FABRIC_COUNTINFO
                            .Include(c => c.LOT)
                            .Where(c => c.FABCODE.Equals(cosPreCostingDetails.FABCODE) && c.COUNTID.Equals(cosPreCostingDetails.COUNTID) && c.YARNFOR.Equals(cosPreCostingDetails.YARN_FOR))
                            .Select(c => c.Color.COLOR)
                            .FirstOrDefaultAsync();
                    }


                    cosPreCostingDetails.BasYarnCountInfo.RND_COUNTNAME = cosPreCostingDetails.BasYarnCountInfo.RND_COUNTNAME + ' ' + Type + (color != null ? '(' + color + ')' : "");

                    cosPreCostingDetails.Yarnfor = await DenimDbContext.YARNFOR
                        .Where(c => c.YARNID.Equals(cosPreCostingDetails.YARN_FOR)).FirstOrDefaultAsync();

                }
                return cosPreCostingDetailsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateBasYarnCountInfoViewModel> GetInitObjects(CreateBasYarnCountInfoViewModel createBasYarnCountInfoViewModel)
        {
            createBasYarnCountInfoViewModel.BasYarnCategories = await DenimDbContext.BAS_YARN_CATEGORY.Select(e => new BAS_YARN_CATEGORY
            {
                YARN_CAT_ID = e.YARN_CAT_ID,
                CATEGORY_NAME = e.CATEGORY_NAME
            }).OrderBy(e => e.CATEGORY_NAME).ToListAsync();

            createBasYarnCountInfoViewModel.BasYarnLotInfos = await DenimDbContext.BAS_YARN_LOTINFO.Select(e => new BAS_YARN_LOTINFO
            {
                LOTID = e.LOTID,
                LOTNO = e.LOTNO
            }).OrderBy(e => e.LOTNO).ToListAsync();

            createBasYarnCountInfoViewModel.BasColors = await DenimDbContext.BAS_COLOR.Where(e => !string.IsNullOrEmpty(e.COLOR)).Select(e => new BAS_COLOR
            {
                COLORCODE = e.COLORCODE,
                COLOR = e.COLOR
            }).OrderBy(e => e.COLOR).ToListAsync();

            createBasYarnCountInfoViewModel.BasYarnPartNo = await DenimDbContext.BAS_YARN_PARTNO.Select(e => new BAS_YARN_PARTNO
            {
                PART_NO = e.PART_NO,
                PART_ID = e.PART_ID
            }).OrderBy(e => e.PART_NO).ToListAsync();

            return createBasYarnCountInfoViewModel;
        }

        public async Task<CreateBasYarnCountInfoViewModel> FindByCountIdAsync(int countId)
        {
            try
            {
                var basYarnCountinfo = await DenimDbContext.BAS_YARN_COUNTINFO
                    .Include(e => e.BAS_YARN_COUNT_LOT_INFO)
                    .ThenInclude(e => e.LOT)
                    .FirstOrDefaultAsync(e => e.COUNTID.Equals(countId));

                var createBasYarnCountInfoViewModel = new CreateBasYarnCountInfoViewModel
                {
                    BasYarnCountinfo = new ExtendBasYarnCountInfo
                    {
                        EncryptedId = _protector.Protect(basYarnCountinfo.COUNTID.ToString()),
                        COUNTID = basYarnCountinfo.COUNTID,
                        COUNTNAME = basYarnCountinfo.COUNTNAME,
                        RND_COUNTNAME = basYarnCountinfo.RND_COUNTNAME,
                        YARN_CAT_ID = basYarnCountinfo.YARN_CAT_ID,
                        DESCRIPTION = basYarnCountinfo.DESCRIPTION,
                        PART_ID = basYarnCountinfo.PART_ID,
                        UNIT = basYarnCountinfo.UNIT,
                        COLOR = basYarnCountinfo.COLOR,
                        REMARKS = basYarnCountinfo.REMARKS
                    },

                    BasYarnCountLotInfoList = basYarnCountinfo.BAS_YARN_COUNT_LOT_INFO.ToList()
                };

                return createBasYarnCountInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> FindByCountnameByAsync(string countName)
        {
            try
            {
                return await DenimDbContext.BAS_YARN_COUNTINFO.AnyAsync(yc => yc.COUNTNAME.Equals(countName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<YarnCountUpdateViewModel> GetInitYarnObjects(YarnCountUpdateViewModel yarnCountUpdateViewModel)
        {
            try
            {
                yarnCountUpdateViewModel.BasYarnCountInfosJustRnd = await DenimDbContext.BAS_YARN_COUNTINFO
                        .Where(c => c.YARN_CAT_ID.Equals(8102699)).ToListAsync();

                yarnCountUpdateViewModel.BasYarnCountInfosWithoutRnd = await DenimDbContext.BAS_YARN_COUNTINFO
                        .Where(c => !c.YARN_CAT_ID.Equals(8102699) && c.RND_COUNTNAME == null)
                        .ToListAsync();

                yarnCountUpdateViewModel.BasYarnCountInfos = await DenimDbContext.BAS_YARN_COUNTINFO
                        .Where(c => !c.YARN_CAT_ID.Equals(8102699))
                        .ToListAsync();

                yarnCountUpdateViewModel.PiDetailsList = await DenimDbContext.COM_EX_PI_DETAILS
                        .ToListAsync();

                return yarnCountUpdateViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> InsertByAndGetIdAsync(BAS_YARN_COUNTINFO basYarnCountInfo)
        {
            await DenimDbContext.BAS_YARN_COUNTINFO.AddAsync(basYarnCountInfo);
            await SaveChanges();
            return basYarnCountInfo.COUNTID;
        }

        public async Task<IEnumerable<BAS_YARN_COUNT_LOT_INFO>> GetLotList(int countId)
        {
            try
            {
                var result = await DenimDbContext.BAS_YARN_COUNT_LOT_INFO
                    .Where(c => c.COUNTID.Equals(countId))
                    .ToListAsync();

                return result;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
