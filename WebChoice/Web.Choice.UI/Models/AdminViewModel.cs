using System;
using System.ComponentModel.DataAnnotations;
using Web.Choice.Entity;

namespace Web.Choice.UI.Models
{
    public class AdminViewModel
    {
        public int AdminId { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống"), Display(Name = "Tên đăg nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống"), Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống"), Display(Name = "Email")]
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "Ngày sinh không được bỏ trống"), Display(Name = "Ngày sinh")]
        public System.DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public int PermissionId { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastSeen { get; set; }
        public string LastSeenUrl { get; set; }
        public Nullable<System.DateTime> TimeStamps { get; set; }
        public Admin Admin { get; set; }
    }
}