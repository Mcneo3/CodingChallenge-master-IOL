/*
 * Refactorear la clase para respetar principios de programación orientada a objetos. Qué pasa si debemos soportar un nuevo idioma para los reportes, o
 * agregar más formas geométricas?
 *
 * Se puede hacer cualquier cambio que se crea necesario tanto en el código como en los tests. La única condición es que los tests pasen OK.
 *
 * TODO: Implementar Trapecio/Rectangulo, agregar otro idioma a reporting.
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using static CodingChallenge.Data.Classes.FormaGeometrica;
[assembly: NeutralResourcesLanguageAttribute("en-US")]
namespace CodingChallenge.Data.Classes
{

    public class FormaGeometrica
    {
        #region Formas
        
        public enum Formas
        {
            Cuadrado = 1,
            Circulo = 2,
            TrianguloEquilatero = 3,
            Trapecio =4,
            Rectangulo=5,
        }
        #endregion

        #region Idiomas
        public enum Idiomas
        {
            Castellano = 1,

            Ingles = 2
        }
        #endregion


        private readonly decimal _lado;
        private readonly decimal _alto;
        private readonly decimal _baseinferior;
        private readonly decimal _altoinferior;
        public int Tipo { get; set; }

        public FormaGeometrica(int tipo, decimal ancho, decimal alto =0, decimal anchoInferior = 0, decimal altoinferior = 0)
        {
            Tipo = tipo;
            _lado = ancho;
            _alto = alto;
            _baseinferior = anchoInferior;
            _altoinferior = altoinferior;

    }

    public class Resultado { 
        
            public int tipo { get; set; }

            public int ResultadoNumero { get; set; }

            public decimal ResultadoArea { get; set; }

            public decimal ResultadoPerimetro { get; set; }
    }

    public static string Imprimir(List<FormaGeometrica> formas, int idioma)
        {
            var sb = new StringBuilder();
            if (idioma == (int)Idiomas.Castellano)
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-AR");
            else
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            if (!formas.Any())   {
                sb.Append(string.Format("<h1>{0}</h1>", msg.ListaVacia));
            }
            else
            {
                // Hay por lo menos una forma
                // HEADER             
                sb.Append(string.Format("<h1>{0}</h1>", msg.ReporteFormas));
              
                List<Resultado> resumen = new List<Resultado>();

                foreach (var item in Enum.GetValues(typeof(Formas)))
                {
                   
                    List<FormaGeometrica> fg = new List<FormaGeometrica>();
                    fg = formas.Where(n => n.Tipo == (int)item).ToList();
                    Resultado x = new Resultado();
                    foreach (FormaGeometrica y in fg)
                    {
                        x.tipo = y.Tipo;
                        x.ResultadoNumero++;
                        x.ResultadoArea += y.CalcularArea();
                        x.ResultadoPerimetro  +=y.CalcularPerimetro(); 
                    
                    }
                    resumen.Add(x);
                    sb.Append(ObtenerLinea(x.ResultadoNumero, x.ResultadoArea, x.ResultadoPerimetro, x.tipo));
                }


                //// FOOTER
                sb.Append(string.Format("{0}:<br/>", msg.TOTAL));
                sb.Append(string.Format("{0} {1} ",resumen.Sum(o => o.ResultadoNumero), msg.formas));
                sb.Append(msg.Perimetro + " " + (resumen.Sum(o => o.ResultadoPerimetro)).ToString("#.##") + " ");
                sb.Append("Area " + (resumen.Sum(o => o.ResultadoArea)).ToString("#.##"));
 }

            return sb.ToString();
        }

        private static string ObtenerLinea(int cantidad, decimal area, decimal perimetro, int tipo)
        {
            if (cantidad > 0)
            {
                 return string.Format("{0} {1} | {2} {3} | {4} {5} <br/>", cantidad, TraducirForma(tipo, cantidad), msg.area, Math.Round(area, 2), msg.Perimetro, Math.Round(perimetro, 2));
            }

            return string.Empty;
        }

        private static string TraducirForma(int tipo, int cantidad)
        {
            string traduccion =string.Empty;
            var forma = (Formas)tipo;
            switch (tipo)
            {
                case (int)Formas.Cuadrado:
                    traduccion = (cantidad == 1) ? msg.Cuadrado : msg.Cuadrados;
                    break;              
                case (int)Formas.Circulo:
                    traduccion = (cantidad == 1) ? msg.Círculo : msg.Círculos;
                    break;
                case (int)Formas.TrianguloEquilatero:
                        traduccion = (cantidad == 1) ? msg.Triangulo: msg.Triangulos;
                    break;
                case (int)Formas.Trapecio:
                    traduccion = (cantidad == 1) ? msg.Trapecio : msg.Trapecios;
                    break;
                case (int)Formas.Rectangulo:
                    traduccion = (cantidad == 1) ? msg.Rectangulo : msg.Rectangulos;
                    break;
            }

            return traduccion ;
        }

        public decimal CalcularArea()
        {
            switch (Tipo)
            {
                case (int)Formas.Cuadrado: return _lado * _lado;
                case (int)Formas.Circulo: return (decimal)Math.PI * (_lado / 2) * (_lado / 2);
                case (int)Formas.TrianguloEquilatero: return ((decimal)Math.Sqrt(3) / 4) * _lado * _lado;
                case (int)Formas.Rectangulo:  return  (_lado * _alto);
                case (int)Formas.Trapecio: return  ( _alto * ((_baseinferior + _lado)/2)); //  private readonly decimal _
                default:
                    throw new ArgumentOutOfRangeException(msg.FormaDesconocida);
            }
        }

        public decimal CalcularPerimetro()
        {
            switch (Tipo)
            {
                case (int)Formas.Cuadrado: return _lado * 4;
                case (int)Formas.Circulo: return (decimal)Math.PI * _lado;
                case (int)Formas.TrianguloEquilatero: return _lado * 3;
                case (int)Formas.Rectangulo: return (_lado * _alto); //  private readonly decimal _
                case (int)Formas.Trapecio: return _lado + _baseinferior + _alto + _altoinferior;
                default:
                    throw new ArgumentOutOfRangeException(msg.FormaDesconocida);
            }
        }
    }
}
