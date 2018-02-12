using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticSharp.Domain;
using GeneticSharp.Extensions;
using GeneticSharp.Infrastructure;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Domain.Populations;
using System.Threading;
using GeneticSharp.Runner.GtkApp;
using GeneticSharp.Runner.GtkApp.Samples;
using Gdk;
using Gtk;
using GeneticSharp.Infrastructure.Framework.Reflection;

namespace TechnikiOptymalizacjiAG
{
    public partial class TechnikiOptymalizacjiAGMainWindow : Form
    {
        #region pola genetyczny
        private GeneticAlgorithm m_ga;
        private IFitness m_fitness;
        private ISelection m_selection;
        private ICrossover m_crossover;
        private IMutation m_mutation;
        private IReinsertion m_reinsertion;
        private ITermination m_termination;
        private IGenerationStrategy m_generationStrategy;
        private ISampleController m_sampleController;
        private SampleContext m_sampleContext;
        private Thread m_evolvingThread;
        #endregion
        #region konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// 
        #endregion
        #region pola PSO
        PSO optymalizacja;

        private Thread animace;
        private Populacja population { get; set; }
        private Populacja populationestore { get; set; }
        private Populacja MaxEpoch;

        private double maxX { get; set; }
        private double minX { get; set; }
        private short ileCzastek;
        private short maxEpochs;
        private string funkcja;
        private string krzyzowanie;
        private string selekcja;
        private int mutacja;

        private bool _threadPaused = false;
        private bool thesame = false;
        private double c1 = 1.49445;
        private double c2 = 1.49445;
        private int i = 0;
        private int j = 0;
        private int dim = 2;
        private int MaxEpochSize = 1000;
        private int testnumber = 100;
        private int PopulationSize = 20;
        private int numberIterations = 50;
        private int nrIteracji = 50;
        private double inertiaw = 0.75;
        private double error = 0.005;

        private float[] avfitness = new float[1];
        private float[] avbfitness = new float[1];
        private float[] avvelocity = new float[1];

        private bool r1r2 = false;
        private bool linearinertia = false;
        private  Form frm;

        private Dictionary<string, Tuple<double, double>> dziedzinyFunkcji = new Dictionary<string, Tuple<double, double>>();
        private Thread model;

        private EventWaitHandle wh = new AutoResetEvent(false);

        #endregion
        #region konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// 

        public TechnikiOptymalizacjiAGMainWindow()
        {
            InitializeComponent();
            dziedzinyFunkcji.Add("DeJong1", new Tuple<double, double>(-5.12, 5.12));
            dziedzinyFunkcji.Add("Schwefel)", new Tuple<double, double>(-500, 500));
            dziedzinyFunkcji.Add("Rosenbrock", new Tuple<double, double>(-2.048, 2.048));
            dziedzinyFunkcji.Add("Rastrigin)", new Tuple<double, double>(-5.12, 5.12));
            
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            MaxEpochUpDown.Value = MaxEpochUpDown.Minimum;
            ParticleQuantityUpDown.Value = ParticleQuantityUpDown.Minimum;
        }


