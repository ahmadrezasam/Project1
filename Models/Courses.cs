using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudentManagementSystemCore.Models;

namespace StudentManagementSystemCore.Models
{
    public class Courses
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        [Key]
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// SIS ID for the course.
        /// </summary>
        [Display(Name = "SIS ID")]
        public string SisId { get; set; }

        [Required]
        public string Name { get; set; }
            
        public decimal? Fees { get; set; }
        public string Duration { get; set; }
        public string Tool_Name { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public virtual Student Students { get; set; }
    }
}
