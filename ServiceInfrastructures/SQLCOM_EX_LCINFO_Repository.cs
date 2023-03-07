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
using DenimERP.ViewModels.Com;
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Com.InvoiceExport;
using DenimERP.ViewModels.Home;
using DenimERP.ViewModels.StaticData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_LCINFO_Repository : BaseRepository<COM_EX_LCINFO>, ICOM_EX_LCINFO
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLCOM_EX_LCINFO_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor) : base(denimDbContext)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        #region MyRegion

        public async Task<IEnumerable<COM_EX_LCINFO>> ComExLcInfoListWithPaged(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var excludeResult = (pageSize * pageNumber) - pageSize;
                var result = await DenimDbContext.COM_EX_LCINFO
                    .Where(lc => lc.ISDELETE.Equals(false))
                    .OrderByDescending(lci => lci.LCID)
                    .Skip(excludeResult)
                    .Take(pageSize)
                    .ToListAsync();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> TotalNumberOfComExLcInfoList()
        {
            var totalItem = await DenimDbContext.COM_EX_LCINFO.Where(lc => lc.ISDELETE.Equals(false)).CountAsync();
            return totalItem;
        }

        public async Task<COM_EX_LCINFO> FindByLcNoIsDeleteAsync(string lcNo)
        {
            try
            {
                var lcInfo = await DenimDbContext.COM_EX_LCINFO
                    .Where(e => e.LCNO.Equals(lcNo))
                    .FirstOrDefaultAsync();

                return lcInfo;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<COM_EX_LCINFO> FindByIdIncludeAllAsync(int lcId)
        {
            try
            {
                var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.BANK)
                    .Include(e => e.BANK_)
                    .Include(e => e.BUYER)
                    .Include(e => e.TEAM)
                    .Include(e => e.COM_TENOR)
                    .Include(e => e.COM_EX_LCDETAILS)
                    .Where(e => e.LCID.Equals(lcId)).FirstOrDefaultAsync();

                return comExLcinfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<COM_EX_LCINFO> FindByIdIsDeleteFalseAsync(int id)
        {
            try
            {
                var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.BANK)
                    .Include(e => e.BANK_)
                    .Include(e => e.BUYER)
                    .Include(e => e.TEAM)
                    .Include(e => e.COM_TENOR)
                    .Where(e => e.LCID.Equals(id))
                    .FirstOrDefaultAsync();

                return comExLcinfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<COM_EX_LCINFO>> GetAllGreaterThan(DateTime importLcDate)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_LCINFO.Where(e => e.SHIP_DATE > importLcDate).Select(e => new COM_EX_LCINFO
                {
                    LCID = e.LCID,
                    LCNO = $"{e.LCNO} ({e.FILENO})"
                }).OrderBy(e => e.LCNO).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComExLcInfoViewModel> InitComExLcInfoViewModel(ComExLcInfoViewModel comExLcInfoViewModel)
        {
            comExLcInfoViewModel.Currency = StaticData.GetCurrency();
            comExLcInfoViewModel.Status = StaticData.GetStatus();

            comExLcInfoViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            comExLcInfoViewModel.bAS_BUYER_BANK_MASTERs = await DenimDbContext.BAS_BUYER_BANK_MASTER
                .Select(c => new BAS_BUYER_BANK_MASTER
                {
                    BANK_ID = c.BANK_ID,
                    PARTY_BANK = $"{c.PARTY_BANK} ({c.ADDRESS})"
                }).OrderBy(e => e.PARTY_BANK).ToListAsync();

            comExLcInfoViewModel.MktTeams = await DenimDbContext.MKT_TEAM.Select(e => new MKT_TEAM
            {
                MKT_TEAMID = e.MKT_TEAMID,
                PERSON_NAME = e.PERSON_NAME
            }).OrderBy(e => e.PERSON_NAME).ToListAsync();

            comExLcInfoViewModel.bAS_BEN_BANK_MASTERs = await DenimDbContext.BAS_BEN_BANK_MASTER.Select(e => new BAS_BEN_BANK_MASTER
            {
                BANKID = e.BANKID,
                BEN_BANK = e.BEN_BANK
            }).OrderBy(e => e.BEN_BANK).ToListAsync();

            comExLcInfoViewModel.cOM_EX_PIMASTERs = await DenimDbContext.COM_EX_PIMASTER.Select(e => new COM_EX_PIMASTER
            {
                PIID = e.PIID,
                PINO = e.PINO
            }).OrderBy(e => e.PINO).ToListAsync();

            comExLcInfoViewModel.ComTenors = await DenimDbContext.COM_TENOR.ToListAsync();
            comExLcInfoViewModel.ComTradeTerms = await DenimDbContext.COM_TRADE_TERMS.Where(c => c.ISACTIVE).ToListAsync();
            comExLcInfoViewModel.ComTradeTermsEdit = await DenimDbContext.COM_TRADE_TERMS.ToListAsync();

            return comExLcInfoViewModel;
        }

        public async Task<COM_EX_LCINFO> FindByLcIdIsDeleteAsync(int lcId)
        {
            var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                .Include(e => e.BANK)
                .Include(e => e.BANK_)
                .Include(e => e.BUYER)
                .Include(e => e.TEAM)
                .Include(c => c.COM_TENOR)
                .Include(e => e.COM_EX_LCDETAILS)
                .FirstOrDefaultAsync(e => e.LCID.Equals(lcId));

            return comExLcinfo;
        }

        public async Task<IEnumerable<COM_EX_LCINFO>> GetForDataTableByAsync()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, "CustomsPolicy");

            return DenimDbContext.COM_EX_LCINFO
                .Include(e => e.NTFYBANK)
                .Include(e => e.BUYER)
                .Include(e => e.AccExportDoMaster)
                .Include(e => e.ACC_LOCAL_DOMASTER)
                .Where(e => !e.ISDELETE.Equals(true))
                .Select(e => new COM_EX_LCINFO
                {
                    LCID = e.LCID,
                    EncryptedId = _protector.Protect(e.LCID.ToString()),
                    LCNO = e.LCNO,
                    FILENO = e.FILENO,
                    LCDATE = e.LCDATE,
                    NTFYBANK = new BAS_BUYER_BANK_MASTER
                    {
                        PARTY_BANK = e.NTFYBANK.PARTY_BANK
                    },
                    BUYER = new BAS_BUYERINFO
                    {
                        BUYER_NAME = e.BUYER.BUYER_NAME
                    },
                    AMENTNO = e.AMENTNO,
                    AMENTDATE = e.AMENTDATE,
                    SHIP_DATE = e.SHIP_DATE,
                    EX_DATE = e.EX_DATE,
                    UP_DATE = e.UP_DATE,
                    UDSUBDATE = e.UDSUBDATE,
                    MLCSUBDATE = e.MLCSUBDATE,
                    VALUE = e.VALUE,
                    REMARKS = e.REMARKS,
                    ReadOnly = authorizationResult.Succeeded,
                    IsInDo = e.AccExportDoMaster.Any() || e.ACC_LOCAL_DOMASTER.Any(),
                    DoNo = !e.AccExportDoMaster.Any() ? e.ACC_LOCAL_DOMASTER.Any() ? "" : e.ACC_LOCAL_DOMASTER.FirstOrDefault().DONO : e.AccExportDoMaster.FirstOrDefault().DONO
                });
        }

        public async Task<int> TotalPercentageOfComExLcInfoList(DateTime dateTime, int days = 7)
        {
            return await DenimDbContext.COM_EX_LCINFO.Where(e => e.LCDATE > dateTime.AddDays(-days)).CountAsync();
        }

        public async Task<COM_EX_LCINFO> FindByLcIdWithDetailsIsDeleteAsync(int lcId)
        {
            try
            {
                var comExLcinfo = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.BANK)
                    .Include(e => e.BANK_)
                    .Include(e => e.BUYER)
                    .Include(e => e.TEAM.BasTeamInfo)
                    .Include(c => c.COM_TENOR)
                    .Include(c => c.COM_TRADE_TERMS)
                    .Include(e => e.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.BANK)
                    .Include(e => e.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.BANK_)
                    .Include(e => e.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                    .FirstOrDefaultAsync(e => e.LCID.Equals(lcId));

                comExLcinfo.EncryptedId = _protector.Protect(comExLcinfo.LCID.ToString());
                comExLcinfo.LC_QTY = comExLcinfo.COM_EX_LCDETAILS.Sum(d => d.PI.PI_QTY ?? 0);

                return comExLcinfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<COM_EX_PIMASTER> FindByPiIdIncludeAllAsync(int piId)
        {
            var comExPimaster = await DenimDbContext.COM_EX_PIMASTER
                .Include(e => e.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.F_BAS_UNITS)
                .Include(e => e.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.STYLE)
                .ThenInclude(e => e.FABCODENavigation)
                .ThenInclude(e => e.WV)
                .FirstOrDefaultAsync(e => e.PIID.Equals(piId));

            return comExPimaster;
        }

        public async Task<ComExLcInfoWithDetailsForEditViewModel> GetInitObjects(ComExLcInfoWithDetailsForEditViewModel comExLcInfoWithDetailsForEditViewModel)
        {
            comExLcInfoWithDetailsForEditViewModel.Currency = StaticData.GetCurrency();
            comExLcInfoWithDetailsForEditViewModel.Status = StaticData.GetStatus();

            comExLcInfoWithDetailsForEditViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            comExLcInfoWithDetailsForEditViewModel.bAS_BUYER_BANK_MASTERs = await DenimDbContext.BAS_BUYER_BANK_MASTER.Select(e => new BAS_BUYER_BANK_MASTER
            {
                BANK_ID = e.BANK_ID,
                PARTY_BANK = e.PARTY_BANK
            }).OrderBy(e => e.PARTY_BANK).ToListAsync();

            comExLcInfoWithDetailsForEditViewModel.bAS_TEAMINFOs = await DenimDbContext.BAS_TEAMINFO.Select(e => new BAS_TEAMINFO
            {
                TEAMID = e.TEAMID,
                TEAM_NAME = e.TEAM_NAME
            }).OrderBy(e => e.TEAM_NAME).ToListAsync();

            comExLcInfoWithDetailsForEditViewModel.bAS_BEN_BANK_MASTERs = await DenimDbContext.BAS_BEN_BANK_MASTER.Select(e => new BAS_BEN_BANK_MASTER
            {
                BANKID = e.BANKID,
                BEN_BANK = e.BEN_BANK
            }).OrderBy(e => e.BEN_BANK).ToListAsync();

            comExLcInfoWithDetailsForEditViewModel.cOM_EX_PIMASTERs = await DenimDbContext.COM_EX_PIMASTER.Select(e => new COM_EX_PIMASTER
            {
                PIID = e.PIID,
                PINO = e.PINO
            }).OrderBy(e => e.PINO).ToListAsync();

            comExLcInfoWithDetailsForEditViewModel.ComTenors = await DenimDbContext.COM_TENOR.Select(e => new COM_TENOR
            {
                TID = e.TID,
                NAME = e.NAME
            }).OrderBy(e => e.NAME).ToListAsync();

            return comExLcInfoWithDetailsForEditViewModel;
        }

        public async Task<double> GetSumOfTotalFromComExPiDetails(int lcId)
        {
            try
            {
                var sumOfTotal = await DenimDbContext.COM_EX_PI_DETAILS
                    .Where(e => DenimDbContext.COM_EX_LCDETAILS
                        .Include(f => f.LC)
                        .Where(g => g.LC.LCID == lcId)
                        .Select(h => h.PIID).Contains(e.PIID)).SumAsync(e => e.TOTAL);

                return sumOfTotal ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<string> GetNextLcFileNoByAsync(DateTime? dateTime = null, bool prevYear = false)
        {
            try
            {
                var fileNo = "";
                var year = (prevYear && dateTime != null ? dateTime?.Year : DateTime.Now.Year) - 2000;
                var result = await DenimDbContext.COM_EX_LCINFO.Where(e => e.FILENO.StartsWith($"P/{year}/")).OrderByDescending(c => c.FILENO).Select(c => c.FILENO).FirstOrDefaultAsync();

                if (result != null)
                {
                    var resultArray = result.Split("/");

                    if (int.Parse(resultArray[1]) < year)
                    {
                        fileNo = "P/" + year + "/" + "1".PadLeft(4, '0');
                    }
                    else
                    {
                        fileNo = "P/" + year + "/" + (int.Parse(resultArray[2]) + 1).ToString().PadLeft(4, '0');
                    }
                }
                else
                {
                    fileNo = "P/" + year + "/" + "1".PadLeft(4, '0');
                }

                return fileNo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateComExLcInfoViewModel> FindByLcIdForDeleteAsync(int lcId)
        {
            return await DenimDbContext.COM_EX_LCINFO
                .Include(e => e.COM_EX_LCDETAILS)
                .Where(e => e.LCID.Equals(lcId))
                .Select(e => new CreateComExLcInfoViewModel
                {
                    ComExLcinfo = new COM_EX_LCINFO
                    {
                        LCID = e.LCID
                    },
                    ComExLcdetailses = e.COM_EX_LCDETAILS.Select(f => new COM_EX_LCDETAILS
                    {
                        TRNSID = f.TRNSID
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<DashboardViewModel> GetLCChartData()
        {
            try
            {
                var date = Convert.ToDateTime("2022-01-24");
                var dashBoardViewModel = new DashboardViewModel()
                {
                    LcChartDataViewModel = new LcChartDataViewModel()
                    {
                        LCQty = await DenimDbContext.COM_EX_LCINFO
                            .Where(c => c.LCDATE.Equals(Convert.ToDateTime("2022-01-24")))
                            .Select(d => new COM_EX_LCINFO()
                            {
                                LCNO = d.LCNO

                            }).CountAsync(),
                        LCValue = await DenimDbContext.COM_EX_LCINFO
                            .Where(c => c.LCDATE.Equals(Convert.ToDateTime("2022-01-24")))
                            .Select(d => new COM_EX_LCINFO()
                            {
                                VALUE = d.VALUE

                            }).SumAsync(c => Convert.ToDouble(c.VALUE ?? 0)),

                        LCQtyY = await DenimDbContext.COM_EX_LCINFO
                            .Where(c => c.LCDATE.Equals(date.AddDays(-1)))
                            .Select(d => new COM_EX_LCINFO()
                            {
                                LCNO = d.LCNO

                            }).CountAsync(),
                        LCValueY = await DenimDbContext.COM_EX_LCINFO
                            .Where(c => c.LCDATE.Equals(date.AddDays(-1)))
                            .Select(d => new COM_EX_LCINFO()
                            {
                               VALUE = d.VALUE

                            }).SumAsync(c => Convert.ToDouble(c.VALUE ?? 0)),



                    }
                };


                return dashBoardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComExInvoiceMasterCreateViewModel> GetLcDetailsByIdAsync(int lcId)
        {
            try
            {
                var comExInvoiceMasterCreateViewModel = await DenimDbContext.COM_EX_LCINFO
                    .Include(e => e.COM_EX_LCDETAILS)
                    .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                    .ThenInclude(e => e.STYLE.FABCODENavigation)
                    .Include(e => e.COM_EX_INVOICEMASTER)
                    .ThenInclude(e => e.ComExInvdetailses)
                    .ThenInclude(e => e.ComExFabstyle.FABCODENavigation)
                    .Where(e => e.LCID.Equals(lcId))
                    .Select(e => new ComExInvoiceMasterCreateViewModel
                    {
                        ComExInvoicemaster = new COM_EX_INVOICEMASTER
                        {
                            ComExInvdetailses = e.COM_EX_INVOICEMASTER.SelectMany(x => x.ComExInvdetailses.Select(ef => new COM_EX_INVDETAILS
                            {
                                INVNO = ef.INVNO,
                                RATE = ef.RATE,
                                QTY = ef.QTY,
                                AMOUNT = ef.AMOUNT,
                                ComExFabstyle = new COM_EX_FABSTYLE
                                {
                                    STYLEID = ef.ComExFabstyle.STYLEID,
                                    STYLENAME = ef.ComExFabstyle.STYLENAME,
                                    FABCODENavigation = ef.ComExFabstyle.FABCODENavigation
                                }
                            })).ToList(),

                            LC = new COM_EX_LCINFO
                            {
                                COM_EX_LCDETAILS = e.COM_EX_LCDETAILS.Select(g => new COM_EX_LCDETAILS
                                {
                                    PI = new COM_EX_PIMASTER
                                    {
                                        COM_EX_PI_DETAILS = g.PI.COM_EX_PI_DETAILS.Select(h => new COM_EX_PI_DETAILS
                                        {
                                            PIMASTER = h.PIMASTER,
                                            UNITPRICE = h.UNITPRICE,
                                            QTY = h.QTY,
                                            TOTAL = h.TOTAL,
                                            STYLE = new COM_EX_FABSTYLE
                                            {
                                                STYLEID = h.STYLE.STYLEID,
                                                STYLENAME = h.STYLE.STYLENAME,
                                                FABCODENavigation = h.STYLE.FABCODENavigation
                                            }
                                        }).ToList()
                                    }
                                }).ToList()
                            }
                        }
                    }).FirstOrDefaultAsync();

                return comExInvoiceMasterCreateViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ComExLcInfoViewModel> FindByLcIdAllAsync(int lcId)
        {
            try
            {
                var x = await DenimDbContext.COM_EX_LCINFO
                    .Include(c => c.COM_EX_LCDETAILS)
                    .ThenInclude(c => c.PI)
                    .ThenInclude(c => c.COM_EX_PI_DETAILS)
                    .Include(c => c.COM_EX_LCDETAILS)
                    .ThenInclude(c => c.BANK)
                    .Include(c => c.COM_EX_LCDETAILS)
                    .ThenInclude(c => c.BANK_)
                    .FirstOrDefaultAsync(c => c.LCID.Equals(lcId));

                var comExLcInfoViewModel = new ComExLcInfoViewModel
                {
                    cOM_EX_LCINFO = x,
                    //comExPIViewModels = x.COM_EX_LCDETAILS.Select(c => new ComExPIViewModel
                    //{
                    //    PIID =(int)c.PIID,
                    //    REMARKS =c.REMARKS,
                    //    BANKID =c.BANKID,
                    //    BANK_ID =c.BANK_ID,
                    //    PIFILEUPLOADTEXT = c.PIFILE,
                    //    ComExPimaster = c.PI,
                    //    BasBenBankMaster = c.BANK,
                    //    BasBenBankMaster_Nego = c.BANK_,
                    //    TOTAL = c.PI.COM_EX_PI_DETAILS.Sum(e=>e.TOTAL)
                    //}).ToList(),
                };

                return comExLcInfoViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<IEnumerable<COM_EX_LCINFO>> GetAllLcByLcNo(string lcNo)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_LCINFO.Where(c => c.LCNO.Equals(lcNo)).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<CreateComExLcInfoViewModel> GetInitObjForDetailsTable(CreateComExLcInfoViewModel createComExLcInfoViewModel)
        {
            foreach (var item in createComExLcInfoViewModel.ComExLcdetailses)
            {
                var comExPimaster = await DenimDbContext.COM_EX_PIMASTER
                    .Include(e => e.COM_EX_PI_DETAILS)
                    .FirstOrDefaultAsync(e => e.PIID.Equals(item.PIID));
                item.PI = comExPimaster;
                //item.PI.COM_EX_PI_DETAILS = comExPimaster.COM_EX_PI_DETAILS;
                item.BANK = await DenimDbContext.BAS_BEN_BANK_MASTER.FirstOrDefaultAsync(e => e.BANKID.Equals(item.BANKID));
                item.BANK_ = await DenimDbContext.BAS_BEN_BANK_MASTER.FirstOrDefaultAsync(e => e.BANKID.Equals(item.BANK_ID));
            }

            return createComExLcInfoViewModel;
        }

        #endregion
        public async Task<CreateComExLcInfoViewModel> GetInitObjByAsync(CreateComExLcInfoViewModel comExInvoiceMasterViewModel)
        {
            comExInvoiceMasterViewModel.Currency = StaticData.GetCurrency();
            comExInvoiceMasterViewModel.Status = StaticData.GetStatus();

            comExInvoiceMasterViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            comExInvoiceMasterViewModel.bAS_BUYER_BANK_MASTERs = await DenimDbContext.BAS_BUYER_BANK_MASTER
                .Select(c => new BAS_BUYER_BANK_MASTER
                {
                    BANK_ID = c.BANK_ID,
                    PARTY_BANK = $"{c.PARTY_BANK} ({c.ADDRESS})"
                }).OrderBy(e => e.PARTY_BANK).ToListAsync();

            comExInvoiceMasterViewModel.MktTeams = await DenimDbContext.MKT_TEAM.Select(e => new MKT_TEAM
            {
                MKT_TEAMID = e.MKT_TEAMID,
                PERSON_NAME = e.PERSON_NAME
            }).OrderBy(e => e.PERSON_NAME).ToListAsync();

            comExInvoiceMasterViewModel.bAS_BEN_BANK_MASTERs = await DenimDbContext.BAS_BEN_BANK_MASTER.Select(e => new BAS_BEN_BANK_MASTER
            {
                BANKID = e.BANKID,
                BEN_BANK = e.BEN_BANK
            }).OrderBy(e => e.BEN_BANK).ToListAsync();

            //comExInvoiceMasterViewModel.cOM_EX_PIMASTERs = await _denimDbContext.COM_EX_PIMASTER
            //    .Where(e => !_denimDbContext.COM_EX_LCDETAILS.Any(f => f.PIID.Equals(e.PIID)))
            //    .Select(e => new COM_EX_PIMASTER
            //    {
            //        PIID = e.PIID,
            //        PINO = e.PINO
            //    }).OrderBy(e => e.PINO).ToListAsync();

            comExInvoiceMasterViewModel.ComTenors = await DenimDbContext.COM_TENOR.ToListAsync();
            comExInvoiceMasterViewModel.ComTradeTerms = await DenimDbContext.COM_TRADE_TERMS.Where(c => c.ISACTIVE).ToListAsync();
            comExInvoiceMasterViewModel.ComTradeTermsEdit = await DenimDbContext.COM_TRADE_TERMS.ToListAsync();

            return comExInvoiceMasterViewModel;
        }

        public async Task<CreateComExLcInfoViewModel> FindBy_IdIncludeAllAsync(int lcId)
        {
            return await DenimDbContext.COM_EX_LCINFO
                .Include(e => e.COM_EX_LCDETAILS)
                .ThenInclude(e => e.BANK)
                .Include(e => e.COM_EX_LCDETAILS)
                .ThenInclude(e => e.BANK_)
                .Include(e => e.COM_EX_LCDETAILS)
                .ThenInclude(e => e.PI.COM_EX_PI_DETAILS)
                .Where(e => e.LCID.Equals(lcId))
                .Select(e => new CreateComExLcInfoViewModel
                {
                    ComExLcinfo = new COM_EX_LCINFO
                    {
                        LCID = e.LCID,
                        EncryptedId = _protector.Protect(e.LCID.ToString()),
                        FILENO = e.FILENO,
                        LCNO = e.LCNO,
                        LC_STATUS = e.LC_STATUS,
                        MLCDATE = e.MLCDATE,
                        LCDATE = e.LCDATE,
                        AMENTNO = e.AMENTNO,
                        AMENTDATE = e.AMENTDATE,
                        AMENTVALUE = e.AMENTVALUE,
                        VALUE = e.VALUE,
                        CURRENCY = e.CURRENCY,
                        BUYERID = e.BUYERID,
                        BANK_ID = e.BANK_ID,
                        TEAMID = e.TEAMID,
                        EX_DATE = e.EX_DATE,
                        SHIP_DATE = e.SHIP_DATE,
                        TTERMS = e.TTERMS,
                        TID = e.TID,
                        TRADETERMS = e.TRADETERMS,
                        TENOR = e.TENOR,
                        ODUEINTEREST = e.ODUEINTEREST,
                        EXP = e.EXP,
                        MLCNO = e.MLCNO,
                        IRC = e.IRC,
                        ERC = e.ERC,
                        GARMENT_QTY = e.GARMENT_QTY,
                        UNIT = e.UNIT,
                        ITEM = e.ITEM,
                        VAT_REG = e.VAT_REG,
                        VAT_REG_BANK = e.VAT_REG_BANK,
                        AREA = e.AREA,
                        TIN = e.TIN,
                        OTHERS = e.OTHERS,
                        BVAT_REG = e.BVAT_REG,
                        BAREA = e.BAREA,
                        BTIN = e.BTIN,
                        HSCODE = e.HSCODE,
                        ADREF = e.ADREF,
                        BANKID = e.BANKID,
                        NEGOBANKID = e.NEGOBANKID,
                        NTFYBANKID = e.NTFYBANKID,
                        LCRCVDATE = e.LCRCVDATE,
                        DISCOUNT = e.DISCOUNT,
                        REMARKS = e.REMARKS,
                        UDNO = e.UDNO,
                        UDDATE = e.UDDATE,
                        UDSUBDATE = e.UDSUBDATE,
                        MLCSUBDATE = e.MLCSUBDATE,
                        UPNO = e.UPNO,
                        UP_DATE = e.UP_DATE,
                        PORTLOADING = e.PORTLOADING,
                        PORTDISCHARGE = e.PORTDISCHARGE,
                        VESSEL = e.VESSEL,
                        MARKS = e.MARKS,
                        SAILING = e.SAILING,
                        DOSTATUS = e.DOSTATUS,
                        CONTRACTSTATUS = e.CONTRACTSTATUS,
                        EXPORTSTATUS = e.EXPORTSTATUS,
                        ISDELETE = e.ISDELETE,
                        USRID = e.USRID,
                        UDFILEUPLOAD = e.UDFILEUPLOAD,
                        UPFILEUPLOAD = e.UPFILEUPLOAD,
                        COSTSHEETFILEUPLOAD = e.COSTSHEETFILEUPLOAD,
                        MLCFILE = e.MLCFILE,
                        LCFILE = e.LCFILE,
                        LC_CANCEL_REMARKS = e.LC_CANCEL_REMARKS
                    },
                    ComExLcdetailses = e.COM_EX_LCDETAILS.Select(f => new COM_EX_LCDETAILS
                    {
                        TRNSID = f.TRNSID,
                        LCNO = f.LCNO,
                        LCID = f.LCID,
                        PIID = f.PIID,
                        REMARKS = f.REMARKS,
                        USRID = f.USRID,
                        ISDELETE = f.ISDELETE,
                        PIFILE = f.PIFILE,
                        BANKID = f.BANKID,
                        BANK_ID = f.BANK_ID,
                        PI = new COM_EX_PIMASTER
                        {
                            PINO = f.PI.PINO,
                            COM_EX_PI_DETAILS = f.PI.COM_EX_PI_DETAILS.Select(g => new COM_EX_PI_DETAILS
                            {
                                SO_STATUS = g.SO_STATUS,
                                QTY = g.QTY,
                                TOTAL = g.TOTAL
                            }).ToList()
                        },
                        BANK = f.BANK,
                        BANK_ = f.BANK_
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> IsFileNoInUseByAsync(string fileNo)
        {
            return await DenimDbContext.COM_EX_LCINFO.AnyAsync(e => e.FILENO.Equals(fileNo));
        }
    }
}
