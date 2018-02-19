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
using GeneticSharp.Domain.Chromosomes;

namespace TechnikiOptymalizacjiAG
{
    public partial class TechnikiOptymalizacjiAGMainWindow : Form
    {
        #region pola genetyczny
        private GeneticAlgorithm m_ga;
        private IFitness m_fitness;
        private ISelection m_selection;
        private ICrossover m_crossover;
        private IMutation m_mutation = new FlipBitMutation();

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
        /// <summary>
        /// Ilość cząstek w populacji
        /// </summary>
        private short ileCzastek;
        private short maxEpochs;
        /// <summary>
        /// Tekstowa reprezentacja funkcji
        /// </summary>
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
        private Form frm;

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
            //blokada pól zmiany wartości przed wyborem warunku stopu

            IterationThresholdUpDown.Enabled = false;
            TimeThresholdUpDown.Enabled = false;
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
                        Invoke(new System.Action(() => richTextBox1.AppendText("Iteracja: " + k.ToString() + "  x" + "[" + j + "]" + "   " + item.NajlepszaPozycja[j] + "\n")));
                    }
                    Invoke(new System.Action(() => richTextBox1.AppendText("    Minimum: " + item.NajlepszaFitness + "\n")));
                    Invoke(new System.Action(() => richTextBox1.ScrollToCaret()));

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

        private void RunGA(System.Action runAction)
        {
            //try
            // {
            runAction();
            //}
            //catch (Exception ex)
            //{
            /*  Invoke(delegate
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
          });*/

        }


