using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AM_Proyecto.Interfaces;
using AM_Proyecto.Services;


namespace AM_Proyecto.ViewModels
{
    public class UsuarioVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nombre)));
            }
        }

        private string clave;
        public string Clave
        {
            get => clave;
            set
            {
                clave = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clave)));
            }
        }

        private string mensaje;
        public string Mensaje
        {
            get => mensaje;
            set
            {
                mensaje = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Mensaje)));
            }
        }

        public ICommand LoginCommand { get; }

        public UsuarioVM()
        {
            LoginCommand = new Command(OnLogin);
        }

        private async void OnLogin()
        {

        }
    }
}

