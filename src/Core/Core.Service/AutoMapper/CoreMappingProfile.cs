using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            //CreateMap<SystemConfigurationModel, SystemConfigurationEntity>();
            //CreateMap<SystemConfigurationEntity, SystemConfigurationModel>();
            //CreateMap<CommonTopicDto, CommonTopic>();
            //CreateMap<CommonTopic, CommonTopicDto>();

            //CreateMap<StudentNoteDto, StudentNote>()
            //    .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => Convert(src.StudentId)))
            //    .ForMember(dest => dest.Student, opt => opt.Ignore())
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Convert(src.StaffId)))
            //    .ForMember(dest => dest.User, opt => opt.Ignore());
            //CreateMap<StudentNote, StudentNoteDto>()
            //    .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student != null ? src.Student.Id.ToString() : null))
            //    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.DisplayName.ToString() : null))
            //    .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => src.User != null ? src.User.Id.ToString() : null))
            //    .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.User != null ? src.User.DisplayName.ToString() : null));
        }

        private static Guid? Convert(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return Guid.Parse(id);
        }
    }
}