using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class CourseWithModuleDto
    {

            public string Title { get; set; }
            public DateTime StartDate { get; set; }

            public DateTime EndDate => StartDate.AddMonths(3).AddDays(-3);
            public ICollection<Module> Modules { get; set; }
        


    }


}
