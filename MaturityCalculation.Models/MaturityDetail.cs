using System;
using System.Collections.Generic;
using System.Text;

namespace MaturityCalculation.Models
{
    /// <summary>
    /// The Maturity Details
    /// </summary>
    public class MaturityDetail
    {
        /// <summary>
        /// Get/set the Policy Number.
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Get/Set the Maturity Value.
        /// </summary>
        public double MaturityValue { get; set; }
    }
}
