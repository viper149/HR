using System;
using System.Linq;
using System.Threading.Tasks;
using DenimERP.Security;
using DenimERP.ServiceInterfaces.Marketing;
using DenimERP.ViewModels.Marketing;
using DenimERP.ViewModels.StaticData;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace DenimERP.Controllers
{
    public class MktSwatchCardController : Controller
    {
        private readonly IMKT_SWATCH_CARD _mktSwatchCard;
        private readonly IDataProtector _protector;

        public MktSwatchCardController(IMKT_SWATCH_CARD mktSwatchCard,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _mktSwatchCard = mktSwatchCard;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMktSwatchCard(string swCdId)
        {
            try
            {
                var mktSwatchCard = await _mktSwatchCard.FindByIdAsync(int.Parse(_protector.Unprotect(swCdId)));

                if (mktSwatchCard != null)
                {
                    if (await _mktSwatchCard.Delete(mktSwatchCard))
                    {
                        TempData["message"] = "Successfully Deleted Swatch Card.";
                        TempData["type"] = "success";
                    }
                    else
                    {
                        TempData["message"] = "Failed To Delete Swatch Card.";
                        TempData["type"] = "error";
                    }
                }
                else
                {
                    TempData["message"] = "Unable To Find Swatch Card.";
                    TempData["type"] = "error";
                }

                return RedirectToAction("GetMktSwatchCard", $"MktSwatchCard");
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DetailsMktSwatchCard(string swCdId)
        {
            try
            {
                return View(await _mktSwatchCard.FindBySwCdIdAsync(int.Parse(_protector.Unprotect(swCdId))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMktSwatchCard(CreateMktSwatchCardViewModel createMktSwatchCardViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    createMktSwatchCardViewModel.MktSwatchCard.SWCDID = int.Parse(_protector.Unprotect(createMktSwatchCardViewModel.MktSwatchCard.EncryptedId));

                    if (await _mktSwatchCard.Update(createMktSwatchCardViewModel.MktSwatchCard))
                    {
                        TempData["message"] = "Successfully Updated Swatch Card.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetMktSwatchCard", $"MktSwatchCard");
                    }

                    TempData["message"] = "Failed To Update Swatch Card.";
                    TempData["type"] = "error";
                    return View(await _mktSwatchCard.GetInitObjects(createMktSwatchCardViewModel));
                }

                TempData["message"] = "Failed To Update Swatch Card.";
                TempData["type"] = "error";
                return View(await _mktSwatchCard.GetInitObjects(createMktSwatchCardViewModel));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditMktSwatchCard(string swCdId)
        {
            try
            {
                ViewBag.OrderTypes = StaticData.GetOrderTypes();
                return View(await _mktSwatchCard.GetInitObjects(await _mktSwatchCard.FindBySwCdIdAsync(int.Parse(_protector.Unprotect(swCdId)))));
            }
            catch (Exception)
            {
                return View($"Error");
            }
        }

        [HttpPost]
        public async Task<JsonResult> GetTableData()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault()?.ToUpper();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault()?.ToUpper();
                int.TryParse(length, out var pageSize);
                int.TryParse(start, out var skip);

                var forDataTableByAsync = await _mktSwatchCard.GetForDataTableByAsync(sortColumn, sortColumnDirection, searchValue, draw, skip, pageSize);

                return Json(new
                {
                    draw = forDataTableByAsync.Draw,
                    recordsFiltered = forDataTableByAsync.RecordsTotal,
                    recordsTotal = forDataTableByAsync.RecordsTotal,
                    data = forDataTableByAsync.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetMktSwatchCard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMktSwatchCard(CreateMktSwatchCardViewModel createMktSwatchCard)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _mktSwatchCard.InsertByAsync(createMktSwatchCard.MktSwatchCard))
                    {
                        TempData["message"] = "Successfully Added Swatch Card.";
                        TempData["type"] = "success";
                        return RedirectToAction("GetMktSwatchCard", $"MktSwatchCard");
                    }

                    TempData["message"] = "Failed To Add Swatch Card.";
                    TempData["type"] = "success";
                    return View(await _mktSwatchCard.GetInitObjects(createMktSwatchCard));
                }

                TempData["message"] = "Failed To Add Swatch Card.";
                TempData["type"] = "error";
                return View(await _mktSwatchCard.GetInitObjects(createMktSwatchCard));
            }
            catch (Exception)
            {
                return View($"Error");
            }

        }

        [HttpGet]
        public async Task<IActionResult> CreateMktSwatchCard()
        {
            try
            {
                ViewBag.OrderTypes = StaticData.GetOrderTypes();
                return View(await _mktSwatchCard.GetInitObjects(new CreateMktSwatchCardViewModel()));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View($"Error");
            }
        }
    }
}