using NetworkSolver.Common.Topology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetworkSolver.DataAccess.InputFileParser
{
    public class InputFileParser
    {
        Network result;

        public Network ReadNetwork(string fileName) 
        {
            string pathToFile = System.IO.Path.Combine(Directory.GetCurrentDirectory(), $"Resources\\{fileName}");
            if (File.Exists(pathToFile))
                return ParseFileToNetwork(pathToFile);
            

            Console.WriteLine("File does not exist");
            return null;
        }

        public Network ReadNetworkFromInput(List<string> input)
        {
            return ParseInputToNetwork(input);
        }

        private Network ParseFileToNetwork(string pathToFile)
        {
            result = new Network();
            List<string> fileLines = File.ReadAllLines(pathToFile).ToList();
            AddNodesToNetwork(fileLines);
            AddNodesConnections(fileLines);
            AddLinksToNetwork();
            return result;
        }

        private Network ParseInputToNetwork(List<string> input)
        {
            result = new Network();

            AddNodesToNetwork(input);
            AddNodesConnections(input);
            AddLinksToNetwork();

            return result;
        }

        private void AddLinksToNetwork()
        {
            int currentLinkId = 1;
            foreach(Node currentNode in result.Nodes)
            {
                foreach(Node connectedToCurrentNode in currentNode.ConnectedNodes)
                {
                    if (result.FindLinkByNodes(currentNode.NodeId, connectedToCurrentNode.NodeId) == null) 
                    {
                        result.Links.Add(new Link(currentLinkId, currentNode.NodeId, connectedToCurrentNode.NodeId));
                        currentLinkId++;
                    }
                }
            }
        }

        private void AddNodesConnections(List<string> fileLines)
        {
            foreach(var line in fileLines)
            {
                var currentNodeId = int.Parse(line.Split(':')[0]);
                var currentNode = result.Nodes.Find(x => x.NodeId == currentNodeId);
                var connectedNodes = line.Split(':')[1].Split(',');

                foreach(var connectedNodeId in connectedNodes) 
                {
                    currentNode.ConnectedNodes.Add(result.Nodes.Find(x => x.NodeId == int.Parse(connectedNodeId)));
                }              
            }
        }

        private void AddNodesToNetwork(List<string> fileLines)
        {
            for(int i=0; i<fileLines.Count(); i++)
            {
                int nodeId = Int32.Parse(fileLines[i].Split(':')[0]);
                result.Nodes.Add(new Node(nodeId));
            }
        }
    }
}
