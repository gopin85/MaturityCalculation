using System;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.MaturityCalculator.Implementation
{
    public class PolicyAMaturityCalculator : MaturityCalculator
    {
        /// <summary>
        /// The Management fee for PolicyA
        /// </summary>
        protected override double GetManagementFee => 0.03;
        
        /// <summary>
        /// The Discretionary Bonus for PolicyA
        /// </summary>
        /// <param name="policyDetail">The Policy Details</param>
        /// <returns>The discretionary bonus</returns>
        protected override double GetDiscretionaryBonus(PolicyDetail policyDetail)
        {
            var difference = DateTime.Compare(policyDetail.PolicyStartDate.Date, PolicyTakenOutPeriod.Date);
            return difference < 0 ? policyDetail.DiscretionaryBonus : 0;
        }
    }
}
