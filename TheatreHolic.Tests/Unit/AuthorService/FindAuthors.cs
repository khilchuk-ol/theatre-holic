using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.AuthorService;

public class FindAuthors
{
    [Fact]
    public void NameIsNull_GetAllData()
    {
        var authorSvc = GetService();
        
        var authors = Fixtures.Authors;
        var authorsDomain = authors.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(s)).ToList();

        Setup.AuthorRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(authors);
        var res = authorSvc.FindAuthors(null, -1, -1);

        res.Should().BeEquivalentTo(authorsDomain);

        Setup.AuthorRepository.Setup(repo => repo.GetPage(2, 2)).Returns(authors.Skip(2).Take(2).ToList());
        res = authorSvc.FindAuthors(null, 2, 2);

        res.Should().BeEquivalentTo(authorsDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void NameIsWhitespace_GetAllData()
    {
        var authorSvc = GetService();
        var name = "    ";
        
        var authors = Fixtures.Authors;
        var authorsDomain = authors.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(s)).ToList();

        Setup.AuthorRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(authors);
        var res = authorSvc.FindAuthors(name, -1, -1);

        res.Should().BeEquivalentTo(authorsDomain);

        Setup.AuthorRepository.Setup(repo => repo.GetPage(2, 2)).Returns(authors.Skip(2).Take(2).ToList());
        res = authorSvc.FindAuthors(name, 2, 2);

        res.Should().BeEquivalentTo(authorsDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void Success()
    {
        var authorSvc = GetService();
        var name = "3";
        
        var authors = Fixtures.Authors.Where(a => a.Name.Contains(name)).ToList();
        var authorsDomain = authors.Select(g => Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(g)).ToList();

        Setup.AuthorRepository.Setup(repo => repo.Filter(g => g.Name.Contains(name), -1, -1)).Returns(authors);
        var res = authorSvc.FindAuthors(name, -1, -1);

        res.Should().BeEquivalentTo(authorsDomain);

        Setup.AuthorRepository.Setup(repo => repo.Filter(g => g.Name.Contains(name), 2, 2)).Returns(authors.Skip(2).Take(2).ToList());
        res = authorSvc.FindAuthors(name, 2, 2);

        res.Should().BeEquivalentTo(authorsDomain.Skip(2).Take(2).ToList());
    }

    private IAuthorService GetService()
    {
        return new Domain.Services.Impl.AuthorService(Setup.AuthorRepository.Object, Setup.Mapper);
    }
}