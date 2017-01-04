using DataAccess.Scaffold.Attributes;
using Needletail.DataAccess.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoApp.Models.DBModels
{
    public class Course
    {

        [Required]
        [TableKey(CanInsertKey = true)]
        public Guid Id { get; set; }

        [Required]
        [MaxLen(50)]
        public string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        public Guid DepartmentID { get; set; }
    }
}
