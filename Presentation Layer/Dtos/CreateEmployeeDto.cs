using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Range(22,60, ErrorMessage ="Age Must Be Between 22 And 60")]
        public int Age { get; set; }
        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",ErrorMessage = "Address Must Be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Email Is Not Valid")]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("HiringDate")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Date Of Creation")]
        public DateTime CreateAt { get; set; }

        public int? DepartmentId { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }
    }
}