        /// <summary>
        /// Uruchamianie wątku, w którym pracuje Algorytm Genetyczny
        /// </summary>
        /// <param name="isResuming">Czy praca algorytmu jest właśnie wznawiana?</param>
        private void RunGAThread(bool isResuming)
        {
            //vbxSample.Sensitive = false;
            //vbxGA.Sensitive = false;
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
                m_sampleContext.Population = new Population(Convert.ToInt32(PopulationMinUpDown.Value),Convert.ToInt32(PopulationMaxUpDown.Value),m_sampleController.CreateChromosome());//new IChromosome.BinaryChromosomeBase());
                m_sampleContext.Population.GenerationStrategy = m_generationStrategy;
                m_ga = new GeneticAlgorithm(m_sampleContext.Population,m_fitness,m_selection,m_crossover,m_mutation);

                m_ga.CrossoverProbability = 0.75f;//Convert.ToSingle(hslCrossoverProbability.Value);
                m_ga.MutationProbability = Convert.ToSingle(MutationProbTrackbar.Value); // przechwycenie mutacji
                m_ga.Reinsertion = m_reinsertion;
                m_ga.Termination = m_termination;
                /*m_ga.Termination = new Termination(
                    if (TimeThresholdRadioBtn) m_termination = new TimeEvolvingTermination();
                    else m_termination = new GenerationNumberTermination());*/
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

            float[] globalmin = new float[testnumber];
            double wynik = 0;
            double bestresult = 0;
            double worstresult = 0;
            double percentsucess = 0;
            double tmpbest = 0;
            double tmpworst = 0;

                //m_ga.CrossoverProbability = Convert.ToSingle(hslCrossoverProbability.Value);
                m_ga.MutationProbability = Convert.ToSingle(MutationProbTrackbar.Value);
            m_ga.Reinsertion = m_reinsertion;
            m_ga.Termination = m_termination;
            richTextAlGenet.AppendText("Średnie wartości funkci: " + wynik / testnumber + "\n" + "\n");
            richTextAlGenet.AppendText("Najlepsza wartość funkcji: " + bestresult + "\n" + "\n");
            richTextAlGenet.AppendText("Najgorsza wartość funkcji: " + worstresult + "\n" + "\n");
            richTextAlGenet.AppendText("Procent sukcesu: " + percentsucess / testnumber * 100 + "%" + "\n" + "\n");
            m_ga.Resume();
        });
    }

    private void PrepareSamples()
    {
        //LoadComboBox(cmbSample, TypeHelper.GetDisplayNamesByInterface<ISampleController>());
        //m_sampleController = TypeHelper.CreateInstanceByName<ISampleController>(cmbSample.ActiveText);

        // Sample context.
        //var layout = new Pango.Layout(this.PangoContext);
        //layout.Alignment = Pango.Alignment.Center;
        // layout.FontDescription = Pango.FontDescription.FromString("Arial 16");

        //m_sampleContext.GC = m_sampleContext.CreateGC(new Gdk.Color(255, 50, 50));

        m_sampleController.Context = m_sampleContext;
        m_sampleController.Reconfigured += delegate
        {
                //ResetSample();
            }; 

            //problemConfigWidgetContainer.Add(m_sampleController.CreateConfigWidget());
            //problemConfigWidgetContainer.ShowAll();

        //SetSampleOperatorsToComboxes();
        //?
        /* cmbSample.Changed += delegate
         {
             m_sampleController = TypeHelper.CreateInstanceByName<ISampleController>(cmbSample.ActiveText);
             SetSampleOperatorsToComboxes();

             m_sampleController.Context = m_sampleContext;
             m_sampleController.Reconfigured += delegate
             {
                 ResetSample();
             };

             if (problemConfigWidgetContainer.Children.Length > 0)
             {
                 problemConfigWidgetContainer.Children[0].Destroy();
             }

             problemConfigWidgetContainer.Add(m_sampleController.CreateConfigWidget());
             problemConfigWidgetContainer.ShowAll();

             ResetBuffer();
             ResetSample();
         };*/
        }


    private void SetSampleOperatorToCombobox(Func<IList<Type>> getCrossoverTypes, Func<ICrossover> createCrossover, Action<ICrossover> p, object CrossingCombo)
    {
        throw new NotImplementedException();
    }
    //SetSampleOperatorToCombobox(CrossoverService.GetCrossoverTypes, m_sampleController.CreateCrossover, (c) => m_crossover = c, Gtk.ComboBox.CrossingCombo);
    //zmień operator do Coboboxa( CrossoverService jest to biblioteka GeneticSharp.Domain.Crossovers "Creates the ICrossover's implementation with the specified name.", GetCrossoverTypes zabiera Typ Krzyżowania nazwe itd, wrzuca do tworzenia, przerzucenie parametrów , nazwa comboBoxa);
    // nie jestem pewna czy to to samo

    private void SetSampleOperatorToCombobox(Func<IList<Type>> getSelectionTypes, Func<ISelection> CreateSelection, Action<ISelection> p, object SelectionCombo)
    {
        throw new NotImplementedException();
    }

    //SetSampleOperatorToCombobox(SelectionService.GetSelectionTypes, m_sampleController.CreateSelection, (c) => m_selection = c, cmbSelection);


    private void SetSampleOperatorToCombobox<TOperator>(Func<IList<Type>> getOperatorTypes, Func<TOperator> getOperator, System.Action<TOperator> setOperator, System.Windows.Forms.ComboBox combobox)
    {
        var @operator = getOperator();
        var operatorType = @operator.GetType();

        var opeartorIndex = getOperatorTypes().Select((type, index) => new { type, index }).First(c => c.type.Equals(operatorType)).index;
        combobox.SelectedIndex= opeartorIndex;
        
        setOperator(@operator);
    }

   /* private void PrepareComboBoxes()
    {
        PrepareEditComboBox(
            SelectionCombo,
            SelectionService.GetSelectionNames,
            SelectionService.GetSelectionTypeByName,
            SelectionService.CreateSelectionByName,
            () => m_selection,
            (i) => m_selection = i)
            ;

        PrepareEditComboBox(
            CrossingCombo,
            CrossoverService.GetCrossoverNames,
            CrossoverService.GetCrossoverTypeByName,
            CrossoverService.CreateCrossoverByName,
            () => m_crossover,
            (i) => m_crossover = i)
            ;
    }*/

        private void PrepareEditComboBox<TItem>(System.Windows.Forms.ComboBox selectionCombo, Func<IList<string>> getSelectionNames, Func<string, Type> getSelectionTypeByName, Func<string, object[], ISelection> createSelectionByName, Func<string, object[], TItem> p1, Func<object, TItem> p2)
        {
            throw new NotImplementedException();
        }

        private void PrepareEditComboBox<TItem>(System.Windows.Forms.ComboBox comboBox, System.Windows.Forms.Button editButton, Func<IList<string>> getNames, Func<string, Type> getTypeByName, Func<string, object[], TItem> createItem, Func<TItem> getItem, Action<TItem> setItem)
        {
            // ComboBox.
           /* LoadComboBox(comboBox, getNames());

            comboBox.Changed += delegate
            {
                var item = createItem(comboBox.ActiveText, new object[0]);
                setItem(item);
                ShowButtonByEditableProperties(editButton, item);
            };

            setItem(createItem(comboBox.ActiveText, new object[0]));

            comboBox.ExposeEvent += delegate
            {
                ShowButtonByEditableProperties(editButton, getItem());
            };

            // Edit button.
            editButton.Clicked += delegate
            {
                var editor = new PropertyEditor(getTypeByName(comboBox.ActiveText), getItem());
                editor.Run();
                setItem((TItem)editor.ObjectInstance);
            };*/
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
            foreach (Particle item in populationestore.population)
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

       // List<Populacja> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);
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

        richTextAlGenet.AppendText("Średnie wartości funkcji: " + wynik / testnumber + "\n" + "\n");
        richTextAlGenet.AppendText("Najlepsza wartość funkcji: " + bestresult + "\n" + "\n");
        richTextAlGenet.AppendText("Najgorsza wartość funkcji: " + worstresult + "\n" + "\n");
        richTextAlGenet.AppendText("Procent sukcesu: " + percentsucess / testnumber * 100 + "%" + "\n" + "\n");

        for (int i = 0; i < testnumber; ++i)
        {
            population = new Populacja(PopulationSize, dim, Funkcje.FunctionName.type);
            population.SetRangeOfPopulation(Funkcje.FunctionName.type, error);
            population.GeneratePopulation(dim);
            population.ObliczPopulFitness(Funkcje.FunctionName.type);

            List<Populacja> tmp = new PSO(numberIterations, inertiaw, c1, c2, r1r2, linearinertia).PSOALG(population);//numberIterations, inertiaw
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
        richTextBox1.AppendText("Średnie wartości funkcji: " + wynik / testnumber + "\n" + "\n");

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
            m_termination = new GenerationNumberTermination(Convert.ToInt16(IterationThresholdUpDown.Value));
    }

    private void TimeThresholdUpDown_ValueChanged(object sender, EventArgs e)
    {
            m_termination = new TimeEvolvingTermination(new TimeSpan(0,0,(int)TimeThresholdUpDown.Value));
           
        }

    private void IterationThresholdRadioBtn_CheckedChanged(object sender, EventArgs e)
    {
            if (TimeThresholdRadioBtn.Checked)IterationThresholdUpDown.Enabled = false;
            else IterationThresholdUpDown.Enabled = true;
        }

    private void TimeThresholdRadioBtn_CheckedChanged(object sender, EventArgs e)
    {
            if (IterationThresholdRadioBtn.Checked == true)
            {
                TimeThresholdUpDown.Enabled = false;
            }
            else
            {
                TimeThresholdUpDown.Enabled = true;
            }
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

    private void label7_Click(object sender, EventArgs e)
    {

    }

}
}