using AutoMapper;
using TheatreHolic.Data.Models;

namespace TheatreHolic.WebApi.Mapping;

using Domain = Domain.Models;
using Dto = WebApi.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Show, Dto.Show.Show>();
        CreateMap<Dto.Show.Show, Domain.Show>();
        CreateMap<Dto.Show.CreateShowDto, Domain.Show>()
            .ForMember(s => s.Author,
                cd => cd.MapFrom(map => new Author
                {
                    Id = map.AuthorId
                }))
            .ForMember(s => s.Genre,
                cd => cd.MapFrom(map => new Genre
                {
                    Id = map.GenreId
                }));
        CreateMap<Dto.Show.UpdateShowDto, Domain.Show>()
            .ForMember(s => s.Author,
                cd => cd.MapFrom(map => map.AuthorId != null
                    ? new Author
                    {
                        Id = map.AuthorId.Value
                    }
                    : null))
            .ForMember(s => s.Genre,
                cd => cd.MapFrom(map => map.GenreId != null
                    ? new Genre
                    {
                        Id = map.GenreId.Value
                    }
                    : null));

        CreateMap<Domain.Ticket, Dto.Ticket.Ticket>();
        CreateMap<Dto.Ticket.Ticket, Domain.Ticket>();

        CreateMap<Domain.Author, Dto.Author>();
        CreateMap<Dto.Author, Domain.Author>();

        CreateMap<Domain.Genre, Dto.Genre>();
        CreateMap<Dto.Genre, Domain.Genre>();

        CreateMap<Domain.TicketState, Dto.Ticket.TicketState>();
        CreateMap<Dto.Ticket.TicketState, Domain.TicketState>();
        CreateMap<Dto.Ticket.CreateTicketDto, Domain.Ticket>()
            .ForMember(s => s.Show,
                cd => cd.MapFrom(map => new Show
                {
                    Id = map.ShowId
                }));
    }
}