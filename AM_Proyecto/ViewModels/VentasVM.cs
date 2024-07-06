using AM_Proyecto.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.ViewModels
{
    public partial class VentasVM : ObservableObject
    {
        private readonly IVentaServicio _ventaServicio;

        public VentasVM()
        {
            
        }

        public VentasVM(IVentaServicio ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        private decimal _montoEfectivo;
        public decimal MontoEfectivo
        {
            get => _montoEfectivo;
            set => SetProperty(ref _montoEfectivo, value);
        }

        private decimal _montoTarjeta;
        public decimal MontoTarjeta
        {
            get => _montoTarjeta;
            set => SetProperty(ref _montoTarjeta, value);
        }

        private decimal _montoSinpeMovil;
        public decimal MontoSinpeMovil
        {
            get => _montoSinpeMovil;
            set => SetProperty(ref _montoSinpeMovil, value);
        }

        [RelayCommand]
        public async Task GetResumenVentas()
        {
            var ventas = await _ventaServicio.GetVentasDiarias();
            MontoEfectivo = ventas.Where(v => v.TipoPago == "Efectivo").Sum(v => v.Monto);
            MontoTarjeta = ventas.Where(v => v.TipoPago == "Tarjeta").Sum(v => v.Monto);
            MontoSinpeMovil = ventas.Where(v => v.TipoPago == "SinpeMovil").Sum(v => v.Monto);
        }
    }
}
