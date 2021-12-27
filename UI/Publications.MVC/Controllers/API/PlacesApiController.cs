using Microsoft.AspNetCore.Mvc;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.MVC.Controllers.API;

[ApiController, Route("api/places")]
public class PlacesApiController : ControllerBase
{
    private readonly IRepository<Place> _Repository;

    public PlacesApiController(IRepository<Place> Repository) { _Repository = Repository; }

    [HttpGet]
    public async Task<IEnumerable<Place>> GetAllAsync()
    {
        return await _Repository.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Place?> GetAsync(int id)
    {
        return await _Repository.GetAsync(id);
    }

    [HttpPost]
    public async Task<int> AddAsync(Place item)
    {
        return await _Repository.AddAsync(item);
    }

    [HttpPut]
    public async Task<bool> UpdateAsync(Place item)
    {
        return await _Repository.UpdateAsync(item);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(int id)
    {
        return await _Repository.DeleteAsync(new() { Id = id });
    }
}
