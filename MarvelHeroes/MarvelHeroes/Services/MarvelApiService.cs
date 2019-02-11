using MarvelHeroes.Helpers;
using MarvelHeroes.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MarvelHeroes.Services
{
    public class MarvelApiService
    {
        private static readonly string[] HEROIS = new string[]
        {
            "Captain America", "Iron Man", "Thor", "Wolverine", "Daredevil"
        };

        public async Task<List<Personagem>> GetPersonagensAsync()
        {
            List<Personagem> personagens = new List<Personagem>();
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var publicKey = Constantes.PublicKey;
                var privateKey = Constantes.PrivateKey;
                var ts = DateTime.Now.Ticks.ToString();

                var hash = SecurityHelper.GerarHash(ts, publicKey, privateKey);

                foreach (var heroi in HEROIS)
                {
                    var response = await httpClient.GetAsync(
                        Constantes.ApiBaseUrl + $"characters?ts={ts}&apikey={publicKey}&hash={hash}&name={Uri.EscapeUriString(heroi)}"
                    ).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        var conteudo = response.Content.ReadAsStringAsync().Result;
                        dynamic resultado = JsonConvert.DeserializeObject(conteudo);

                        var personagem = new Personagem
                        {
                            Nome = resultado.data.results[0].name,
                            Descricao = resultado.data.results[0].description,
                            UrlImagem = resultado.data.results[0].thumbnail.path + "." + resultado.data.results[0].thumbnail.extension,
                            UrlWiki = resultado.data.results[0].urls[1].url
                        };

                        personagens.Add(personagem);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return personagens;
        }
    }
}
