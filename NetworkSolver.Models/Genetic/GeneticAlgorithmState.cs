using NetworkSolver.Common.Topology;
using System.Diagnostics;

namespace NetworkSolver.Common.Genetic
{
    public class GeneticAlgorithmState
    {
        public Stopwatch ElapsedTime { get; set; }

        public int NumberOfGenerations { get; set; }

        public int NumberOfMutations { get; set; }

        public int NumberOfGenerationsWithoutImprovement { get; set; }

        public Network BestChromosomeOptimizationResult { get; set; }

        public NetworkSolution BestChromosomeNetworkSolution { get; set; }

        public int BestChromosomeFitness { get; set; }
    }
}
