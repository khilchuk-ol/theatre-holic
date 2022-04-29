using System;
using System.Collections.Generic;
using System.Linq;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Tests.Unit;

public static class Fixtures
{
    public static List<Author> Authors { get; } = new List<Author>();
    public static List<Genre> Genres { get; } = new List<Genre>();
    public static List<Show> Shows { get; } = new List<Show>();
    public static List<Ticket> Tickets { get; } = new List<Ticket>();

    static Fixtures()
    {
        for (var i = 0; i < 5; i++)
        {
            Authors.Add(
                new Author()
                {
                    Id = i,
                    Name = "Author " + i
                });
        }
        
        for (var i = 0; i < 5; i++)
        {
            Genres.Add(
                new Genre()
                {
                    Id = i,
                    Name = "Genre " + i
                });
        }

        for (var i = 0; i < 5; i++)
        {
            Shows.Add(new Show()
            {
                Id = i,
                Author = Authors[i],
                AuthorId = i,
                Genre = Genres[i],
                GenreId = i,
                Title = "Show " + i,
                Date = DateTime.Now.AddDays(10 * i)
            });
        }
        
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                Tickets.Add(new Ticket()
                {
                    Id = i * 5 + j,
                    Row = j % 2,
                    Seat = j,
                    Price = 50.5,
                    Show = Shows[i],
                    ShowId = i,
                    State = j % 2 == 0 ? TicketState.Available : TicketState.Booked
                });
            }
        }
    }
}