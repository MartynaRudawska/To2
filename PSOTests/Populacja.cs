using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
//using PSOTests.Funkcje;

namespace PSOTests
{

    public class Populacja
    {
#region publiczne
        public static int numParticles { get; private set; }
        public static int maxEpochs = 1000;
        public double exitError { get; private set; }
        public double minX { get; private set; } // problem-dependent
        public double maxX { get; private set; }
        public double min { get; private set; }
        public static string PostacFunkcji;
        public int populationSize { get; private set; }
        public int dim;//{ get; set; }
        public double[] NajlepszaPozycja; //= new double[dim];
        public double NajlepszaFitness = double.MaxValue;
        public Funkcje.FunctionName.Type type { get; private set; }
        //public int populationSize = 40000;
        public List<Particle> population = new List<Particle>();
#endregion
        Particle[] roj;
        #region prywatne
        private static double najlepszaPozycja;
        private int axisTicks { get; set; }
        private double entireInterval { get; set; }
        private double interval { get; set; }
        private int MaxEpochSize;
        #endregion


        #region konstruktory
        public Populacja(Funkcje.FunctionName.Type type)
        {
            this.type = type;
        }
        public Populacja(int populationSize, int dim, Funkcje.FunctionName.Type type)
        {
            this.populationSize = populationSize;
            this.type = type;
            this.dim = dim;
        }
        public Populacja(int populationSize, Funkcje.FunctionName.Type type)
        {
            this.populationSize = populationSize;
            this.type = type;
        }
        public Populacja() { }

