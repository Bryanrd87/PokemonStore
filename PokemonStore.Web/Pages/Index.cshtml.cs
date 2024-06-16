using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using PokemonStore.Web.Models;
using PokemonStore.Web.Service.IService;

namespace PokemonStore.Web.Pages;
public class IndexModel(IPokemonService pokemonService) : PageModel
{
    [BindProperty]
    public string SearchTerm { get; set; }
    public List<PokemonDto> Pokemons { get; set; } = [];
    public PokemonDto SearchResult { get; set; }

    public async Task OnGetAsync()
    {
        await LoadAllPokemonsAsync();
    }

    public async Task<IActionResult> OnPostSearchAsync()
    {
        if (!string.IsNullOrEmpty(SearchTerm))
        {
            var searchResponse = await pokemonService.GetPokemonByNameAsync(SearchTerm);
            if (searchResponse.IsSuccess && searchResponse.Result != null)
            {
                SearchResult = JsonConvert.DeserializeObject<PokemonDto>(Convert.ToString(searchResponse.Result));
            }
            else
            {
                ModelState.AddModelError(string.Empty, searchResponse.Message ?? "Error searching for Pokémon.");
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Type a name");
            await LoadAllPokemonsAsync();
            return Page();
        }

        await LoadAllPokemonsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCollectionAsync(int id, string name, int baseExp)
    {
        var pokemon = new PokemonDto(id, name, baseExp);
        var response = await pokemonService.CreatePokemonsAsync(pokemon);

        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
            await LoadAllPokemonsAsync();
            return Page();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveFromCollectionAsync(int id)
    {
        var response = await pokemonService.DeletePokemonAsync(id);
        if (!response.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, response.Message);
        }

        return RedirectToPage();
    }

    private async Task LoadAllPokemonsAsync()
    {
        var response = await pokemonService.GetAllPokemonsAsync();
        if (response.IsSuccess)
        {
            Pokemons = JsonConvert.DeserializeObject<List<PokemonDto>>(Convert.ToString(response.Result));
        }
        else
        {
            ModelState.AddModelError(string.Empty, response.Message ?? "Error loading Pokémon collection.");
        }
    }
}
