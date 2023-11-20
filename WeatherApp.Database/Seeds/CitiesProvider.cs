using WeatherApp.Core.Models;

namespace WeatherApp.Database.Seeds;

public static class CitiesProvider
{
    public static List<City> Get()
    {
        return new List<City>
        {
            new City { Id = Guid.Parse("e70869e5-3a4d-441a-b99e-c4af0a3380a0"), Name = "Seattle", CountryName = "USA", CountryCode = "US" },
            new City { Id = Guid.Parse("0624b834-8fa6-41f7-8d34-49376233900a"), Name = "Miami", CountryName = "USA", CountryCode = "US" },
            new City { Id = Guid.Parse("0a407814-9626-4c59-a5ea-b00426914752"), Name = "Madrid", CountryName = "Spain", CountryCode = "ES" },
            new City { Id = Guid.Parse("24d949e3-8119-471c-9d6a-0689e6a1cd53"), Name = "Barcelona", CountryName = "Spain", CountryCode = "ES" },
            new City { Id = Guid.Parse("85cb5fcb-91fa-4447-94c9-abc033d901bf"), Name = "Tokio", CountryName = "Japan", CountryCode = "JP" },
            new City { Id = Guid.Parse("42aa1ff8-1d5b-40c6-81c7-26877fb2117d"), Name = "Osaka", CountryName = "Japan", CountryCode = "JP" },
            new City { Id = Guid.Parse("60f42283-cf39-412b-926e-1dab71e9489c"), Name = "Kair", CountryName = "Egypt", CountryCode = "EG " },
            new City { Id = Guid.Parse("2d9d229d-f663-4daf-83b8-96753217d61f"), Name = "Giza", CountryName = "Egypt", CountryCode = "EG " },
            new City { Id = Guid.Parse("9bdd1e40-cbb2-4dd7-8050-5085de73a409"), Name = "Sydney", CountryName = "Australia", CountryCode = "AU" },
            new City { Id = Guid.Parse("ce2d311f-5795-4ea3-84bd-9fc797ff5f63"), Name = "Perth", CountryName = "Australia", CountryCode = "AU" },
        };
    }
}
