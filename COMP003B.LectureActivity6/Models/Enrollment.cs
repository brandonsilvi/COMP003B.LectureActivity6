using COMP003B.LectureActivity6.Models;

namespace COMP003B.LectureActivity6.Models
{

    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        //nav properties
        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }
    }
}