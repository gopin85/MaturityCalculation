using System;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.MaturityCalculator.Implementation
{
    public class PolicyCMaturityCalculator : MaturityCalculator
    {
        /// <summary>
        /// The management fee for PolicyC
        /// </summary>
        protected override double GetManagementFee => 0.07;

        /// <summary>
        /// The Discretionary Bonus for PolicyC
        /// </summary>
        /// <param name="policyDetail">Policy Details</param>
        /// <returns>The Discretionary Bonus</returns>
        protected override double GetDiscretionaryBonus(PolicyDetail policyDetail)
        {
            var difference = DateTime.Compare(policyDetail.PolicyStartDate.Date, PolicyTakenOutPeriod.Date);
            return difference >= 0 && policyDetail.Membership ? policyDetail.DiscretionaryBonus : 0;
        }
    }
}
