using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.Model
{
    public enum Categoria
    {
        [Description("Clase A: artículos caros y de alta gama con controles estrictos e inventarios reducidos")]
        ClaseA = 1,
        [Description("Clase B: artículos de precio medio, de prioridad media, con un volumen de ventas y unas existencias medias")]
        ClaseB = 2,
        [Description("Clase C: artículos de bajo valor y bajo coste con grandes ventas y enormes inventarios")]
        ClaseC = 3
    
    }
}
