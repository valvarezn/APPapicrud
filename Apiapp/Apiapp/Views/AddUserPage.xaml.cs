using Apiapp.API;
using Apiapp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Apiapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPage : ContentPage
    {
        private TypeAction _action { get; set;}
        private int?_iduser { get; set; }
        private User _user { get; set; }
        public AddUserPage(TypeAction action, int? iduser = null, User user = null)
        {
            InitializeComponent();
            _action = action;
            _iduser = iduser;
            _user = user;
            if (action == TypeAction.Update)
            {
                if (_iduser == null) throw new NullReferenceException("Iduser no puede ser null");
                BindingContext = user ?? throw new NullReferenceException("User no puede ser null");
               
            }
            BoxEmail.Keyboard = Keyboard.Email;
            BoxCedula.Keyboard = Keyboard.Numeric;
            BtnSave.Clicked += BtnSave_Clicked;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            BtnSave.IsEnabled = false;

            var nombre = BoxNombre.Text ?? "";
            var email = BoxEmail.Text ?? "";
            var cedula = BoxCedula.Text ?? "";

            if (string.IsNullOrEmpty(nombre))
            {
                await DisplayAlert("Usuario", "Ingresa un nombre", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Usuario", "Ingresa un correo", "Aceptar");
                return;
            }
            if (string.IsNullOrEmpty(cedula))
            {
                await DisplayAlert("Usuario", "Ingresa una cedula", "Aceptar");
                return;
            }

            UserRequest request = new UserRequest(App.RestClient);
            var user = new Usuario
            {
                nombre = nombre,
                email = email,
                cedula = cedula
            };

            if (_action == TypeAction.Add)
            {
                if (await request.Add(user))
                {
                    await DisplayAlert("Usuario", "Se creó el usuario", "Aceptar");
                }
                else
                {
                    await DisplayAlert("Usuario", "No se pudo crear el usuario", "Aceptar");
                }
            }
            else
            {
                if (_iduser.HasValue)
                {
                    if (await request.Update(user, _iduser.Value))
                    {
                        await DisplayAlert("Usuario", "Se Actualizó el usuario", "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert("Usuario", "No se pudo Actualizar el usuario", "Aceptar");
                    }
                }
                else
                {

                }
            }
                BoxNombre.Text = "";
                BoxEmail.Text = "";
                BoxCedula.Text = "";

                BtnSave.IsEnabled = true;
            
        }
    }

    public enum TypeAction
    {
        Add, Update
    }
}