        private void functionButtonSet(bool value)
        {
            //Invoke(new Gtk.Action(()                  ?
            
                /* Invoke(new Action(() =>
                 {
                     _threadPaused = value;

                     StartBtn.Enabled = value;

                 }*/
               // ));
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////                                                                                                          //////
        /////                   PSO                                                                                   ///////
        /////                                                                                                        ////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowParticleMove(object obj)
        {
            List<Populacja> list = (List<Populacja>)obj;

            int i = 0;
            foreach (Populacja item in list)
            {
                //surface = new ILSurface(data);
                //surface.Fill.Markable = false;
                //surface.Wireframe.Markable = false;
                //this.ShowParticleOnGraph(item, 8);
                //this.ShowGraphChart(item);
                //this.ShowCharts(item);
                //this.RefreshModel();

                if (i < nrIteracji)
                {
                    int k = i + 1;
                    for (int j = 0; j < item.NajlepszaPozycja.Length; ++j)
                    {
                        Invoke(new System.Action(() => richTextPSO.AppendText("Iteracja: " + k.ToString() + "  x" + "[" + j + "]" + "   " + item.NajlepszaPozycja[j] + "\n")));
                    }
                    Invoke(new System.Action(() => richTextPSO.AppendText("    Minimum: " + item.NajlepszaFitness + "\n")));
                    Invoke(new System.Action(() => richTextPSO.ScrollToCaret()));

                }
                // Thread.Sleep(1000);


                if (_threadPaused)
                    wh.WaitOne();
                ++i;
            }
            //Invoke(new Action(() => playPausePictureBox.Visible = false));

        }

        private void GenerateGraph()
        {
            //Invoke(new Action(() => ilgraf.Visible = true));

            MaxEpoch = new Populacja(MaxEpochSize, Funkcje.FunctionName.type);
            MaxEpoch.SetRangeOfPopulation(Funkcje.FunctionName.type, error);
            MaxEpoch.GenerateGraphPopulation();
            MaxEpoch.ObliczPopulFitness(Funkcje.FunctionName.type);

            population = new Populacja(PopulationSize, dim, Funkcje.FunctionName.type);
            population.SetRangeOfPopulation(Funkcje.FunctionName.type, error);
            population.GeneratePopulation(dim);
            population.ObliczPopulFitness(Funkcje.FunctionName.type);

            float[] newData = new float[MaxEpoch.population.Count * 3];
            Parallel.For(0, MaxEpoch.population.Count, i =>
            {
                newData[i] = (float)MaxEpoch.population[i].fitnessValue;  // Z
                newData[i + MaxEpoch.population.Count] = (float)MaxEpoch.population[i].position[0]; // X
                newData[i + 2 * MaxEpoch.population.Count] = (float)MaxEpoch.population[i].position[1]; // Y


            });

            int size = (int)Math.Sqrt(MaxEpoch.population.Count);
           // data = ILNumerics.ILMath.array(newData, size, size, 3);
            //surface = new ILSurface(data);
           // surface.Fill.Markable = false;
            //surface.Wireframe.Markable = false;
            //surface.Colormap = Colormaps.Copper;
            //ILColorbar color = new ILColorbar() { Location = new PointF(.96f, 0.1f) };
            //surface.Children.Add(color);
            //this.ShowParticleOnGraph(population);
            //this.RefreshModel();

        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////                                                                                                          //////
        /////                   Algorytm Genetyczny                                                                   ///////
        /////                                                                                                        ////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        #region Metody Algorytmu Genetycznego


        /// <summary>
        /// Uruchamianie wątku, w którym pracuje Algorytm Genetyczny
        /// </summary>
        /// <param name="isResuming">Czy praca algorytmu jest właśnie wznawiana?</param>
        private void RunGAThread(bool isResuming)
        {
            vbxSample.Sensitive = false;
            vbxGA.Sensitive = false;
            m_evolvingThread = isResuming ? new Thread(ResumeGA) : new Thread(StartGA);
            m_evolvingThread.Start();
        }
        /// <summary>
        /// Uruchamianie Algorytmu Genetycznego
        /// </summary>
        private void StartGA()
        {
           
                RunGA(() =>
            {
                m_sampleController.Reset();
                m_sampleContext.Population = new Population(
                    Convert.ToInt32(PopulationMinUpDown.Value),
                    Convert.ToInt32(PopulationMaxUpDown.Value),
                    m_sampleController.CreateChromosome());

                m_sampleContext.Population.GenerationStrategy = m_generationStrategy;

                m_ga = new GeneticAlgorithm(
                    m_sampleContext.Population,
                    m_fitness,
                    m_selection,
                    m_crossover,
                    m_mutation);

                m_ga.CrossoverProbability = 0.75f;//Convert.ToSingle(hslCrossoverProbability.Value);
                m_ga.MutationProbability = Convert.ToSingle(MutationProbTrackbar.Value); // przechwycenie mutacji
                m_ga.Reinsertion = m_reinsertion;
                m_ga.Termination = m_termination;

                m_sampleContext.GA = m_ga;
                m_ga.GenerationRan += delegate
                {
                    Gtk.Application.Invoke(delegate
                    {
                        m_sampleController.Update();
                    });
                };

                m_sampleController.ConfigGA(m_ga);
                m_ga.Start();
            });
            
        }
        /// <summary>
        /// Wznawianie Algorytmu Genetycznego
        /// </summary>
        private void ResumeGA()
        {
            RunGA(() =>
            {
                m_ga.Population.MinSize = Convert.ToInt32(PopulationMinUpDown.Value);
                m_ga.Population.MaxSize = Convert.ToInt32(PopulationMaxUpDown.Value);
                m_ga.Selection = m_selection;
                m_ga.Crossover = m_crossover;
                m_ga.Mutation = m_mutation;
                //m_ga.CrossoverProbability = Convert.ToSingle(hslCrossoverProbability.Value);
                m_ga.MutationProbability = Convert.ToSingle(MutationProbTrackbar.Value);
                m_ga.Reinsertion = m_reinsertion;
                m_ga.Termination = m_termination;

                m_ga.Resume();
            });
        }

        private void RunGA(System.Action runAction)
        {
            try
            {
                runAction();
            }
            catch (Exception ex)
            {
                Invoke(delegate
                {
                    var msg = new MessageDialog(
                        this,
                        DialogFlags.Modal,
                        MessageType.Error,
                        ButtonsType.YesNo,
                        "{0}\n\nDo you want to see more details about this error?",
                        ex.Message);

                    if (msg.Run() == (int)ResponseType.Yes)
                    {
                        var details = new MessageDialog(
                            this,
                            DialogFlags.Modal,
                            MessageType.Info,
                            ButtonsType.Ok,
                            "StackTrace");

                        details.SecondaryText = ex.StackTrace;
                        details.Run();
                        details.Destroy();
                    }

                msg.Destroy();
                });
            }

            Invoke(delegate
            {
                btnNew.Visible = true;
                btnResume.Visible = true;
                btnStop.Visible = false;
                vbxGA.Sensitive = true;
            });
        }
        #endregion

        #region Event Handlers


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////                                                                                                          //////
        /////                   Form                                                                                  ///////
        /////                                                                                                        ////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //reset
        private void Reset_Click(object sender, EventArgs e)
        {
            i = 0;
            j = 0;

            List<Particle> itemList = new List<Particle>();
            population = new Populacja(populationestore.dim);
            population = (Populacja)populationestore.Clone();
            {
                Particle tmp = new Particle(populationestore.dim);
                Particle.copy(item, tmp);
                population.population.Add(tmp);
            }

            //model = new Thread(GenGraph2);
            //model.IsBackground = true;
            //model.Start();
            List<Populacja> tmp1 = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);
            this.functionButtonSet(false);

            //animace = new Thread(ShowParticleMove);
            //animace.IsBackground = true;
            animace.Start(tmp1);
        }


        //start
        private void StartBtn_Click(object sender, EventArgs e)
        {
            string f = FunctionSelectionCombo.SelectedItem.ToString();
            string g = SelectionCombo.SelectedItem.ToString();
            string h = CrossingCombo.SelectedItem.ToString();
            if (!String.IsNullOrEmpty(funkcja) && !funkcja.Equals("Proszę wybrać funkcję do optymalizacji"))
            {
                ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
              maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
             //optymalizacja = new PSO(dziedzinyFunkcji[funkcja], ileCzastek, maxEpochs, funkcja);
            //MessageBox.Show(string.Format("Znalezione minimum to {0} z błędem {1}", PSO.PSOSolution().Item1, PSO.PSOSolution().Item2), "Rezultat optymalizacji", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
             else MessageBox.Show("Nie wybrano funkcji do optymalizacji", "BŁĄD!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            List<Populacja> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);
            this.functionButtonSet(false);
            if (thesame == true)
            {
                Reset.Enabled = true;
            }


            //List<Populacja> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);
            this.functionButtonSet(false);
            if (thesame == true)
            {
                Reset.Enabled = true;
            }




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
                population = new Populacja(PopulationSize, dim, Funkcje.FunctionName.type);
                population.SetRangeOfPopulation(Funkcje.FunctionName.type, error);
                population.GeneratePopulation(dim);
                population.ObliczPopulFitness(Funkcje.FunctionName.type);

                //List<Populacja> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);//numberIterations, inertiaw
                tmp.Remove(tmp.Last());
                tab[i] = tmp.Min((x => x.NajlepszaFitness));  //tablica wartości wyników-z tego obliczyc % sukcesów
                wynik += tab[i];
                globalmin[i] = (float)tmp.Min((x => x.NajlepszaFitness));


                if (Math.Abs(tab[i] - tmp.First().min) < tmp.First().exitError)
                    percentsucess++;

                if (i == 0)
                {
                    tmpbest = tab[i];
                    tmpworst = tab[i];
                }
                int popnumber = 0;

                foreach (Populacja pop in tmp)
                {
                    float b = 0;
                    float c = 0;

                    bgfav[popnumber] += (float)pop.NajlepszaFitness / testnumber;
                    foreach (Particle item in pop.population)
                    {
                        // MessageBox.Show(a.Length.ToString()+"  " +tmp.populationSize.ToString());
                        b += (float)item.fitnessValue;
                    }
                    c = (b / pop.population.Count) / testnumber;
                    sum[popnumber] += c;
                    if (tab[i] <= tmpbest)
                    {
                        best[popnumber] = b / pop.population.Count;
                        bgfbest[popnumber] = (float)pop.NajlepszaFitness;
                        bestresult = pop.NajlepszaFitness;
                        tmpbest = tab[i];
                    }

                    if (tab[i] >= tmpworst)
                    {
                        worst[popnumber] = b / pop.population.Count;
                        bgfworst[popnumber] = (float)pop.NajlepszaFitness;
                        worstresult = pop.NajlepszaFitness;
                        tmpworst = tab[i];
                    }
                    popnumber++;

                }
            }
            richTextBox1.AppendText("Średnie wartości funkci: " + wynik / testnumber + "\n" + "\n");

            richTextBox1.AppendText("Najlepsza wartość funkcji: " + bestresult + "\n" + "\n");
            richTextBox1.AppendText("Najgorsza wartość funkcji: " + worstresult + "\n" + "\n");
            richTextBox1.AppendText("Procent sukcesu: " + percentsucess / testnumber * 100 + "%" + "\n" + "\n");
            

        }


        private void ParticleQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
        }


        private void MaxEpochUpDown_ValueChanged(object sender, EventArgs e)
        {
            maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
        }

        //nazwa funkcji
        private void FunctionSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            funkcja = FunctionSelectionCombo.SelectedItem.ToString();
        }

