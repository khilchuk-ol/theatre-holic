using System.Globalization;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Utils;

namespace TheatreHolic.CLI.Presentation;

public class CLI : IShowable
{
    private ITicketService _ticketSvc;
    private IShowService _showSvc;
    private IAuthorService _authorSvc;
    private IGenreService _genreSvc;

    public CLI(ITicketService ticketSvc,
        IShowService showSvc,
        IAuthorService authorSvc,
        IGenreService genreSvc)
    {
        _ticketSvc = ticketSvc;
        _showSvc = showSvc;
        _authorSvc = authorSvc;
        _genreSvc = genreSvc;
    }

    public void Show()
    {
        Menu();

        Console.Clear();
        Console.WriteLine("Bye");
        Thread.Sleep(500);
    }

    private void Menu()
    {
        Console.Clear();
        Console.WriteLine("Choose options: \n" +
                          "s - find shows\n" +
                          "t - find tickets\n" +
                          "e - exit");
        var inp = Console.ReadLine().Trim();

        switch (inp.Trim().ToLower())
        {
            case "s":
                FindShows();
                Menu();
                break;
            case "t":
                FindTickets();
                Menu();
                break;
            case "e":
                return;
            default:
                Menu();
                break;
        }
    }

    private void FindShows()
    {
        Console.Clear();
        Console.WriteLine("Choose value to search by:\n" +
                          "t - title\n" +
                          "a - author\n" +
                          "g - genre\n\n" +
                          "any other symbol - return to menu");
        var inp = Console.ReadLine();

        switch (inp?.Trim().ToLower())
        {
            case "t":
                FindShowsByTitle();
                break;
            case "a":
                FindShowsByAuthor();
                break;
            case "g":
                FindShowsByGenre();
                break;
            default:
                return;
        }
    }

    private void FindShowsByTitle()
    {
        Console.Clear();
        Console.WriteLine("You have chosen finding shows by title");
        Console.Write("Input title: ");
        var str = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(str))
        {
            Console.WriteLine("Bad input");
            Console.Write("Press any key to return to menu");
            Console.ReadLine();
            return;
        }

        var res = _showSvc.FindShows(new SearchShowsOptions()
        {
            Title = str
        }, -1, -1).ToList();

        if (!res.Any())
            Console.WriteLine("Nothing was found");
        else
        {
            Console.WriteLine("\nResult\n");
            Console.WriteLine($"{"ID",-15} | {"Title",-40} | {"Date",-40} | {"Available tickets amount",-20}");

            foreach (var s in res)
            {
                var tickets = _ticketSvc.GetAvailableTickets(s.Id);

                Console.WriteLine(
                    $"{s.Id,-15} | {s.Title,-40} | {s.Date.ToString("f", CultureInfo.GetCultureInfo("en-US")),-40} | {tickets.Count(),-20}");
            }
        }

