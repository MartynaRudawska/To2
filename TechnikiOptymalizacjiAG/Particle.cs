using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnikiOptymalizacjiAG
{
    public class Particle
    {
        public double[] position;
        public double error;
        public double[] velocity;
        public double[] bestPosition;
        public double bestError;
        public double pozycja;
        public double predkosc;
        public double najlPozycja;
        public double najlBlad;
        /// <summary>
        /// Multi-dimensional 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="err"></param>
        /// <param name="vel"></param>
        /// <param name="bestPos"></param>
        /// <param name="bestErr"></param>
        public Particle(double[] pos, double err, double[] vel, double[] bestPos, double bestErr)
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

    } // Particle
}
