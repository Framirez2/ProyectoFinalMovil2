using Firebase.Auth;
using Newtonsoft.Json;
using ProyectoFinalMovil2.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ProyectoFinalMovil2.Controllers
{
    public class ContM_CrearCuenta
    {
        public async Task<bool> CrearCuentaUser(string Correo, string Contrasena)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                await authProvider.CreateUserWithEmailAndPasswordAsync(Correo, Contrasena);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task ValidarCuentaUser(string Correo, string Contrasena)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(Correo, Contrasena);
            string gettoken = auth.FirebaseToken;
            var content = await auth.GetFreshAuthAsync();
            var serializartoken = JsonConvert.SerializeObject(auth);
            Preferences.Set("MyFirebaseRefreshToken", serializartoken);
            Preferences.Set("MyToken", gettoken);

            if (content.User.IsEmailVerified == false)
            {
                await authProvider.SendEmailVerificationAsync(gettoken);
            }
        }

        public async Task ValidarCuenta(string correo, string contraseña)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
            var auth = await authProvider.SignInWithEmailAndPasswordAsync(correo, contraseña);
            string gettoken = auth.FirebaseToken;
            var content = await auth.GetFreshAuthAsync();
            var serializartoken = JsonConvert.SerializeObject(auth);
            Preferences.Set("MyFirebaseRefreshToken", serializartoken);
            Preferences.Set("MyToken", gettoken);
           // string message = "";
            //bool send = false;

            if (content.User.IsEmailVerified == false)
            {
                await DisplayAlert("Aviso", "Su correo electronico aun no s ha activado, verfique su correo", "OK");
                //var popup = new JMConfirmation("Advertencia", "Su correo electrónico no se ha activado ¿desea enviar el código de activación de nuevo?", JMConfirmation.Warning);
                /* popup.OnDialogClosed += (s, arg) =>
                 {
                     message = arg.Message;
                     send = arg.Success;
                 };
                 await App.Current.MainPage.Navigation.PushPopupAsync(popup, true);

                 if (message.Equals("Accept") || send)
                 {
                     await authProvider.SendEmailVerificationAsync(gettoken);
                 }*/
            }
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}
