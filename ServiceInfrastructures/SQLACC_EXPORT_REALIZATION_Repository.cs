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
using DenimERP.ViewModels.Home;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DenimERP.ServiceInfrastructures
{
    public class SQLACC_EXPORT_REALIZATION_Repository : BaseRepository<ACC_EXPORT_REALIZATION>, IACC_EXPORT_REALIZATION
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataProtector _protector;

        public SQLACC_EXPORT_REALIZATION_Repository(DenimDbContext denimDbContext,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings,
            UserManager<ApplicationUser> userManager) : base(denimDbContext)
        {
            _userManager = userManager;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        public async Task<IEnumerable<ACC_EXPORT_REALIZATION>> GetAccExRealizationAllAsync()
        {
            return await DenimDbContext.ACC_EXPORT_REALIZATION
                    .Include(e => e.INVOICE)
                    .Select(e => new ACC_EXPORT_REALIZATION
                    {
                        INVID = e.INVID,
                        EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                        REZDATE = e.REZDATE,
                        PRC_USD = e.PRC_USD,
                        ERQ = e.ERQ,
                        ORQ = e.ORQ,
                        CD = e.CD,
                        OD = e.OD,
                        RATE = e.RATE,
                        REMARKS = e.REMARKS,
                        INVOICE = new COM_EX_INVOICEMASTER
                        {
                            INVNO = $"{e.INVOICE.INVNO}"
                        }
                    }).ToListAsync();
        }

        public async Task<AccExRealizationViewModel> FindByIdIncludeAllAsync(int trnsId)
        {
            var accExRealizationViewModel = await DenimDbContext.ACC_EXPORT_REALIZATION
                .Include(e => e.INVOICE.LC.BUYER)
                .Include(e => e.INVOICE.LC.BANK_)
                .Include(e => e.INVOICE.ComExInvdetailses)
                .ThenInclude(e => e.ComExFabstyle.FABCODENavigation)
                .Where(e => e.TRNSID.Equals(trnsId))
                .Select(e => new AccExRealizationViewModel
                {
                    AccExportRealization = new ACC_EXPORT_REALIZATION
                    {
                        TRNSID = e.TRNSID,
                        EncryptedId = _protector.Protect(e.TRNSID.ToString()),
                        TRNSDATE = e.TRNSDATE,
                        INVID = e.INVID,
                        REZDATE = e.REZDATE,
                        PRC_USD = e.PRC_USD,
                        PRC_EURO = e.PRC_EURO,
                        ERQ = e.ERQ,
                        AUDITDATE = e.AUDITDATE,
                        AUDITBY = e.AUDITBY,
                        REMARKS = e.REMARKS,
                        INVOICE = new COM_EX_INVOICEMASTER
                        {
                            INVNO = $"{e.INVOICE.INVNO}",
                            INVDATE = e.INVOICE.INVDATE,
                            NEGODATE = e.INVOICE.NEGODATE,
                            INVDURATION = e.INVOICE.INVDURATION,
                            DELDATE = e.INVOICE.DELDATE,
                            DOC_SUB_DATE = e.INVOICE.DOC_SUB_DATE,
                            DOC_RCV_DATE = e.INVOICE.DOC_RCV_DATE,
                            BNK_SUB_DATE = e.INVOICE.BNK_SUB_DATE,
                            BANK_REF = e.INVOICE.BANK_REF,
                            BILL_DATE = e.INVOICE.BILL_DATE,
                            DISCREPANCY = e.INVOICE.DISCREPANCY,
                            BNK_ACC_DATE = e.INVOICE.BNK_ACC_DATE,
                            MATUDATE = e.INVOICE.MATUDATE,
                            BNK_ACC_POSTING = e.INVOICE.BNK_ACC_POSTING,
                            EXDATE = e.INVOICE.EXDATE,
                            ODAMOUNT = e.INVOICE.ODAMOUNT,
                            ODRCVDATE = e.INVOICE.ODRCVDATE,
                            PRCAMOUNT = e.INVOICE.PRCAMOUNT,
                            PRCDATE = e.INVOICE.PRCDATE,
                            PDOCNO = e.INVOICE.PDOCNO,
                            BANKREFPATH = e.INVOICE.BANKREFPATH,
                            BANKACCEPTPATH = e.INVOICE.BANKACCEPTPATH,
                            DISCREPANCYPATH = e.INVOICE.DISCREPANCYPATH,
                            PAYMENTPATH = e.INVOICE.PAYMENTPATH,
                            LC = new COM_EX_LCINFO
                            {
                                LCNO = $"{e.INVOICE.LC.LCNO}",
                                FILENO = $"{e.INVOICE.LC.FILENO}",
                                LC_STATUS = e.INVOICE.LC.LC_STATUS,
                                BANK_ = new BAS_BUYER_BANK_MASTER
                                {
                                    PARTY_BANK = $"{e.INVOICE.LC.BANK_.PARTY_BANK}"
                                }
                            },
                            BUYER = new BAS_BUYERINFO
                            {
                                BUYER_NAME = $"{e.INVOICE.BUYER.BUYER_NAME}"
                            }
                        }
                    },
                    ComExInvdetailses = e.INVOICE.ComExInvdetailses.Select(f => new COM_EX_INVDETAILS
                    {
                        TRNSID = f.TRNSID,
                        ROLL = f.ROLL,
                        QTY = f.QTY,
                        RATE = f.RATE,
                        AMOUNT = f.AMOUNT,
                        REMARKS = f.REMARKS,
                        ComExFabstyle = new COM_EX_FABSTYLE
                        {
                            FABCODENavigation = new RND_FABRICINFO
                            {
                                STYLE_NAME = $"{f.ComExFabstyle.FABCODENavigation.STYLE_NAME}"
                            }
                        }
                    }).ToList()
                }).FirstOrDefaultAsync();

            switch (accExRealizationViewModel.AccExportRealization)
            {
                case { AUDITBY: { } }:
                    {
                        var resultUserName = await _userManager.FindByIdAsync(accExRealizationViewModel.AccExportRealization.AUDITBY);
                        accExRealizationViewModel.AccExportRealization.AUDITBY = resultUserName.UserName;
                        break;
                    }
            }

            return accExRealizationViewModel;
        }

        public async Task<DashboardViewModel> GetRealizatioData()
        {
            try
            {
                var date = Convert.ToDateTime("2018-11-04");
                var dashboardViewModel = new DashboardViewModel
                {
                    RealizationViewModel = new RealizationViewModel()
                    {
                        TotalRealization = await DenimDbContext.ACC_EXPORT_REALIZATION
                                               .Where(c => c.REZDATE.Equals(date))
                                               .Select(d => new ACC_EXPORT_REALIZATION()
                                               {
                                                   PRC_USD = d.PRC_USD

                                               }).SumAsync(c => Convert.ToDouble(c.PRC_USD ?? 0)) ,


                        TotalRealizationY = await DenimDbContext.ACC_EXPORT_REALIZATION
                            .Where(c => c.REZDATE.Equals(date.AddDays(-1)))
                                                .Select(d => new ACC_EXPORT_REALIZATION()
                                                {
                                                   PRC_USD = d.PRC_USD

                                                }).SumAsync(c => Convert.ToDouble(c.PRC_USD ?? 0)) 
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
