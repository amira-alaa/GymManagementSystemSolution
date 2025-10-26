using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Name Is Required")]
        [StringLength(50 ,MinimumLength = 2 ,ErrorMessage ="Name Must be between 2 and 50 Character")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Must be only letters and spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [StringLength(100,MinimumLength = 5 ,ErrorMessage = "Email Must be between 5 and 100 Character")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Is Required")]  
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone Number Must be Egyptian")]
        public string Phone { get; set; } = null!;
        [Required(ErrorMessage = "BirthDate Is Required")]
        [DataType(DataType.Date)]
        public DateOnly BirthDate { get; set; }
        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

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
        [Required(ErrorMessage = "HealthRecord Is Required")]
        public HealthRecordViewModel HealthRecord { get; set; } = null!;

    }
}
