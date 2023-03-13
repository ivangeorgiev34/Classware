using Classware.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classware.Infrastructure.Models
{
    public class Class
    {
        public Class()
        {
            Students = new List<Student>();
        }
        /// <summary>
        /// Class' primary key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Class' name 
        /// </summary>
        [Required]
        [StringLength(InfrastructureConstants.Class.CLASS_NAME_MAX_LENGTH, MinimumLength = InfrastructureConstants.Class.CLASS_NAME_MAX_LENGTH)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Is class active
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Class' students
        /// </summary>
        public ICollection<Student> Students { get; set; }
    }
}