        Console.Write("Press any key to return to menu");
        Console.ReadLine();
    }

    private void FindShowsByAuthor()
    {
        Console.Clear();
        Console.WriteLine("You have chosen finding shows by author");
        Console.Write("Input author: ");
        var str = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(str))
        {
            Console.WriteLine("Bad input");
            Console.Write("Press any key to return to menu");
            Console.ReadLine();
            return;
        }

        var authors = _authorSvc.FindAuthors(str, -1, -1).ToList();

        if (!authors.Any())
            Console.WriteLine("Nothing was found");
        else
        {
            var res = _showSvc.FindShows(new SearchShowsOptions()
            {
                AuthorIds = authors.Select(a => a.Id).ToList()
            }, -1, -1).ToList();

            if (!res.Any())
                Console.WriteLine("Nothing was found");
            else
            {
                Console.WriteLine("\nResult\n");
                Console.WriteLine($"{"ID",-15} | {"Title",-40} | {"Date",-40} | {"Available tickets amount",-20}");

                foreach (var s in res)
                {
                    var tickets = _ticketSvc.GetAvailableTickets(s.Id);

                    Console.WriteLine(
                        $"{s.Id,-15} | {s.Title,-40} | {s.Date.ToString("f", CultureInfo.GetCultureInfo("en-US")),-40} | {tickets.Count(),-20}");
                }
            }
        }

        Console.Write("Press any key to return to menu");
        Console.ReadLine();
    }

    private void FindShowsByGenre()
    {
        Console.Clear();
        Console.WriteLine("You have chosen finding shows by genre");
        Console.Write("Input genre: ");
        var str = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(str))
        {
            Console.WriteLine("Bad input");
            Console.Write("Press any key to return to menu");
            Console.ReadLine();
            return;
        }

        var genres = _genreSvc.FindGenres(str, -1, -1).ToList();

        if (!genres.Any())
            Console.WriteLine("Nothing was found");
        else
        {
            var res = _showSvc.FindShows(new SearchShowsOptions()
            {
                GenreIds = genres.Select(g => g.Id).ToList()
            }, -1, -1).ToList();

            if (!res.Any())
                Console.WriteLine("Nothing was found");
            else
            {
                Console.WriteLine("\nResult\n");
                Console.WriteLine($"{"ID",-15} | {"Title",-40} | {"Date",-40} | {"Available tickets amount",-20}");

                foreach (var s in res)
                {
                    var tickets = _ticketSvc.GetAvailableTickets(s.Id);

                    Console.WriteLine(
                        $"{s.Id,-15} | {s.Title,-40} | {s.Date.ToString("f", CultureInfo.GetCultureInfo("en-US")),-40} | {tickets.Count(),-20}");
                }
            }
        }

        Console.Write("Press any key to return to menu");
        Console.ReadLine();
    }

    private void FindTickets()
    {
        Console.Clear();
        Console.WriteLine("You have chosen to find tickets");
        Console.Write("Input show id: ");
        var str = Console.ReadLine()?.Trim();

        int id;
        var ok = int.TryParse(str, out id);

        if (string.IsNullOrWhiteSpace(str) || !ok)
        {
            Console.WriteLine("Bad input");
            Console.Write("Press any key to return to menu");
            Console.ReadLine();
            return;
        }

        var tickets = _ticketSvc.GetAvailableTickets(id).ToList();

        if (!tickets.Any())
            Console.WriteLine("Nothing was found");
        else
        {
            Console.WriteLine("\nResult\n");
            Console.WriteLine($"{"ID",-15} | {"Price",-10} | {"Row",-10} | {"Seat",-10}");

            foreach (var t in tickets)
            {
                Console.WriteLine($"{t.Id,-15} | {t.Price.ToString("C", CultureInfo.GetCultureInfo("en-US")),-10} | {t.Row,-10} | {t.Seat,-10}");
            }

            Console.Write("\nEnter ticket id to book or buy it: ");
            var ticketStr = Console.ReadLine()?.Trim();
            int tId;
            ok = int.TryParse(ticketStr, out tId);

            if (string.IsNullOrWhiteSpace(ticketStr) || !ok)
            {
                Console.WriteLine("Bad input");
                Console.Write("Press any key to return to menu");
                Console.ReadLine();
                return;
            }

            ProcessTicket(tId);
        }

        Console.Write("Press any key to return to menu");
        Console.ReadLine();
    }

    private void ProcessTicket(int ticketId)
    {
        Console.Clear();
        Console.WriteLine("Choose action to apply to ticket:\n" +
                          "b - buy\n" +
                          "k - book\n" +
                          "any other symbol - return to menu");
        var inp = Console.ReadLine();

        string msg;

        switch (inp?.Trim().ToLower())
        {
            case "b":
                msg = _ticketSvc.BuyTicket(ticketId) ? 
                    "Successfully bought" : 
                    "Something went wrong. Try again";
                Console.WriteLine(msg + "\n");

                break;
            case "k":
                msg = _ticketSvc.BookTicket(ticketId) ? 
                    "Successfully booked" : 
                    "Something went wrong. Try again";
                Console.WriteLine(msg + "\n");

                break;
            default:
                return;
        }

        Console.Write("Press any key to return to menu");
        Console.ReadLine();
    }
}