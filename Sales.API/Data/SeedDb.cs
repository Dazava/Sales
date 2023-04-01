using Microsoft.EntityFrameworkCore;
using Sales.API.Helpers;
using Sales.API.Services;
using Sales.Shared.Entities;
using Sales.Shared.Enums;
using Sales.Shared.Responses;

namespace Sales.API.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Daza", "Zapata", "daza@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }


        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                Response responseCountries = await _apiService.GetListAsync<CountryResponse>("/v1", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = (List<CountryResponse>)responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country? country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = (List<StateResponse>)responseStates.Result!;
                                foreach (StateResponse stateResponse in states!)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = (List<CityResponse>)responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }
        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Lacteos" });
                _context.Categories.Add(new Category { Name = "Cárnicos" });
                _context.Categories.Add(new Category { Name = "Electrodomésticos" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Bebés" });
                _context.Categories.Add(new Category { Name = "Dama" });
                _context.Categories.Add(new Category { Name = "Caballero" });
                _context.Categories.Add(new Category { Name = "Niños" });
                _context.Categories.Add(new Category { Name = "Niñas" });
                _context.Categories.Add(new Category { Name = "Deporte" });
                _context.Categories.Add(new Category { Name = "Productos de aseo" });
                _context.Categories.Add(new Category { Name = "Productos de Belleza" });
                _context.Categories.Add(new Category { Name = "Accesorios de moda" });
                _context.Categories.Add(new Category { Name = "Accesorios" });
                _context.Categories.Add(new Category { Name = "Tables" });
                _context.Categories.Add(new Category { Name = "Celulares" });
                _context.Categories.Add(new Category { Name = "Laptops" });
                _context.Categories.Add(new Category { Name = "Televisores" });
                _context.Categories.Add(new Category { Name = "Electrónica de consumo" });
                _context.Categories.Add(new Category { Name = "Muebles" });
                _context.Categories.Add(new Category { Name = "Decoración" });
                _context.Categories.Add(new Category { Name = "Iluminación" });
                _context.Categories.Add(new Category { Name = "Lamparas de techo" });
                _context.Categories.Add(new Category { Name = "Lamparas de mano" });
                _context.Categories.Add(new Category { Name = "Cosmeticos" });
                _context.Categories.Add(new Category { Name = "Perfumes" });
                _context.Categories.Add(new Category { Name = "Fragancias" });
                _context.Categories.Add(new Category { Name = "Cuidado del cabello" });
                _context.Categories.Add(new Category { Name = "Alimentos" });
                _context.Categories.Add(new Category { Name = "Bebidas" });
                _context.Categories.Add(new Category { Name = "Bebidas alcoholicas" });
                _context.Categories.Add(new Category { Name = "Bebidas no alcoholicas" });
                _context.Categories.Add(new Category { Name = "Suplementos" });
                _context.Categories.Add(new Category { Name = "Vitaminas" });
                _context.Categories.Add(new Category { Name = "Medicamentos" });
                _context.Categories.Add(new Category { Name = "Viajes" });
                _context.Categories.Add(new Category { Name = "Productos para mascotas" });
                _context.Categories.Add(new Category { Name = "Libros" });
                _context.Categories.Add(new Category { Name = "Revistas" });
                _context.Categories.Add(new Category { Name = "Comics" });
                _context.Categories.Add(new Category { Name = "Musica" });
                _context.Categories.Add(new Category { Name = "Peliculas" });
                _context.Categories.Add(new Category { Name = "Juegos de mesa" });
                _context.Categories.Add(new Category { Name = "Camping" });
                _context.Categories.Add(new Category { Name = "Instrumentos musicales" });
                _context.Categories.Add(new Category { Name = "Instrumentos de cuerdas" });
                _context.Categories.Add(new Category { Name = "Instrumentos de percusión" });
                _context.Categories.Add(new Category { Name = "Micrófonos" });
                _context.Categories.Add(new Category { Name = "Pianos" });
                _context.Categories.Add(new Category { Name = "Vehículos" });
                _context.Categories.Add(new Category { Name = "Automóviles" });
                _context.Categories.Add(new Category { Name = "Motocicletas" });
                _context.Categories.Add(new Category { Name = "Herramientas" });
                _context.Categories.Add(new Category { Name = "Herramientas eléctricas" });
                _context.Categories.Add(new Category { Name = "Arte" });
                _context.Categories.Add(new Category { Name = "Manualidades" });
                _context.Categories.Add(new Category { Name = "Pintura" });
                _context.Categories.Add(new Category { Name = "Escultura" });
                _context.Categories.Add(new Category { Name = "Equipos médicos" });
                _context.Categories.Add(new Category { Name = "Suministros médicos" });
                _context.Categories.Add(new Category { Name = "Materiales de construcción" });
                _context.Categories.Add(new Category { Name = "Productos de viaje" });
                _context.Categories.Add(new Category { Name = "Limpiadores de autos" });
                _context.Categories.Add(new Category { Name = "Bicicletas" });
                _context.Categories.Add(new Category { Name = "Aire libre" });
                _context.Categories.Add(new Category { Name = "Fotografía" });
                _context.Categories.Add(new Category { Name = "Licencias de software" });
                _context.Categories.Add(new Category { Name = "Puertas" });
                _context.Categories.Add(new Category { Name = "Puertas de baño" });
                _context.Categories.Add(new Category { Name = "Piscinas" });
                _context.Categories.Add(new Category { Name = "Mouses" });
                _context.Categories.Add(new Category { Name = "Teclados" });
                await _context.SaveChangesAsync();
            }
        }
    }
}