using MarvelHeroes.ViewModels;
using Xamarin.Forms;

namespace MarvelHeroes
{
    public partial class DetalhesPage : ContentPage
    {
        private DetalhesViewModel ViewModel => BindingContext as DetalhesViewModel;

        public DetalhesPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel.LoadAsync();
        }
    }
}
