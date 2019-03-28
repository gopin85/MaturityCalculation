using System;
using System.IO;
using System.Collections.Generic;
using MaturityCalculation.Models;
using MaturityCalculation.Business.FileManagement;

namespace MaturityCalculation.Business.PolicyManagement.Implementation
{
    /// <summary>
    /// The Policy Management
    /// </summary>
    public class PolicyManagement : IPolicyManagement
    {       
        private readonly IPolicyFactory policyFactory;
        private readonly IFileReader fileReader;
        private readonly IFileWriter fileWriter;


        public PolicyManagement(IPolicyFactory _policyFactory,
            IFileReader _fileReader,
            IFileWriter _fileWriter)
        {
            policyFactory = _policyFactory;
            fileReader = _fileReader;
            fileWriter = _fileWriter;
        }

        /// <summary>
        /// To get the Maturity Details
        /// </summary>
        /// <param name="policyDetails">The List of policy details</param>
        /// <returns>The List of maturity details</returns>
        public List<MaturityDetail> GetMaturityDetails(List<PolicyDetail> policyDetails)
        {

            var maturityDetails = new List<MaturityDetail>();
            try
            {
                policyDetails.ForEach(policyDetail => {
                    var maturityCalculator = policyFactory.Create(policyDetail.PolicyNumber.Substring(0,1));
                    var maturityDetail = maturityCalculator.CalculateMaturity(policyDetail);
                    maturityDetails.Add(maturityDetail);
                });

                return maturityDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the policy details
        /// </summary>
        /// <param name="stream">The file stream</param>
        /// <returns>The list of policy details</returns>
        public List<PolicyDetail> GetPolicyDetails(Stream stream)
        {
            return fileReader.Read(stream);
        }

        /// <summary>
        /// To get the policy maturity
        /// </summary>
        /// <param name="maturityDetails">The list of maturity details</param>
        /// <returns>The XML file with maturity details</returns>
        public byte[] GetPolicyMaturities(List<MaturityDetail> maturityDetails)
        {
            return fileWriter.Write(maturityDetails);
        }
    }
}
