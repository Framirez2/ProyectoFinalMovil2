using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinalMovil2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerMenu : MasterDetailPage
	{
		public CustomerMenu ()
		{
			InitializeComponent ();
			this.Master = new NavCustomer();

			//pagina que mostrara de inicio
			this.Detail = new NavigationPage (new CategoriesServices());
			App.MasterDet = this;
		}
	}
}