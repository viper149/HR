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
using DenimERP.ViewModels.Com.Export;
using DenimERP.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLCOM_EX_PIMASTER_Repository : BaseRepository<COM_EX_PIMASTER>, ICOM_EX_PIMASTER
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _protector;

        public SQLCOM_EX_PIMASTER_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor) : base(denimDbContext)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<COM_EX_PIMASTER>> GetForDataTableByAsync()
        {
            return await DenimDbContext.COM_EX_PIMASTER
                .Include(d => d.BUYER)
                .Include(d => d.BANK)
                .Include(d => d.PersonMktTeam.BasTeamInfo)
                .Include(d => d.COM_EX_PI_DETAILS)
                .ThenInclude(d=>d.STYLE.FABCODENavigation)
                .Select(d => new COM_EX_PIMASTER
                {
                    PIID = d.PIID,
                    EncryptedId = _protector.Protect(d.PIID.ToString()),
                    PINO = d.PINO,
                    PIDATE = d.PIDATE,
                    LcNoPi = !d.COM_EX_LCDETAILS.Any() ? "" : d.COM_EX_LCDETAILS.FirstOrDefault().LC.LCNO,
                    FileNo = !d.COM_EX_LCDETAILS.Any() ? "" : d.COM_EX_LCDETAILS.FirstOrDefault().LC.FILENO,
                    PI_QTY = d.PI_QTY,
                    PI_TOTAL_VALUE = d.PI_TOTAL_VALUE,
                    NON_EDITABLE = d.NON_EDITABLE,
                    BUYER = new BAS_BUYERINFO
                    {
                        BUYER_NAME = d.BUYER.BUYER_NAME
                    },
                    BANK = new BAS_BEN_BANK_MASTER
                    {
                        BEN_BANK = d.BANK.BEN_BANK
                    },
                    PersonMktTeam = new MKT_TEAM
                    {
                        PERSON_NAME = $"{d.PersonMktTeam.PERSON_NAME} ({d.PersonMktTeam.BasTeamInfo.TEAM_NAME})"
                    },
                    COM_EX_PI_DETAILS = d.COM_EX_PI_DETAILS.Select(e=> new COM_EX_PI_DETAILS
                    {
                        SO_NO = e.SO_NO ?? "",
                        STYLE = new COM_EX_FABSTYLE
                        {
                            FABCODENavigation = new RND_FABRICINFO
                            {
                                STYLE_NAME = e.STYLE != null ? e.STYLE.FABCODENavigation != null ? e.STYLE.FABCODENavigation.STYLE_NAME : "" : ""
                            }
                        }
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<COM_EX_PIMASTER> FindByPiIdAsync(int piId)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_PIMASTER
                    .Include(c => c.BANK)
                    .Include(c => c.BRAND)
                    .Include(c => c.BANK_)
                    .Include(c => c.TEAM)
                    .Include(c => c.COM_EX_PI_DETAILS)
                    .ThenInclude(c => c.STYLE)
                    .ThenInclude(c => c.FABCODENavigation)
                    .ThenInclude(c => c.WV)
                    .Where(pi => pi.PIID == piId)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<COM_EX_PIMASTER> FindByIdPIInfoAsync(int? piId)
        {
            try
            {
                var result = await DenimDbContext.COM_EX_PIMASTER
                    .Where(pi => pi.PIID == piId)
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> InsertAndGetIdByAsync(COM_EX_PIMASTER comExPiMaster)
        {
            try
            {
                var entityEntry = await DenimDbContext.COM_EX_PIMASTER.AddAsync(comExPiMaster);
                await SaveChanges();
                return entityEntry.Entity.PIID;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> TotalPercentageOfComExPiMaster(int days = 7)
        {
            var countAsync = await DenimDbContext.COM_EX_PIMASTER.Where(e => e.PIDATE > DateTime.Now.AddDays(-days)).CountAsync();
            return countAsync;
        }

        public async Task<bool> FindByPINoInUseAsync(string piNo)
        {
            var IsExistPINo = await DenimDbContext.COM_EX_PIMASTER.AnyAsync(pi => pi.PINO.Equals(piNo));
            return IsExistPINo;
        }

        public async Task<IEnumerable<COM_EX_FABSTYLE>> FindFabStyleByPiIdAsync(int piId)
        {
            try
            {
                //var cOM_EX_FABSTYLEs = new List<COM_EX_FABSTYLE>();
                //var cOM_EX_PI_DETAILs = await _denimDbContext.COM_EX_PI_DETAILS.Where(c => c.PIID == piId).ToListAsync();


                //foreach (var item in cOM_EX_PI_DETAILs)
                //{
                //    var fabStyle = await _denimDbContext.COM_EX_FABSTYLE.Where(c => c.STYLEID == item.STYLEID).FirstOrDefaultAsync();
                //    cOM_EX_FABSTYLEs.Add(new COM_EX_FABSTYLE
                //    {
                //        STYLEID = fabStyle.STYLEID,
                //        STYLENAME = fabStyle.STYLENAME
                //    });
                //}

                var cOM_EX_FABSTYLEs = await DenimDbContext.COM_EX_PI_DETAILS
                    .Include(c => c.STYLE.FABCODENavigation)
                    .Where(c => c.PIID.Equals(piId))
                    .Select(c => new COM_EX_FABSTYLE
                    {
                        STYLEID = c.STYLE.STYLEID,
                        STYLENAME = $"{c.STYLE.FABCODENavigation.STYLE_NAME} - {c.STYLE.STYLENAME}"
                    })
                    .ToListAsync();

                return cOM_EX_FABSTYLEs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ComExPIMasterViewModel> GetInitObjects(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            var currency = new Dictionary<int, string>
            {
                {2, "USD$"},
                {11, "Euro€"}
            };

            comExPiMasterViewModel.Currencies = await DenimDbContext.CURRENCY
                .Where(d => currency.ContainsKey(d.Id) || currency.ContainsValue(d.CODE))
                .Select(d => new CURRENCY
                {
                    Id = d.Id,
                    CODE = d.CODE
                }).OrderBy(d => d.Id).ToListAsync();

            comExPiMasterViewModel.FBasUnitses = await DenimDbContext.F_BAS_UNITS
                .Where(e => e.UNAME.ToLower().Contains("yds") || e.UNAME.ToLower().Contains("mtr"))
                .Select(e => new F_BAS_UNITS
                {
                    UID = e.UID,
                    UNAME = e.UNAME
                }).OrderBy(e => e.UNAME).ToListAsync();

            comExPiMasterViewModel.Exportstatuses = await DenimDbContext.EXPORTSTATUS.Select(e => new EXPORTSTATUS
            {
                EXTYPEID = e.EXTYPEID,
                EXPPORTTYPE = e.EXPPORTTYPE
            }).OrderBy(e => e.EXPPORTTYPE).ToListAsync();

            comExPiMasterViewModel.bAS_BRANDINFOs = await DenimDbContext.BAS_BRANDINFO.Select(e => new BAS_BRANDINFO
            {
                BRANDID = e.BRANDID,
                BRANDNAME = e.BRANDNAME
            }).OrderBy(e => e.BRANDNAME).ToListAsync();

            comExPiMasterViewModel.bAS_BUYER_BANK_MASTERs = await DenimDbContext.BAS_BUYER_BANK_MASTER.Select(e => new BAS_BUYER_BANK_MASTER
            {
                BANK_ID = e.BANK_ID,
                PARTY_BANK = e.PARTY_BANK
            }).OrderBy(e => e.PARTY_BANK).ToListAsync();

            comExPiMasterViewModel.bAS_BEN_BANK_MASTERs = await DenimDbContext.BAS_BEN_BANK_MASTER.Select(e => new BAS_BEN_BANK_MASTER
            {
                BANKID = e.BANKID,
                BEN_BANK = e.BEN_BANK

            }).OrderBy(e => e.BEN_BANK).ToListAsync();

            comExPiMasterViewModel.bAS_TEAMINFOs = await DenimDbContext.BAS_TEAMINFO
                .Where(e => e.TEAM_NAME.Contains("Team-A") || e.TEAM_NAME.Contains("Team-B") || e.TEAM_NAME.Contains("Team-C"))
                .Select(e => new BAS_TEAMINFO
                {
                    TEAMID = e.TEAMID,
                    TEAM_NAME = e.TEAM_NAME
                }).OrderBy(e => e.TEAM_NAME).ToListAsync();

            comExPiMasterViewModel.MktTeams = await DenimDbContext.MKT_TEAM.Select(e => new MKT_TEAM
            {
                MKT_TEAMID = e.MKT_TEAMID,
                PERSON_NAME = e.PERSON_NAME
            }).OrderBy(e => e.PERSON_NAME).ToListAsync();

            comExPiMasterViewModel.rND_FABRICINFOs = await DenimDbContext.RND_FABRICINFO.Select(e => new RND_FABRICINFO
            {
                DEVID = e.DEVID,
                FABCODE = e.FABCODE
            }).OrderBy(e => e.FABCODE).ToListAsync();

            comExPiMasterViewModel.bAS_COLORs = await DenimDbContext.BAS_COLOR.Select(e => new BAS_COLOR
            {
                COLORCODE = e.COLORCODE,
                COLOR = e.COLOR
            }).OrderBy(e => e.COLOR).ToListAsync();

            comExPiMasterViewModel.bAS_BUYERINFOs = await DenimDbContext.BAS_BUYERINFO.Select(e => new BAS_BUYERINFO
            {
                BUYERID = e.BUYERID,
                BUYER_NAME = e.BUYER_NAME
            }).OrderBy(e => e.BUYER_NAME).ToListAsync();

            comExPiMasterViewModel.cOM_EX_FABSTYLEs = await DenimDbContext.COM_EX_FABSTYLE
                .Include(c => c.FABCODENavigation.WV)
                .Include(c => c.BRAND)
                .Select(c => new COM_EX_FABSTYLE
                {
                    STYLEID = c.STYLEID,
                    STYLENAME = $"{c.STYLENAME}, {c.FABCODENavigation.STYLE_NAME}, {c.BRAND.BRANDNAME}",
                    FABCODENavigation = new RND_FABRICINFO
                    {
                        WV = new RND_SAMPLE_INFO_WEAVING
                        {
                            FABCODE = c.FABCODENavigation.WV.FABCODE
                        },
                        STYLE_NAME = c.FABCODENavigation.STYLE_NAME
                    },
                    BRAND = new BAS_BRANDINFO
                    {
                        BRANDNAME = c.BRAND.BRANDNAME
                    }
                }).OrderByDescending(c => c.STYLEID).ToListAsync();


            comExPiMasterViewModel.ComTenors = await DenimDbContext.COM_TENOR
                //.Where(e => new int[] { 46, 47, 48, 49, 50 }.Any(f => f.Equals(e.TID)))
                .Select(e => new COM_TENOR
                {
                    TID = e.TID,
                    NAME = e.NAME,
                    CODE_LEVEL = e.CODE_LEVEL
                }).OrderBy(e => e.CODE_LEVEL).ToListAsync();

            comExPiMasterViewModel.CosPreCostingMasters = await DenimDbContext.COS_PRECOSTING_MASTER.Select(c => new COS_PRECOSTING_MASTER
            {
                CSID = c.CSID,
                OPTION1 = $"CS-{c.CSID}"
            }).OrderBy(e => e.OPTION1).ToListAsync();


            comExPiMasterViewModel.BasSeasonList= await DenimDbContext.BAS_SEASON
                .Select(d => new BAS_SEASON
                {
                    SID = d.SID,
                    SNAME = d.SNAME,
                }).ToListAsync();

            return comExPiMasterViewModel;
        }

        public async Task<ComExPIMasterViewModel> GetCostRefNo(int styleId)
        {
            var comExFabstyle = await DenimDbContext.COM_EX_FABSTYLE
                .Include(e => e.FABCODENavigation)
                .FirstOrDefaultAsync(e => e.STYLEID.Equals(styleId));

            var cosPrecostingMasters = await DenimDbContext.COS_PRECOSTING_MASTER
                .Include(e => e.FABCODENavigation)
                .Where(e => e.FABCODENavigation.STYLE_NAME.Contains(comExFabstyle.FABCODENavigation.STYLE_NAME))
                .ToListAsync();


            var comExPiMasterViewModel = new ComExPIMasterViewModel
            {
                ComExFabStyle = await DenimDbContext.COM_EX_FABSTYLE
                .Include(c => c.FABCODENavigation.COS_PRECOSTING_MASTER)
                .Include(c => c.FABCODENavigation.WV)
                .Include(c => c.FABCODENavigation.COLORCODENavigation)
                .Where(c => c.FABCODENavigation.STYLE_NAME.StartsWith(comExFabstyle.FABCODENavigation.STYLE_NAME))
                .FirstOrDefaultAsync(),
                CosPreCostingMasters = cosPrecostingMasters
            };

            return comExPiMasterViewModel;
        }

        public async Task<string> GetLastPINoAsync()
        {
            try
            {
                string piNo;
                var result = await DenimDbContext.COM_EX_PIMASTER.OrderByDescending(c => c.PINO).Select(c => c.PINO).FirstOrDefaultAsync();
                var year = DateTime.Now.Year % 100;

                if (result != null)
                {
                    var resultArray = result.Split("/");
                    if (int.Parse(resultArray[1]) < year)
                    {
                        piNo = $"PDL/{year}/{"1".PadLeft(4, '0')}";
                    }
                    else
                    {
                        int.TryParse(new string(resultArray[2].SkipWhile(x => !char.IsDigit(x)).TakeWhile(char.IsDigit).ToArray()), out var currentNumber);

                        piNo = $"PDL/{year}/{(currentNumber + 1).ToString().PadLeft(4, '0')}";
                    }
                }
                else
                {
                    piNo = $"PDL/{year}/{"1".PadLeft(4, '0')}";
                }

                return piNo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ComExPIMasterViewModel> GetInitObjForDetailsTable(ComExPIMasterViewModel comExPiMasterViewModel)
        {
            foreach (var item in comExPiMasterViewModel.cOM_EX_PI_DETAILs)
            {
                item.STYLE = await DenimDbContext.COM_EX_FABSTYLE
                    .Include(e => e.FABCODENavigation)
                    .FirstOrDefaultAsync(e => e.STYLEID.Equals(item.STYLEID));

                item.F_BAS_UNITS = await DenimDbContext.F_BAS_UNITS.FirstOrDefaultAsync(e => e.UID.Equals(item.UNIT));

                double? totalWidth = null;
                double? grossWeight;

                var position = item.STYLE.FABCODENavigation.WIDEC.IndexOf('/');
                var characterArray = item.STYLE.FABCODENavigation.WIDEC.ToCharArray();

                if (position != -1)
                {
                    var leftWidth = $"{characterArray[position - 2]}{characterArray[position - 1]}";
                    var rightWidth = $"{characterArray[position + 1]}{characterArray[position + 2]}";
                    totalWidth = double.Parse(leftWidth) + double.Parse(rightWidth);
                }
                else
                {
                    if (item.STYLE.FABCODENavigation.WIDEC.ToLower().Contains("cw") && !item.STYLE.FABCODENavigation.WIDEC.ToLower().Contains("/"))
                    {
                        var strings = item.STYLE.FABCODENavigation.WIDEC.ToLower().Split(new char[] { 'c', 'w', '"' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
                        totalWidth = double.Parse(strings ?? "0");
                    }
                }


                var netWeight = double.Parse(item.STYLE.FABCODENavigation.WGDEC) * (totalWidth / 2) / 36 / 16 / 2.2046 * item.QTY;

                switch (item.UNIT)
                {
                    // YDS = 7
                    case 7:

                        grossWeight = item.QTY / 90;

                        if (netWeight != null && grossWeight != null)
                        {
                            comExPiMasterViewModel.NET_WEIGHT += Math.Round((double)netWeight, 2, MidpointRounding.AwayFromZero);
                            comExPiMasterViewModel.GROSS_WEIGHT += Math.Round((double)(netWeight + grossWeight), 2, MidpointRounding.AwayFromZero);
                        }

                        break;
                    // MTR = 6 [1 METER = 1.09361 YARDS]
                    case 6:

                        netWeight *= 1.09361;
                        grossWeight = item.QTY / 90;

                        if (netWeight != null && grossWeight != null)
                        {
                            comExPiMasterViewModel.NET_WEIGHT += Math.Round((double)netWeight, 2, MidpointRounding.AwayFromZero);
                            comExPiMasterViewModel.GROSS_WEIGHT += Math.Round((double)(netWeight + grossWeight), 2, MidpointRounding.AwayFromZero);
                        }

                        break;
                }
            }

            return comExPiMasterViewModel;
        }

        public async Task<ComExPIMasterViewModel> FindByPiIdIncludeAllAsync(int piId)
        {
            var comExPiMasterViewModel = await DenimDbContext.COM_EX_PIMASTER
                .Include(c => c.BANK)
                .Include(c => c.BRAND)
                .Include(c => c.BANK_)
                .Include(c => c.TEAM)
                .Include(e => e.EXPORTSTATUS)
                .Include(c => c.COM_EX_PI_DETAILS)
                .ThenInclude(e => e.F_BAS_UNITS)
                .Include(c => c.COM_EX_PI_DETAILS)
                .ThenInclude(c => c.STYLE.FABCODENavigation.WV)
                .Include(c => c.COM_EX_PI_DETAILS)
                .ThenInclude(c => c.PRECOSTING)
                .Include(c => c.COM_EX_PI_DETAILS)
                .ThenInclude(c => c.RND_PRODUCTION_ORDER)
                .Where(pi => pi.PIID == piId && !pi.NON_EDITABLE)
                .Select(e => new ComExPIMasterViewModel
                {
                    cOM_EX_PIMASTER = new COM_EX_PIMASTER
                    {
                        PIID = e.PIID,
                        EncryptedId = _protector.Protect(e.PIID.ToString()),
                        PINO = e.PINO,
                        PIDATE = e.PIDATE,
                        DURATION = e.DURATION,
                        VALIDITY = e.VALIDITY,
                        BUYERID = e.BUYERID,
                        TENOR = e.TENOR,
                        CURRENCY = e.CURRENCY,
                        BANKID = e.BANKID,
                        DEL_PERIOD = e.DEL_PERIOD,
                        TOLERANCE = e.TOLERANCE,
                        NEGOTIATION = e.NEGOTIATION,
                        INCOTERMS = e.INCOTERMS,
                        INSPECTION = e.INSPECTION,
                        TEAMID = e.TEAMID,
                        TEAM_PERSONID = e.TEAM_PERSONID,
                        ORDER_REF = e.ORDER_REF,
                        BANK_ID = e.BANK_ID,
                        POL = e.POL,
                        POD = e.POD,
                        GRS_WEIGHT = e.GRS_WEIGHT,
                        NET_WEIGHT = e.NET_WEIGHT,
                        COO = e.COO,
                        INSURANCE_COVERAGE = e.INSURANCE_COVERAGE,
                        PAYMENT = e.PAYMENT,
                        LCNO = e.LCNO,
                        SHIPDATE = e.SHIPDATE,
                        FREIGHT = e.FREIGHT,
                        PONOTE = e.PONOTE,
                        PREVIOUS_DELIVERY_NOTE = e.PREVIOUS_DELIVERY_NOTE,
                        EXP_STATUS = e.EXP_STATUS,
                        STATUS = e.STATUS,
                        DEL_START = e.DEL_START,
                        DEL_CLOSE = e.DEL_CLOSE,
                        BRANDID = e.BRANDID,
                        FLWBY = e.FLWBY,
                        ISDELETE = e.ISDELETE,
                        OPT1 = e.OPT1,
                        OPT2 = e.OPT2,
                        SID = e.SID,
                        BANK = e.BANK,
                        BANK_ = e.BANK_,
                        BUYER = e.BUYER,
                        BRAND = e.BRAND,
                        TEAM = e.TEAM,
                        PersonMktTeam = e.PersonMktTeam,
                        TENORNavigation = e.TENORNavigation,
                        EXPORTSTATUS = e.EXPORTSTATUS,
                        CURRENCYS = e.CURRENCYS
                    },
                    cOM_EX_PI_DETAILs = e.COM_EX_PI_DETAILS.Where(c => c.SO_STATUS).Select(f => new COM_EX_PI_DETAILS
                    {
                        TRNSID = f.TRNSID,
                        PIID = f.PIID,
                        PINO = f.PINO,
                        SO_NO = f.SO_NO,
                        STYLEID = f.STYLEID,
                        UNIT = f.UNIT,
                        COSTID = f.COSTID,
                        COSTREF = f.COSTREF,
                        QTY = f.QTY,
                        INITIAL_QTY = f.INITIAL_QTY,
                        UNITPRICE = f.UNITPRICE,
                        TOTAL = f.TOTAL,
                        REMARKS = f.REMARKS,
                        ISDELETE = f.ISDELETE,
                        SO_STATUS = f.SO_STATUS,
                        STYLE = new COM_EX_FABSTYLE
                        {
                            STYLEID = f.STYLE != null ? f.STYLE.STYLEID : 0,
                            STYLENAME = $"{f.STYLE.STYLENAME}",
                            FABCODENavigation = new RND_FABRICINFO
                            {
                                FABCODE = f.STYLE != null && f.STYLE.FABCODENavigation != null ? f.STYLE.FABCODENavigation.FABCODE : 0,
                                STYLE_NAME = $"{f.STYLE.FABCODENavigation.STYLE_NAME}"
                            }
                        },
                        PRECOSTING = new COS_PRECOSTING_MASTER
                        {
                            CSID = f.PRECOSTING != null ? f.PRECOSTING.CSID : 0,
                            OPTION1 = f.PRECOSTING != null ? $"CS-{f.PRECOSTING.CSID}" : ""
                        },
                        F_BAS_UNITS = new F_BAS_UNITS
                        {
                            UID = f.F_BAS_UNITS != null ? f.F_BAS_UNITS.UID : 0,
                            UNAME = $"{f.F_BAS_UNITS.UNAME}"
                        },
                        RND_PRODUCTION_ORDER = f.RND_PRODUCTION_ORDER.Select(g=> new RND_PRODUCTION_ORDER
                        {
                            POID = g.POID
                        }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();

            return comExPiMasterViewModel;
        }

        public async Task<CreateComExLcInfoViewModel> GetPiInformationByAsync(CreateComExLcInfoViewModel comExLcInfoViewModel, string search, int page = 1)
        {
            var createComExLcInfoViewModel = new CreateComExLcInfoViewModel();

            if (!string.IsNullOrEmpty(search))
            {
                createComExLcInfoViewModel.cOM_EX_PIMASTERs = await DenimDbContext.COM_EX_PIMASTER
                    .OrderByDescending(e => e.PINO)
                    .Select(e => new COM_EX_PIMASTER
                    {
                        PIID = e.PIID,
                        PINO = e.PINO
                    }).Where(e => !DenimDbContext.COM_EX_LCDETAILS.Any(f => f.PIID.Equals(e.PIID)) && !comExLcInfoViewModel.ComExLcdetailses.Any(f => f.PIID.Equals(e.PIID)) && e.PINO.ToLower().Contains(search.ToLower())).ToListAsync();
            }
            else
            {
                createComExLcInfoViewModel.cOM_EX_PIMASTERs = await DenimDbContext.COM_EX_PIMASTER
                    .OrderByDescending(e => e.PINO)
                    .Where(e => !DenimDbContext.COM_EX_LCDETAILS.Any(f => f.PIID.Equals(e.PIID)) && !comExLcInfoViewModel.ComExLcdetailses.Any(f => f.PIID.Equals(e.PIID)))
                    .Select(e => new COM_EX_PIMASTER
                    {
                        PIID = e.PIID,
                        PINO = e.PINO
                    }).ToListAsync();
            }

            return createComExLcInfoViewModel;
        }

        public async Task<IEnumerable<COM_EX_PIMASTER>> GetPiByBuyerAsync(int buyerId)
        {
            return await DenimDbContext.COM_EX_PIMASTER
                .Where(d => d.BUYERID.Equals(buyerId))
                .Select(d => new COM_EX_PIMASTER
                {
                    PIID = d.PIID,
                    PINO = d.PINO
                }).ToListAsync();
        }

        public async Task<DashboardViewModel> GetPIChartData()
        {
            try
            {
                var date = Convert.ToDateTime("2019-09-22 00:00:00.000");
                var dashboardViewModel = new DashboardViewModel
                {
                    PiChartDataViewModel = new PiChartDataViewModel
                    {
                        PIValue = await DenimDbContext.COM_EX_PIMASTER
                            .Where(c => c.PIDATE.Equals(date))
                            .Select(d => new COM_EX_PIMASTER()
                            {
                                //PINO = d.PINO,
                                PI_QTY = d.PI_QTY

                            }).SumAsync(c => Convert.ToDouble(c.PI_QTY ?? 0)),
                        PIQty = await DenimDbContext.COM_EX_PIMASTER
                            .Where(c => c.PIDATE.Equals(date))
                            .Select(d => new COM_EX_PIMASTER()
                            {
                                PINO = d.PINO

                            }).CountAsync(),
                        PIValueY = await DenimDbContext.COM_EX_PIMASTER
                            .Where(c => c.PIDATE.Equals(date.AddDays(-1)))
                            .Select(d => new COM_EX_PIMASTER()
                            {
                                //PINO = d.PINO,
                                PI_QTY = d.PI_QTY

                            }).SumAsync(c => Convert.ToDouble(c.PI_QTY ?? 0)),
                        PIQtyY = await DenimDbContext.COM_EX_PIMASTER
                            .Where(c => c.PIDATE.Equals(date.AddDays(-1)))
                            .Select(d => new COM_EX_PIMASTER()
                            {
                                PINO = d.PINO

                            }).CountAsync()
                    }
                };
                
                return dashboardViewModel;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
