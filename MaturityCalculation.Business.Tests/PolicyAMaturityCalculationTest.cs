using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculation.Models;
using MaturityCalculation.Business.MaturityCalculator.Implementation;

namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Unit test class for PolicyAMaturityCalculator.
    /// </summary>
    [TestClass]
    public class PolicyAMaturityCalculationTest
    {
        PolicyAMaturityCalculator policyACalculator;

        [TestInitialize]
        [Description("Initialising PolicyAMaturityCalculator instance.")]
        public void Test_Initialize()
        {
            policyACalculator = new PolicyAMaturityCalculator();
        }
       
        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is 0.03")]
        public void ManagementFee_Verification_CorrectValue()
        {
            var policyAMaturityCalculatorSpy = new PolicyAMaturityCalculatorSpy();

            var managementFee = policyAMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreEqual(0.03, managementFee);
        }

        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is 0.05 or 0.07")]
        public void ManagementFee_Verification_InCorrectValue()
        {
            var policyAMaturityCalculatorSpy = new PolicyAMaturityCalculatorSpy();

            var managementFee = policyAMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreNotEqual(0.05, managementFee);

            Assert.AreNotEqual(0.07, managementFee);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date on 1-1-1990. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenOn_1_1_1990_VerifyDiscretionaryBonus_Zero()
        {
            var startDate = DateTime.ParseExact("1/1/1990", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "A100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000
            };
            var policyAMaturityCalculatorSpy = new PolicyAMaturityCalculatorSpy();

            var discretionaryBonus = policyAMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date after 1-1-1990. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenAfter_1_1_1990_VerifyDiscretionaryBonus_Zero()
        {
            var startDate = DateTime.ParseExact("1/1/1995", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "A100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000
            };
            var policyAMaturityCalculatorSpy = new PolicyAMaturityCalculatorSpy();

            var discretionaryBonus = policyAMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date before 1-1-1990. To Verify discretionaryBonus amount is 1000.")]
        public void GetDiscretionaryBonus_PolicyTakenBefore_1_1_1990_VerifyDiscretionaryBonus_Thousand()
        {
            var startDate = DateTime.ParseExact("5/12/1986", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "A100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000
            };
            var policyAMaturityCalculatorSpy = new PolicyAMaturityCalculatorSpy();

            var discretionaryBonus = policyAMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(1000, discretionaryBonus);
        }
        

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1985", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "A100002",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1350,
                TotalPremiums = 12500,
                Membership = true,
                UpliftPercentage = 37.5
            };

            var maturityDetail = policyACalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(18528.125, maturityDetail.MaturityValue);
        }

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount not applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusNotApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1990", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "A100002",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1350,
                TotalPremiums = 12500,
                Membership = false,
                UpliftPercentage = 37.5
            };

            var maturityDetail = policyACalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(16671.875, maturityDetail.MaturityValue);
        }       

    }
  
    /// <summary>
    /// Spy class to test method and property in PolicyAMaturityCalculator.
    /// </summary>
    public class PolicyAMaturityCalculatorSpy : PolicyAMaturityCalculator
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


