using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Text;
using PSOTests.Funkcje;

namespace PSOTests
{
    public partial class Form1 : Form
    {
        PSO optymalizacja;
        private static ILScene scena;
        private bool _threadPaused = false;
        private bool thesame = false;
        private Thread animace;
        private ILArray<float> data;
        private double maxX { get; set; }
        private double minX { get; set; }
        private short ileCzastek;
        private short maxEpochs;
        private string funkcja;
        private int nrIteracji = 50;
        private Populacja population { get; set; }
        private ILSurface surface;
        private Populacja populationestore { get; set; }
        private int i = 0;
        private int j = 0;
        private float[] avfitness = new float[1];
        private float[] avbfitness = new float[1];
        private float[] avvelocity = new float[1];
        private int dim = 2;
        private int modelpopulationSize = 1000;
        private int testnumber = 100;
        private double error = 0.005;
        private int PopulationSize = 20;
        private int numberIterations = 50;
        private double inertiaw = 0.75;
        private Populacja modelpopulation;
        private System.Windows.Forms.Button thesamepopulation;


        private Dictionary<string, Tuple<double, double>> dziedzinyFunkcji = new Dictionary<string, Tuple<double, double>>();
        //;

        public Form1()
        {
            InitializeComponent();
            dziedzinyFunkcji.Add(" DeJong1 ", new Tuple<double, double>(-5.12, 5.12));
            dziedzinyFunkcji.Add(" Rosenbrock)", new Tuple<double, double>(-2.048, 2.048));
            dziedzinyFunkcji.Add("Rastrigin", new Tuple<double, double>(-5.12, 5.12));
            dziedzinyFunkcji.Add("Schwefel)", new Tuple<double, double>(-500, 500));
    }

        private void Form1_Load(object sender, EventArgs e)
        {
            MaxEpochUpDown.Value = MaxEpochUpDown.Minimum;
            ParticleQuantityUpDown.Value=ParticleQuantityUpDown.Minimum;

        }

        /*
        static double Error(double x)
        {
            double trueMin = 0, y = 0;
            switch (funkcja)
            {

                case "2*x^2+x-2":
                    trueMin = -2.125; // @x=-0.25
                    y = 2 * Math.Pow(x, 2) + x - 2;
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
                case "sin(2 x)+ln(x^2)":
                    //BRAK GLOBALNEGO MINIMUM !!!
                    trueMin = double.MaxValue;
                    y = Math.Sin(2 * x) + Math.Log(Math.Pow(x, 2));
                    break;
                case "|(log_{10}(x^2)|":
                    //BRAK GLOBALNEGO MINIMUM !!!
                    trueMin = double.MaxValue;
                    y = Math.Abs(Math.Log10(Math.Pow(x, 2)));
                    break;


            }
            return (y - trueMin) * (y - trueMin);
        }
        */
        /*
        static double Solve(int numParticles, double minX, double maxX, int maxEpochs, double exitError)
        {
            
            Random rnd = new Random(0);

            Particle[] swarm = new Particle[numParticles];
            double bestGlobalPosition= double.MaxValue; ; // Najlepsza pozycja znaleziona przez którąkolwiek cząstkę roju
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
                    bestGlobalPosition=swarm[i].pozycja;
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
                        currP.najlPozycja=newPosition;
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
                        
                        for (int j = 0; j < currP.position.Length; ++j)
                            currP.position[j] = (maxX - minX) * rnd.NextDouble() + minX;
                        currP.error = Error(currP.pozycja);
                        currP.position.CopyTo(currP.bestPosition, 0);
                        currP.bestError = currP.error;

                        if (currP.error < bestGlobalError) //przypadkowe trafienie w globalne minimum?
                        {
                            bestGlobalError = currP.error;
                            bestGlobalPosition=currP.pozycja;
                        }
                    }

                } 
                ++epoch;
            } // while

            // Rój po przejściu wszystkich iteracji
            for (int i = 0; i < swarm.Length; ++i)
                Console.WriteLine(swarm[i].ToString());

            double result;
            result=bestGlobalPosition;
            return result;
            // Solve
        }


        private void ParticleSwarm()
        {
            Console.WriteLine("\nSetting problem dimension to " + dim);
            Console.WriteLine("Setting numParticles = " + numParticles);
            Console.WriteLine("Setting maxEpochs = " + maxEpochs);
            Console.WriteLine("Setting early exit error = " + exitError.ToString("F4"));
            Console.WriteLine("Setting minX, maxX = " + minX.ToString("F1") + " " + maxX.ToString("F1"));

            double bestPosition = Solve(dim, numParticles, minX, maxX, maxEpochs, exitError);
            double bestError = Error(bestPosition);

            Console.WriteLine("Best position/solution found:");
            
                Console.Write("x" + " = ");
                Console.WriteLine(bestPosition.ToString("F6") + " ");
            
            Console.WriteLine("");
            Console.Write("Final best error = ");
            Console.WriteLine(bestError.ToString("F5"));
        }
        */

