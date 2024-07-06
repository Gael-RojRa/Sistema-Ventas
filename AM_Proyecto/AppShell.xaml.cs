using AM_Proyecto.Views;

namespace AM_Proyecto
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(InventarioLista), typeof(InventarioLista));
        }
    }
}
