using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaturityCalculation.Business.FileManagement.Implementation;

namespace MaturityCalculation.Business.Tests
{
    /// <summary>
    /// Unit Test class for FileReader.
    /// </summary>
    [TestClass]
    public class FileReaderTest
    {
        FileReader fileReader;
        StringBuilder fileContent;

        [TestInitialize]
        [Description("Initialising FileReader and fileContent instances.")]
        public void Test_Initialize()
        {
            fileReader = new FileReader();
            fileContent = new StringBuilder();
        }        

        [TestMethod]
        [Description("To test Read method with valid header and Three data row. To verify policy details collection count nine.")]
        public void Read__withValidHeader_WithValidData_ThreeDataRow_VerifyPolicyDetailsCount_Countnine()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("A100001,1/1/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("A100002,1/1/1990,17000,Y,3000,47").AppendLine();
            fileContent.Append("A100003,1/1/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("B100001,1/1/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("B100002,1/1/1990,17000,Y,3000,47").AppendLine();
            fileContent.Append("B100003,1/1/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("C100001,1/1/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("C100002,1/1/1990,17000,Y,3000,47").AppendLine();
            fileContent.Append("C100003,1/1/1990,17000,Y,3000,46").AppendLine();
            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNotNull(policyDetails);

            Assert.AreEqual(9, policyDetails.Count);
        }

        [TestMethod]
        [Description("To test Read method with valid header and one data row. To verify policy details collection count one.")]
        public void Read_withValidHeader_WithValidFileContent_OneRow_VerifyPolicyDetailsCount_CountOne()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("A100001,1/6/1986,10000,Y,1000,40").AppendLine();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNotNull(policyDetails);

            Assert.AreEqual(1, policyDetails.Count);
        }

        [TestMethod]
        [Description("To test Read method with invalid header. Expect validation excepttion caught and incorrect header error message thrown.")]
        [ExpectedException(typeof(CsvHelper.CsvHelperException), "Incorrect headers. Please correct the headers and upload it again.")]
        public void Read_WithInValidHeader_Expect_Exception()
        {
            fileContent.Append("policynumber,policystart_date,premiums,membership,discretionarybonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("A100001,1/6/1986,10000,Y,1000,40").AppendLine();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNull(policyDetails);
        }

        [TestMethod]
        [Description("To test Read method with valid header but with invalid PolicyNumber(D100003) at second row. Expect validation excepttion caught and appropriate message thrown.")]
        [ExpectedException(typeof(CsvHelper.ReaderException), "Incorrect headers. Please correct the headers and upload it again.")]
        public void Read_WithValidHeader_InValidPolicyNumberAtSecondRow_Expect_Exception()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("D100003,1/1/1990,17000,Y,3000,46").AppendLine();            

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNull(policyDetails);
        }

        [TestMethod]
        [Description("To test Read method with valid header but with invalid premium(NA) at second row. Expect validation excepttion caught and appropriate message thrown.")]
        [ExpectedException(typeof(CsvHelper.ReaderException), "Policy Number is in Incorrect Format at row # 2. It should start with A-C and followed by 6 digit numbers.Premium is invalid value or Zero at row # 2.")]
        public void Read_WithValidHeader_InValidPremiumAtSecondRow_Expect_Exception()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("D100003,1/1/1990,NA,Y,3000,46").AppendLine();
            fileContent.Append("C100002,1/1/1990,17000,Y,3000,47").AppendLine();
            fileContent.Append("C100001,1/1/1990,17000,Y,3000,46").AppendLine();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNull(policyDetails);
        }

        [TestMethod]
        [Description("To test Read method with valid header but invalid start date(NA) at third row. To verify validation excepttion caught and policyDetails count still zero.")]
        [ExpectedException(typeof(CsvHelper.ReaderException), ": Policy Number is in Incorrect Format at row # 2. It should start with A-C and followed by 6 digit numbers")]
        public void Read_WithValidHeader_InValidStartDateAtThirdRow_VerifyExceptionCaught_PolicyDetailsCountZero()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("D100003,01/01/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("C100002,NA,17000,Y,3000,47").AppendLine();
            fileContent.Append("C100001,01/01/1990,17000,Y,3000,46").AppendLine();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNull(policyDetails);
        }

        [TestMethod]
        [Description("To test Read method with valid header but without premium and discretionary bonus at Fourth row. To verify validation excepttion caught and policyDetails count still zero.")]
        [ExpectedException(typeof(CsvHelper.ReaderException), "Policy Number is in Incorrect Format at row # 2. It should start with A-C and followed by 6 digit numbers.")]
        public void Read_WithValidHeader_WithNoPremiumAndDiscretionaryBonus_VerifyExceptionCaught_PolicyDetailsCountZero()
        {
            fileContent.Append("policy_number,policy_start_date,premiums,membership,discretionary_bonus,uplift_percentage");
            fileContent.AppendLine();
            fileContent.Append("D100003,01/01/1990,17000,Y,3000,46").AppendLine();
            fileContent.Append("C100002,01/01/1990,17000,Y,3000,47").AppendLine();
            fileContent.Append("C100001,01/01/1990,,Y,,46").AppendLine();

            var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent.ToString()));

            var policyDetails = fileReader.Read(contentStream);

            Assert.IsNull(policyDetails);
        }
    }
}
