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
        private double maxX;
        private double minX;
        private short ileCzastek;
        private short maxEpochs;
        private string funkcja;
        private Dictionary<string, Tuple<double, double>> dziedzinyFunkcji = new Dictionary<string, Tuple<double, double>>();
        #endregion
        #region konstruktor
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// 


        public TechnikiOptymalizacjiAGMainWindow()
        {
            InitializeComponent();
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

        private void ParticleQuantityUpDown_ValueChanged(object sender, EventArgs e)
        {
            ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            //string f = FunctionSelectionCombo.SelectedItem.ToString();
            if (!String.IsNullOrEmpty(funkcja) && !funkcja.Equals("Proszę wybrać funkcję do optymalizacji"))
            {
                ileCzastek = Convert.ToInt16(ParticleQuantityUpDown.Value);
                maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
                optymalizacja = new PSO(dziedzinyFunkcji[funkcja], ileCzastek, maxEpochs, funkcja);
                MessageBox.Show(string.Format("Znalezione minimum to {0} z błędem {1}", PSO.PSOSolution().Item1, PSO.PSOSolution().Item2), "Rezultat optymalizacji", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Nie wybrano funkcji do optymalizacji", "BŁĄD!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MaxEpochUpDown_ValueChanged(object sender, EventArgs e)
        {
            maxEpochs = Convert.ToInt16(MaxEpochUpDown.Value);
        }

        private void FunctionSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            funkcja = FunctionSelectionCombo.SelectedItem.ToString();
        }

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

               // m_ga.CrossoverProbability = Convert.ToSingle(hslCrossoverProbability.Value);
                m_ga.MutationProbability = Convert.ToSingle(MutationProbTrackbar.Value);
                m_ga.Reinsertion = m_reinsertion;
                m_ga.Termination = m_termination;

                m_sampleContext.GA = m_ga;
                m_ga.GenerationRan += delegate
                {
                    Application.Invoke(delegate
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
                Application.Invoke(delegate
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

            Application.Invoke(delegate
            {
                btnNew.Visible = true;
                btnResume.Visible = true;
                btnStop.Visible = false;
                vbxGA.Sensitive = true;
            });
        }
        #endregion

        #region Event Handlers

        private void CompareBtn_Click(object sender, EventArgs e)
        {

        }

        private void TimeThresholdUpDown_Click(object sender, EventArgs e)
        {

        }

        private void IterationThresholdUpDown_ValueChanged(object sender, EventArgs e)
        {
            MessageBox.Show(IterationThresholdUpDown.Value.ToString());
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

        private void MutationProbTrackbar_Scroll(object sender, EventArgs e)
        {

        }

        private void CrossingCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PopulationMinUpDown_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine(PopulationMinUpDown.Value.ToString());
        }

        private void PopulationMaxUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void TechnikiOptymalizacjiAGMainWindow_Load(object sender, EventArgs e)
        {

        }
        /* private void FunctionSelectionCombo_SelectedIndexChanged(object sender, EventArgs e)
         {
             MessageBox.Show(FunctionSelectionCombo.SelectedItem.ToString());
         }*/
        #endregion
    }
}