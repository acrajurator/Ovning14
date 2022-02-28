using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        public static async Task InitAsync(LmsApiContext db)
        {
            if (await db.Course.AnyAsync()) return;
          
            var modules = GetModules();
            await db.AddRangeAsync(modules);
            var courses = GetCourses();
            await db.AddRangeAsync(courses);


            await db.SaveChangesAsync();

        }

        private static IEnumerable<Module> GetModules()
        {
            var list = new List<Module>();

            list.Add(new Module { Title = "Hej", CourseId = 1, StartDate = DateTime.Now });
            list.Add(new Module { Title = "Då", CourseId = 2, StartDate = DateTime.Now });
            list.Add(new Module { Title = "Nej", CourseId = 4, StartDate = DateTime.Now });
            list.Add(new Module { Title = "Ja", CourseId = 3, StartDate = DateTime.Now });
            list.Add(new Module { Title = "Goddag", CourseId = 3, StartDate = DateTime.Now });
            list.Add(new Module { Title = "Vandra", CourseId = 5, StartDate = DateTime.Now });
        return list;
        }

        private static IEnumerable<Course> GetCourses()
        {
            var list = new List<Course>();

            list.Add(new Course { Title = "Matte", StartDate = DateTime.Now });
            list.Add(new Course { Title = "Engelska", StartDate = DateTime.Now });
            list.Add(new Course { Title = "Svenska", StartDate = DateTime.Now });
            list.Add(new Course { Title = "Biologi", StartDate = DateTime.Now });
            list.Add(new Course { Title = "Kemi", StartDate = DateTime.Now });
            list.Add(new Course { Title = "Idrott", StartDate = DateTime.Now });

            
            return list;
        }
    }
}
