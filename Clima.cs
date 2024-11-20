using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoG18
{
    // Clase para almacenar los datos del clima
    public class Clima
    {
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public float Temperatura { get; set; }
        public int Humedad { get; set; }
        public float SensacionTermica { get; set; }
        public float Viento { get; set; }
    }
}
