using NetworkSolver.Common;
using NetworkSolver.Common.Genetic;
using NetworkSolver.Common.Topology;
using NetworkSolver.DataAccess.InputFileParser;
using NetworkSolver.Services.GeneticService;
using System;
using System.Collections.Generic;

namespace NetworkSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new InputFileParser();

            // get input network from user
            Console.WriteLine("Please specify network topology which you want to solve.");

            Console.Write("How many nodes are in the network? ");
            var nodesCountInput = Console.ReadLine();

            if (nodesCountInput is null)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var nodesCount = int.Parse(nodesCountInput);
            var nodeConnections = new List<string>(nodesCount);
            for (var i = 1; i <= nodesCount; i++)
            {
                Console.WriteLine($"Enter nodes reachable from node {i} separated by comma: ");
                var currentNodeConnectionsInput = Console.ReadLine();
                
                nodeConnections.Add($"{i}:{currentNodeConnectionsInput}");
            }

            var inputNetwork = parser.ReadNetworkFromInput(nodeConnections);

            // find all node pairs
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

            new GeneticService(parameters, inputNetwork, pathFinder).Solve();
        }
    }
}
