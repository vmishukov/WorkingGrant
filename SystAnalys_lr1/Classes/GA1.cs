using System;
using System.Collections.Generic;
using System.Linq;
using SystAnalys_lr1;
using System.IO;




namespace SystAnalys_lr1
{
    class GA1
    {
        List<Vertex> _vertexCollection;
        List<Edge> _edgeCollection;
        int[] _steinerVertexCollection;
        //List<Point2D> _steinerVertexCollection;
        //List<Edge> _steinerEdgeCollection;
        int _populationSize;
        double _mutateRate;
        int _generaionSize;
        double _maxCost;
        double _limitCostRate;
        double _limitCost;
        int _CarCount;
        static Random _random = new Random();
        private Chromosome _betterChromosome;
        int _key;

        public GA1(List<Vertex> vertexCollection, List<Edge> edgeCollection, int[] steinerVertexCollection, /*List<Point2D> steinerVertexCollection, List<Edge> steinerEdgeCollection,*/ int populationSize, int generationSize, double maxCost, double limitCostRate, double mutateRate, int CarCount, int key)
        {
            _vertexCollection = vertexCollection;
            _edgeCollection = edgeCollection;
            //_steinerEdgeCollection = steinerEdgeCollection;
            _steinerVertexCollection = steinerVertexCollection;
            _populationSize = populationSize;
            _generaionSize = generationSize;
            _mutateRate = mutateRate;
            _maxCost = maxCost;
            _limitCostRate = limitCostRate;
            _limitCost = _maxCost;
            _CarCount = CarCount;
            _key = key;
        }

        public void Go()
        {

            List<Chromosome> population = new List<Chromosome>();//прпуляция как набор хромосом 
            GaFunctions gaFunc = new GaFunctions(_vertexCollection, _edgeCollection, _steinerVertexCollection, /*_steinerEdgeCollection,*/ _limitCost, _CarCount);
            //создали объект класса
            for (int i = 0; i < _populationSize; i++)
            {
                population.Add(gaFunc.GenerateChromosomeByCost(_random));//создаем новую популяцию
                //if (population[i].Fitness == key)
                //{
                //   _betterChromosome = population[i];
                //    break;
                //}
                    
                //population.Last().fitness = gaFunc.Fitness(population.Last());
            }
            
            _betterChromosome = population.First();//берем первый элемент последовательности
          
            File.Delete("gist.txt");
            StreamWriter print2 = new StreamWriter("gist.txt", true);
            print2.Write(_generaionSize);
            print2.WriteLine();
            for (int iteration = 0; iteration < _generaionSize; iteration++)//выполняем пока не достигнем нужного количество итераций
            {
                //if (Math.Round(_betterChromosome.Fitness) == _key)
                //{
                //    break;
                //}
                CompareChromosome(population, _key);//сравниваем первый элемент с совсеми хромосомами в популяции, находим наименьшую?
                var min = population.Select(x => x.Fitness).Max();//Проецирует каждый элемент последовательности в новую форму, вместо икс пишем эффективность, то есть вместо индекса?
                Log.WriteLog("Iteration " + iteration + ": " + (Math.Round(betterChromosome.Fitness).ToString()));// выводим номер итерации и минимальную эффективность округленную
                string splitpopulation = "";
                print2.Write(iteration);
                print2.WriteLine();
                print2.Write(betterChromosome.Fitness);
                print2.WriteLine();
                foreach (var chromosome in population)
                {
                    splitpopulation += (chromosome.Fitness.ToString()) + " ";// cоздаем строку с эффективностью для каждой хромосомы в популяции

                }
                Log.WriteLog(splitpopulation);
                population = gaFunc.TournamentSelection(population, _random);// запуск 
                //population = gaFunc.RulletSelection(population, _random);// запуск рулетки

                gaFunc.Mutate(population, _mutateRate, _random);//мутация

                population = gaFunc.KillWorst(population);//убийство

                if (population.Count < _populationSize)//если популяция не полная, набираем до конца
                {
                    population.Add(_betterChromosome);
                    var deficit = _populationSize - population.Count;
                    for (var i = 0; i < deficit; i++)
                    {
                        population.Add(gaFunc.GenerateChromosomeByCost(_random));
                    }
                }
                population = population.OrderByDescending(x => x.Fitness).ToList();
                if (Math.Round(_betterChromosome.Fitness) == _key)
                {
                    break;
                }
            }
            print2.Close();
            
        }


        private void CompareChromosome(List<Chromosome> population, int key)
        {
            foreach (var chromosome in population)
            {
                if (Math.Round(chromosome.Fitness) == key)
                {
                    _betterChromosome = chromosome;
                }
            }
        }

        public Chromosome betterChromosome
        {
            get { return _betterChromosome; }

        }

    }

}

