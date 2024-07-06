using AM_Proyecto.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AM_Proyecto.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.ViewModels
{
    public partial class InventarioVM : ObservableObject
    {
        private readonly IInventarioServicio _inventarioServicio;

        public ObservableCollection<Inventario> Inventarios { get; set; } = new ObservableCollection<Inventario>();

        public InventarioVM()
        {
           
        }

        public InventarioVM(IInventarioServicio inventarioServicio)
        {
            _inventarioServicio = inventarioServicio;
        }

        [RelayCommand]
        public async Task GetInventarios()
        {
            var inventarios = await _inventarioServicio.GetAllInventarios();
            Inventarios.Clear();
            foreach (var inventario in inventarios)
            {
                Inventarios.Add(inventario);
            }
        }
    }
}

