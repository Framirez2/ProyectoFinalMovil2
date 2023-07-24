using Firebase.Database;

namespace ProyectoFinalMovil2.Services
{
    public class FirebaseConnection
    {
        public static FirebaseClient conexionFirebase = new FirebaseClient("https://mov2nailbarfirebase-default-rtdb.firebaseio.com");

        public const string Clave_APIweb = "AIzaSyCpF6hjpk1NdBOboU26Pp2LHz-Ps2mmTrw";
    }
}
