using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatreHolic.Data.Models;
using TheatreHolic.Domain.Services;

namespace TheatreHolic.WebApi.Controllers;

[ApiController]
public class GenresController
{
    private readonly ILogger<GenresController> _logger;
    private readonly IGenreService _service;
    private readonly IMapper _mapper;

    public GenresController(ILogger<GenresController> logger, IGenreService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("/theatreholic/api/genres/{id:int}")]
    public IResult GetGenre(int id)
    {
        var genre = _service.FindGenresByIds(new List<int> { id }, -1, -1).FirstOrDefault();
        var res = genre == null ? null : _mapper.Map<Domain.Models.Genre, Genre>(genre);

        return res == null ? Results.NotFound() : Results.Ok(res);
    }

    [HttpGet]
    [Route("/theatreholic/api/genres/")]
    public IResult GetGenres([FromQuery] int[] ids, [FromQuery] int page, [FromQuery] int size)
    {
        var genres = _service.FindGenresByIds(ids.ToList(), (page - 1) * size, size).ToList();

        return !genres.Any()
            ? Results.NotFound()
            : Results.Ok(genres.Select(g => _mapper.Map<Domain.Models.Genre, Genre>(g)).ToList());
    }
    
    [HttpGet]
    [Route("/theatreholic/api/genres/filter/")]
    public IResult GetGenres([FromQuery] string name, [FromQuery] int page, [FromQuery] int size)
    {
        var genres = _service.FindGenres(name, (page - 1) * size, size).ToList();

        return !genres.Any()
            ? Results.NotFound()
            : Results.Ok(genres.Select(g => _mapper.Map<Domain.Models.Genre, Genre>(g)).ToList());
    }
}