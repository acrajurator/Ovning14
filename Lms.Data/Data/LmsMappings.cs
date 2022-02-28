using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Lms.Data.Data
{
    public class LmsMappings : Profile
    {
        public LmsMappings()
        {

            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Course, CourseWithModuleDto>().ReverseMap();
            CreateMap<Module, ModuleDto>().ReverseMap(); 
            CreateMap<JsonPatchDocument<CourseDto>, JsonPatchDocument<Course>>().ReverseMap();
            CreateMap<JsonPatchDocument<ModuleDto>, JsonPatchDocument<Module>>().ReverseMap();
            CreateMap<Operation<CourseDto>, Operation<Course>>();
            CreateMap<Operation<ModuleDto>, Operation<Module>>();
            


        }

    }
}
