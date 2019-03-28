using System;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using MaturityCalculationWeb.Models;
using MaturityCalculation.Business.PolicyManagement;

namespace MaturityCalculationWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPolicyManagement policyManagement;

        
        public HomeController(IPolicyManagement _policyManagement)
        {
            policyManagement = _policyManagement;
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(MaturityDataFile maturityDataFile)
        {
            try
            {                
                if (ModelState.IsValid)
                {
                    var policyDetails = policyManagement.GetPolicyDetails(maturityDataFile.File.InputStream);

                    if (!policyDetails.Any())
                    {
                        ModelState.AddModelError("Data Validation", "No policies found.");
                        return View("Index");
                    }

                    var maturityDetails = policyManagement.GetMaturityDetails(policyDetails);

                    var fileContent = policyManagement.GetPolicyMaturities(maturityDetails);

                    ViewBag.Message = "Input File has been uploaded successfully and Output file has been downloaded successfully";

                    ModelState.Clear();

                    return File(fileContent, MediaTypeNames.Text.Xml, "PolicyMaturity.xml");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error Message", ex.Message);
            }

            return View("Index");
        }
    }
}