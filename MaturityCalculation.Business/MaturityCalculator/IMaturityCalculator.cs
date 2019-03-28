using MaturityCalculation.Models;

namespace MaturityCalculation.Business.MaturityCalculator
{
    /// <summary>
    /// The Maturity Calculator
    /// </summary>
    public interface IMaturityCalculator
    {
        MaturityDetail CalculateMaturity(PolicyDetail policyDetail);
    }
}
