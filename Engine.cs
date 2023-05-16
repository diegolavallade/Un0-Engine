using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace YourNameSpace
{
    public class Engine
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        public static void Window(int width, int height)
        {
            AllocConsole();
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
        }

        

        private static string ObtenerColorMasCercano(Color color, Dictionary<Color, string> diccionario)
        {
            Dictionary<double, string> distancias = new Dictionary<double, string>();
            foreach (Color colorDicc in diccionario.Keys)
            {
                double distancia = CalcularDistanciaLAB(color, colorDicc);
                distancias[distancia] = diccionario[colorDicc];
            }
            return distancias[ObtenerMinimaDistancia(distancias.Keys)];
        }

        private static double CalcularDistanciaLAB(Color color1, Color color2)
        {
            double[] lab1 = RGBtoLAB(color1.R, color1.G, color1.B);
            double[] lab2 = RGBtoLAB(color2.R, color2.G, color2.B);

            double deltaL = lab1[0] - lab2[0];
            double deltaA = lab1[1] - lab2[1];
            double deltaB = lab1[2] - lab2[2];

            return Math.Sqrt(deltaL * deltaL + deltaA * deltaA + deltaB * deltaB);
        }

        private static double[] RGBtoLAB(int r, int g, int b)
        {
            double[] lab = new double[3];

            double[] xyz = RGBtoXYZ(r, g, b);
            lab[0] = 116 * FuncionF(xyz[1]) - 16; // L*
            lab[1] = 500 * (FuncionF(xyz[0] / 0.9505) - FuncionF(xyz[1])); // a*
            lab[2] = 200 * (FuncionF(xyz[1]) - FuncionF(xyz[2] / 1.0890)); // b*

            return lab;
        }

        private static double[] RGBtoXYZ(int r, int g, int b)
        {
            double[] xyz = new double[3];

            double rLinear = r / 255.0;
            double gLinear = g / 255.0;
            double bLinear = b / 255.0;

            rLinear = (rLinear > 0.04045) ? Math.Pow((rLinear + 0.055) / 1.055, 2.4) : (rLinear / 12.92);
            gLinear = (gLinear > 0.04045) ? Math.Pow((gLinear + 0.055) / 1.055, 2.4) : (gLinear / 12.92);
            bLinear = (bLinear > 0.04045) ? Math.Pow((bLinear + 0.055) / 1.055, 2.4) : (bLinear / 12.92);
            xyz[0] = (rLinear * 0.4124564) + (gLinear * 0.3575761) + (bLinear * 0.1804375);
            xyz[1] = (rLinear * 0.2126729) + (gLinear * 0.7151522) + (bLinear * 0.0721750);
            xyz[2] = (rLinear * 0.0193339) + (gLinear * 0.1191920) + (bLinear * 0.9503041);

            return xyz;
        }

        private static double FuncionF(double t)
        {
            double epsilon = 0.008856;
            double kappa = 903.3;

            if (t > epsilon)
                return Math.Pow(t, 1.0 / 3.0);
            else
                return (kappa * t + 16.0) / 116.0;
        }

        private static double ObtenerMinimaDistancia(IEnumerable<double> distancias)
            {
                double minDistancia = double.MaxValue;
                foreach (double distancia in distancias)
                {
                    if (distancia < minDistancia)
                    {
                        minDistancia = distancia;
                    }
                }
                return minDistancia;
            }

        private static List<string> ConvertirImagenATexto(string rutaImagen)
                {
                    Dictionary<Color, string> COLORS = new Dictionary<Color, string>()
            {
                {Color.FromArgb(0, 0, 0), "0"},                  // Negro
                {Color.FromArgb(0, 55, 218), "1"},               // Azul
                {Color.FromArgb(19, 161, 14), "2"},              // Verde
                {Color.FromArgb(58, 150, 221), "3"},             // Aguamarina
                {Color.FromArgb(197, 15, 31), "4"},              // Rojo
                {Color.FromArgb(136, 23, 152), "5"},             // Púrpura
                {Color.FromArgb(193, 156, 0), "6"},              // Amarillo
                {Color.FromArgb(204, 204, 204), "7"},            // Blanco chafa
                {Color.FromArgb(118, 118, 118), "8"},            // Gris
                {Color.FromArgb(59, 120, 255), "9"},             // Azul claro
                {Color.FromArgb(22, 198, 12), "A"},              // Verde claro
                {Color.FromArgb(97, 214, 214), "B"},             // Aguamarina claro
                {Color.FromArgb(231, 72, 86), "C"},              // Rojo claro
                {Color.FromArgb(180, 0, 158), "D"},              // Púrpura claro
                {Color.FromArgb(249, 241, 165), "E"},            // Amarillo palido
                {Color.FromArgb(242, 242, 242), "F"}             // Blanco Brillante
            };

                    Bitmap imagen = new Bitmap(rutaImagen);
                    int ancho = imagen.Width;
                    int alto = imagen.Height;

                // Ajustar la imagen si supera el tamaño máximo permitido
                if (ancho > (Console.BufferWidth/2) || alto > Console.BufferHeight)
                    {
                        double relacionAspecto = (double)ancho / alto;

                        if (relacionAspecto > 2) // La imagen es más ancha que alta
                        {
                            ancho = Console.BufferWidth/2;
                            alto = (int)(ancho / relacionAspecto);
                        }
                        else if (relacionAspecto == 0) //La imagen es cuadrada
                        {
                            ancho = Console.BufferHeight;
                            alto = ancho;
                        }
                        else// La imagen es más alta que ancha
                        {
                            alto = Console.BufferHeight;
                            ancho = (int)(alto * relacionAspecto);
                        }

                        imagen = new Bitmap(imagen, new Size(ancho, alto));
                    }

                    List<string> resultado = new List<string>();
                    for (int y = 0; y < alto; y++)
                    {
                        string linea = "";
                        for (int x = 0; x < ancho; x++)
                        {
                            Color color = imagen.GetPixel(x, y);
                            string colorValor;
                            colorValor = COLORS.TryGetValue(color, out colorValor) ? colorValor : ObtenerColorMasCercano(color, COLORS);
                            linea += colorValor + colorValor;
                        }
                        resultado.Add(linea);
                    }
                    return resultado;
                }

        public static void Pause(int miliseconds)
                {
                Thread.Sleep(miliseconds);
                }

        public static void Put(string str, int x, int y, bool Delay)
        {
            // Obtener la consola actual y su ancho
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            // Imprimir la cadena de texto
            int len = str.Length;
            int start = x;
            int end = consoleWidth - x - 2;

            Console.SetCursorPosition(x, y);

            if (Delay)
            {
                if (len <= end)
                {
                    for (int i = 0; i < len; i++)
                    {
                        Console.Write(str[i]);
                        Console.Out.Flush(); // flush stdout before sleeping
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        Console.Write(str[i]);
                        Console.Out.Flush(); // flush stdout before sleeping
                        Thread.Sleep(0);
                        if (i == end)
                        {
                            if (y < consoleHeight - 1)
                            {
                                Console.SetCursorPosition(x, y + 1);
                            }
                        }
                        else if (i / 2 == end)
                        {
                            if (y < consoleHeight - 2)
                            {
                                Console.SetCursorPosition(x, y + 2);
                            }
                        }
                        else if (i / 3 == end)
                        {
                            if (y < consoleHeight - 3)
                            {
                                Console.SetCursorPosition(x, y + 3);
                            }
                        }
                        else if (i / 4 == end)
                        {
                            if (y < consoleHeight - 4)
                            {
                                Console.SetCursorPosition(x, y + 4);
                            }
                        }
                        else if (i / 5 == end)
                        {
                            if (y < consoleHeight - 5)
                            {
                                Console.SetCursorPosition(x, y + 5);
                            }
                        }
                        else if (i / 6 == end)
                        {
                            if (y < consoleHeight - 6)
                            {
                                Console.SetCursorPosition(x, y + 6);
                            }
                        }
                    }
                }
            }
            else
            {
                Console.Write(str);
            }
        }

        public static void ConCol(ConsoleColor back, ConsoleColor fore)
        {

            Console.BackgroundColor = back;
            Console.ForegroundColor = fore;

        }
        
        public static void Pixel(ConsoleColor color, int x, int y)
        {
            // Establece el color del texto y el fondo de la consola al mismo valor (el del pixel)
            ConCol(color, ConsoleColor.Black);

            // Muestra un par de espacios en blanco en la posición deseada, creando el efecto de un pixel de un solo carácter de ancho y alto.

            Put("  ", x, y, false);

        }
        
        public static void uDraw(string[] lain, int Largo_Sprite, int x, int y)
        {
            // Recorremos cada línea
            for (int i = 0; i < Largo_Sprite; i++)
            {
                string line = lain[i];
                int lineLength = line.Length;

                // Recorremos cada caracter de la línea
                for (int j = 0; j < lineLength; j++)
                {
                    // Si encontramos un par de '#', entonces llamamos a Pixel
                    if (line[j] == '0' && line[j + 1] == '0')
                    {
                        // Saltamos el siguiente caracter '#' para evitar procesarlo de nuevo
                        Pixel(ConsoleColor.Black, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '1' && line[j + 1] == '1')
                    {
                        Pixel(ConsoleColor.DarkBlue, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '2' && line[j + 1] == '2')
                    {
                        Pixel(ConsoleColor.DarkGreen, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '3' && line[j + 1] == '3')
                    {
                        Pixel(ConsoleColor.DarkCyan, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '4' && line[j + 1] == '4')
                    {
                        Pixel(ConsoleColor.DarkRed, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '5' && line[j + 1] == '5')
                    {
                        Pixel(ConsoleColor.DarkMagenta, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '6' && line[j + 1] == '6')
                    {
                        Pixel(ConsoleColor.DarkYellow, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '7' && line[j + 1] == '7')
                    {
                        Pixel(ConsoleColor.Gray, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '8' && line[j + 1] == '8')
                    {
                        Pixel(ConsoleColor.DarkGray, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == '9' && line[j + 1] == '9')
                    {
                        Pixel(ConsoleColor.Blue, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'A' && line[j + 1] == 'A')
                    {
                        Pixel(ConsoleColor.Green, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'B' && line[j + 1] == 'B')
                    {
                        Pixel(ConsoleColor.Cyan, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'C' && line[j + 1] == 'C')
                    {
                        Pixel(ConsoleColor.Red, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'D' && line[j + 1] == 'D')
                    {
                        Pixel(ConsoleColor.Magenta, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'E' && line[j + 1] == 'E')
                    {
                        Pixel(ConsoleColor.Yellow, x + j, y + i);
                        j++;
                    }
                    else if (line[j] == 'F' && line[j + 1] == 'F')
                    {
                        Pixel(ConsoleColor.White, x + j, y + i);
                        j++;
                    }
                }
            }
        }

        public static void iDraw(string opfile, int x, int y)
        {
            List<string> lines = ConvertirImagenATexto(opfile);
            string[] lineas = new string[100];

            for (int i = 0; i < lines.Count; i++)
            {
                lineas[i] = lines[i];
            }

            uDraw(lineas, lines.Count, x, y);
        }
    }
}
