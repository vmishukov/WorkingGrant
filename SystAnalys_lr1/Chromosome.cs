using System;
using SystAnalys_lr1;

namespace SystAnalys_lr1
{
    class Chromosome : ICloneable
    {

        //TODO: Количество точек Ш. задаеться жестко, либо каким то образом расчитывается из ограничений???! 
        public bool[] Ch { get; private set; }
        public double Fitness { get; set; }
        
        public Chromosome(Random r, int size, int maxFermathPoint)
        {
            Ch = new bool[size];

            int countGenInCh = 0;

            while (countGenInCh != maxFermathPoint)
            {
                var rand = r.Next(0, Ch.Length);
                if (Ch[rand] != true)
                {
                    Ch[rand] = true;
                    countGenInCh++;
                }
            }
        }

        public Chromosome(bool[] ch)
        {
            this.Ch=ch; 
        }

        public Chromosome(bool[] ch,double fitness)
        {
            this.Ch = ch;
            this.Fitness = fitness;
        }

        public Chromosome(int size)
        {
            Ch = new bool[size];
        }

        public Chromosome()
        {
        }

        public void AddOneGen(Random r)
        {
            var chLenght = Ch.Length;
            bool flag = true;
            int item;
            while (flag)
            {
                item = r.Next(0, chLenght);
                if (Ch[item] ==false)
                {
                    Ch[item] = true;
                    flag = false;
                }
            }
        }

        public void Mutate(Random r, double mutateRate)
        {
            for (int i = 0; i < Ch.Length; i++)
            {
                if (r.NextDouble() < mutateRate)
                {
                    Ch[i] = SwapGen(Ch[i]);
                  //  Console.WriteLine("mutate!");
                }
            }

        }

        public void Show()
        {
            for (int i = 0; i < Ch.Length; i++)
            {
                Console.Write(Convert.ToInt32(Ch[i]) + " ");
            }
            Console.WriteLine();
        }

        private bool SwapGen(bool Gen)
        {
            if (Gen == true)
            {
                return false;
            }
            return true;
        }

        public object Clone()
        {
            return new Chromosome((bool[])Ch.Clone(),Fitness);
        }
    }
}
