﻿using System.ComponentModel.DataAnnotations;

namespace Proyecto_Programado.UI.ViewModels
{
    public class UsuarioLoginVM
    {

        public string NombreUsuario { get; set; }


        [DataType(DataType.Password)]
        public string Clave { get; set; }


    }
}
