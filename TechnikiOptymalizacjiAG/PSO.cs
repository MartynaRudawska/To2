using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnikiOptymalizacjiAG
{
    class PSO
    {
        public static int numParticles = 5;
        public static int maxEpochs = 1000;         //Max ilość Iteracji
        public static double exitError = 0.0;
        public static double minX; // problem-dependent
        public static double maxX;
        public static string PostacFunkcji;
        Particle[] roj;
        private static double najlepszaPozycja;

        /// <summary>
        /// Konstruktor algorytmu PSO
        /// </summary>
        /// <param name="dziedzina">Dziedzina funkcji do optymalizacji</param>
        /// <param name="ilCzastek">Ilość cząstek roju</param>
        /// <param name="maxIteracji">maksymalna ilość iteracji</param>
        /// <param name="Funkcja">Tekstowa postać funkcji do optymalizacji</param>
        public PSO(Tuple<double, double> dziedzina, int ilCzastek, int maxIteracji, string Funkcja)
        {
            minX = dziedzina.Item1;
            maxX = dziedzina.Item2;
            numParticles = ilCzastek;
            maxEpochs = maxIteracji;
            PostacFunkcji = Funkcja;
            roj = new Particle[ilCzastek];
        }

        private static double randomPoint(double a, double b)
        {
            System.Random r = new System.Random();

            return a + r.NextDouble() * (b - a);
        }

        private static double Error(double x)
        {
            double trueMin = 0, y = 0, xp,xk;
            switch (PostacFunkcji)
            {

                case "2*x^2+x-2":
                    trueMin = -2.125; // @x=-0.25
                    xp = -3;
                    xk = 3;
                    y = Math.Pow(randomPoint(xp, xk), 2) * 2 + randomPoint(xp, xk) - 2;
                    break;
                case "x^2+sin(3 cos(5x))":
                    trueMin = 0.14112;//@x=0  
                    //trueMin=-0.82205 @x =~= -0.41935
                    y = Math.Pow(x, 2) + Math.Sin(3 * Math.Cos(5 * x));
                    break;
                case "x^4+x^3-7x^2-5x+10":
                    trueMin = -5.4686;//@x=1.7153
                    y = Math.Pow(x, 4) + Math.Pow(x, 3) - 7 * Math.Pow(x, 2) - 5 * x + 10;
                    break;
                case "sin(2 x)+log_{10}(x^2)":
                    //BRAK GLOBALNEGO MINIMUM !!!
                    trueMin = double.MaxValue;
                    y = Math.Sin(2 * x) + Math.Log10(Math.Pow(x, 2));
                    break;
                case "|(log_{10}(x^2)|":
                    //BRAK GLOBALNEGO MINIMUM !!!
                    trueMin = double.MaxValue;
                    y = Math.Abs(Math.Log10(Math.Pow(x, 2)));
                    break;
            }
            return Math.Sqrt((y - trueMin) * (y - trueMin));
        }


        private static double Solve(int numParticles, double minX, double maxX, int maxEpochs, double exitError)
        {

            Random rnd = new Random(0);

            Particle[] swarm = new Particle[numParticles];
            double bestGlobalPosition = new double();//double.MaxValue; // Najlepsza pozycja znaleziona przez którąkolwiek cząstkę roju
            double bestGlobalError = double.MaxValue; // im mniejsza tym lepsza

            // Inicjalizacja roju
            for (int i = 0; i < swarm.Length; ++i)
            {
                double randomPosition;
                randomPosition = (maxX - minX) * rnd.NextDouble() + minX; // 

                double error = Error(randomPosition);
                double randomVelocity;

                double lo = minX * 0.1;
                double hi = maxX * 0.1;
                randomVelocity = (hi - lo) * rnd.NextDouble() + lo;

                swarm[i] = new Particle(randomPosition, error, randomVelocity, randomPosition, error);

                // Sprawdzenie, czy cząstka ma najlepszą pozycję / rozwiązanie
                if (swarm[i].error < bestGlobalError)
                {
                    bestGlobalError = swarm[i].error;
                    bestGlobalPosition = swarm[i].pozycja;
                }
            }

            // parametry literaturowe
            double w = 0.729;
            double c1 = 1.49445;
            double c2 = 1.49445;
            double r1, r2;
            double probDeath = 0.01;
            int epoch = 0;

            double newVelocity;
            double newPosition;
            double newError;

            // główna pętla PSO
            while (epoch < maxEpochs)
            {
                for (int i = 0; i < swarm.Length; ++i) // dla każdej cząstki
                {
                    Particle currP = swarm[i];


                    r1 = rnd.NextDouble();
                    r2 = rnd.NextDouble();

                    newVelocity = (w * currP.predkosc) +
                      (c1 * r1 * (currP.najlPozycja - currP.pozycja)) +
                      (c2 * r2 * (bestGlobalPosition - currP.pozycja));

                    currP.predkosc = newVelocity;

                    // new position

                    newPosition = currP.pozycja + newVelocity;
                    if (newPosition < minX)
                        newPosition = minX;
                    else if (newPosition > maxX)
                        newPosition = maxX;

                    currP.pozycja = newPosition;

                    newError = Error(newPosition);
                    currP.error = newError;

                    if (newError < currP.bestError)
                    {
                        currP.najlPozycja = newPosition;
                        currP.bestError = newError;
                    }

                    if (newError < bestGlobalError)
                    {
                        bestGlobalPosition = newPosition;
                        bestGlobalError = newError;
                    }

                    // Czy uśmiercić cząstkę?
                    double die = rnd.NextDouble();
                    if (die < probDeath)
                    {

                        //for (int j = 0; j < currP.position.Length; ++j)
                        currP.pozycja = (maxX - minX) * rnd.NextDouble() + minX;
                        currP.error = Error(currP.pozycja);
                        currP.najlPozycja = currP.pozycja;// currP.position.CopyTo(currP.bestPosition, 0);
                        currP.bestError = currP.error;

                        if (currP.error < bestGlobalError) //przypadkowe trafienie w globalne minimum?
                        {
                            bestGlobalError = currP.error;
                            bestGlobalPosition = currP.pozycja;
                        }
                    }

                }
                ++epoch;
            } // while

            // Rój po przejściu wszystkich Iteracji
            for (int i = 0; i < swarm.Length; ++i)
                Console.WriteLine(swarm[i].ToString());

            double result;
            result = bestGlobalPosition;
            return result;
            // Solve
        }
        public static Tuple<double, double> PSOSolution()
        {
            najlepszaPozycja = Solve(numParticles, minX,maxX, maxEpochs, exitError);
            return new Tuple<double, double>(najlepszaPozycja, Error(najlepszaPozycja));
        }

    }
}
