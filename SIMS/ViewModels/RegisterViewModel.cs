using System.ComponentModel.DataAnnotations;

namespace SIMS.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng chọn quyền.")]
        public int RoleId { get; set; }  // Mặc định gán role khi đăng ký

        [StringLength(15, ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }
}
