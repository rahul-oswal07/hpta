using AutoMapper;
using HPTA.Data.Entities;
using HPTA.DTO;

namespace HPTA.Mapping
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<DevCentralTeamsResponse.EmployeeInfo, User>();
        }
    }
}
