using AutoMapper;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services.Impl;

public class AuthorService : IAuthorService
{
    private IAuthorRepository _repository;
    private IMapper _mapper;

    public AuthorService(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }


    public IEnumerable<Author> FindAuthors(string name, int offset, int amount)
    {
        if (String.IsNullOrEmpty(name))
        {
            return _repository.GetPage(offset, amount).Select(
                a => _mapper.Map<Data.Models.Author, Author>(a));
        }

        return _repository.Filter(
            a => a.Name.Contains(name.Trim()),
            offset,
            amount).Select(
            a => _mapper.Map<Data.Models.Author, Author>(a));
    }

    public IEnumerable<Author> FindAuthorsByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount).Select(
                a => _mapper.Map<Data.Models.Author, Author>(a));
        }

        return _repository.Filter(
            a => ids.Contains(a.Id),
            offset,
            amount).Select(
            a => _mapper.Map<Data.Models.Author, Author>(a));
    }
}