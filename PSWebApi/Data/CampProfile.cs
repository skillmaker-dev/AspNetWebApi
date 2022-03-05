using AutoMapper;
using CoreCodeCamp.Data;
using PSWebApi.Models;

namespace PSWebApi.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>();
            this.CreateMap<Camp, CampModel>().ReverseMap();

            this.CreateMap<Talk, TalkModel>();
            this.CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.TalkId, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore())
                .ForMember(t => t.Camp, opt => opt.Ignore());

            this.CreateMap<Speaker,SpeakerModel>();
            this.CreateMap<Speaker, SpeakerModel>().ReverseMap();
        }
    }
}
