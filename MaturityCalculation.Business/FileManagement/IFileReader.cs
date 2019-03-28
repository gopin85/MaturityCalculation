using System.Collections.Generic;
using System.IO;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.FileManagement
{
    /// <summary>
    /// The file reader
    /// </summary>
    public interface IFileReader
    {
        List<PolicyDetail> Read(Stream stream);
    }
}
