using System.ComponentModel.DataAnnotations;

namespace buoi5.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Họ không được để trống.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên không được để trống.")]
        public string LastName { get; set; } = string.Empty;

        public int GradeId { get; set; }

        public Grade? Grade { get; set; }
    }
}