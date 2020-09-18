using System;
using System.Collections.Generic;
using System.Linq;
using SystAnalys_lr1;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;


namespace SystAnalys_lr1
{
    class GaFunctions
    {
        List<Vertex> _defaultPoints;
        List<Edge> _defaultEdges;
        int[] _shPoints;
        //List<Edge> _shEdge;
        double _limitCost;
        double[] _shPointsCosts;
        int[,] CarsPoint;
        int[] Chromosome_true;
        int _CarCount;

        public GaFunctions(List<Vertex> point, List<Edge> edges, int[] shPoint,/* List<Edge> shEdge,*/ double limitCost, int CarCount)
        {
            _defaultPoints = point;
            _defaultEdges = edges;
            //_shEdge = shEdge;
            _shPoints = shPoint;
            _limitCost = limitCost;
            _CarCount = CarCount/5;
            //_shPointsCosts = ShPointGetCost();
        }
        public GaFunctions() { }


        public Chromosome Crossover(Chromosome parentA, Chromosome parentB)
        {
            var chLenght = parentA.Ch.Length;
            Chromosome nextChromosome = new Chromosome(chLenght);
            int[] isUsed = new int[chLenght];
            double curentCost = 0;

            var r = new Random();
            for (int i = 0; i < chLenght; i++)
            {
                if (parentA.Ch[i] == parentB.Ch[i]) //копируем совпадающие гены родителей в потомка
                {
                    nextChromosome.Ch[i] = parentA.Ch[i];
                    if (parentA.Ch[i] == true)
                    {
                        //curentCost += _shPointsCosts[i];
                    }
                    isUsed[i] = 1;
                }
            }

            int item = 0;
            Chromosome childChromosome = null;
            while (curentCost < _limitCost)
            {
                childChromosome = (Chromosome) nextChromosome.Clone();
                var freeGene = isUsed.Where(elem => elem == 0).Count();
                var rand = r.Next(1, freeGene + 1);
                for (var i = 0; i < chLenght; i++)
                {
                    if (isUsed[i] == 0) item++;
                    if (item == rand)
                    {
                        if (nextChromosome.Ch[i] == false)
                        {
                            nextChromosome.Ch[i] = true;
                            //curentCost += _shPointsCosts[i];
                            isUsed[i] = 1;
                        }
                        item = 0;
                        break;
                    }
                }

                if (((isUsed.Where(elem => elem == 0).Count()) == 0) && (curentCost < _limitCost))
                {
                    childChromosome = (Chromosome)nextChromosome.Clone();
                    break;
                }
            }

            //var childShCount = childChromosome.Ch.Where(item => item == true).Count();
            return childChromosome;
        }


        public List<Chromosome> RulletSelection(List<Chromosome> population, Random r)
        {
            /*параметры для сохранения лучших хромосомв новой популяции*/
            //const int k = 5; //сколько % 
            //double goodnest = (population.Count * k) / 10;

            population = population.OrderBy(x => x.Fitness).ToList();
            int populationSize = population.Count;
            var chromasomeLenght = population[0].Ch.Length;
            List<Chromosome> newPopulation = new List<Chromosome>();
            var parentA = new Chromosome(chromasomeLenght);
            var parentB = new Chromosome(chromasomeLenght);
            Chromosome child;
            var parentNum = 0;
            List<double> P_sel = new List<double>();
            double fitnessSum = 0;


            List<Chromosome> populationToSelection = new List<Chromosome>();
            population.Add((Chromosome) population[0].Clone());
            foreach (var chromosome in population)
            {
                if (!populationToSelection.Any(item => item.Fitness == chromosome.Fitness))
                {
                    populationToSelection.Add((Chromosome) chromosome.Clone());
                }
            }

            foreach (var chromosome in populationToSelection)
            {
                fitnessSum += 1/chromosome.Fitness;
            }

            foreach (var chromosome in populationToSelection)
            {
                P_sel.Add((1/ chromosome.Fitness) /(fitnessSum));
            }

            var d = P_sel.Sum();
        
            //Добавление 10% лучших хромосом в популяцию
            /*for (int i = 0; i < goodnest; i++)
            {
                parentA = (Chromosome)population[i].Clone();
                var rn = r.NextDouble();
                double sum = 0;
                int j = 0;
                while (rn > sum)
                {
                    if (i >= populationSize) break;
                    sum += P_sel[j];
                    j++;
                }
                parentB = (Chromosome) population[j - 1].Clone();
                child = Crossover(parentA, parentB);
                child.Fitness = Fitness(child);
                if ((child.Fitness < parentA.Fitness) && (child.Fitness < parentB.Fitness))
                {
                    newPopulation.Add((Chromosome)child.Clone());
                }
                else
                {
                    newPopulation.Add((Chromosome)parentA.Clone());
                }
            }*/
            //newPopulation.Add((Chromosome)population[0].Clone()); //лучшая особь хромосомы всегда попадает в популяцию

            while (newPopulation.Count < populationSize)
            {
                var rn = r.NextDouble();
                double sum = 0;
                int i = 0;
                while (rn >= sum)
                {
                    if (i >= populationSize) break; 
                    sum = sum + P_sel[i];
                    i++;
                }
                if (parentNum == 0)
                {
                    parentA = (Chromosome)populationToSelection[i - 1].Clone();
                    parentNum = 1;
                }
                else if (parentNum == 1)
                {
                    parentB = (Chromosome)populationToSelection[i - 1].Clone();
                    child = Crossover(parentA, parentB);
                    child.Fitness = Fitness(child);
                    if ((child.Fitness < parentA.Fitness) && (child.Fitness < parentB.Fitness))
                    {
                        newPopulation.Add((Chromosome) child.Clone());   
                    }
                    else
                    {
                        newPopulation.Add(parentA.Fitness < parentB.Fitness ? (Chromosome)parentA.Clone() : (Chromosome) parentB.Clone());
                    }
                    parentNum = 0;
                }
            }
            return newPopulation;
        }


