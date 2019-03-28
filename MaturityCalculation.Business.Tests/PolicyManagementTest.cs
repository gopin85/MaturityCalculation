using System;
using System.IO;
using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculation.Models;
using MaturityCalculation.Business.PolicyManagement;
using MaturityCalculation.Business.FileManagement;
using MaturityCalculation.Business.MaturityCalculator;

namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Test class for Policy Management.
    /// </summary>
    [TestClass]
    public class PolicyManagementTest
    {
        PolicyManagement.Implementation.PolicyManagement policyManagement;

        [TestMethod]
        [Description("To test CalculateMaturity method and mock PolicyFactory and MaturityCalculator for Policy Type A and fack BuildPolicyType,Calculate methods. To verify maturity details as fack setup.")]
        public void CalculateMaturity_WithOnePolicyDetailsTypeA_Verify_MaturityDetails()
        {
            var mockPolicyFactory = new Mock<IPolicyFactory>();
            var mockFileReader = new Mock<IFileReader>();
            var mockFileWriter = new Mock<IFileWriter>();
            var mockMaturityCalculator = new Mock<IMaturityCalculator>();
            policyManagement = new PolicyManagement.Implementation.PolicyManagement(mockPolicyFactory.Object, mockFileReader.Object,mockFileWriter.Object);
            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail { PolicyNumber = "A1000001", TotalPremiums = 1000, Membership = true }
            };
            var maturityDetail = new MaturityDetail { PolicyNumber = "A1000001", MaturityValue = 10000 };

            mockPolicyFactory.Setup(policyFactory => policyFactory.Create("A")).Returns(mockMaturityCalculator.Object);
            mockMaturityCalculator.Setup(maturityCalculator => maturityCalculator.CalculateMaturity(It.IsAny<PolicyDetail>())).Returns(maturityDetail);

            var maturityDetails = policyManagement.GetMaturityDetails(policyDetails);

            Assert.IsNotNull(maturityDetails);
            Assert.AreEqual(1, maturityDetails.Count);
            Assert.AreEqual("A1000001", maturityDetails[0].PolicyNumber);
            Assert.AreEqual(10000, maturityDetails[0].MaturityValue);
        }

        [TestMethod]
        [Description("To test CalculateMaturity method and mock PolicyFactory and MaturityCalculator for Policy Type B and fack BuildPolicyType,Calculate methods. To verify maturity details as fack setup.")]
        public void CalculateMaturity_WithOnePolicyDetailsTypeB_Verify_MaturityDetails()
        {
            var mockPolicyFactory = new Mock<IPolicyFactory>();
            var mockFileReader = new Mock<IFileReader>();
            var mockFileWriter = new Mock<IFileWriter>();
            var mockMaturityCalculator = new Mock<IMaturityCalculator>();
            policyManagement = new PolicyManagement.Implementation.PolicyManagement(mockPolicyFactory.Object, mockFileReader.Object, mockFileWriter.Object);
            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail { PolicyNumber = "B1000001", TotalPremiums = 2000, Membership = false }
            };
            var maturityDetail = new MaturityDetail { PolicyNumber = "B1000001", MaturityValue = 20000 };

            mockPolicyFactory.Setup(policyFactory => policyFactory.Create("B")).Returns(mockMaturityCalculator.Object);
            mockMaturityCalculator.Setup(maturityCalculator => maturityCalculator.CalculateMaturity(It.IsAny<PolicyDetail>())).Returns(maturityDetail);

            var maturityDetails = policyManagement.GetMaturityDetails(policyDetails);

            Assert.IsNotNull(maturityDetails);
            Assert.AreEqual(1, maturityDetails.Count);
            Assert.AreEqual("B1000001", maturityDetails[0].PolicyNumber);
            Assert.AreEqual(20000, maturityDetails[0].MaturityValue);
        }        

        [TestMethod]
        [Description("To test CalculateMaturity method and mock PolicyFactory and MaturityCalculator for Policy Type B and fack Caculate with exception. Expect exception caught.")]
        [ExpectedException(typeof(Exception))]
        public void CalculateMaturity_Calcualte_ThrowsException_Expect_ExceptionCaught()
        {
            var mockPolicyFactory = new Mock<IPolicyFactory>();
            var mockMaturityCalculator = new Mock<IMaturityCalculator>();
            var mockFileReader = new Mock<IFileReader>();
            var mockFileWriter = new Mock<IFileWriter>();
            policyManagement = new PolicyManagement.Implementation.PolicyManagement(mockPolicyFactory.Object, mockFileReader.Object, mockFileWriter.Object);
            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail { PolicyNumber = "B1000001", TotalPremiums = 2000, Membership = false }
            };
            var maturityDetail = new MaturityDetail { PolicyNumber = "B1000001", MaturityValue = 20000 };

            mockPolicyFactory.Setup(policyFactory => policyFactory.Create("B")).Returns(mockMaturityCalculator.Object);
            mockMaturityCalculator.Setup(maturityCalculator => maturityCalculator.CalculateMaturity(It.IsAny<PolicyDetail>())).Throws(new Exception());

            var maturityDetails = policyManagement.GetMaturityDetails(policyDetails);
        }

        [TestMethod]
        [Description("To test Read method and create mock object for Reader and fack Read method. To verify Policy details count one as fack setup.")]
        public void ReadPolicyDetails_VerifyPolicyDetailsCount_One()
        {
            var mockPolicyFactory = new Mock<IPolicyFactory>();
            var mockFileReader = new Mock<IFileReader>();
            var mockFileWriter = new Mock<IFileWriter>();
            var stream = new MemoryStream();
            policyManagement = new PolicyManagement.Implementation.PolicyManagement(mockPolicyFactory.Object, mockFileReader.Object, mockFileWriter.Object);
            
            var policyDetails = new List<PolicyDetail>()
            {
                new PolicyDetail() {DiscretionaryBonus = 10, Membership =true, PolicyNumber ="A1000001", TotalPremiums =1000, UpliftPercentage = 25}
            };

            mockFileReader.Setup(reader => reader.Read(It.IsAny<MemoryStream>())).Returns(policyDetails);

            var policyDetailsResult = policyManagement.GetPolicyDetails(new MemoryStream());

            Assert.AreEqual(1, policyDetailsResult.Count);
        }

        [TestMethod]
        [Description("To test Write method and create mock object for Writer and fack Write method. To verify file content retrieved as fack setup.")]
        public void WritePolicyMaturities_VerifyFileContentResult_NotNull()
        {
            var mockPolicyFactory = new Mock<IPolicyFactory>();
            var mockFileReader = new Mock<IFileReader>();
            var mockFileWriter = new Mock<IFileWriter>();
            var stream = new MemoryStream();
            policyManagement = new PolicyManagement.Implementation.PolicyManagement(mockPolicyFactory.Object, mockFileReader.Object, mockFileWriter.Object);
            var fileContent = new MemoryStream().ToArray();

            var maturityDetails = new List<MaturityDetail>()
            {
                new MaturityDetail() {PolicyNumber ="A1000001", MaturityValue =1000 }
            };

            mockFileWriter.Setup(writer => writer.Write(maturityDetails)).Returns(fileContent);

            var fileContentResult = policyManagement.GetPolicyMaturities(maturityDetails);

            Assert.IsNotNull(fileContentResult);
        }
    }
}

