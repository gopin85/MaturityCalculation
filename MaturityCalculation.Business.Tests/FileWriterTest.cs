using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculation.Models;
using MaturityCalculation.Business.FileManagement.Implementation;


namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Unit Test class for FileWriter.
    /// </summary>
    [TestClass]
    public class FileWriterTest
    {
        FileWriter fileWriter;

        [TestInitialize]
        [Description("Initialising XmlWriter instnace.")]
        public void Test_Initialize()
        {
            fileWriter = new FileWriter();
        }

        [TestMethod]
        [Description("To test the file write method with the maturity details. To verify file content is not null and returns length greater than zero.")]
        public void Write_WithMaturityDetails_VerifyByteArrayLength_GreaterThanZero()
        {
            var maturityDetails = new List<MaturityDetail>()
            {
                new MaturityDetail() {PolicyNumber ="A100001", MaturityValue = 1000 },
                new MaturityDetail() {PolicyNumber ="B100002", MaturityValue = 2000 },
                new MaturityDetail() {PolicyNumber ="C100003", MaturityValue = 3000 }
            };

            var fileContent = fileWriter.Write(maturityDetails);

            Assert.IsNotNull(fileContent);

            Assert.IsTrue(fileContent.Length > 0);
        }

        [TestMethod]
        [Description("To test the file write method with out maturity details. To verify file content is not null and returns length greater than zero.")]
        public void Write_WithOutMaturityDetails_VerifyByteArrayLength_GreaterThanZero()
        {
            var maturityDetails = new List<MaturityDetail>();

            var fileContent = fileWriter.Write(maturityDetails);

            Assert.IsNotNull(fileContent);

            Assert.IsTrue(fileContent.Length > 0);
        }
    }
}
