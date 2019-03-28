using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculationWeb.Models;
using MaturityCalculationWeb.Controllers;
using MaturityCalculation.Models;
using MaturityCalculation.Business.PolicyManagement;
using Moq;

namespace MaturityCalculationWeb.Tests.Controllers
{
    /// <summary>
    /// Test class for HomeController.
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        HomeController homeController;
        MaturityDataFile uploadFile;

        [TestInitialize]
        [Description("Test Initialize")]
        public void Test_Intialize()
        {
            uploadFile = new MaturityDataFile();
        }

        [TestMethod]
        [Description("To test Index method get type and verify action result type is ViewResult")]
        public void Index_VerifyActionResultType_ViewResult()
        {
            var policyManagement = new Mock<IPolicyManagement>();         
            homeController = new HomeController(policyManagement.Object);
            var result = homeController.Index();

            Assert.IsNotNull(result is ViewResult);
        }

        [TestMethod]
        [Description("To test FileUpload method when validation error thrown by data annotation.To verify action result type is ViewResult")]
        public void FileUpload_WithValidationError_VerifyActionResultType_ViewResult()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);
            homeController.ModelState.AddModelError("ValidationErrorMessage", "Please upload your File of type:.csv");
            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is ViewResult);
        }

        [TestMethod]
        [Description("To test FileUpload method when no record to Read. To verify action result type is ViewResult and model state count one")]
        public void FileUpload_WhenNoRecordToRead_Verify_ActionResultType_ViewResult_ModelStateCountOne()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);

            var mockHtpPostedFileBase = new Mock<HttpPostedFileBase>();
            uploadFile.File = mockHtpPostedFileBase.Object;

            var policyDetails = new List<PolicyDetail>();
            mockHtpPostedFileBase.Setup(file => file.InputStream).Returns(new MemoryStream());
            policyManagement.Setup(repository => repository.GetPolicyDetails(It.IsAny<Stream>())).Returns(policyDetails);
            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is ViewResult);
            Assert.AreEqual(1, homeController.ModelState.Count);
        }

        [TestMethod]
        [Description("To test FileUpload method when ReadPolicyDetails Throws Exception. To verify action result type is ViewResult and model state count one")]
        public void FileUpload_WhenReadPolicyDetailsThrowsException_Verify_ActionResultType_ViewResult_ModelStateCountOne()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);

            var mockHtpPostedFileBase = new Mock<HttpPostedFileBase>();
            uploadFile.File = mockHtpPostedFileBase.Object;

            var policyDetails = new List<PolicyDetail>();
            mockHtpPostedFileBase.Setup(file => file.InputStream).Returns(new MemoryStream());
            policyManagement.Setup(repository => repository.GetPolicyDetails(It.IsAny<Stream>())).Throws(new Exception());
            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is ViewResult);
            Assert.AreEqual(1, homeController.ModelState.Count);
        }

        [TestMethod]
        [Description("To test FileUpload method when ReadPolicyDetails returns one PolicyDetails but CalculateMaturity Throws Exception. To verify action result type is ViewResult and model state count one")]
        public void FileUpload_WhenCalculateMaturityThrowsException_Verify_ActionResultType_ViewResult_ModelStateCountOne()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);

            var mockHtpPostedFileBase = new Mock<HttpPostedFileBase>();
            uploadFile.File = mockHtpPostedFileBase.Object;

            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail() { PolicyNumber = "A1000001", Membership =true}
            };

            mockHtpPostedFileBase.Setup(file => file.InputStream).Returns(new MemoryStream());
            policyManagement.Setup(repository => repository.GetPolicyDetails(It.IsAny<Stream>())).Returns(policyDetails);
            policyManagement.Setup(repository => repository.GetMaturityDetails(It.IsAny<List<PolicyDetail>>())).Throws(new Exception());
            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is ViewResult);
            Assert.AreEqual(1, homeController.ModelState.Count);
        }

        [TestMethod]
        [Description("To test FileUpload method when ReadPolicyDetails returns one PolicyDetails, CalculateMaturity returns one MaturityDetails" +
                    "  but WritePolicyMaturities Throws Exception." +
                    " To verify action result type is ViewResult and model state count one")]
        public void FileUpload_WhenWritePolicyMaturitiesThrowsException_Verify_ActionResultType_ViewResult_ModelStateCountOne()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);
            var mockHtpPostedFileBase = new Mock<HttpPostedFileBase>();
            uploadFile.File = mockHtpPostedFileBase.Object;
            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail() { PolicyNumber = "A1000001", Membership =true}
            };
            var maturityDetails = new List<MaturityDetail>()
            {
                new MaturityDetail() { PolicyNumber = "A1000001", MaturityValue =1000 }
            };

            mockHtpPostedFileBase.Setup(file => file.InputStream).Returns(new MemoryStream());
            policyManagement.Setup(repository => repository.GetPolicyDetails(It.IsAny<Stream>())).Returns(policyDetails);
            policyManagement.Setup(repository => repository.GetMaturityDetails(It.IsAny<List<PolicyDetail>>())).Returns(maturityDetails);
            policyManagement.Setup(repository => repository.GetPolicyMaturities(It.IsAny<List<MaturityDetail>>())).Throws(new Exception());

            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is ViewResult);
            Assert.AreEqual(1, homeController.ModelState.Count);
        }

        [TestMethod]
        [Description("To test FileUpload method with all valid datas and when non exception thrown " +
                    "To verify action result type is FileResult and model state count zero")]
        public void FileUpload_WithValidDats_WithNoExceptionThrowsException_Verify_ActionResultType_FileResult_ModelStateCountZero()
        {
            var policyManagement = new Mock<IPolicyManagement>();
            homeController = new HomeController(policyManagement.Object);
            var mockHtpPostedFileBase = new Mock<HttpPostedFileBase>();
            uploadFile.File = mockHtpPostedFileBase.Object;

            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail() { PolicyNumber = "A1000001", Membership =true}
            };

            var maturityDetails = new List<MaturityDetail>()
            {
                new MaturityDetail() { PolicyNumber = "A1000001", MaturityValue =1000}
            };

            var xmlContent = new MemoryStream().ToArray();

            mockHtpPostedFileBase.Setup(file => file.InputStream).Returns(new MemoryStream());
            policyManagement.Setup(repository => repository.GetPolicyDetails(It.IsAny<Stream>())).Returns(policyDetails);
            policyManagement.Setup(repository => repository.GetMaturityDetails(It.IsAny<List<PolicyDetail>>())).Returns(maturityDetails);
            policyManagement.Setup(repository => repository.GetPolicyMaturities(It.IsAny<List<MaturityDetail>>())).Returns(xmlContent);

            var result = homeController.FileUpload(uploadFile);

            Assert.IsNotNull(result is FileContentResult);
            Assert.AreEqual(0, homeController.ModelState.Count);
        }
    }
}

