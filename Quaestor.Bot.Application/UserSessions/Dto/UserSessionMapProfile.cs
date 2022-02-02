using AutoMapper;

namespace Quaestor.Bot.UserSessions.Dto
{
    public class UserSessionMapProfile : Profile
    {
        public UserSessionMapProfile()
        {
            CreateMap<CreateUserSessionInput, UserSession>();
            CreateMap<UserSessionDto, UserSession>();
            CreateMap<UserSessionDto, UserSession>()
          .ForMember(x => x.CreationTime, opt => opt.Ignore())
          .ForMember(x => x.LastModificationTime, opt => opt.Ignore())
             .ForMember(x => x.CreatorUserId, opt => opt.Ignore())
          .ForMember(x => x.LastModifierUserId, opt => opt.Ignore());
            CreateMap<UserSessionListDto, UserSession>();
            //CreateMap<CreateUserSessionInput, UserSession>().ForMember(x => x.UserSessionDetails, opt => opt.Ignore());
        }
    }
}
