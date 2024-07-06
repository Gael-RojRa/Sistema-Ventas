using AM_Proyecto.ViewModels;

namespace AM_Proyecto.Views
{
    public partial class VentasLista : ContentPage
    {
        public VentasLista(VentasVM viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
