using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModel
{
    public class UpdatePlanViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description Is Required.")]
        [StringLength(50,MinimumLength = 5 ,ErrorMessage = "Description Must be between 5 and 50 Characters.")]
        public string Description { get; set; } = null!;
        [Required(ErrorMessage = "DurationDays Is Required.")]
        [Range(1, 365, ErrorMessage = "DurationDays Must be between 1 and 365.")]
        public int DurationDays { get; set; }
        [Required(ErrorMessage = "Price Is Required.")]
        [Range(0.1, 10000, ErrorMessage = "Price Must be between 0.1 and 10000.")]
        public decimal Price { get; set; }
    }
}
