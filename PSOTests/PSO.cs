using System;
using System.Collections.Generic;
//using PSOTests.Funkcje;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using ILNumerics;


namespace PSOTests
{
    public class PSO
    {

        public double NajlepszaPozycja;
        public static double minX;
        public static double maxX;


        public PSO(int nrIteracji, double w, double c1, double c2, bool r1r2 = false, bool linearinertia = false)//
        {
            this.nrIteracji = nrIteracji;
            this.r1r2 = r1r2;
            this.w = w;
            this.linearinertia = linearinertia;
            this.c1 = c1;
            this.c2 = c2;
        }
        public int nrIteracji;
        public double w = 0.79; //inertia weight
        public double c1= 1.49445;
        public double c2= 1.49445;

        public double setw(int k)
        {
            w = 0.9 - k * (0.9 - 0.4) / nrIteracji;
            return w;
        }
        
        public bool r1r2;
        public bool linearinertia;

        public List<Populacja> PSOALG(Populacja p)
        {
            Random ran = new Random();
            double minBound = p.minX;
            double maxBound = p.maxX;
            double minV = -1.0 * maxBound;
            double maxV = maxBound;

            int iteracja = 0;
            double[] NajlepszaPozycja = new double[p.dim];
            double NajlepszaFitness = double.MaxValue;

            List<Populacja> roj = new List<Populacja>();
            roj.Add(p);

            foreach (Particle item in p.population)
            {
                if (item.fitnessValue < NajlepszaFitness)
                {
                    NajlepszaFitness = item.fitnessValue;
                    item.position.CopyTo(NajlepszaPozycja, 0);

                }
                p.NajlepszaPozycja = new double[p.dim];
                NajlepszaPozycja.CopyTo(p.NajlepszaPozycja, 0);
                p.NajlepszaFitness = NajlepszaFitness;

            }
        
            double r1, r2; // poznawczy i ogólne spułczynniki randoma

            // tworzenie roju ->swam

            while (iteracja < nrIteracji)
            {
                ++iteracja;
                Populacja currP2 = new Populacja(p.type);
                double[] newVelocity = new double[p.dim];
                double[] newPosition = new double[p.dim];
                double newFitness;

                foreach (Particle item in roj.Last().population)
                {

                    if (linearinertia)
                    {
                        this.w = setw(iteracja);
                    }
                    
                    // główna pętla PSO przejście przez wszystkie iteracje
                    for (int j = 0; j < item.velocity.Length; ++j)
                    {
                        if (r1r2)
                        {
                            r1 = ran.NextDouble();
                            r2 = ran.NextDouble();
                        }
                        else
                        {
                            r1 = 1;
                            r2 = 1;
                        }

                        newVelocity[j] = (w * item.velocity[j]) +
                        (c1 * r1 * (item.bestPosition[j] - item.position[j])) +
                         (c2 * r2 * (NajlepszaPozycja[j] - item.position[j]));

                        if (newVelocity[j] < minV)
                            newVelocity[j] = minV;
                        else if (newVelocity[j] > maxV)
                            newVelocity[j] = maxV;
                    }

                    newVelocity.CopyTo(item.velocity, 0);

                    for (int j = 0; j < item.position.Length; ++j)
                    {
                        newPosition[j] = item.position[j] + newVelocity[j];
                        if (newPosition[j] < minBound)
                            newPosition[j] = minBound;
                        else if (newPosition[j] > maxBound)
                            newPosition[j] = maxBound;
                    }
                    Particle best = new Particle(p.dim);
                    newPosition.CopyTo(item.position, 0);
                    newPosition.CopyTo(best.position, 0);
                    Particle.ObliczIndiwFitness(best);
                    newFitness = best.fitnessValue;
                    item.fitnessValue = newFitness;

                    if (newFitness < item.bestFitness)
                    {
                        newPosition.CopyTo(item.bestPosition, 0);
                        item.bestFitness = newFitness;
                    }

                    if (newFitness < NajlepszaFitness)
                    {
                        newPosition.CopyTo(NajlepszaPozycja, 0);
                        NajlepszaFitness = newFitness;

                    }
                    Particle tmp = new Particle(p.dim);


                    //kopia
                    Particle.copy(item, tmp);
                    currP2.population.Add(tmp);
                    roj.Last().NajlepszaPozycja = new double[p.dim];
                    NajlepszaPozycja.CopyTo(roj.Last().NajlepszaPozycja, 0);
                    roj.Last().NajlepszaFitness = NajlepszaFitness;

                }

                roj.Add(currP2);
            }
            return roj;
        }
    }

    /// ///////////

    /*
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

                // główna pętla PSO przejście przez wszystkie iteracje
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

                // Rój po przejściu wszystkich iteracji
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

        }*/
}
