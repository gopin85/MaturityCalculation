using System.IO;
using System.Collections.Generic;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.PolicyManagement
{
    /// <summary>
    /// The Policy Management
    /// </summary>
    public interface IPolicyManagement
    {
        List<MaturityDetail> GetMaturityDetails(List<PolicyDetail> policyDetails);

        List<PolicyDetail> GetPolicyDetails(Stream stream);

        byte[] GetPolicyMaturities(List<MaturityDetail> maturityDetails);
    }
    
}
