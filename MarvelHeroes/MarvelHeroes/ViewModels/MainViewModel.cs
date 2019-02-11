using MarvelHeroes.Models;
using MarvelHeroes.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelHeroes.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Personagem> Personagens { get; }

        public Command<Personagem> ExibirPersonagemCommand { get; }

        private MarvelApiService _marvelApiService;

        public MainViewModel()
        {
            Titulo = "Heróis Marvel";

            Personagens = new ObservableCollection<Personagem>();

            ExibirPersonagemCommand = new Command<Personagem>(ExecuteExibirPersonagemCommand);

            _marvelApiService = new MarvelApiService();
        }

        private async void ExecuteExibirPersonagemCommand(Personagem personagem)
        {
            await Navigation.PushAsync<DetalhesViewModel>(false, personagem);
        }

        public override async Task LoadAsync()
        {
            Ocupado = true;
            try
            {
                var personagens = await _marvelApiService.GetPersonagensAsync();

                Personagens.Clear();

                foreach (var personagem in personagens)
                {
                    Personagens.Add(personagem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erro", ex.Message);
            }
            finally
            {
                Ocupado = false;
            }
        }
    }
}
