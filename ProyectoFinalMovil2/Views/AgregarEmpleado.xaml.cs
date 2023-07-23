using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarEmpleado : ContentPage{
        MediaFile file;
        private List<string> types;

        public AgregarEmpleado(){
            InitializeComponent();
            fillList();
            cmbBoxType.ItemsSource = types;
        }

        private void btnAgr_Clicked(object sender, EventArgs e){
            //if (cmbBoxType.SelectedIndex == 0)  DisplayAlert("alerta", "valor: " + cmbBoxType.SelectedItem.ToString(), "Ok");
            if (file != null){
                if (!string.IsNullOrEmpty(txtNames.Text)){
                    if (!string.IsNullOrEmpty(txtEmail.Text)){
                        if (!string.IsNullOrEmpty(txtPassword.Text)){
                            if (!string.IsNullOrEmpty(txtRepeatPass.Text)){
                                if (cmbBoxType.SelectedIndex != -1) {
                                    if (txtPassword.Text == txtRepeatPass.Text) {
                                        if (txtEmail.Text.Contains("@") && txtEmail.Text.Contains(".")){
                                            DisplayAlert("Alerta","Todo Ok", "OK");
                                        } else {
                                            //el email no es correcto
                                        }
                                    } else {
                                        //las contraseñas no son iguales
                                    }
                                } else {
                                    //si no se selecciono nada en el comboBox
                                }
                            } else {
                                //no repitio la contraseña
                            }
                        } else {
                            //no tiene contraseña
                        }
                    } else{
                        //no tiene email
                    }
                } else {
                    //no tiene nombre
                }
            } else{
                //no se tomo la imagen
            }
        }

        private async void btnTake_Clicked(object sender, EventArgs e){
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "NailBars",
                Name = "foto.jpg",
                SaveToAlbum = true
            });
            if (file == null) { return; }
            else imgFoto.Source = ImageSource.FromStream(() => { return file.GetStream(); });
        }

        private async void btnSelect_Clicked(object sender, EventArgs e){
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null) { return; }
                else
                {
                    imgFoto.Source = ImageSource.FromStream(() => {
                        var rutaImg = file.GetStream();

                        return rutaImg;
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        /*private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }*/

        /*private async void btnSelect_Clicked(object sender, EventArgs e){
            await CrossMedia.Current.Initialize();
            try{
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions(){ 
                    PhotoSize = PhotoSize.Medium
                });
                if (file == null){ return; }
                else{
                    imgFoto.Source = ImageSource.FromStream(() =>{
                        var rutaImg = file.GetStream();

                        return rutaImg;
                    });
                }
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); }
        }

        private async void btnTake_Clicked(object sender, EventArgs e){
            file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions{
                Directory = "NailBars",
                Name = "foto.jpg",
                SaveToAlbum = true
            });
            if (file == null) { return; }
            else imgFoto.Source = ImageSource.FromStream(() => { return file.GetStream(); });
        }*/

        private void fillList(){
            types = new List<string>();
            types.Add("Empleado");
            types.Add("admin");
        }
    }
}