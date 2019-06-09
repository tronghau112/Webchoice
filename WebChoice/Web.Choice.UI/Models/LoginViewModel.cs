using System.ComponentModel.DataAnnotations;

namespace Web.Choice.UI.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống"), Display(Name = "Tên đăg nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống"), Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}