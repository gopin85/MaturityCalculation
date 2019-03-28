using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.FileManagement.Implementation
{
    /// <summary>
    /// To generate XML document
    /// </summary>
    public class FileWriter: IFileWriter
    {
        /// <summary>
        /// To create the XML file
        /// </summary>
        /// <param name="maturityDetails">The List of maturity details</param>
        /// <returns>The file stream</returns>
        public byte[] Write(List<MaturityDetail> maturityDetails)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<MaturityDetail>), new XmlRootAttribute("MaturityDetails"));
            byte[] content;

            using (var memorystream = new MemoryStream())
            {
                xmlSerializer.Serialize(memorystream, maturityDetails);
                memorystream.Position = 0;
                content = memorystream.ToArray();
            }

            return content;
        }
    }
}