        //wybranie Krzyżowania
        private void CrossingCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            krzyzowanie = CrossingCombo.SelectedItem.ToString();
        }


        //wybranie Selekcji
        private void SelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selekcja = SelectionCombo.SelectedItem.ToString();
        }

        //Mutacja
        private void MutationProbTrackbar_Scroll(object sender, EventArgs e)
        {
            
        }

        private void TimeThresholdUpDown_Click(object sender, EventArgs e)
        {

        }

        private void IterationThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {
           // MessageBox.Show(IterationThresholdUpDown.Value.ToString());
        }

        private void TimeThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void IterationThresholdRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TimeThresholdRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PopulationMinUpDown_ValueChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(PopulationMinUpDown.Value.ToString());
        }

        private void PopulationMaxUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TechnikiOptymalizacjiAGMainWindow_Load(object sender, EventArgs e)
        {

        }

        /*private void GenGraph2()
        {
            this.ShowParticleOnGraph(population);
            this.RefreshModel();
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

        private void RefreshModel()
        {
            Invoke(new System.Action(() =>
            {
                scena = new ILScene { new ILPlotCube(twoDMode: false) { surface } };

                if (dim == 2)
                {
                    scena.First<ILPlotCube>().Rotation = Matrix4.Rotation(new Vector3(1, 1, 1), 0.3f);
                }
                scena.Screen.First<ILLabel>().Visible = false;

                ilgraf.Scene = scena;
                ilgraf.Refresh();
            }));
        }*/
        /* private void FunctionSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
         {
             MessageBox.Show(FunctionSelectionCombo.SelectedItem.ToString());
         }*/
        #endregion
    }
}