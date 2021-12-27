using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Publications.Domain.Entities;
using Publications.Interfaces.Repositories;

namespace Publications.WebAPI.Clients;

public class PlacesClient : IRepository<Place>
{
    private readonly HttpClient _Client;
    private readonly ILogger<PlacesClient> _Logger;

    public PlacesClient(HttpClient Client, ILogger<PlacesClient> Logger)
    {
        _Client = Client;
        _Logger = Logger;
    }

    public async Task<IEnumerable<Place>> GetAllAsync(CancellationToken Cancel = default)
    {
        var result = await _Client.GetFromJsonAsync<IEnumerable<Place>>("api/place", Cancel);
        return result!;
    }

    public async Task<Place?> GetAsync(int id, CancellationToken Cancel = default)
    {
        var result = await _Client.GetFromJsonAsync<Place>($"api/place/{id}", Cancel).ConfigureAwait(false);
        return result;
    }

    public async Task<int> AddAsync(Place item, CancellationToken Cancel = default)
    {
        var response = await _Client.PostAsJsonAsync("api/place", item, Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<int>(cancellationToken: Cancel);
    }

    public async Task<bool> UpdateAsync(Place item, CancellationToken Cancel = default)
    {
        var response = await _Client.PutAsJsonAsync("api/place", item, Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: Cancel);
    }

    public async Task<bool> DeleteAsync(Place item, CancellationToken Cancel = default)
    {
        var response = await _Client.DeleteAsync($"api/place/{item.Id}", Cancel).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: Cancel);
    }
}
