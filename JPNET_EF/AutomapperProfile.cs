namespace JPNET_EF;

using Abstractions.dbo;
using Abstractions.Entity;
using AutoMapper;

public class AutomapperProfile  : Profile
{
    public AutomapperProfile()
    {
        CreateMap<ClientResource, Client>();
        CreateMap<ClientResource, InternetClient>();
        CreateMap<OrderResource, InternetOrder>()
            .ForMember(d => d.IpAddress, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Client, o => o.Ignore())
            .ForMember(d => d.ClientId, o => o.MapFrom(s => s.ClientId))
            .ForMember(d => d.Items, o => o.Ignore())
            .ForMember(d => d.IsFinished, o => o.MapFrom(_ => false));
        CreateMap<OrderResource, Order>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Client, o => o.Ignore())
            .ForMember(d => d.ClientId, o => o.MapFrom(s => s.ClientId))
            .ForMember(d => d.Items, o => o.Ignore())
            .ForMember(d => d.IsFinished, o => o.MapFrom(_ => false));
    }
}