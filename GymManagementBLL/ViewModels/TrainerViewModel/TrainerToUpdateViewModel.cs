using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities.Enums;

namespace GymManagementBLL.ViewModels.TrainerViewModel
{
    internal class TrainerToUpdateViewModel
    {
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email Is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be between 5 and 100 Character")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must be Egyptian")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, 1000, ErrorMessage = "Building Number Must be between 1 and 1000")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be between 2 and 30 Character")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be between 2 and 30 Character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Must be only letters and spaces")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Specialties Is Required")]
        public Specialties specialties { get; set; }

    }
}
