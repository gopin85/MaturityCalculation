using System.Collections.Generic;
using MaturityCalculation.Models;

namespace MaturityCalculation.Business.FileManagement
{
    /// <summary>
    /// The file writer
    /// </summary>
    public interface IFileWriter
    {
        byte[] Write(List<MaturityDetail> maturityDetails);
    }
}