        public float Fitness(Chromosome chromosome)
        {
            
            int lenght = 2;
            CarsPoint = new int[_CarCount, lenght];
            Chromosome_true = new int[Convert.ToInt32(_limitCost)];
            int tpoints;
            tpoints = Convert.ToInt32(_limitCost);
            //var tpoints = new List<Vertex>();
            ////Array.Copy(tpoints, 0, _shPoints, 0, Convert.ToInt32(_limitCost));
            int CountCarOn = 0;
            double Percent = 0;

            StreamReader print2 = new StreamReader("mas26.txt");
            for (int j = 0; j < _CarCount; j++)
            {
                string line1 = print2.ReadLine();
                string[] fields = line1.Split('|');
                for (int k = 0; k < lenght; k++)
                {
                    CarsPoint[j, k] = Convert.ToInt32(fields[k]);
                }

            }
            print2.Close();
            int m = 0;
            for (int l = 0; l < _defaultPoints.Count; l++)
            {
                if (m < tpoints)
                {
                    if ((chromosome.Ch[l]) && (m < tpoints))
                    {
                        Chromosome_true[m] = l;
                        m++;
                    }
                }
                 
            }
            m = 0;
            for (int j = 0; j < _CarCount; j++)
            {
                for (int k = 0; k < lenght; k++)
                {
                    for (int l = 0; l < Chromosome_true.Length; l++)
                    {
                        if (CarsPoint[j, k] == Chromosome_true[l])
                        {
                            CountCarOn++;
                            m = 2;
                            break;
                        }
                    }
                    if (m == 2)
                    {
                        m = 0;
                        break;
                    }

                }
            }
            Percent = Convert.ToDouble(CountCarOn / (_CarCount / 100M));
            return (float)Percent;
        }
                
            

            //    var kr = new TryKruskal();


            //    List<Edge> kruskalResult = kr.DoKruskal(tpoints, tedge);

            //    foreach (var edge in kruskalResult)
            //    {
            //        if (edge != null)
            //        {
            //            distance = distance + edge.Weight;
            //        }
            //    }
           
        

        public List<Chromosome> TournamentSelection(List<Chromosome> population, Random r)
        {
            var popSize = population.Count;
            List<Chromosome> newPopulation = new List<Chromosome>();
            while (population.Count%3 != 0)//увеливаем популяцию в трое
            {
                population.Add(new Chromosome());
            }
            var etalonChromosom = new List<Chromosome>();
            var tournirList = new List<Chromosome>();
            for (int i = 0; i < population.Count; i+=3)
            {
                tournirList.Add(population[i]);
                tournirList.Add(population[i + 1]);
                tournirList.Add(population[i + 2]);    
                etalonChromosom.Add(tournirList.Find(x => x.Fitness == tournirList.Max(y => y.Fitness)));
                tournirList.Clear();
            }
            for (int i = 0; i < popSize; i++)
            {
                var parentA = etalonChromosom[r.Next(etalonChromosom.Count)];
                var parentB = etalonChromosom[r.Next(etalonChromosom.Count)];
                var child = Crossover(parentA, parentB);
                child.Fitness = Fitness(child);
                if ((child.Fitness > parentA.Fitness) && (child.Fitness > parentB.Fitness))
                {
                    newPopulation.Add((Chromosome)child.Clone());
                }
                else
                {
                    newPopulation.Add(parentA.Fitness < parentB.Fitness ? (Chromosome)parentB.Clone() : (Chromosome)parentA.Clone());
                }
            }
            return newPopulation;
        }

