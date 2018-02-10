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



/*

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
using PSO.Functions;



namespace PSO
{
    public partial class Main : Form
    {
        private static ILScene scena;
        private bool _threadPaused = false;
        private bool thesame = false;
        private Thread animace;
        private ILArray<float> data;

        private int PopulationSize = 20;
        private int numberIterations =50;
        private double inertiaw = 0.75;
        private double c1 = 1.49445; // cognitive/local weight
        private double c2 = 1.49445;
        private int dim = 2;
        private int modelpopulationSize = 1000;
        private int testnumber = 100;
        private double error = 0.005;

        private Form1 frm;
        private Population modelpopulation;

        private bool intOnly = false;
        private bool r1r2 = false;
        private bool linearinertia = false;
        private Thread model;

        private Population population { get; set; }

        private ILSurface surface;

        private Population populationestore { get; set; }

        private int i = 0;
        private int j = 0;
        private float[] avfitness = new float[1];
        private float[] avbfitness = new float[1];
        private float[] avvelocity = new float[1];





        private EventWaitHandle wh = new AutoResetEvent(false);

        public Main()
        {
            InitializeComponent();
            modelpopulationUpDown1.Value = modelpopulationSize;
            PopulationUpDown.Minimum = 5;
            PopulationUpDown.Value = PopulationSize;
            dimensionsUpDown.Value = dim;
            dimensionsUpDown.ValueChanged += NumericUpDown1_ValueChanged;
            testnumberUpDown.Value = testnumber;
            errornumericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            testnumberUpDown.ValueChanged += NumericUpDown1_ValueChanged;
            IterationsnumericUpDown1.Value = numberIterations;
            inertiawnumeric.ValueChanged += NumericUpDown1_ValueChanged;
            C1UpDown.ValueChanged += NumericUpDown1_ValueChanged;
            C2UpDown.ValueChanged += NumericUpDown1_ValueChanged;
            IterationsnumericUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            modelpopulationUpDown1.ValueChanged += NumericUpDown1_ValueChanged;
            PopulationUpDown.ValueChanged += NumericUpDown1_ValueChanged;
            model = new Thread(GenerateGraph);
                    
        }

        public void ShowParticleMove(object obj)
        {
            List<Population> list = (List<Population>)obj;

            int i = 0;
            foreach (Population item in list)
            {
               
                surface = new ILSurface(data);
                surface.Fill.Markable = false;
                surface.Wireframe.Markable = false;
                this.ShowParticleOnGraph(item, 8);
                this.ShowGraphChart(item);  
                this.ShowCharts(item);
                this.RefreshModel();

                if (i < numberIterations)
                {
                    int k = i + 1;
                    for (int j = 0; j < item.bestGlobalPosition.Length; ++j)
                    {
                        Invoke(new Action(() => richTextBox1.AppendText("Iteracja: " + k.ToString() + "  x" + "[" + j + "]" + "   " + item.bestGlobalPosition[j] + "\n")));
                    }
                    Invoke(new Action(() => richTextBox1.AppendText("    Minimum: " + item.bestGlobalFitness + "\n")));
                    Invoke(new Action(() => richTextBox1.ScrollToCaret()));

                }
                // Thread.Sleep(1000);
                

                if (_threadPaused)
                    wh.WaitOne();
                ++i;
            }
            Invoke(new Action(() => playPausePictureBox.Visible = false));

        }

        private void CheckBoxSetting_CheckedChanged(object sender, EventArgs e)
        {
            

            if (sender == randomr1r2CheckBox)
            {
                if (randomr1r2CheckBox.Checked)
                    r1r2 = true;
                else
                    r1r2 = false;

            }
            if (sender == linearInertiaCheckBox)
            {
                if (linearInertiaCheckBox.Checked)
                    linearinertia = true;
                else
                    linearinertia = false;

            }

        }



        private void functionButtonSet(bool value)
        {
            Invoke(new Action(() =>
            {
                _threadPaused = value;
                playPausePictureBox.Visible = !value;

                PSObutton.Enabled = value;
                savepopulation.Enabled = value;
                if (thesame == false)
                {
                    thesamepopulation.Enabled = false;
                }
                else
                {
                    thesamepopulation.Enabled = true;
                }
               

            }
                ));
        }


      /*  private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripDropDown item = (ToolStripDropDown)sender;

            MessageBox.Show(item.Text.ToString());

        }*/

    /*
private void GenModel()
{
    this.functionButtonSet(true);
    thesame = false;


    if (model.IsAlive)
        model.Abort();

    model = new Thread(GenerateGraph);
    model.IsBackground = true;
    model.Start();
    i = 0;
    j = 0;
    avbfitness = new float[1];
    avfitness = new float[1];
    avvelocity = new float[1];



}

private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
{
    if (sender == modelpopulationUpDown1)
    {
        modelpopulationSize = (int)modelpopulationUpDown1.Value;
        GenModel();
    }
    if (sender == PopulationUpDown)
    {
        PopulationSize = (int)PopulationUpDown.Value;
    }
    if (sender == IterationsnumericUpDown1)
    {
        numberIterations = (int)IterationsnumericUpDown1.Value;
    }
    if (sender == C1UpDown)
    {
        c1 = (double)C1UpDown.Value;
    }
    if (sender == C2UpDown)
    {
        c2 = (double)C2UpDown.Value;
    }
    if (sender == dimensionsUpDown)
    {
        dim = (int)dimensionsUpDown.Value;
    }
    if (sender == testnumberUpDown)
    {
        testnumber = (int)testnumberUpDown.Value;
    }
    if (sender == errornumericUpDown1)
    {
        error = (double)errornumericUpDown1.Value;
    }
    else inertiaw = (double)inertiawnumeric.Value;
}

private void playPauseClick(object sender, EventArgs e)
{
    if (animace != null)
    {
        if (animace.IsAlive)
        {
            if (_threadPaused)
            {
                _threadPaused = false;
                playPausePictureBox.Image = Properties.Resources._1385568086_pause;
                wh.Set();
            }
            else
            {
                _threadPaused = true;
                playPausePictureBox.Image = Properties.Resources._1385568066_play;
            }
        }
    }
}

private void RefreshModel()
{
    Invoke(new Action(() =>
    {
        scena = new ILScene { new ILPlotCube(twoDMode: false) { surface } };

        if (dim == 2)
        {
            scena.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1, 1, 1), 0.3f);
        }
        scena.Screen.First<ILLabel>().Visible = false;



        ilPanel1.Scene = scena;
        ilPanel1.Refresh();
        ilPanel2.Refresh();
        ilPanel4.Refresh();

        //  ilPanel2.Refresh();
    }));
}

private void ShowParticleOnGraph(Population tmp, int size = 10)
{
    double best = tmp.population.Min(x => x.fitnessValue);
    foreach (Individual item in tmp.population)
    {
        ILArray<float> coords = new float[3];
        coords[0] = (float)item.parameters[0];
        coords[1] = (float)item.parameters[1];
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



private void ShowGraphChart(Population tmp)
{
    var scene = new ILScene();
    scene.Screen.First<ILLabel>().Visible = false;
    int i = 0;
    ILArray<float> A; float[] a = new float[tmp.population.Count];
    foreach (Individual item in tmp.population)
    {

        a[i] = (float)(item.fitnessValue);
        i++;
    }
    A = a;
    var pplot = scene.Add(new ILPlotCube()
    {

        Childs = { new ILLinePlot(A.T,markerStyle: MarkerStyle.Dot,lineColor:Color.Gray)

                    },
        Axes =
                {
                    XAxis =
                    {
                        Label = { Text = "numer cząsteczki" },
                        //Ticks = { Visible = false }
                    },
                    // Setting the default size for tick labels affects the width of the 
                    // cube. This would 'disalign' both plot cubes: 
                    YAxis =
                    {
                        Label = { Text = "wartość cząsteczki" },
                        // Ticks = { DefaultTickLabelSize = new SizeF(79, 20) }
                    }
                }
        ,
    });
    ilPanel2.Scene = scene;
}
private void ShowCharts(Population tmp)
{
    float avf = 0;
    float avb = 0;
    float avv = 0;

    var scene = new ILScene();
    scene.Screen.First<ILLabel>().Visible = false;

    foreach (Individual item in tmp.population)
    {
        avf += (float)item.fitnessValue;
        avb += (float)item.bestFitness;
        for (int k = 0; k < item.velocity.Length; ++k)
        {
            avv += (float)Math.Abs(item.velocity[k]);
        }
        //MessageBox.Show(avv.ToString());
    }
    avfitness[j] = avf / tmp.population.Count;
    avbfitness[j] = avb / tmp.population.Count;
    avvelocity[i] = avv / tmp.population.Count;

    using (ILScope.Enter())
    {
        ILArray<float> AVFITNESS = avfitness;
        ILArray<float> AVBFITNESS = avbfitness;
        ILArray<float> AVVELOCITY = avvelocity;

        var fplot = scene.Add(new ILPlotCube()
        {
            ScreenRect = new RectangleF(0, 0, 1, 0.5f),
            Childs = { new ILLinePlot(AVFITNESS.T,lineColor:Color.Black),

                    },
            Axes =
                    {
                        XAxis =
                        {
                            Label = { Text = "numer iteracji" },
                            //Ticks = { Visible = false }
                        },
                        // Setting the default size for tick labels affects the width of the 
                        // cube. This would 'disalign' both plot cubes: 
                        YAxis =
                        {
                            Label = { Text = "średnia wartość cząsteczek" },
                            // Ticks = { DefaultTickLabelSize = new SizeF(79, 20) }
                        }
                    }
            ,
        });

        var f2 = fplot.AddDataGroup();
        f2.Add(new ILLinePlot(AVBFITNESS.T, lineColor: Color.Red));
        var axisY2 = fplot.Axes.Add(new ILAxis(f2)
        {
            AxisName = AxisNames.YAxis,
            Position = new Vector3(1, 0, 0),
            ColorOverride = Color.Red,
            Label = { Text = "średnia najlepsza wartość cząsteczek", Color = Color.Red },
            Ticks = { DefaultLabel = { Color = Color.Red } }
        });

        var vplot = scene.Add(new ILPlotCube()
        {
            ScreenRect = new RectangleF(0f, 0.5f, 1, 0.5f),
            Childs = { new ILLinePlot(AVVELOCITY.T,lineColor:Color.Black),

                    },
            Axes =
                        {
                            XAxis =
                            {
                                Label = { Text = "numer iteracji" },
                                //Ticks = { Visible = false }
                            },
                            // Setting the default size for tick labels affects the width of the 
                            // cube. This would 'disalign' both plot cubes: 
                            YAxis =
                            {
                                Label = { Text = "średnia prędkość cząsteczek" },
                                // Ticks = { DefaultTickLabelSize = new SizeF(79, 20) }
                            }
                        }
            ,
        });
        var v2 = vplot.AddDataGroup();
        v2.Add(new ILLinePlot(AVVELOCITY.T, lineColor: Color.Red));
        v2.ScaleModes.YAxisScale = AxisScale.Logarithmic;
        var axisY = vplot.Axes.Add(new ILAxis(v2)
        {
            AxisName = AxisNames.YAxis,
            Position = new Vector3(1, 0, 0),
            ColorOverride = Color.OrangeRed,
            Label = { Text = "średnia prędkość cząsteczek (log)", Color = Color.Red },
            Ticks = { DefaultLabel = { Color = Color.Red } }
        });


        ilPanel4.Scene = scene;

        Array.Resize(ref avfitness, avfitness.Length + 1);
        Array.Resize(ref avbfitness, avbfitness.Length + 1);
        Array.Resize(ref avvelocity, avvelocity.Length + 1);
        j++;
        i++;
        // fplot.DataScreenRect = new RectangleF(0.12f, 0.05f, 0.84f, 0.70f);
        // vplot.DataScreenRect = new RectangleF(0.12f, 0.05f, 0.84f, 0.50f);
    }
}




private void GenerateGraph()
{
    Invoke(new Action(() => ilPanel1.Visible = true));



    modelpopulation = new Population(modelpopulationSize, FunctionName.type);
    modelpopulation.SetRangeOfPopulation();
    modelpopulation.GenerateGraphPopulation();
    modelpopulation.CalculatePopulationFitness();

    population = new Population(PopulationSize, dim, FunctionName.type);
    population.SetRangeOfPopulation();
    population.GeneratePopulation(dim);
    population.CalculatePopulationFitness();

    float[] newData = new float[modelpopulation.population.Count * 3];
    Parallel.For(0, modelpopulation.population.Count, i =>
    {
        newData[i] = (float)modelpopulation.population[i].fitnessValue;  // Z
        newData[i + modelpopulation.population.Count] = (float)modelpopulation.population[i].parameters[0]; // X
        newData[i + 2 * modelpopulation.population.Count] = (float)modelpopulation.population[i].parameters[1]; // Y


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
    this.ShowGraphChart(population);
    this.ShowCharts(population);
    this.RefreshModel();
    Invoke(new Action(() => nameFunctionLabel.Text = population.type.ToString()));
    Invoke(new Action(() => nameFunctionLabel.Visible = true));
    // Invoke(new Action(() => nameFunctionLabel.Visible = true));
    //Invoke(new Action(() => ilPanel1.Visible = true));
}




private void PSObutton_Click_1(object sender, EventArgs e)
{
    List<Population> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);//numberIterations, inertiaw
    this.functionButtonSet(false);

    animace = new Thread(ShowParticleMove);
    animace.IsBackground = true;
    animace.Start(tmp);
    if (thesame == true)
    {
        thesamepopulation.Enabled = true;
    }

}



private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Schwefel");
    GenModel();
    thesamepopulation.Enabled = false;
}

private void dejong1ToolStripMenuItem_Click(object sender, EventArgs e)
{

    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "DeJong1");
    GenModel();
    thesamepopulation.Enabled = false;
}

private void deJong3ToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "DeJong3");
    GenModel();
    thesamepopulation.Enabled = false;
}

private void deJong4ToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "DeJong4");
    GenModel();
    thesamepopulation.Enabled = false;
}

private void dolinaRosenbrockaToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Rosenbrock");
    GenModel();
    thesamepopulation.Enabled = false;
}

private void funkcjaRastriginaToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Rastrigin");
    GenModel();
    thesamepopulation.Enabled = false;
}


private void ackley2ToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Ackley");
    GenModel();
    thesamepopulation.Enabled = false;
}
private void michalewiczToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Michalewicz");
    GenModel();
    thesamepopulation.Enabled = false;
}
private void grToolStripMenuItem_Click(object sender, EventArgs e)
{
    FunctionName.type = (FunctionName.Type)Enum.Parse(typeof(FunctionName.Type), "Griewank");
    GenModel();
    thesamepopulation.Enabled = false;
}


private void savepopulation_Click(object sender, EventArgs e)
{
    thesame = true;

    populationestore = new Population(population.dim);
    populationestore = population.copy();

    foreach (Individual item in population.population)
    {
        Individual tmp = new Individual(population.dim);
        Individual.copy(item, tmp);
        populationestore.population.Add(tmp);
    }

}

private void thesamepopulation_Click(object sender, EventArgs e)
{

    i = 0;
    j = 0;

    avvelocity = new float[1];
    avfitness = new float[1];
    avbfitness = new float[1];
    List<Individual> itemList = new List<Individual>();
    population = new Population(populationestore.dim);
    population = populationestore.copy();
    foreach (Individual item in populationestore.population)
    {
        Individual tmp = new Individual(populationestore.dim);
        Individual.copy(item, tmp);
        population.population.Add(tmp);
    }

    model = new Thread(GenGraph2);
    model.IsBackground = true;
    model.Start();
    List<Population> tmp1 = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);
    this.functionButtonSet(false);

    animace = new Thread(ShowParticleMove);
    animace.IsBackground = true;
    animace.Start(tmp1);

}
private void GenGraph2()
{
    this.ShowParticleOnGraph(population);
    this.ShowGraphChart(population);
    this.ShowCharts(population);
    this.RefreshModel();
}



private void testbutton_Click(object sender, EventArgs e)
{

    frm = new Form1();
    double[] tab = new double[testnumber];
    float[] sum = new float[numberIterations];
    float[] best = new float[numberIterations];
    float[] worst = new float[numberIterations];
    float[] bgfworst = new float[numberIterations];
    float[] bgfbest = new float[numberIterations];
    float[] bgfav = new float[numberIterations];

    float[] globalmin = new float[testnumber];
    double wynik = 0;
    double bestresult = 0;
    double worstresult = 0;
    double percentsucess = 0;
    double tmpbest = 0;
    double tmpworst = 0;


    for (int i = 0; i < testnumber; ++i)
    {
        population = new Population(PopulationSize, dim, FunctionName.type);
        population.SetRangeOfPopulation(error);
        population.GeneratePopulation(dim);
        population.CalculatePopulationFitness();

        List<Population> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);//numberIterations, inertiaw
        tmp.Remove(tmp.Last());
        tab[i] = tmp.Min((x => x.bestGlobalFitness));  //tablica wartości wyników-z tego obliczyc % sukcesów
        wynik += tab[i];
        globalmin[i] = (float)tmp.Min((x => x.bestGlobalFitness));


        if (Math.Abs(tab[i] - tmp.First().min) < tmp.First().error)
            percentsucess++;

        if (i == 0)
        {
            tmpbest = tab[i];
            tmpworst = tab[i];
        }
        int popnumber = 0;

        foreach (Population pop in tmp)
        {
            float b = 0;
            float c = 0;


            bgfav[popnumber] += (float)pop.bestGlobalFitness / testnumber;




            var scene = new ILScene();
            scene.Screen.First<ILLabel>().Visible = false;

            foreach (Individual item in pop.population)
            {
                // MessageBox.Show(a.Length.ToString()+"  " +tmp.populationSize.ToString());
                b += (float)item.fitnessValue;
            }
            c = (b / pop.population.Count) / testnumber;
            sum[popnumber] += c;
            if (tab[i] <= tmpbest)
            {
                best[popnumber] = b / pop.population.Count;
                bgfbest[popnumber] = (float)pop.bestGlobalFitness;
                bestresult = pop.bestGlobalFitness;
                tmpbest = tab[i];
            }

            if (tab[i] >= tmpworst)
            {
                worst[popnumber] = b / pop.population.Count;
                bgfworst[popnumber] = (float)pop.bestGlobalFitness;
                worstresult = pop.bestGlobalFitness;
                tmpworst = tab[i];
            }
            popnumber++;



        }

    }

    frm.richTextBox1.AppendText("Średnie wartości funkci: " + wynik / testnumber + "\n" + "\n");
    frm.richTextBox1.AppendText("Najlepsza wartość funkcji: " + bestresult + "\n" + "\n");
    frm.richTextBox1.AppendText("Najgorsza wartość funkcji: " + worstresult + "\n" + "\n");
    frm.richTextBox1.AppendText("Procent sukcesu: " + percentsucess / testnumber * 100 + "%" + "\n" + "\n");




    var scena = new ILScene();
    var scena1 = new ILScene();
    var scena2 = new ILScene();
    //scena.Screen.First<ILLabel>().Visible = false;
    using (ILScope.Enter())
    {
        ILArray<float> AV = sum;
        ILArray<float> BEST = best;
        ILArray<float> WORST = worst;
        ILArray<float> BGFworst = bgfworst;
        ILArray<float> BGFbest = bgfbest;
        ILArray<float> BGFav = bgfav;
        ILArray<float> GLOBAL = globalmin;
        //  MessageBox.Show(BGFworst+" "+BGFbest+" "+BGFav.ToString());
        var plot = scena.Add(new ILPlotCube()
        {
            ScreenRect = new RectangleF(0, 0, 1, 0.4f),
            Childs = { new ILLinePlot(AV.T,lineColor:Color.Yellow),
                                new ILLinePlot(BEST.T,lineColor:Color.Blue),
                                 new ILLinePlot(WORST.T,lineColor:Color.Red),
                    },
            Axes =
                    {
                        XAxis =
                        {
                            Label = { Text = "numer iteracji" },

                        },

                        YAxis =
                        {
                            Label = { Text = "średnia wartość cząsteczek (cała populacja)" },
                        }
                    }
            ,
        });
        var legend = plot.Add(new ILLegend("średnia z przebiegów", "najlepszy z przebiegów", "najgorszy z przebiegów"));



        var plot1 = scena.Add(new ILPlotCube()
        {
            ScreenRect = new RectangleF(0, 0.33f, 1, 0.4f),
            Childs = { new ILLinePlot(BGFav.T,lineColor:Color.Yellow),
                                new ILLinePlot(BGFbest.T,lineColor:Color.Blue),
                                 new ILLinePlot(BGFworst.T,lineColor:Color.Red),
                    },
            Axes =
                    {
                        XAxis =
                        {
                            Label = { Text = "numer iteracji" },
                            //Ticks = { Visible = false }
                        },
                        // Setting the default size for tick labels affects the width of the 
                        // cube. This would 'disalign' both plot cubes: 
                        YAxis =
                        {
                            Label = { Text = "osiągnięte minimum (najlepsze całej populacji)" },
                            //Ticks = { DefaultTickLabelSize = new SizeF(79, 20) }
                        }
                    }
            ,
        });


        var legend2 = plot1.Add(new ILLegend("średnia z przebiegów", "najlepszy z przebiegów", "najgorszy z przebiegów"));
        var plot2 = scena.Add(new ILPlotCube()
        {
            ScreenRect = new RectangleF(0, 0.66f, 1, 0.3f),
            Childs = { new ILLinePlot(GLOBAL.T, markerStyle: MarkerStyle.Diamond, lineColor: Color.Black) },
            Axes =
                    {
                        XAxis =
                        {
                            Label = { Text = "numer kolejnego testu" },
                            //Ticks = { Visible = false }
                        },
                        // Setting the default size for tick labels affects the width of the 
                        // cube. This would 'disalign' both plot cubes: 
                        YAxis =
                        {
                            Label = { Text = "osiągnięte minimum  " },
                           
                            //Ticks = { DefaultTickLabelSize = new SizeF(79, 20) }
                        }
                    },
        });

        var dg2 = plot2.AddDataGroup();
        dg2.Add(new ILLinePlot(GLOBAL.T, markerStyle: MarkerStyle.Diamond, lineColor: Color.Red));//,lineColor: Color.Red));
        dg2.ScaleModes.YAxisScale = AxisScale.Logarithmic;
        var axisY2 = plot2.Axes.Add(new ILAxis(dg2)
        {
            AxisName = AxisNames.YAxis,
            Position = new Vector3(1, 0, 0),
            // ColorOverride = Color.Red,
            Label = { Text = "osiągnięte minimum (log)", Color = Color.Red },
            Ticks = { DefaultLabel = { Color = Color.Red } }
        });

        //  plot.DataScreenRect = new RectangleF(0.12f, 0.05f, 0.84f, 0.5f);
        // plot1.DataScreenRect = new RectangleF(0.12f, 0.05f, 0.84f, 0.5f);
        // plot2.DataScreenRect = new RectangleF(0.12f, 0.05f, 0.84f, 0.5f); 
        frm.ilPanel1.Scene = scena;
        frm.Show();
    }

}

private void ilPanel1_Load(object sender, EventArgs e)
{

}
    }
    }

        

       
    
*/

