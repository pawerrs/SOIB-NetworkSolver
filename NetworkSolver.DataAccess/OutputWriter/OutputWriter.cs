using NetworkSolver.Common.Topology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetworkSolver.DataAccess.OutputWriter
{
    public class OutputWriter
    {
        private static StreamWriter CreateFile(string fileName)
        {
            bool freeNameNotFound = true;
            int currentFileNameNumber = 1;
            string baseFileName = fileName;
            string pathToFile, newFileName;
            do
            {
                newFileName = baseFileName + "_" + currentFileNameNumber;
                pathToFile = System.IO.Path.Combine(Environment.CurrentDirectory, newFileName);
                if (!File.Exists(pathToFile + ".txt"))
                {
                    freeNameNotFound = false;
                }
                currentFileNameNumber++;
            } 
            while (freeNameNotFound);
            Console.WriteLine("Saving output to: {0}.txt", newFileName);


            return File.CreateText(pathToFile + ".txt");
        }

        public void SaveOutputToTheFile(Network bestChromosomeOptimizationResult, List<PathAllocation> paths,
            bool isNetworkFromFile, string fileName)
        {
            var filePath = $"{Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName).FullName}\\NetworkSolutions\\";
            filePath = isNetworkFromFile ? $"{ filePath }{ fileName }" : $"{ filePath }Solution_{ GetCurrentDateTime() }";

            using StreamWriter fileStream = CreateFile(filePath);

            fileStream.WriteLine("Solution for " + fileName + ".txt");
            fileStream.WriteLine("Used lambdas: {0}", bestChromosomeOptimizationResult.HighestLambdaId);
            fileStream.WriteLine("Defined demands: (Demand_ID:  First_Node_ID  Second_Node_ID)");

            foreach (var pathAllocation in paths)
            {
                fileStream.WriteLine($"{pathAllocation.Demand.DemandId}: {pathAllocation.Demand.StartNode.NodeId} {pathAllocation.Demand.EndNode.NodeId}");
            }

            fileStream.WriteLine();
            fileStream.WriteLine("Links defined in network: (Link_ID: Start_Node - End_Node)");

            bestChromosomeOptimizationResult.Links.OrderBy(x => x.LinkId);

            foreach (var link1 in bestChromosomeOptimizationResult.Links)
            {
                fileStream.WriteLine($"{link1.LinkId}: {link1.ConnectedByLink[0]}-{link1.ConnectedByLink[1]}");
            }

            fileStream.WriteLine();
            fileStream.WriteLine("Chosen paths for each demand: (Demand_ID: <list of links>)");

            foreach (var pathAllocation1 in paths)
            {
                var sb = new StringBuilder();
                foreach (var link in pathAllocation1.ChosenPath.PathLinks)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(link.LinkId);
                }
                fileStream.WriteLine($"{pathAllocation1.Demand.DemandId}:  {sb}");
            }

            fileStream.WriteLine();
            fileStream.WriteLine("Easier to imagine version: (Start_node -> End_Node: *Lambda_ID* <list of nodes on path>)");

            foreach (PathAllocation pathAllocation2 in paths)
            {
                var sb = new StringBuilder();
                foreach (Node node in pathAllocation2.ChosenPath.PathNodes)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(" -> ");
                    }
                    sb.Append(node.NodeId);
                }

                fileStream.WriteLine($"{pathAllocation2.Demand.StartNode.NodeId} -> {pathAllocation2.Demand.EndNode.NodeId}: *{pathAllocation2.LambdaId}* {sb}");
            }

            fileStream.WriteLine();
            fileStream.WriteLine("Links load (Link_ID:  <list of lambdas>)");

            foreach (var link in bestChromosomeOptimizationResult.Links)
            {
                var sb = new StringBuilder();
                foreach (int lambdaId in link.LambdasIds)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(" ");
                    }
                    sb.Append(lambdaId);
                }

                fileStream.WriteLine($"{link.LinkId}: {sb}");
            }
        }
        private string GetCurrentDateTime()
        {
            return DateTime.Now.ToString("yyyyMMdd_HHmmss");
        }
    }
}
