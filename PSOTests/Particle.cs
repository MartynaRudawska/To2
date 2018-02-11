using System;
using System.Collections.Generic;
//using PSOTests.Funkcje;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PSOTests
{
    public class Particle : Funkcje
    {
        public double[] position { get; set; }                  // &parameters
        public double error;
        public double[] velocity { get; set; }
        public double[] bestPosition { get; set; }
        public double bestError;
        public double pozycja;
        public double predkosc;
        public double NajlepszaPozycja { get; set; }                 //&bestPosition
        public double najlBlad;
        public double fitnessValue { get; set; }
        public double bestFitness { get; set; }



        /// <summary>
        /// Multi-dimensional 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="err"></param>
        /// <param name="vel"></param>
        /// <param name="bestPos"></param>
        /// <param name="bestErr"></param>
        /// 
       // [Serializable]
        public Particle()
        {
        }
        public Particle(int dim)
        {
            this.position = new double[dim];
            this.velocity = new double[dim];
            this.bestPosition = new double[dim];

        }

        public Particle(double param1, double param2)
        {
            this.position = new double[2];
            this.position[0] = param1;
            this.position[1] = param2;

        }
        public Particle(double[] param)
        {
            this.position = new double[param.Length];
            param.CopyTo(this.position, 0);

        }

        public Particle(double[] param, double bestFitness, double[] velocity, double[] bestPosition)
        {
            this.position = new double[param.Length];
            param.CopyTo(this.position, 0);
            this.velocity = new double[param.Length];
            velocity.CopyTo(this.velocity, 0);
            this.bestPosition = new double[param.Length];//[bestPosition.Length];
            bestPosition.CopyTo(this.bestPosition, 0);
            this.bestFitness = bestFitness;
        }

        public static void copy(Particle src, Particle dest)
        {
            Particle tmp = new Particle(src.position.Length);
            src.position.CopyTo(dest.position, 0);
            src.velocity.CopyTo(dest.velocity, 0);
            src.bestPosition.CopyTo(dest.bestPosition, 0);
            src.bestPosition.CopyTo(dest.bestPosition, 0);
            dest.bestFitness = src.bestFitness;
            dest.fitnessValue = src.fitnessValue;

        }

        public static void ObliczIndiwFitness(Particle ind)
        {
            switch (FunctionName.type)
            {
                case FunctionName.Type.Schwefel:
                    Schwefel.setFitness(ind);
                    break;

                case FunctionName.Type.DeJong1:
                    DeJong1.setFitness(ind);
                    break;

                case FunctionName.Type.Rastrigin:
                    Rastrigin.setFitness(ind);
                    break;

                case FunctionName.Type.Rosenbrock:
                    Rosenbrock.setFitness(ind);
                    break;

            }
        }


        /// ///////////////////

        /*   public Particle(double[] pos, double err, double[] vel, double[] bestPos, double bestErr)
           {
               this.position = new double[pos.Length];
               pos.CopyTo(this.position, 0);
               this.error = err;
               this.velocity = new double[vel.Length];
               vel.CopyTo(this.velocity, 0);
               this.bestPosition = new double[bestPos.Length];
               bestPos.CopyTo(this.bestPosition, 0);
               this.bestError = bestErr;
           }

           /// <summary>
           /// Konstruktor cząstki - Przypadek 1D
           /// </summary>
           /// <param name="poz">Pozycja cząstki</param>
           /// <param name="blad">Błąd</param>
           /// <param name="v">Prędkość cząstki</param>
           /// <param name="najPoz">Najlepsza pozycja</param>
           /// <param name="najBlad">najlepszy błąd</param>
           public Particle(double poz, double blad, double v, double najPoz, double najBlad)
           {
               this.pozycja = poz;
               this.error = blad;
               this.predkosc = v;
               this.najlPozycja = najPoz;
               this.najlBlad = najBlad;
           }
           public override string ToString()
           {
               string s = "";
               s += "==========================\n";
               s += "Position: ";
               //for (int i = 0; i < this.position.Length; ++i)
               s += this.pozycja.ToString("F4") + " ";
               s += "\n";
               s += "Error = " + this.error.ToString("F4") + "\n";
               s += "Velocity: ";
               //for (int i = 0; i < this.velocity.Length; ++i)
               s += this.predkosc.ToString("F4") + " ";
               s += "\n";
               s += "Best Position: ";
               //for (int i = 0; i < this.bestPosition.Length; ++i)
               s += this.najlPozycja.ToString("F4") + " ";
               s += "\n";
               s += "Best Error = " + this.bestError.ToString("F4") + "\n";
               s += "==========================\n";
               return s;
           }

       } // Particle */
    }
}

