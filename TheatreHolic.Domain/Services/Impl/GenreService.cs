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
    
    public IEnumerable<Genre> FindGenres(string name, int offset, int amount)
    {
        if (String.IsNullOrEmpty(name))
        {
            return _repository.GetPage(offset, amount).Select(
                g => _mapper.Map<Data.Models.Genre, Genre>(g));
        }

        return _repository.Filter(
            g => g.Name.Contains(name.Trim()),
            offset,
            amount).Select(
            g => _mapper.Map<Data.Models.Genre, Genre>(g));
    }

    public IEnumerable<Genre> FindGenresByIds(List<int> ids, int offset, int amount)
    {
        if (!ids.Any())
        {
            return _repository.GetPage(offset, amount).Select(
                g => _mapper.Map<Data.Models.Genre, Genre>(g));
        }

        return _repository.Filter(
            g => ids.Contains(g.Id),
            offset,
            amount).Select(
            g => _mapper.Map<Data.Models.Genre, Genre>(g));
    }
}