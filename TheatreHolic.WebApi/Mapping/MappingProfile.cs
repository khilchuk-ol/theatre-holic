using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        CreateMap<Dto.Ticket.CreateTicketDto, Domain.Ticket>()
            .ForMember(s => s.Show,
                cd => cd.MapFrom(map => new Show
                {
                    Id = map.ShowId
                }));

        CreateMap<Domain.Author, Dto.Author.Author>();
        CreateMap<Dto.Author.Author, Domain.Author>();
        CreateMap<Dto.Author.CreateAuthorDto, Domain.Author>();
        CreateMap<Dto.Author.UpdateAuthorDto, Domain.Author>();

        CreateMap<Domain.Genre, Dto.Genre.Genre>();
        CreateMap<Dto.Genre.Genre, Domain.Genre>();
        CreateMap<Dto.Genre.CreateGenreDto, Domain.Genre>();
        CreateMap<Dto.Genre.UpdateGenreDto, Domain.Genre>();

        CreateMap<Domain.TicketState, Dto.Ticket.TicketState>();
        CreateMap<Dto.Ticket.TicketState, Domain.TicketState>();
        
    }
}