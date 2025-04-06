using System.ComponentModel.DataAnnotations;

namespace Presentation_Layer.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "New Password Is Required !!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required !!")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Does Not Match The Password !!")]
        public string ConfirmPassword { get; set; }
    }
}
