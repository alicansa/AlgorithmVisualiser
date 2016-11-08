using System.Collections.Generic;
using System;
using System.Linq;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization.Attributes;
using DijkstraApi.Helpers.Utils;



// Paths consists of linked nodes
// each node can be linked to multiple nodes
// send each node with its neighbors and their distances to initialize the class
//

namespace DijkstraApi.Models
{
    [BsonIgnoreExtraElements]
    public class Map {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name {get;set;}
          
        [BsonElement("shortestPaths")]
        public Dictionary<string,Dictionary<string,Path<string>>> ShortestPaths {get;set;}

        [BsonElement("nodes")]
        public Dictionary<string,Node> Nodes {get;set;}

        public Map(string name, Dictionary<string,Node> nodes){
            //use this constructor to create the shortest Paths
            Name = name;
            Nodes = nodes;
            ShortestPaths = new Dictionary<string,Dictionary<string,Path<string>>>();
            this.ComputeAllMinPaths();   
        }

        public Map(string name){
            Name = name;
            ShortestPaths = new Dictionary<string,Dictionary<string,Path<string>>>();
            Nodes = new Dictionary<string,Node>();
        }

        public void AddNode(Node node){
            //add the node
            Nodes.Add(node.Id, node);
        }

        public bool RemoveNode(string nodeId){

            if (Nodes.ContainsKey(nodeId)){

                foreach(Node mapNode in Nodes.Values){

                    INode nodeNeighbor = (from nodes in Nodes[mapNode.Id].Neighbors
                                                where nodes.Id.Equals(nodeId)
                                                select nodes).First(); //there must be only one

                    Nodes[nodeNeighbor.Id].RemoveNeighbor(nodeNeighbor.Id);

                }

                Nodes.Remove(nodeId);
                //compute all the min paths
                ComputeAllMinPaths();
                return true;

            }else return false;
            //go through all the nodes and delete the node from their neighbor nodes
            

        }

        public bool AddConnection(string startNodeId, string endNodeId, int distance){
            
            if (Nodes.ContainsKey(endNodeId) && Nodes.ContainsKey(startNodeId)){    
                Nodes[startNodeId].AddNeighbor(new NodeNeighbor(endNodeId,distance));
                ComputeAllMinPaths();
                return true;
            }
            else return false;
        }

        public bool RemoveConnection(string startNodeId, string endNodeId){

            if (Nodes[startNodeId].RemoveNeighbor(endNodeId)){
                //compute all the min paths 
                ComputeAllMinPaths();
                return true;
            }

            return false;
            
        }

        public void ComputeMinPaths(string rootNodeName){

            Node startNode = Nodes[rootNodeName];
            string currentNodeName;
            BinaryHeap<Path<string>> unusedPaths = new BinaryHeap<Path<string>>(BinaryHeapType.MIN);
            
            // create all possible paths first
            Dictionary<string,Path<string>> pathDict = new Dictionary<string,Path<string>>();
            ShortestPaths.Add(rootNodeName,pathDict);

            foreach(string name in Nodes.Keys){

                Path<string> path; 

                if (name.Equals(rootNodeName)){
                    path = new Path<string>(new string[]{rootNodeName});
                    path.Distance = 0;
                }
                else{ 
                    path = new Path<string>(new string[]{rootNodeName,name});
                    path.Distance = Int32.MaxValue;
                }
                
                pathDict.Add(name,path);
                unusedPaths.Add(path);
            }

            while(unusedPaths.Count > 0){

                //get the destination node of the path with the smallest global distance
                currentNodeName = unusedPaths[0][unusedPaths[0].Count-1];
                Path<string> currentPath = ShortestPaths[rootNodeName][currentNodeName];
                //get all neighboring nodes and calculate the distance
                foreach (NodeNeighbor neighbor in Nodes[currentNodeName].Neighbors){ // update all distance
                   
                    int neighborDistance = neighbor.Distance;
                    Path<string> neighborPath = ShortestPaths[rootNodeName][neighbor.Id];

                    int oldDistance = neighborPath.Distance;
                    int newDistance = currentPath.Distance + neighborDistance;

                    if (newDistance < oldDistance && newDistance >= 0){
                        //merge the two paths

                        neighborPath.Clear();
                        foreach (string name in currentPath)
                            neighborPath.Add(name);
                        
                        neighborPath.Add(neighbor.Id);
                        neighborPath.Distance = newDistance; 

                    }
                }
                unusedPaths.Remove(currentPath);
            }
  
        }
        public void ComputeAllMinPaths(){
             ShortestPaths = new Dictionary<string,Dictionary<string,Path<string>>>();
            //create all the shortest paths
            foreach(string startNodeName in Nodes.Keys){
                ComputeMinPaths(startNodeName);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

      
     }

     //MapListItem
     public class MapSummary{

         [BsonId]
         [BsonRepresentation(BsonType.ObjectId)]
          public string Id { get; set; }

         [BsonElement("name")]
          public string Name {get;set;}
     }
}
