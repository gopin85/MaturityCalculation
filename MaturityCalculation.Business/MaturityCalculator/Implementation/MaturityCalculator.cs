using System;
using System.Globalization;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.MaturityCalculator.Implementation
{
    /// <summary>
    /// The Maturity calculator
    /// </summary>
    public abstract class MaturityCalculator : IMaturityCalculator
    {
        /// <summary>
        /// Policy taken out period
        /// </summary>
        protected virtual DateTime PolicyTakenOutPeriod => DateTime.ParseExact("01/01/1990", "mm/dd/yyyy", CultureInfo.InvariantCulture);

        /// <summary>
        /// To get the management
        /// </summary>
        protected abstract double GetManagementFee { get; }

        /// <summary>
        /// To get the discretionary bonus calculation based on policy
        /// </summary>
        /// <param name="policyDetail"></param>
        /// <returns></returns>
        protected abstract double GetDiscretionaryBonus(PolicyDetail policyDetail);

        /// <summary>
        /// To calculate the maturity
        /// </summary>
        /// <param name="policyDetail">The policy details</param>
        /// <returns>The maturity details</returns>

        public virtual MaturityDetail CalculateMaturity(PolicyDetail policyDetail)
        {
            var maturityValue = ((policyDetail.TotalPremiums - (policyDetail.TotalPremiums * GetManagementFee) + GetDiscretionaryBonus(policyDetail))
                * Convert.ToDouble(1 + policyDetail.UpliftPercentage/100));
            return new MaturityDetail() { PolicyNumber = policyDetail.PolicyNumber, MaturityValue = maturityValue };
        }
    }
}
