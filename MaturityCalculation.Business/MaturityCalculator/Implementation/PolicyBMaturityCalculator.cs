using System;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.MaturityCalculator.Implementation
{
    public class PolicyBMaturityCalculator : MaturityCalculator
    {
        /// <summary>
        /// The management fee for PolicyB
        /// </summary>
        protected override double GetManagementFee => 0.05;

        /// <summary>
        /// The Discretionary Bonus for PolicyB
        /// </summary>
        /// <param name="policyDetail">The policy details</param>
        /// <returns>The Discretionary Bonus</returns>
        protected override double GetDiscretionaryBonus(PolicyDetail policyDetail)
        {            
            return policyDetail.Membership ? policyDetail.DiscretionaryBonus : 0;
        }
    }
}