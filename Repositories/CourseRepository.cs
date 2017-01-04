using demoApp.Models.DBModels;
using Needletail.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoApp.Repositories
{
    public class CourseRepository
    {
        public DBTableDataSourceBase<Course, Guid> DatabaseTable { get; private set; }

        public CourseRepository(string connectionString)
        {
            this.DatabaseTable = new DBTableDataSourceBase<Models.DBModels.Course, Guid>(connectionString, "Course");
        }
    }
}
