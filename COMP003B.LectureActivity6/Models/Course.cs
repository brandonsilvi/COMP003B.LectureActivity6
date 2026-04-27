using System.ComponentModel.DataAnnotations;
using COMP003B.LectureActivity6.Models;

namespace COMP003B.LectureActivity6.Models
{
    public class Course
        {
            public int CourseId { get; set; }
            
            [Required]
            public string Title { get; set; }
            
            //Coll navigation property
            public virtual ICollection<Enrollment>? Enrollments { get; set; }
        }

    }