        public Populacja(int populationSize)
        {
            this.populationSize = populationSize;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dziedzina"></param>
        /// <param name="populationSize"></param>
        /// <param name="ilCzastek"></param>
        /// <param name="maxIteracji"></param>
        /// <param name="Funkcja"></param>
        /// <param name="dim"></param>
        /// <param name="type"></param>
        public Populacja(Tuple<double, double> dziedzina, int populationSize, int ilCzastek, int maxIteracji, string Funkcja, int dim, Funkcje.FunctionName.Type type)
         {
            minX = dziedzina.Item1;
             maxX = dziedzina.Item2;
             numParticles = ilCzastek;
             maxEpochs = maxIteracji;
             PostacFunkcji = Funkcja;
             roj = new Particle[ilCzastek];
             this.populationSize = populationSize;


         }

        #endregion
        private static double randomPoint(double a, double b)
        {
            System.Random r = new Random();

            return a + r.NextDouble() * (b - a);
        }
        // Utwórz populacje
        public void GeneratePopulation(int Dim)
        {
            Particle ind;
            Random r = new Random();

            for (int i = 0; i < populationSize; i++)// tworzenie
            {
                double[] x = new double[Dim];
                double[] NajlepszaPozycja = new double[Dim];
                double bestFitness = double.MaxValue;
                double[] velocity = new double[Dim];
                double[] randomVelocity = new double[Dim];

                for (int j = 0; j < randomVelocity.Length; ++j)//losowa predkosc
                {
                    double lo = -1.0 * Math.Abs(maxX - minX);
                    double hi = Math.Abs(maxX - minX);
                    randomVelocity[j] = (hi - lo) * r.NextDouble() + lo;
                }
                for (int j = 0; j < Dim; ++j)
                {

                    x[j] = r.NextDouble() * (maxX - minX) + minX;
                    NajlepszaPozycja[j] = x[j];
                }
                velocity = randomVelocity;
                Particle tmp = new Particle(x);
                Particle.ObliczIndiwFitness(tmp);
                bestFitness = tmp.fitnessValue;
                ind = new Particle(x, bestFitness, velocity, NajlepszaPozycja);
                this.population.Add(ind);
            }
        }

        public static Populacja copy()
        {
            //Populacja item = new Populacja();
            
            //item.maxX = maxX;
            //item.minX = minX;
            //item.populationSize =this.populationSize;
            //item.type = type;
            //item.dim = dim;
            return new Populacja(new Tuple<double, double>(minX, maxX), populationSize, numParticles, maxEpochs, PostacFunkcji, type);//Tuple<double, double, int, int, string, int, int, type>((minX, maxX, numParticles, maxEpochs, PostacFunkcji, populationSize, dim, type);
        }



        public void GenerateGraphPopulation()
        {
            Particle ind;
            Random r = new Random();
            double param1;
            double param2;
            entireInterval = maxX - minX;
            axisTicks = (int)Math.Sqrt(this.populationSize) / 2;
            interval = entireInterval / (axisTicks * 2);

            for (int i = -axisTicks; i <= axisTicks; i++)
                for (int j = -axisTicks; j <= axisTicks; j++)
                {
                    param1 = (i + axisTicks) * (entireInterval / (2 * axisTicks)) + minX;
                    param2 = (j + axisTicks) * (entireInterval / (2 * axisTicks)) + minX;

                    ind = new Particle(param1, param2);
                    this.population.Add(ind);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        internal void ObliczPopulFitness(Funkcje.FunctionName.Type type)
        {
            for (int j = 0; j < population.Count; j++)
            {
                switch (type)
                {

                    case Funkcje.FunctionName.Type.DeJong1:
                        Funkcje.DeJong1.setFitness(population[j]);
                        break;

                    case Funkcje.FunctionName.Type.Schwefel:
                        Funkcje.Schwefel.setFitness(population[j]);
                        break;

                    case Funkcje.FunctionName.Type.Rastrigin:
                        Funkcje.Rastrigin.setFitness(population[j]);
                        break;

                    case Funkcje.FunctionName.Type.Rosenbrock:
                        Funkcje.Rosenbrock.setFitness(population[j]);
                        break;


                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="error2"></param>
        internal void SetRangeOfPopulation(Funkcje.FunctionName.Type type, double error2)
        {
            switch (type)
            {

                case Funkcje.FunctionName.Type.DeJong1:
                    minX = Funkcje.DeJong1.minX;
                    maxX = Funkcje.DeJong1.maxX;
                    exitError = error2;
                    min = Funkcje.DeJong1.min;
                    break;
                case Funkcje.FunctionName.Type.Schwefel:
                    minX = Funkcje.Schwefel.minX;
                    maxX = Funkcje.Schwefel.maxX;
                    exitError = error2;
                    min = Funkcje.Schwefel.min;
                    break;
                case Funkcje.FunctionName.Type.Rastrigin:
                    minX = Funkcje.Rastrigin.minX;
                    maxX = Funkcje.Rastrigin.maxX;
                    exitError = error2;
                    min = Funkcje.Rastrigin.min;
                    break;
                case Funkcje.FunctionName.Type.Rosenbrock:
                    minX = Funkcje.Rosenbrock.minX;
                    maxX = Funkcje.Rosenbrock.maxX;
                    exitError = error2;
                    min = Funkcje.Rosenbrock.min;
                    break;

            }
        }
/// <summary>
/// 
/// </summary>
/// <param name="type"></param>
        internal void SetRangeOfPopulation(Funkcje.FunctionName.Type type)
        {
            switch (type)
            {

                case Funkcje.FunctionName.Type.DeJong1:
                    minX = Funkcje.DeJong1.minX;
                    maxX = Funkcje.DeJong1.maxX;
                    exitError = Funkcje.DeJong1.exitError;
                    min = Funkcje.DeJong1.min;
                    break;

                case Funkcje.FunctionName.Type.Schwefel:
                    minX = Funkcje. Schwefel.minX;
                    maxX = Funkcje.Schwefel.maxX;
                    exitError = Funkcje.Schwefel.exitError;
                    min = Funkcje.Schwefel.min;
                    break;
                case Funkcje.FunctionName.Type.Rastrigin:
                    minX = Funkcje.Rastrigin.minX;
                    maxX = Funkcje.Rastrigin.maxX;
                    exitError = Funkcje.Rastrigin.exitError;
                    min = Funkcje.Rastrigin.min;
                    break;
                case Funkcje.FunctionName.Type.Rosenbrock:
                    minX = Funkcje.Rosenbrock.minX;
                    maxX = Funkcje.Rosenbrock.maxX;
                    exitError = Funkcje.Rosenbrock.exitError;
                    min = Funkcje.Rosenbrock.min;
                    break;

            }
        }
    }
}
