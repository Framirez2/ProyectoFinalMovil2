﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FinalizarReservaciones : ContentPage
    {
        public FinalizarReservaciones()
        {
            InitializeComponent();
        }

        private void btnFinalizarRes_Clicked(object sender, EventArgs e)
        {
            /*DateTime cTime = DateTime.Now;
            string formattedDate = cTime.ToString("dd/MM/yyyy");
            string formattedDate = cTime.ToString("HH:mm:ss");
            DisplayAlert("Fecha", formattedDate, "OK");*/
        }

        private async void listReservaciones_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            /*Solo cargar las reservaciones si la fecha de reservacion es igual a la fecha actual,
              la hora de reservacion es igual o menor a la hora actual y el estado no es finalizado
             */
        }
    }
}