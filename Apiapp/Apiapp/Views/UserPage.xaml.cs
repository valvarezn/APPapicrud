using Apiapp.API;
using Apiapp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Apiapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public ObservableCollection<User> _items;
        public UserPage()
        {
            InitializeComponent();
            _items = new ObservableCollection<User>();

            ListUsers.ItemsSource = _items;

            ListUsers.IsPullToRefreshEnabled = true;

            ListUsers.Refreshing += ListUsers_Refreshing;

            ListUsers.ItemSelected += ListUsers_ItemSelected;

            ListUsers.ItemTapped += ListUsers_ItemTapped;

            BtnAdd.Clicked += BtnAddClicked;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await RefreshItems();

        }


        private async void BtnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddUserPage(TypeAction.Add),true );
        }

        private async void ListUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item is User usuario)
            {
                var userindb = await new UserRequest(App.RestClient).Get(usuario.IdUsuario);

                if(userindb != null)
                {
                    await DisplayAlert("Usuario", userindb.nombre, "Aceptar");
                }
                else
                {
                    await DisplayAlert("Usuario", "No se ha encontrado el usuario, actualiza la pagina" , "Aceptar");
                }

                await DisplayAlert("Usuario", usuario.Nombre, "Aceptar");
            }
        }

        private void ListUsers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ListUsers.SelectedItem = null;
        }

        private async void ListUsers_Refreshing(object sender, EventArgs e)
        {
            await RefreshItems();

            ListUsers.EndRefresh();
        }

        private async Task RefreshItems()
        {
            var listusers = await new UserRequest(App.RestClient).All();
            _items.Clear();
            foreach (var item in listusers)
            {
                _items.Add(User.FromUsuario(item, Editar, Eliminar));
            }

            ListUsers.EndRefresh();
        }

        private async void Editar(User user)
        {
            var userindb = await new UserRequest(App.RestClient).Get(user.IdUsuario);

            if (userindb != null)
            {
                user = User.FromUsuario(userindb);
                await Navigation.PushAsync(new AddUserPage(TypeAction.Update, user.IdUsuario, user), true);
            }
            else
            {
                await DisplayAlert("Usuario", "No se ha encontrado el usuario, actualiza la pagina", "Aceptar");
            }
        }
        private async void Eliminar(User user)
        {
            var userindb = await new UserRequest(App.RestClient).Get(user.IdUsuario);

            if (userindb != null)
            {

                if (await new UserRequest(App.RestClient).Delete(user.IdUsuario))
                {
                    _items.Remove(user);
                }
                else { }
            }
            else
            {
                _items.Remove(user);
            }
        }
    }
}