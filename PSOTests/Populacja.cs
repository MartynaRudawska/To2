using System;
using System.Collections.Generic;
//using static PSOTests.Funkcje;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using static PSOTests.Funkcje;

namespace PSOTests
{

    public class Populacja
    {
        public static int numParticles { get; private set; }
        public static int maxEpochs = 1000;
        public static double exitError { get; private set; }
        public static double minX; // problem-dependent
        public static double maxX;
        public static double min { get; private set; }
        public static string PostacFunkcji;
        Particle[] roj;
        private static double najlepszaPozycja;


        public Funkcje.FunctionName.Type type { get; private set; }
        public int populationSize = 40000;

        public List<Particle> population = new List<Particle>();

        #region konstruktory
        public Populacja(FunctionName.Type type)
        {
            this.type = type;
        }
        public Populacja(int populationSize, int dim, FunctionName.Type type)
        {
            this.populationSize = populationSize;
            this.type = type;
            this.dim = dim;
        }

        
        /// <summary>
        /// Konstruktor algorytmu PSO
        /// </summary>
        /// <param name="dziedzina">Dziedzina funkcji do optymalizacji</param>
        /// <param name="ilCzastek">Ilość cząstek roju</param>
        /// <param name="maxIteracji">maksymalna ilość Iteracji</param>
        /// <param name="Funkcja">Tekstowa postać funkcji do optymalizacji</param>
        public Populacja(Tuple<double, double> dziedzina, int populationSize, int ilCzastek, int maxIteracji, string Funkcja, int dim, FunctionName.Type type)
        {
            minX = dziedzina.Item1;
            maxX = dziedzina.Item2;
            numParticles = ilCzastek;
            maxEpochs = maxIteracji;
            PostacFunkcji = Funkcja;
            roj = new Particle[ilCzastek];
            this.populationSize = populationSize;


        }
              
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelpopulationSize"></param>
        /// <param name="type"></param>
        public Populacja(int modelpopulationSize, FunctionName.Type type)
        {
            this.modelpopulationSize = modelpopulationSize;
            this.type = type;
        }

        
        private int axisTicks { get; set; }
        private double entireInterval { get; set; }
        private double interval { get; set; }
        public int dim;//{ get; set; }
        public double[] NajlepszaPozycja; //= new double[dim];
        public double NajlepszaFitness = double.MaxValue;
        private int modelpopulationSize;

        #endregion
        private static double randomPoint(double a, double b)
        {
            System.Random r = new System.Random();

            return a + r.NextDouble() * (b - a);
        }

        private static double Error(FunctionName.Type PostacFunkcji, double x)
        {
            // double trueMin = 0, y = 0.0, xp, xk;
            switch (PostacFunkcji)
            {
                
                case FunctionName.Type.DeJong1:
                    DeJong1.setFitness([-5.12][5.12]);
                    break;

                case FunctionName.Type.Schwefel:
                    Schwefel.setFitness(population[j]);
                    break;

                case FunctionName.Type.Rastrigin:
                    Rastrigin.setFitness(population[j]);
                    break;

                case FunctionName.Type.Rosenbrock:
                    Rosenbrock.setFitness(population[j]);
                    break;
                    

            }
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
            Populacja item = new Populacja();
            item.maxX = maxX;
            item.minX = minX;
            item.populationSize = populationSize;
            item.type = type;
            item.dim = dim;
            return item;
        }



        public void GenerateGraphPopulation()
        {
            Particle ind;
            Random r = new Random();
            double param1;
            double param2;
            entireInterval = this.maxX - this.minX;
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


        internal void ObliczPopulFitness(FunctionName.Type type)
        {
            for (int j = 0; j < population.Count; j++)
            {
                switch (type)
                {

                    case FunctionName.Type.DeJong1:
                        DeJong1.setFitness(population[j]);
                        break;

                    case FunctionName.Type.Schwefel:
                        Schwefel.setFitness(population[j]);
                        break;

                    case FunctionName.Type.Rastrigin:
                        Rastrigin.setFitness(population[j]);
                        break;

                    case FunctionName.Type.Rosenbrock:
                        Rosenbrock.setFitness(population[j]);
                        break;


                }
            }
        }
        internal void SetRangeOfPopulation(FunctionName.Type type, double error2)
        {
            switch (type)
            {

                case FunctionName.Type.DeJong1:
                    minX = DeJong1.minX;
                    maxX = DeJong1.maxX;
                    exitError = error2;
                    min = DeJong1.min;
                    break;
                case FunctionName.Type.Schwefel:
                    minX = Schwefel.minX;
                    maxX = Schwefel.maxX;
                    exitError = error2;
                    min = Schwefel.min;
                    break;
                case FunctionName.Type.Rastrigin:
                    minX = Rastrigin.minX;
                    maxX = Rastrigin.maxX;
                    exitError = error2;
                    min = Rastrigin.min;
                    break;
                case FunctionName.Type.Rosenbrock:
                    minX = Rosenbrock.minX;
                    maxX = Rosenbrock.maxX;
                    exitError = error2;
                    min = Rosenbrock.min;
                    break;

            }
        }

        internal void SetRangeOfPopulation(FunctionName.Type type)
        {
            switch (type)
            {

                case FunctionName.Type.DeJong1:
                    minX = DeJong1.minX;
                    maxX = DeJong1.maxX;
                    exitError = DeJong1.exitError;
                    min = DeJong1.min;
                    break;

                case FunctionName.Type.Schwefel:
                    minX = Schwefel.minX;
                    maxX = Schwefel.maxX;
                    exitError = Schwefel.exitError;
                    min = Schwefel.min;
                    break;
                case FunctionName.Type.Rastrigin:
                    minX = Rastrigin.minX;
                    maxX = Rastrigin.maxX;
                    exitError = Rastrigin.exitError;
                    min = Rastrigin.min;
                    break;
                case FunctionName.Type.Rosenbrock:
                    minX = Rosenbrock.minX;
                    maxX = Rosenbrock.maxX;
                    exitError = Rosenbrock.exitError;
                    min = Rosenbrock.min;
                    break;

            }
        }
    }
}
