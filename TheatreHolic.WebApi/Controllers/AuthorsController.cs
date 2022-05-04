using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatreHolic.Domain.Services;
using TheatreHolic.WebApi.Dtos.Author;
using Author = TheatreHolic.Data.Models.Author;

namespace TheatreHolic.WebApi.Controllers;

[ApiController]
public class AuthorsController
{
    private readonly ILogger<AuthorsController> _logger;
    private readonly IAuthorService _service;
    private readonly IMapper _mapper;

    public AuthorsController(ILogger<AuthorsController> logger, IAuthorService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("/theatreholic/api/authors/{id:int}")]
    public IResult GetAuthor(int id)
    {
        var author = _service.FindAuthorsByIds(new List<int> { id }, -1, -1).FirstOrDefault();
        var res = author == null ? null : _mapper.Map<Domain.Models.Author, Author>(author);

        return res == null ? Results.NotFound() : Results.Ok(res);
    }

    [HttpGet]
    [Route("/theatreholic/api/authors/")]
    public IResult GetAuthors([FromQuery] int[] ids, [FromQuery] int page, [FromQuery] int size)
    {
        var author = _service.FindAuthorsByIds(ids.ToList(), (page - 1) * size, size).ToList();

        return !author.Any()
            ? Results.NotFound()
            : Results.Ok(author.Select(a => _mapper.Map<Domain.Models.Author, Author>(a)).ToList());
    }
    
    [HttpGet]
    [Route("/theatreholic/api/author/filter/")]
    public IResult GetGenres([FromQuery] string name, [FromQuery] int page, [FromQuery] int size)
    {
        var author = _service.FindAuthors(name, (page - 1) * size, size).ToList();

        return !author.Any()
            ? Results.NotFound()
            : Results.Ok(author.Select(a => _mapper.Map<Domain.Models.Author, Author>(a)).ToList());
    }
    
    [HttpPost]
    [Route("/theatreholic/api/authors/")]
    public IResult CreateAuthor([FromBody] CreateAuthorDto dto)
    {
        var author = _mapper.Map<CreateAuthorDto, Domain.Models.Author>(dto);

        return _service.CreateAuthor(author) ? Results.Ok() : Results.BadRequest();
    }

    [HttpPatch]
    [Route("/theatreholic/api/authors/")]
    public IResult UpdateAuthor([FromBody] UpdateAuthorDto dto)
    {
        var author = _mapper.Map<UpdateAuthorDto, Domain.Models.Author>(dto);

        return _service.UpdateAuthor(author) ? Results.Ok() : Results.BadRequest();
    }

    [HttpDelete]
    [Route("/theatreholic/api/authors/{id:int}")]
    public IResult DeleteAuthor(int id)
    {
        return _service.DeleteAuthor(id) ? Results.Ok() : Results.BadRequest();
    }
}