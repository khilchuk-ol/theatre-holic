using AutoMapper;

namespace TheatreHolic.Domain.Mapping;

using Domain = TheatreHolic.Domain.Models;
using Data = TheatreHolic.Data.Models;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Domain.Show, Data.Show>().ForMember(s => s.AuthorId, 
            cd => cd.MapFrom(map => map.Author!.Id));
        CreateMap<Data.Show, Domain.Show>();
        
        CreateMap<Domain.Ticket, Data.Ticket>();
        CreateMap<Data.Ticket, Domain.Ticket>();
        
        CreateMap<Domain.Author, Data.Author>();
        CreateMap<Data.Author, Domain.Author>();
        
        CreateMap<Domain.Genre, Data.Genre>();
        CreateMap<Data.Genre, Domain.Genre>();

        CreateMap<Domain.TicketState, Data.TicketState>();
        CreateMap<Data.TicketState, Domain.TicketState>();
    }
}