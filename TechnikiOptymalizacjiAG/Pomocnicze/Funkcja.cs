using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Math;

namespace TechnikiOptymalizacjiAG.Pomocnicze
{
    class Funkcja
    {
        double pt;
        double y, x, yMin;
        double xp,xk;                                   //xp- x początkowy      xk- x końcowy


   
        ///<summary>
        ///random number from a to b
        ///</summary>
        ///<returns></returns>
        private static double randomPoint(double a, double b)
        {
            System.Random r = new System.Random();

            return a + r.NextDouble() * (b - a);
        }



        ///<summary>
        ///Banalny sprawdzający
        ///w przedziale [-3;3]
        ///</summary>
        ///<returns></returns>
        public Tuple<double, double> Banalny()
        {
            //y=2*x^2+x-2
            xp = -3;
            xk = 3;
            yMin=-2.125; // @x=-0.25
            y = Math.Pow(x, 2) * 2 + x - 2;   

            return new Tuple<double, double>(yMin, y);
        }

        /// <summary>
        /// przykład funkcji kwadratowej Sin Cos
        /// w przedziale [-1;1]
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> FKwadSinCos()
        {
            //f(x) = x^2+sin(3 cos(5 x))
            y = Math.Pow(randomPoint(xp, xk), 2) + Math.Sin(3 * Math.Cos(5 * randomPoint(xp, xk)));
            xp = -1;
            xk = 1;
            yMin = 0.14112;//@x=0
            return new Tuple<double, double>(yMin, y);
            // return yMin;
        }

        /// <summary>
        /// Przykład funkcji wielomianowej
        /// w przedziale [-5;5]
        /// </summary>
        /// <returns>Tuple of xMin, gMin</returns>
        public Tuple<double, double> FWielom()
        {
            //g(x)= x^4+x^3-7x^2-5x+10
            xp = -5;
            xk = 5;

            y = Math.Pow(randomPoint(xp, xk), 4) + Math.Pow(randomPoint(xp, xk), 3) - 4 * Math.Pow(randomPoint(xp, xk), 2) - 3 * randomPoint(xp, xk) + 5;
            return new Tuple<double, double>(yMin, y);
            //return gMin;
        }

        /// <summary>
        /// Przykład funkcji sin
        /// w przedziale [-3;3]
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> FSin()
        {
            //h(x) = sin(2 x)+log_{10}(x^2)
            xp = -3;
            xk = 3;

            y = Math.Sin(2 * randomPoint(xp, xk)) + Math.Log10(Math.Pow(randomPoint(xp, xk), 2));
            return new Tuple<double, double>(yMin, y);
        }

        /// <summary>
        /// Przykład funkcji Logarytmicznej
        /// w przedziale [0;2]
        /// </summary>
        /// <returns></returns>
        public Tuple<double, double> FLogarytmiczne()
        {
            //p(x) = abs(log_{10}(x^2))
            xp = 0;
            xk = 2;

            y = Math.Abs(Math.Log10(Math.Pow(randomPoint(xp, xk), 2))) + 0.03;
            return new Tuple<double, double>(yMin, y);
            //return yMin;
        }
    }
    class Przystosowanie
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public double fitness(int[] t, int n) // funkcja przystosowania
        {
            double s = 0;
            for (int i = 0; i < n; i++)
            {
                s += t[i];
            }
            return s;
        }
    }

    class Funkcje
    {
        //miejsce zerowe
        //f(x)=ax^2+bx+c gdzie a->nie 0      del=b^2-4ac

        double dell, a, b, c;
        double x0, x1, x2;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dell"></param>
        /// <returns></returns>
        public double Del(double dell)
        {
            dell = Math.Pow(b, 2) - 4 * a * c;
            return dell;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dell">'delta' - discriminant</param>
        /// <returns>Tuple of x1,x2</returns>
        public Tuple<double, double> MiejsceZer(double dell)
        {
            if (dell < 0)
            {
                return new Tuple<double, double>(-1, -1); // nie ma miejsca zerowego w R
            }
            else if (dell == 0)
            {
                x0 = -(b / (2 * a));
                return new Tuple<double, double>(x0, x0);
            }
            else
            {
                x1 = (-b + Math.Sqrt(dell)) / 2 * a;
                x2 = (-b - Math.Sqrt(dell)) / 2 * a;
                return new Tuple<double, double>(x1, x2);
            }
        }
    }
}