        public List<Chromosome> KillWorst(List<Chromosome> population)
        {
            const int k = 10; //сколько % убить
            var sortedPopulation = population.OrderBy(x => x.Fitness).ToList();
            double killCount = (sortedPopulation.Count * k) / 100;
            //for (int i = 0; i < (int)killCount; i++)
            //{
            //    var min = population.Select(x => x.Fitness).Min();//Проецирует каждый элемент последовательности в новую форму, вместо икс пишем эффективность, то есть вместо индекса?
            //    foreach (var chromosome in population)
            //    {
            //        if (chromosome.Fitness == min)
            //        { population.Remove(chromosome); }
            //    }
            //}
            sortedPopulation = population.OrderByDescending(x => x.Fitness).ToList();
            sortedPopulation.RemoveRange((int)(sortedPopulation.Count - killCount), (int)killCount); // указанное количество элементов, начиная с указанной позиции

            
            return sortedPopulation;//возвращаем укрощенную 
        }

        public Chromosome GenerateChromosomeByCost(Random r)//cоздание хромосомы исходя из стоимости
        {
            var nextChromosome = new Chromosome(_defaultPoints.Count);//cоздаем популяцию, размером с _shPoints(возможно должен быть другой размер, список всех точек, а не количество допучтимых), все хромосомы false
            Chromosome chromasome = null;
            int a = 0;
            while (a <= _limitCost)//Cравниваем чтото с возможном количеством точек, параметр для создания популяции хромосом 
            {
                chromasome = (Chromosome)nextChromosome.Clone();//копируем хромосому из nextChromosome в chromasome
                nextChromosome.AddOneGen(r);// меняем значение у случайной хромосомы на true
                a++;
            }
            chromasome.Fitness = Fitness(chromasome);//cделать так что бы было можно проверят эффективность только одной точки
            return chromasome;//
        }

        public double ChromosomeCost(Chromosome chromosome)
        {
            double distance = 0;
            for (int i = 0; i < chromosome.Ch.Length; i++) /*говнокод*/
            {
                //if (chromosome.Ch[i])
                //{
                //    distance += _shEdge[i*3].Weight;
                //    distance += _shEdge[i * 3 + 1].Weight;
                //    distance += _shEdge[i * 3 + 2].Weight;
                //}
            }
            return distance;
        }

        public double ChromosomeCostByIndex(Chromosome chromosome)//Что это? 
        {
            double distance = 0;
            for (int i = 0; i < chromosome.Ch.Length; i++)
            {
                if (chromosome.Ch[i])
                {
                    distance += _shPointsCosts[i];
                }
            }
            return distance;
        }

        public void Mutate(List<Chromosome> population, double mutateRate, Random r)
        {
            var chLength = population[0].Ch.Length;
            foreach (var chromasome in population)
            {
                var tempChromosome = (Chromosome)chromasome.Clone();
                for (var i = 0; i < chLength; i++)
                {
                    if (r.NextDouble() < mutateRate)
                    {
                        tempChromosome.Ch[i] = SwapGen(tempChromosome.Ch[i]);
                    }
                }
                tempChromosome.Fitness = Fitness(tempChromosome);
                if (tempChromosome.Fitness > chromasome.Fitness) 
                {
                    for (int i = 0; i < chLength; i++)
                    {
                        chromasome.Ch[i] = tempChromosome.Ch[i];
                    }
                    chromasome.Fitness = Fitness(chromasome);
                }
            }
        }

        private double[] ShPointGetCost()
        {
            double[] result = new double[_shPoints.Length];
            for (int i = 0; i < _shPoints.Length; i++)
            {
                double cost = 0;
                //cost += _shEdge[i * 3].Weight;
                //cost += _shEdge[i * 3 + 1].Weight;
                //cost += _shEdge[i * 3 + 2].Weight;
                result[i] = cost;
            }
            return result;
        }

        private bool SwapGen(bool Gen)
        {
            if (Gen == true)
            {
                return false;
            }
            return true;
        }


    }
}
