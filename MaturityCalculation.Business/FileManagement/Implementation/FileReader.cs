using System.IO;
using System.Linq;
using System.Collections.Generic;
using CsvHelper;
using MaturityCalculation.Models;
using MaturityCalculation.Business.FileManagement.Mapping;


namespace MaturityCalculation.Business.FileManagement.Implementation
{
    /// <summary>
    /// Class to read the file.
    /// </summary>
    public class FileReader : IFileReader
    {
        /// <summary>
        /// To read the file stream and get the policy details
        /// </summary>
        /// <param name="stream">The csv file stream</param>
        /// <returns>The List of policy details</returns>
        public List<PolicyDetail> Read(Stream stream)
        {
            try
            {
                using (var reader = new StreamReader(stream))
                {
                    var csvHelperReader = new CsvReader(reader);
                    csvHelperReader.Configuration.Delimiter = ",";
                    csvHelperReader.Configuration.HasHeaderRecord = true;
                    csvHelperReader.Read();
                    csvHelperReader.ReadHeader();
                    csvHelperReader.Configuration.RegisterClassMap<PolicyDetailsDataMapping>();
                    csvHelperReader.ValidateHeader<PolicyDetail>();
                    return csvHelperReader.GetRecords<PolicyDetail>().ToList();
                }
            }
            catch (ValidationException ex)
            {
                if (ex.ReadingContext.RawRow == 1)
                    throw new CsvHelperException(ex.ReadingContext, "Incorrect headers. Please correct the headers and upload it again.");
                else
                    throw ex;
            }
        }
    }

    
}
