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

    public bool CreateAuthor(Author item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            return false;
        }

        item.Name = item.Name.Trim();
        _repository.Create(_mapper.Map<Author, Data.Models.Author>(item));

        return true;
    }

    public bool DeleteAuthor(int id)
    {
        return _repository.Remove(id);
    }

    public bool UpdateAuthor(Author item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            return false;
        }
        
        var toPatch = _repository.Find(item.Id);
        if (toPatch == null) return false;

        toPatch.Name = item.Name.Trim();
        _repository.Update(toPatch);

        return true;
    }

    public IEnumerable<Author> FindAuthors(string? name, int offset, int amount)
    {
        name = name?.Trim();

        if (String.IsNullOrEmpty(name))
        {
            return _repository.GetPage(offset, amount)
                .Select(a => _mapper.Map<Data.Models.Author, Author>(a))
                .ToList();
        }

        return _repository.Filter(a => a.Name.Contains(name), offset, amount)
            .Select(a => _mapper.Map<Data.Models.Author, Author>(a))
            .ToList();
    }

    public IEnumerable<Author> FindAuthorsByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount)
                .Select(a => _mapper.Map<Data.Models.Author, Author>(a))
                .ToList();
        }

        return _repository.Filter(a => ids.Contains(a.Id), offset, amount)
            .Select(a => _mapper.Map<Data.Models.Author, Author>(a))
            .ToList();
    }
}