﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Entidades
{
    [XmlInclude(typeof(Moto))]
    [XmlInclude(typeof(Auto))]
    [XmlInclude(typeof(Vehiculo))]
    [XmlInclude(typeof(Fabricante))]
    public class Concesionaria
    {
        private int _capacidad;
        private List<Vehiculo> _vehiculos;

        public int Capacidad
        {
            get { return _capacidad; }
            set { _capacidad = value; }
        }
        public List<Vehiculo> Vehiculos
        {
            get { return _vehiculos; }
            set { _vehiculos = value; }
        }

        public double PrecioDeAutos
        {
            get { return ObtenerPrecio(EVehiculo.Auto); }
            set { }
        }

        public double PrecioDeMotos
        {
            get { return ObtenerPrecio(EVehiculo.Moto); }
            set { }
        }

        public double PrecioTotal
        {
            get { return ObtenerPrecio(EVehiculo.Ambos); }
            set { }
        }

        private Concesionaria()
        {
            _vehiculos = new List<Vehiculo>();
        }

        private Concesionaria(int capacidad)
            : this()
        {
            _capacidad = capacidad;
        }

        private double ObtenerPrecio(EVehiculo tipoVehiculo)
        {
            double total = 0;

            foreach (Vehiculo vehiculo in _vehiculos)
            {
                if (tipoVehiculo == EVehiculo.Ambos)
                {
                    total += vehiculo.Precio;
                }
                else if (tipoVehiculo.ToString() == vehiculo.GetType().Name)
                {
                    total += vehiculo.Precio;
                }
            }

            return total;
        }

        public static string Mostrar(Concesionaria concesionaria)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Capacidad: {concesionaria.Capacidad}");
            sb.AppendLine($"Total por autos: ${concesionaria.PrecioDeAutos}");
            sb.AppendLine($"Total por motos: ${concesionaria.PrecioDeMotos}");
            sb.AppendLine($"Total: ${concesionaria.PrecioTotal}");
            sb.AppendLine($"************************************");
            sb.AppendLine($"         Listado de Vehículos       ");
            sb.AppendLine($"************************************");

            foreach (Vehiculo vehiculo in concesionaria._vehiculos)
            {
                if (vehiculo is Vehiculo auto)
                {
                    sb.AppendLine(auto.ToString());
                }
                else if (vehiculo is Vehiculo moto)
                {
                    sb.AppendLine(moto.ToString());
                }
            }

            return sb.ToString();
        }

        public static implicit operator Concesionaria(int capacidad)
        {
            return new Concesionaria(capacidad);
        }

        public static bool operator ==(Concesionaria concesionaria, Vehiculo vehiculo)
        {
            return concesionaria._vehiculos.Contains(vehiculo);
        }
        public static bool operator !=(Concesionaria concesionaria, Vehiculo vehiculo)
        {
            return !(concesionaria == vehiculo);
        }

        public static Concesionaria operator +(Concesionaria concesionaria, Vehiculo vehiculo)
        {
            if (concesionaria._vehiculos.Count() >= concesionaria._capacidad)
            {
                Console.WriteLine("No hay más lugar en el Concesionaria!!!");
            }
            else if (concesionaria == vehiculo)
            {
                Console.WriteLine("El vehiculo ya está en el Concesionaria!!!");
            }
            else
            {
                concesionaria._vehiculos.Add(vehiculo);
            }

            return concesionaria;
        }
        public static Concesionaria operator -(Concesionaria concesionaria, Vehiculo vehiculo)
        {
            if (concesionaria == vehiculo) // reutiliza operador igualdad
            {
                concesionaria._vehiculos.Remove(vehiculo);
            }
            else
            {
                Console.WriteLine("El vehiculo ya está en el Concesionaria!!!");
            }

            return concesionaria;
        }
        public void GuardarConcesionaria(string rutaArchivo)
        {
            if (!File.Exists(rutaArchivo))
            {
                using (FileStream fileStream = File.Create(rutaArchivo))
                {
                    fileStream.Close();
                }
            }
            using (StreamWriter sw = new StreamWriter(rutaArchivo))
            {
                sw.WriteLine(Mostrar(this));
            }
        }
        public void SerializarConcesionaria(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Concesionaria));

            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                xmlSerializer.Serialize(streamWriter, this);
            }
        }
        public static Concesionaria DeserializarConcesionaria(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Concesionaria));
            ///en vez de escribir, ahora lo leo
            using (StreamReader streamReader = new StreamReader(path))
            {
                return (Concesionaria)xmlSerializer.Deserialize(streamReader);
            }
        }
    }
}