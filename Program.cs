using System;
using System.Threading;

namespace Clases
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int opcion = 0;
            int numSucursales = 0;
            string[] sucursales = new string[0];
            int numProductos = 0;
            string[] productos = new string[0];
            double[,] ventasProductos = new double[0, 0];
            
            while (opcion != 5)
            {
                Console.Clear();
                MostrarTitulo();
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" menu principal");
                Console.ResetColor();
                Console.WriteLine("1. configurar sucursales");
                Console.WriteLine("2. configurar productos");
                Console.WriteLine("3. registrar ventas");
                Console.WriteLine("4. ver informes");
                Console.WriteLine("5. salir del sistema");
                Console.Write("\nselecciona una opcion: ");
                
                opcion = LeerOpcionValida(1, 5);
                
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        MostrarTitulo();
                        Console.Write("cantidad de sucursales: ");
                        numSucursales = LeerNumeroPositivo();
                        sucursales = IngresarSucursales(numSucursales);
                        MostrarMensaje("sucursales registradas correctamente", ConsoleColor.Green);
                        break;
                        
                    case 2:
                        Console.Clear();
                        MostrarTitulo();
                        Console.Write("cantidad de productos: ");
                        numProductos = LeerNumeroPositivo();
                        productos = IngresarProductos(numProductos);
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nproductos registrados:");
                        Console.ResetColor();
                        foreach (string pro in productos)
                        {
                            Console.Write($"• {pro}  ");
                        }
                        Console.WriteLine("\n");
                        MostrarContinuar();
                        break;
                        
                    case 3:
                        if (numSucursales == 0 || numProductos == 0)
                        {
                            MostrarError("debes configurar sucursales y productos primero");
                            MostrarContinuar();
                            break;
                        }
                        Console.Clear();
                        ventasProductos = new double[numSucursales, numProductos];
                        ventasProductos = IngresarVentas(sucursales, productos, ventasProductos);
                        break;
                        
                    case 4:
                        if (ventasProductos.GetLength(0) == 0)
                        {
                            MostrarError("no hay ventas registradas todavia");
                            MostrarContinuar();
                            break;
                        }
                        MenuInformes(sucursales, productos, ventasProductos);
                        break;
                        
                    case 5:
                        Console.Clear();
                        MostrarTitulo();
                        MostrarMensaje("cerrando sistema", ConsoleColor.Red);
                        MostrarCargando(3);
                        break;
                }
            }
        }

        static void MenuInformes(string[] sucursales, string[] productos, double[,] ventas)
        {
            int opcion;
            double[] totales = CalcularTotales(ventas);
            
            do
            {
                Console.Clear();
                MostrarTitulo();
                
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(" informes y estadisticas");
                Console.ResetColor();
                Console.WriteLine("1. lista de sucursales");
                Console.WriteLine("2. lista de productos");
                Console.WriteLine("3. tabla de ventas completa");
                Console.WriteLine("4. totales por producto");
                Console.WriteLine("5. producto mas vendido");
                Console.WriteLine("6. volver al menu principal");
                Console.Write("\nselecciona una opcion: ");
                
                opcion = LeerOpcionValida(1, 6);
                
                Console.Clear();
                MostrarTitulo();
                
                switch (opcion)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("sucursales registradas:");
                        Console.ResetColor();
                        foreach (string suc in sucursales)
                        {
                            Console.WriteLine($"• {suc}");
                        }
                        MostrarContinuar();
                        break;
                        
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("productos registrados:");
                        Console.ResetColor();
                        foreach (string pro in productos)
                        {
                            Console.WriteLine($"• {pro}");
                        }
                        MostrarContinuar();
                        break;
                        
                    case 3:
                        MostrarTablaVentas(sucursales, productos, ventas, totales);
                        MostrarContinuar();
                        break;
                        
                    case 4:
                        MostrarTotalesPorProducto(productos, totales);
                        MostrarContinuar();
                        break;
                        
                    case 5:
                        string resultado = BuscarMasVendido(ventas, sucursales, productos);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(resultado);
                        Console.ResetColor();
                        MostrarContinuar();
                        break;
                        case 6:
                        MostrarCargando(3);
                        break;
                }
            } while (opcion != 6);
        }

        static void MostrarTablaVentas(string[] sucursales, string[] productos, double[,] ventas, double[] totales)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0,-15}", "sucursal");
            foreach (string producto in productos)
            {
                Console.Write("{0,-15}", producto);
            }
            Console.WriteLine("\n" + new string('─', 15 * (productos.Length + 1)));
            Console.ResetColor();

            for (int i = 0; i < sucursales.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0,-15}", sucursales[i]);
                
                for (int j = 0; j < productos.Length; j++)
                {
                    Console.ForegroundColor = ventas[i, j] > 0 ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.Write("{0,-15}", ventas[i, j].ToString("C"));
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("{0,-15}", "total");
            foreach (double total in totales)
            {
                Console.Write("{0,-15}", total.ToString("C"));
            }
            Console.ResetColor();
        }

        static void MostrarTotalesPorProducto(string[] productos, double[] totales)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0,-20} {1,-15}", "producto", "total vendido");
            Console.WriteLine(new string('─', 35));
            Console.ResetColor();

            for (int i = 0; i < productos.Length; i++)
            {
                Console.ForegroundColor = totales[i] > 0 ? ConsoleColor.Green : ConsoleColor.Gray;
                Console.WriteLine("{0,-20} {1,-15}", productos[i], totales[i].ToString("C"));
                Console.ResetColor();
            }
        }

        static int LeerOpcionValida(int min, int max)
        {
            int opcion;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out opcion) && opcion >= min && opcion <= max)
                {
                    return opcion;
                }
                MostrarError($"opcion invalida. ingresa un numero entre {min} y {max}");
                Console.Write("intenta de nuevo: ");
            }
        }

        static int LeerNumeroPositivo()
        {
            int numero;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out numero) && numero > 0)
                {
                    return numero;
                }
                MostrarError("valor invalido. debe ser un numero entero positivo");
                Console.Write("intenta de nuevo: ");
            }
        }

        static double LeerMontoVenta()
        {
            double monto;
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out monto) && monto >= 0)
                {
                    return monto;
                }
                MostrarError("monto invalido. debe ser un valor numerico positivo");
                Console.Write("intenta de nuevo: ");
            }
        }

        static string[] IngresarSucursales(int cantidad)
        {
            string[] sucursales = new string[cantidad];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\ningreso de sucursales");
            Console.ResetColor();
            
            for (int i = 0; i < cantidad; i++)
            {
                Console.Write($"nombre de la sucursal {i + 1}: ");
                sucursales[i] = Console.ReadLine()!.Trim();
            }
            return sucursales;
        }

        static string[] IngresarProductos(int cantidad)
        {
            string[] productos = new string[cantidad];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\ningreso de productos");
            Console.ResetColor();
            
            for (int i = 0; i < cantidad; i++)
            {
                Console.Write($"nombre del producto {i + 1}: ");
                productos[i] = Console.ReadLine()!.Trim();
            }
            return productos;
        }

        static double[,] IngresarVentas(string[] sucursales, string[] productos, double[,] ventas)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("registro de ventas por sucursal");
            Console.ResetColor();
            
            for (int i = 0; i < sucursales.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\nsucursal: {sucursales[i]}");
                Console.ResetColor();
                
                for (int j = 0; j < productos.Length; j++)
                {
                    Console.Write($"ventas de {productos[j]}: ");
                    ventas[i, j] = LeerMontoVenta();
                }
            }
            
            MostrarMensaje("ventas registradas correctamente", ConsoleColor.Green);
            return ventas;
        }

        static double[] CalcularTotales(double[,] ventas)
        {
            double[] totales = new double[ventas.GetLength(1)];
            for (int j = 0; j < ventas.GetLength(1); j++)
            {
                for (int i = 0; i < ventas.GetLength(0); i++)
                {
                    totales[j] += ventas[i, j];
                }
            }
            return totales;
        }

        static string BuscarMasVendido(double[,] ventas, string[] sucursales, string[] productos)
        {
            double maxVenta = -1;
            string productoMax = "";
            string sucursalMax = "";
            
            for (int i = 0; i < ventas.GetLength(0); i++)
            {
                for (int j = 0; j < ventas.GetLength(1); j++)
                {
                    if (ventas[i, j] > maxVenta)
                    {
                        maxVenta = ventas[i, j];
                        productoMax = productos[j];
                        sucursalMax = sucursales[i];
                    }
                }
            }
            return $"el producto mas vendido es '{productoMax}' en la sucursal '{sucursalMax}' con un total de {maxVenta.ToString("C")}!";
        }

        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"error: {mensaje}");
            Console.ResetColor();
        }

        static void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        static void MostrarTitulo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("┌───────────────────────────────────────┐");
            Console.WriteLine("│   supermercado - sistema de gestion   │");
            Console.WriteLine("└───────────────────────────────────────┘");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void MostrarCargando(int puntos)
        {
            for (int i = 0; i < puntos; i++)
            {
                Console.Write(".");
                Thread.Sleep(300);
            }
            Console.WriteLine();
        }

        static void MostrarContinuar()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\npresiona cualquier tecla para continuar...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}