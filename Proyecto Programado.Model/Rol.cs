using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public enum Rol
    {
        [Display(Name = "Administrador del Sistema")]
        AdministradorDelSistema = 1,
        [Display(Name = "Usuario Normal")]
        UsuarioNormal = 2

    }
}
