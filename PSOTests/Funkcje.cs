using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSOTests
{
     public class Funkcje
    {

        private static double randomPoint(double a, double b)
        {
            System.Random r = new System.Random();

            return a + r.NextDouble() * (b - a);
        }

        public static class DeJong1
        {
            public const double minX = -5.12;   // problem-dependent
            public const double maxX = 5.12;
            public const double exitError = 0.05;
            public const double min = 0;


            public static void setFitness(Particle ind)
            {
                foreach (double position in ind.position)
                {
                    ind.fitnessValue += Math.Pow(position, 2);
                }
            }
        }

        public static class Schwefel
        {
            public const double minX = -500;
            public const double maxX = 500;
            public const double exitError = 0.05;
            public const double min = 0;


            public static void setFitness(Particle ind)                               //Particle=Individual
            {
                double y = 0.0;
                double n = ind.position.Length;
                foreach (double randomPoint in ind.position)

                    //f(x)= n E i=o  (x^2_i) 

                    y += (randomPoint * Math.Sin(Math.Sqrt(Math.Abs(randomPoint))));
                ind.fitnessValue = 418.9829 * n - (y);

            }
        }


        public static class Rosenbrock
        {
            public const double minX = -2.048;
            public const double maxX = 2.048;
            public const double exitError = 1;
            public const double min = 0;

            public static void setFitness(Particle ind)
            {

                //f(x)=n-1 E i=0 [100(x_i+1 - x^2_i)^2+(x^2_i - 1)^2]

                for (int j = 0; j < ind.position.Length - 1; ++j)
                {
                    
                    ind.fitnessValue += 100 * Math.Pow(ind.position[j + 1] - Math.Pow(ind.position[j], 2), 2) + Math.Pow(1 - ind.position[j], 2);
                }
                 }
        }

        public static class Rastrigin
        {
            public const double minX = -5.12;
            public const double maxX = 5.12;
            public const double exitError = 1;
            public const double min = 0;


            public static void setFitness(Particle ind)
            {

                //f(x)=10n+ n E i+1 [x^2_i - 10cos(2*pi*x_i)]

                foreach (double parametr in ind.position)
                    ind.fitnessValue += (Math.Pow(parametr, 2) - 10 * Math.Cos(2 * Math.PI * parametr));

                ind.fitnessValue += 10 * ind.position.Length;
            }
        }

        public static class FunctionName
        {
            public static Type type { get; set; }
            public enum Type { DeJong1, Rosenbrock, Rastrigin, Schwefel };
        }

    }
}
