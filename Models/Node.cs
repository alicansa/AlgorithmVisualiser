using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public interface INode {
    string Id {get;set;}
}


public class Node : INode{

    [BsonElement("id")]
    public string Id {get;set;}

    //a node contains a dictionary of neighboring nodes and their distances to itself
    [BsonElement("neighbors")]
    public IList<NodeNeighbor> Neighbors {get;set;}

    public void AddNeighbor(NodeNeighbor neighbor){
        Neighbors.Add(neighbor);
    }
    // public bool RemoveNeighbor(INode neighbor){

    //     return this.Neighbors.Remove(neighbor);

    // }

    public bool RemoveNeighbor(string neighborId){

        return this.Neighbors.Remove((from neighbor in Neighbors
                                     where neighbor.Id.Equals(neighborId)
                                     select neighbor).First());

    }

    public Node(string id){
        Id = id;
        Neighbors = new List<NodeNeighbor>();
    }
    
}

[BsonIgnoreExtraElements]
public class NodeNeighbor : INode{

    [BsonElement("id")]
    public string Id {get;set;}
     
    [BsonElement("distance")]
    public int Distance {get;set;}

    public NodeNeighbor(){
    
    }

    public NodeNeighbor(string id,int distance){
        Id = id;
        Distance = distance;
    }

}

