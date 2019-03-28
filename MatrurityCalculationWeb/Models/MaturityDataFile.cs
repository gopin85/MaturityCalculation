using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MaturityCalculationWeb.Models
{
    public class MaturityDataFile
    {
        [Required(ErrorMessage = "Please Upload File")]
        [Display(Name = "Upload File")]        
        public HttpPostedFileBase File { get; set; }
    }
}