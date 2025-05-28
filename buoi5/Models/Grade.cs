using System.ComponentModel.DataAnnotations;

namespace buoi5.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống.")]
        [StringLength(50, ErrorMessage = "Tên lớp không được vượt quá 50 ký tự.")]
        public string GradeName { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new List<Student>();
    }
}