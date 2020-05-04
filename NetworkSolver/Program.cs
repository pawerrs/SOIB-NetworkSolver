using NetworkSolver.Common;
using NetworkSolver.Common.Genetic;
using NetworkSolver.Common.Topology;
using NetworkSolver.DataAccess.InputFileParser;
using NetworkSolver.Services.GeneticService;
using System;

namespace NetworkSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            InputFileParser parser = new InputFileParser();
            Console.WriteLine("Choose network to optimize:");
            Console.WriteLine("1: Network_1.txt");
            Console.WriteLine("2: Network_2.txt");
            string choice = Console.ReadLine();
            Network inputNetwork;
            if (choice.Equals("1") || choice.Equals("2"))
            {
                inputNetwork = parser.ReadNetwork($"Network_{choice}.txt");
                if (inputNetwork == null)
                    return;
            }
            else
            {
                return;
            }

            //Find all node pairs
            PathFinder pathFinder = new PathFinder();

            for (int i = 1; i < inputNetwork.Nodes.Count; i++)
            {
                for (int j = i + 1; j < inputNetwork.Nodes.Count + 1; j++)
                {
                    pathFinder.FindAllPaths(i, j, inputNetwork);
                }
            }

            // all paths generated. Finding perfect combination.
            GeneticAlgorithmParameters parameters = new GeneticAlgorithmParameters
            {
                InitialPopulationSize = 100,
                CrossoverProbability = (float)0.2,
                MutationProbability = (float)0.1,
                RandomSeed = 4253,
                LimitValue = 30,
                StoppingCriteria = StoppingCriteria.NoImprovement
            };

            new GeneticService(parameters, inputNetwork, pathFinder, $"NetworkSolution_{choice}").Solve();
        }
    }
}
