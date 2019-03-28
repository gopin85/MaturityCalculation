using System;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using MaturityCalculation.Models;


namespace MaturityCalculation.Business.FileManagement.Mapping
{
    public class PolicyDetailsDataMapping : ClassMap<PolicyDetail>
    {
        StringBuilder errorMessages = new StringBuilder();

        /// <summary>
        /// To vlidate the input file and to map the each row values to the model
        /// </summary>
        public PolicyDetailsDataMapping()
        {        

            this.Map(p => p.PolicyNumber).ConvertUsing(
             row =>
             {
                 PolicyRecordsValidation(row);
                 return row.GetField<string>("policy_number");
             });

            this.Map(p => p.PolicyNumber).Name("policy_number");
            this.Map(p => p.TotalPremiums).Name("premiums");           
            this.Map(p => p.Membership).ConvertUsing(row => row.GetField("membership").ToString().ToLower() == "y"? true: false);
            this.Map(p => p.DiscretionaryBonus).Name("discretionary_bonus");
            this.Map(p => p.UpliftPercentage).Name("uplift_percentage");
            this.Map(p => p.PolicyStartDate).ConvertUsing(row => DateTime.ParseExact(row.GetField("policy_start_date").ToString(), "d/m/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));            
        }
       
        /// <summary>
        /// To validate the header details in the file
        /// </summary>
        /// <param name="headerRow">The current row</param>
        private void PolicyRecordsValidation(IReaderRow headerRow)
        {
            ValidatePolicyNumber(headerRow.GetField<string>("policy_number"), headerRow.Context);
            ValidateStartDate(headerRow.GetField<string>("policy_start_date"), headerRow.Context);
            ValidatePremium(headerRow.GetField<string>("premiums"), headerRow.Context);
            ValidateMembership(headerRow.GetField<string>("membership"), headerRow.Context);
            ValidateDiscretion(headerRow.GetField<string>("discretionary_bonus"), headerRow.Context);
            ValidateUploadPercentage(headerRow.GetField<string>("uplift_percentage"), headerRow.Context);

            if (errorMessages.Length > 0) throw new Exception(errorMessages.ToString());
        }
        
        /// <summary>
        /// To validate the formant of policy number
        /// </summary>
        /// <param name="policyNumber">The policy number</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean value</returns>
        private bool ValidatePolicyNumber(string policyNumber, ReadingContext context)
        {
            if (string.IsNullOrEmpty(policyNumber))
            {
                errorMessages.AppendLine($"Policy Number is empty at row # {context.RawRow }.");
                return true;
            }

            if (!Regex.IsMatch(policyNumber, "^[A-C]{1,1}[0-9]{1,6}$"))
                errorMessages.AppendLine($"Policy Number is not in valid format at the row # {context.RawRow }. It should start with A-C and followed by 6 digit numbers.");

            return true;
        }
        
        /// <summary>
        /// To validate start date
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean value</returns>
        private bool ValidateStartDate(string startDate, ReadingContext context)
        {
            if (string.IsNullOrEmpty(startDate))
            {
                errorMessages.AppendLine($"Start date is empty at row # {context.RawRow }.");
                return true;
            }

            DateTime dateTime;
            if (!DateTime.TryParseExact(startDate, "d/m/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out dateTime))
                errorMessages.AppendLine($"Start Date is in Incorrect Format at row # {context.RawRow }. It should be in dd/mm/yyyy.");

            return true;
        }
        
        /// <summary>
        /// To validate total premium
        /// </summary>
        /// <param name="premium">The premium</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean value</returns>
        private bool ValidatePremium(string premium, ReadingContext context)
        {
            if (string.IsNullOrEmpty(premium))
            {
                errorMessages.AppendLine($"Premium is empty at row # {context.RawRow }.");
                return true;
            }

            double premiumAmount;

            if ((!Double.TryParse(premium, out premiumAmount)) || premiumAmount == 0)
                errorMessages.AppendLine($"Premium is invalid value or Zero at row # {context.RawRow }.");

            return true;
        }
       
        /// <summary>
        /// To validate membership
        /// </summary>
        /// <param name="membership">The membership</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean value</returns>
        private bool ValidateMembership(string membership, ReadingContext context)
        {
            if (string.IsNullOrEmpty(membership))
            {
                errorMessages.AppendLine($"Membership is empty at row # {context.RawRow }.");
                return true;
            }

            if (!Regex.IsMatch(membership, "^[Y|N]{1,1}$"))
                errorMessages.AppendLine($"Membership should be Y or N at row # {context.RawRow }.");

            return true;
        }
       
        /// <summary>
        /// To validate discretion
        /// </summary>
        /// <param name="discretion">The discretion bonus</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean values</returns>
        private bool ValidateDiscretion(string discretion, ReadingContext context)
        {
            if (string.IsNullOrEmpty(discretion))
            {
                errorMessages.AppendLine($"Discretion is empty at row # {context.RawRow }.");
                return true;
            }

            double discretionValue;

            if ((!Double.TryParse(discretion, out discretionValue)) || discretionValue == 0)
                errorMessages.AppendLine($"Discretion is invalid value or Zero at row # {context.RawRow }.");

            return true;
        }
        
        /// <summary>
        /// To validate uplift percentage
        /// </summary>
        /// <param name="uplift">The uplift percentage</param>
        /// <param name="context">Current Row</param>
        /// <returns>Boolean value</returns>
        private bool ValidateUploadPercentage(string uplift, ReadingContext context)
        {
            if (string.IsNullOrEmpty(uplift))
            {
                errorMessages.AppendLine($"Uplift percentage is empty at row # {context.RawRow }.");
                return true;
            }

            decimal upliftPercentage;

            if (!Decimal.TryParse(uplift, out upliftPercentage))
                errorMessages.AppendLine($"Uplift percentage is invalid value at row # {context.RawRow }.");

            return true;
        }
    }
}
