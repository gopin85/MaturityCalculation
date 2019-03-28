using MaturityCalculation.Business.MaturityCalculator;


namespace MaturityCalculation.Business.PolicyManagement
{
    /// <summary>
    /// The Policy Factory
    /// </summary>
    public interface IPolicyFactory
    {
        IMaturityCalculator Create(string policyType);
    }    
}
 