        public void ShowParticleMove(object obj)
        {
            List<Populacja> list = (List<Populacja>)obj;

            int i = 0;
            foreach (Populacja item in list)
            {

                surface = new ILSurface(data);
                surface.Fill.Markable = false;
                surface.Wireframe.Markable = false;
                this.ShowParticleOnGraph(item, 8);
                //this.ShowGraphChart(item);
                //this.ShowCharts(item);
                //this.RefreshModel();

                if (i < nrIteracji)
                {
                    int k = i + 1;
                    for (int j = 0; j < item.NajlepszaPozycja.Length; ++j)
                    {
                        Invoke(new Action(() => richTextBox1.AppendText("Iteracja: " + k.ToString() + "  x" + "[" + j + "]" + "   " + item.NajlepszaPozycja[j] + "\n")));
                    }
                    Invoke(new Action(() => richTextBox1.AppendText("    Minimum: " + item.NajlepszaFitness + "\n")));
                    Invoke(new Action(() => richTextBox1.ScrollToCaret()));

                }
                // Thread.Sleep(1000);


                if (_threadPaused)
                    wh.WaitOne();
                ++i;
            }
            //Invoke(new Action(() => playPausePictureBox.Visible = false));

        }

        private void ShowParticleOnGraph(Populacja tmp, int size = 10)
        {
            double best = tmp.population.Min(x => x.fitnessValue);
            foreach (Particle item in tmp.population)
            {
                ILArray<float> coords = new float[3];
                coords[0] = (float)item.position[0];
                coords[1] = (float)item.position[1];
                coords[2] = (float)item.fitnessValue;// +1000;
                ILPoints bod = surface.Add(Shapes.Point);
                //surface.Colormap = Colormaps.Hot;

                if (item.fitnessValue == best)
                {
                    bod.Color = Color.Red;
                    bod.Size = 10;
                }
                else
                {
                    bod.Color = Color.Black;
                    bod.Size = size;
                }

                bod.Positions.Update(coords);
                surface.Add(bod);
            }
        }

        private void GenerateGraph()
        {
            Invoke(new Action(() => panel1.Visible = true));



            modelpopulation = new Populacja(modelpopulationSize, FunctionName.type);
            modelpopulation.SetRangeOfPopulation();
            modelpopulation.GenerateGraphPopulation();
            modelpopulation.ObliczPopulFitness();

            population = new Populacja(PopulationSize, dim, FunctionName.type);
            population.SetRangeOfPopulation();
            population.GeneratePopulation(dim);
            population.ObliczPopulFitness();

            float[] newData = new float[modelpopulation.population.Count * 3];
            Parallel.For(0, modelpopulation.population.Count, i =>
            {
                newData[i] = (float)modelpopulation.population[i].fitnessValue;  // Z
                newData[i + modelpopulation.population.Count] = (float)modelpopulation.population[i].position[0]; // X
                newData[i + 2 * modelpopulation.population.Count] = (float)modelpopulation.population[i].position[1]; // Y


            });

            int size = (int)Math.Sqrt(modelpopulation.population.Count);
            data = ILNumerics.ILMath.array(newData, size, size, 3);
            surface = new ILSurface(data);
            surface.Fill.Markable = false;
            surface.Wireframe.Markable = false;
            //surface.Colormap = Colormaps.Copper;
            ILColorbar color = new ILColorbar() { Location = new PointF(.96f, 0.1f) };
            surface.Children.Add(color);
            this.ShowParticleOnGraph(population);
            //this.ShowGraphChart(population);
            //this.ShowCharts(population);
            //this.RefreshModel();

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ParticleQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //string f = FunctionSelectionCombo.SelectedItem.ToString();
            if (!String.IsNullOrEmpty(funkcja)&&!funkcja.Equals("Proszę wybrać funkcję do optymalizacji"))
            {
                ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
                maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
                optymalizacja = new PSO(dziedzinyFunkcji[funkcja], ileCzastek, maxEpochs, funkcja);
                MessageBox.Show(string.Format("Znalezione minimum to {0} z błędem {1}", PSO.PSOSolution().Item1, PSO.PSOSolution().Item2),"Rezultat optymalizacji",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else MessageBox.Show("Nie wybrano funkcji do optymalizacji","BŁĄD!",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void MaxEpochUpDown_ValueChanged(object sender, EventArgs e)
        {
            maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
        }

        private void FunctionSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            funkcja = FunctionSelectionCombo.SelectedItem.ToString();
            FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), FunctionSelectionCombo.SelectedItem.ToString());
            //GenModel();
            thesamepopulation.Enabled = false;
        }
    }
}
