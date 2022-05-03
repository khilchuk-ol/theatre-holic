using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TheatreHolic.Domain.Services;
using TheatreHolic.Domain.Services.Utils;
using TheatreHolic.WebApi.Dtos.Show;
using Show = TheatreHolic.WebApi.Dtos.Show.Show;

namespace TheatreHolic.WebApi.Controllers;

[ApiController]
public class ShowsController
{
    private readonly ILogger<ShowsController> _logger;
    private readonly IShowService _service;
    private readonly IMapper _mapper;

    public ShowsController(ILogger<ShowsController> logger, IShowService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("/theatreholic/api/shows/{id:int}")]
    public IResult GetShow(int id)
    {
        var show = _service.GetShowsByIdsWithInfo(new List<int> {id}, -1, -1).FirstOrDefault();
        var res = show == null ? null :_mapper.Map<Domain.Models.Show, Show>(show);

        return res == null ? Results.NotFound() : Results.Ok(res);
    }
    
    [HttpGet]
    [Route("/theatreholic/api/shows/")]
    public IResult GetShows([FromQuery] int[] ids, [FromQuery] int page, [FromQuery] int size)
    {
        var shows = _service.GetShowsByIdsWithInfo(ids.ToList(), (page - 1) * size, size).ToList();

        return !shows.Any() ? Results.NotFound() : Results.Ok(shows.Select(s => _mapper.Map<Domain.Models.Show, Show>(s)).ToList());
    }

    [HttpGet]
    [Route("/theatreholic/api/shows/filter")]
    public IResult GetShowsWithFilter([FromQuery(Name = "filter")] SearchFilter? filter, [FromQuery] int page, [FromQuery] int size)
    {
        var opts = new SearchShowsOptions
        {
            Title = filter?.Title,
            MinDateTime = filter?.MinDateTime,
            MaxDateTime = filter?.MaxDateTime,
            AuthorIds = filter?.AuthorIds?.ToList(),
            GenreIds = filter?.GenreIds?.ToList()
        };
        var shows = _service.FindShowsWithInfo(opts, (page - 1) * size, size).ToList();

        return !shows.Any() ? Results.NotFound() : Results.Ok(shows.Select(s => _mapper.Map<Domain.Models.Show, Show>(s)).ToList());
    }
    
    [HttpPost]
    [Route("/theatreholic/api/shows/")]
    public IResult CreateShow([FromBody] CreateShowDto dto)
    {
        var show = _mapper.Map<CreateShowDto, Domain.Models.Show>(dto);

        return _service.CreateShow(show) ? Results.Ok() : Results.BadRequest();
    }
    
    [HttpPatch]
    [Route("/theatreholic/api/shows/")]
    public IResult UpdateShow([FromBody] UpdateShowDto dto)
    {
        var show = _mapper.Map<UpdateShowDto, Domain.Models.Show>(dto);

        return _service.UpdateShow(show) ? Results.Ok() : Results.BadRequest();
    }
    
    [HttpDelete]
    [Route("/theatreholic/api/shows/{id:int}")]
    public IResult DeleteShow(int id)
    {
        return _service.DeleteShow(id) ? Results.Ok() : Results.BadRequest();
    }
}