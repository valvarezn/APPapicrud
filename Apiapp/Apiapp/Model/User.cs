using Apiapp.API;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Apiapp.Model
{
    public class User
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Cedula { get; set; }
        public Command<User> Editar { get; set; }
        public Command<User> Eliminar { get; set; }

        public static User FromUsuario (Usuario item, Action<User> editar = null , Action<User> eliminar = null)
        {
            var user = new User
            {
                Nombre = item.nombre,
                Email = item.email,
                Cedula = item.cedula,
                IdUsuario = item.idusuario,
                
            };

            if ( editar != null)
            {
                user.Editar = new Command<User>(editar);
            }

            if(eliminar != null)
            {
                user.Eliminar = new Command<User>(eliminar);
            }

            return user; 
        
        }

    }
}
