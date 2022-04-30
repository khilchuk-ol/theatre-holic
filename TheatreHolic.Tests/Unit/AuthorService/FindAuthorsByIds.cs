using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TheatreHolic.Domain.Models;
using TheatreHolic.Domain.Services;
using Xunit;

namespace TheatreHolic.Tests.Unit.AuthorService;

public class FindAuthorsByIds
{
    [Fact]
    public void IdsAreEmpty_GetAllData()
    {
        var authorSvc = GetService();

        var authors = Fixtures.Authors;
        var authorsDomain = authors.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(s)).ToList();

        Setup.AuthorRepository.Setup(repo => repo.GetPage(-1, -1)).Returns(authors);
        var res = authorSvc.FindAuthorsByIds(new List<int>(), -1, -1);

        res.Should().BeEquivalentTo(authorsDomain);

        Setup.AuthorRepository.Setup(repo => repo.GetPage(2, 2)).Returns(authors.Skip(2).Take(2).ToList());
        res = authorSvc.FindAuthorsByIds(new List<int>(), 2, 2);

        res.Should().BeEquivalentTo(authorsDomain.Skip(2).Take(2).ToList());
    }
    
    [Fact]
    public void Success()
    {
        var authorSvc = GetService();
        var ids = new List<int> { 2, 3, 4, 5};

        var authors = Fixtures.Authors.Where(g => ids.Contains(g.Id)).ToList();
        var authorsDomain = authors.Select(s => Setup.Mapper.Map<TheatreHolic.Data.Models.Author, Author>(s)).ToList();
        
        Setup.AuthorRepository.Setup(repo => repo.Filter(g => ids.Contains(g.Id), -1, -1)).Returns(authors);
        var res = authorSvc.FindAuthorsByIds(ids, -1, -1);

        res.Should().BeEquivalentTo(authorsDomain);
        
        Setup.AuthorRepository.Setup(repo => repo.Filter(g => ids.Contains(g.Id), 2, 1)).Returns(authors.Skip(2).Take(1).ToList());
        res = authorSvc.FindAuthorsByIds(ids, 2, 1);

        res.Should().BeEquivalentTo(authorsDomain.Skip(2).Take(1).ToList());
    }
    
    private IAuthorService GetService()
    {
        return new Domain.Services.Impl.AuthorService(Setup.AuthorRepository.Object, Setup.Mapper);
    }
}