using Firebase.Auth;
using ProyectoFinalMovil2.Services;
using System;
using System.Threading.Tasks;

namespace ProyectoFinalMovil2.Controllers
{
    public class Cont_RestablecerCuenta
    {
        public async Task<bool> RestablecerClvae(string Correo)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(FirebaseConnection.Clave_APIweb));
                await authProvider.SendPasswordResetEmailAsync(Correo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
