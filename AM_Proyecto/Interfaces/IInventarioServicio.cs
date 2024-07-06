using AM_Proyecto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AM_Proyecto.Interfaces
{
    public interface IInventarioServicio
    {   
        Task<List<Inventario>> GetAllInventarios();
    }
}
