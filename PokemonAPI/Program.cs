
using Newtonsoft.Json;
using PokemonAPI.Models;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace PokemonAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapGet("/randomquote", async () =>
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://api.gameofthronesquotes.xyz/v1/random");
                    var content = await response.Content.ReadAsStringAsync();

                    var quote = JsonConvert.DeserializeObject<Quote>(content);
                    return Results.Ok(quote);
                }
            });

            app.MapGet("/randomjoke", async () =>
            {
                using(var client = new HttpClient())
                {
                    var response = await client.GetAsync("https://api.chucknorris.io/jokes/random");
                    var content = await response.Content.ReadAsStringAsync();

                    var joke = JsonConvert.DeserializeObject<Joke>(content);
                    return Results.Ok(joke);
                }
            });

            var pokemons = new List<Pokemon>
            {
                //new Pokemon { Id = 1, Name = "Bulbasaur", Type = "Grass"},
                //new Pokemon { Id = 2, Name = "Ivysaur", Type = "Grass"},
                //new Pokemon { Id = 3, Name = "Venosaur", Type = "Grass"},
                //new Pokemon { Id = 4, Name = "Charmande", Type = "Fire"}
            };

            app.MapGet("/pokemons", () =>
            {
                return Results.Ok(pokemons);
            });

            app.MapGet("/pokemon/{id}", async (int id) =>
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        var response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{id}/");
                        var content = await response.Content.ReadAsStringAsync();


                        var pokemon = JsonConvert.DeserializeObject<Pokemon>(content);
                        return Results.Ok(pokemon);
                    }
                    catch
                    {
                        return Results.NotFound("Could not found Pokemon");
                    }
                }

            });

            app.MapPost("/pokemon", (Pokemon pokemon) =>
            {
                pokemons.Add(pokemon);
                return Results.Ok("Added succesfully");
            });

            var digimons = new List<Digimon>
            {
                new Digimon { Id = 1, Name = "Digimon", Type = "Unknown"},
                new Digimon { Id = 2, Name = "Digidong", Type = "All"}
            };

            app.MapPut("/pokemon/{id}", (int id, Pokemon pokemon) =>
            {
                //var pokemonToUpdate = pokemons.Find(p => p.Id == id);
                //if (pokemonToUpdate == null)
                //{
                //    return Results.NotFound("Sorry a pokemon by this ID does not exist");
                //}

                //pokemonToUpdate.Name = pokemon.Name;
                //pokemonToUpdate.Type = pokemon.Type;

                //return Results.Ok("Updated Pokemon successfully");
            });

            app.MapDelete("/pokemon/{id}", (int id) =>
            {
                var pokemonToRemove = pokemons.Find(p => p.Id == id);
                if(pokemonToRemove == null)
                {
                    return Results.NotFound("Sorry , there is no pokemon at this id to remove");
                }

                pokemons.Remove(pokemonToRemove);
                return Results.Ok("Pokemon was removed successfully");
            });









            //var digimons = new List<Digimon>
            //{
            //    new Digimon { Id = 1, Name = "Digimon", Type = "Grass"},
            //    new Digimon { Id = 2, Name = "Digidong", Type = "Grass"}
            //};




            app.MapGet("/digimons", () =>
            {
                return Results.Ok(digimons);
            });

            app.MapGet("/digimon/{id}", (int id) =>
            {
                var digimon = digimons.Find(p => p.Id == id);

                if (digimon == null)
                {
                    return Results.NotFound("Sorry this digimon does not exist");
                }

                return Results.Ok(digimons);
            });


            app.MapPost("/digimon", (Digimon digimon) =>
            {
                digimons.Add(digimon);
                return Results.Ok("Added succesfully");
            });

            app.MapPut("/digimon/{id}", (int id, Digimon digimon) =>
            {
                var digimonToUpdate = digimons.Find(p => p.Id == id);
                if (digimonToUpdate == null)
                {
                    return Results.NotFound("Sorry a digimon by this ID does not exist");
                }

                digimonToUpdate.Name = digimon.Name;
                digimonToUpdate.Type = digimon.Type;

                return Results.Ok("Updated Digimon successfully");
            });

            app.MapDelete("/digimon/{id}", (int id) =>
            {
                var digimonToRemove = digimons.Find(p => p.Id == id);
                if (digimonToRemove == null)
                {
                    return Results.NotFound("Sorry , there is no digimon at this id to remove");
                }

                digimons.Remove(digimonToRemove);
                return Results.Ok("digimon was removed successfully");
            });


            app.Run();
        }
    }
}