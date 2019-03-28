using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculation.Models;
using MaturityCalculation.Business.MaturityCalculator.Implementation;


namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Unit test class for PolicyCMaturityCalculatorTest.
    /// </summary>
    [TestClass]
    public class PolicyCMaturityCalculationTest
    {
        PolicyCMaturityCalculator policyCCalculator;

        [TestInitialize]
        [Description("Initialising PolicyCMaturityCalculator instance.")]
        public void Test_Initialize()
        {
            policyCCalculator = new PolicyCMaturityCalculator();
        }

        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is 0.07")]
        public void ManagementFee_Verification_CorrectValue()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var managementFee = policyCMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreEqual(0.07, managementFee);
        }

        [TestMethod]
        [Description("To test ManagementFee proerty. To verify value is not 0.03 0r 0.05")]
        public void ManagementFee_Verification_InCorrectValue()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var managementFee = policyCMaturityCalculatorSpy.GetSpyManagementFee;

            Assert.AreNotEqual(0.03, managementFee);

            Assert.AreNotEqual(0.05, managementFee);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date is before 1-1-1990. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenBefore_1_1_1990_VerifyDiscretionaryBonus_Zero()
        {
            var startDate = DateTime.ParseExact("1/6/1986", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100002",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 2000
            };
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var discretionaryBonus = policyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date on 1-1-1990 and membership right false. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenOn_1_1_1990_MembershipRightsFalse_VerifyDiscretionaryBonus_Zero()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var startDate = DateTime.ParseExact("1/1/1990", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000,
                Membership = false
            };

            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var discretionaryBonus = policyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetrieveDiscretionaryBonus method when the policy start date on 1-1-1990 and membership right true. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenOn_1_1_1990_MembershipRightsTrue_VerifyDiscretionaryBonus_Thousand()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var startDate = DateTime.ParseExact("1/1/1990", "m/d/yyyy", CultureInfo.InvariantCulture);

            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000,
                Membership = true
            };
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var discretionaryBonus = policyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(1000, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GeteDiscretionaryBonus method when the policy start date after 1-1-1990 and membership right false. To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenAfter_1_1_1990_MembershipRightFalse_VerifyDiscretionaryBonus_Zero()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var startDate = DateTime.ParseExact("1/1/1991", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000,
                Membership = false
            };
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var discretionaryBonus = policyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(0, discretionaryBonus);
        }

        [TestMethod]
        [Description("To test GetDiscretionaryBonus method when the policy start date after 1-1-1990 and membership right true To verify discretionaryBonus amount is Zero.")]
        public void GetDiscretionaryBonus_PolicyTakenAfter_1_1_1990_MembershipRightTrue_VerifyDiscretionaryBonus_TwoThousand()
        {
            PolicyCMaturityCalculator PolicyCMaturityCalculator = new PolicyCMaturityCalculator();
            var startDate = DateTime.ParseExact("1/1/1991", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 2000,
                Membership = true
            };
            var policyCMaturityCalculatorSpy = new PolicyCMaturityCalculatorSpy();

            var discretionaryBonus = policyCMaturityCalculatorSpy.GetSpyDiscretionaryBonus(policyDetail);

            Assert.AreEqual(2000, discretionaryBonus);
        }

        

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1990", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100003",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 3000,
                TotalPremiums = 17000,
                Membership = true,
                UpliftPercentage = 46
            };

            var maturityDetail = policyCCalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(27462.6, maturityDetail.MaturityValue);
        }

        [TestMethod]
        [Description("To test calculate method when DiscretionaryBonus amount not applicable. To verify maturity value.")]
        public void Calculate_WhenDiscretionaryBonusNotApplicable_VerifyMaturityValue()
        {
            var startDate = DateTime.ParseExact("1/1/1992", "m/d/yyyy", CultureInfo.InvariantCulture);
            var policyDetail = new PolicyDetail()
            {
                PolicyNumber = "C100001",
                PolicyStartDate = startDate,
                DiscretionaryBonus = 1000,
                TotalPremiums = 13000,
                Membership = false,
                UpliftPercentage = 42
            };

            var maturityDetail = policyCCalculator.CalculateMaturity(policyDetail);

            Assert.IsNotNull(maturityDetail);

            Assert.AreEqual(17167.8, maturityDetail.MaturityValue);
        }      
    }    

    /// <summary>
    /// Spy class to test method and property in PolicyCMaturityCalculator.
    /// </summary>
    public class PolicyCMaturityCalculatorSpy : PolicyCMaturityCalculator
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
