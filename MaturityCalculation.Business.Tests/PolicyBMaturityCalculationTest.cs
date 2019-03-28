using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using MaturityCalculation.Models;
using MaturityCalculation.Business.MaturityCalculator.Implementation;

namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Unit test class for PolicyBMaturityCalculatorTest.
    /// </summary>
    [TestClass]
    public class PolicyBMaturityCalculationTest
    {
        PolicyBMaturityCalculator policyBCalculator;

        [TestInitialize]
        [Description("Initialising PolicyBMaturityCalculator instance.")]
        public void Test_Initialize()
        {
            policyBCalculator = new PolicyBMaturityCalculator();
        }

        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is 0.05")]
        public void ManagementFee_Verification_CorrectValue()
        {
            var policybMaturityCalculatorSpy = new PolicyBMaturityCalculatorSpy();

            var managementFee = policybMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreEqual(0.05, managementFee);
        }

        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is not 0.03 or 0.07")]
        public void ManagementFee_Verification_InCorrectValue()
        {
            var policybMaturityCalculatorSpy = new PolicyBMaturityCalculatorSpy();

            var managementFee = policybMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreNotEqual(0.03, managementFee);

            Assert.AreNotEqual(0.07, managementFee);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when membership false. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_MembershipFalse_VerifyDiscretionaryBonus_Zero()
        {
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "B100002",
                Membership = false,
                DiscretionaryBonus = 2000
            };
            var PolicyCMaturityCalculatorSpy = new PolicyBMaturityCalculatorSpy();

            var discretionaryBonus = PolicyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when membership true. To verify discretionaryBonus amount is Thousand.")]
        public void GetDiscretionaryBonus_MembershipTrue_VerifyDiscretionaryBonus_Thousand()
        {
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "B100003",
                Membership = true,
                DiscretionaryBonus = 1000
            };
            var PolicyCMaturityCalculatorSpy = new PolicyBMaturityCalculatorSpy();

            var discretionaryBonus = PolicyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(1000, discretionaryBonus);
        }

       

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1995", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "B100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 2000,
                TotalPremiums = 12000,
                Membership = true,
                UpliftPercentage = 41
            };

            var maturityDetail = policyBCalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(18894, maturityDetail.MaturityValue);
        }

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount not applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusNotApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1970", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "B100002",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 3000,
                TotalPremiums = 18000,
                Membership = false,
                UpliftPercentage = 43
            };

            var maturityDetail = policyBCalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(24453, maturityDetail.MaturityValue);
        }        
    }   

    /// <summary>
    /// Spy class to test method and property in PolicyBMaturityCalculator.
    /// </summary>
    public class PolicyBMaturityCalculatorSpy : PolicyBMaturityCalculator
    {
        public double GetSpyDiscretionaryBonus(PolicyDetail policyDetail)
        {
            return GetDiscretionaryBonus(policyDetail);
        }

        protected override double GetDiscretionaryBonus(PolicyDetail policyDetail)
        {
            return base.GetDiscretionaryBonus(policyDetail);
        }

        public double GetSpyManagementFee => GetManagementFee;

        protected override double GetManagementFee => base.GetManagementFee;
    }
}
               