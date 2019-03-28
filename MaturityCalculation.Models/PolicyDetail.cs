using System;

namespace MaturityCalculation.Models
{
    /// <summary>
    /// The Policy Details
    /// </summary>
    public class PolicyDetail
    {
        /// <summary>
        /// Get/Set  Policy Number
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Get/Set policy start date
        /// </summary>
        public DateTime PolicyStartDate { get; set; }

        /// <summary>
        /// Get/Set Total Premiums
        /// </summary>
        public double TotalPremiums { get; set; }

        /// <summary>
        /// Get/Set Membership
        /// </summary>
        public bool Membership { get; set; }

        /// <summary>
        /// Get/Set discretionary bonus.
        /// </summary>
        public double DiscretionaryBonus { get; set; }

        /// <summary>
        /// Get/Set uplift percentage
        /// </summary>
        public double UpliftPercentage { get; set; }
    }    
}
