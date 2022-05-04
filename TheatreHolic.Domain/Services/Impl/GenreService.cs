using AutoMapper;
using TheatreHolic.Data.Repository;
using TheatreHolic.Domain.Models;

namespace TheatreHolic.Domain.Services.Impl;

public class GenreService : IGenreService
{
    private IGenreRepository _repository;
    private IMapper _mapper;

    public GenreService(IGenreRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public bool CreateGenre(Genre item)
    {
        if (string.IsNullOrWhiteSpace(item.Name))
        {
            return false;
        }

        item.Name = item.Name.Trim();
        _repository.Create(_mapper.Map<Genre, Data.Models.Genre>(item));

        return true;
    }

    public bool DeleteGenre(int id)
    {
        return _repository.Remove(id);
    }

    public bool UpdateGenre(Genre item)
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
    
    public IEnumerable<Genre> FindGenres(string? name, int offset, int amount)
    {
        name = name?.Trim();

        if (String.IsNullOrEmpty(name))
        {
            return _repository.GetPage(offset, amount)
                .Select(g => _mapper.Map<Data.Models.Genre, Genre>(g))
                .ToList();
        }
        
        return _repository.Filter(g => g.Name.Contains(name), offset, amount)
            .Select(g => _mapper.Map<Data.Models.Genre, Genre>(g))
            .ToList();
    }

    public IEnumerable<Genre> FindGenresByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount)
                .Select(g => _mapper.Map<Data.Models.Genre, Genre>(g))
                .ToList();
        }

        return _repository.Filter(g => ids.Contains(g.Id), offset, amount)
            .Select(g => _mapper.Map<Data.Models.Genre, Genre>(g))
            .ToList();
    }
}