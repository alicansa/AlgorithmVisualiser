using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Path<T>: IComparable{
            //contains a list of node names + the distance between start and end 

    [BsonElement("nodePointers")]
    public IList<T> NodePointers {get;set;}

    [BsonElement("distance")]
    public int Distance {get;set;}


    [BsonIgnoreAttribute]
    public int Count
    {
        get
        {
            return NodePointers.Count;
        }
    }

    public T this[int index]
    {
        get
        {
            return NodePointers[index];
        }

        set
        {
            NodePointers[index] = value;
        }
    }

    public Path(T[] nodes,int distance){
        NodePointers = new List<T>(nodes);
        Distance = distance;
    }

    public Path(T[] nodes){
        NodePointers = new List<T>(nodes);
    }

    public int CompareTo(object obj)
    {
        return this.Distance.CompareTo(((Path<T>)obj).Distance);
    }

    public IEnumerator GetEnumerator()
    {
        return NodePointers.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return NodePointers.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        NodePointers.Insert(index,item);
    }

    public void RemoveAt(int index)
    {
        NodePointers.RemoveAt(index);
    }

    public void Add(T item)
    {
        NodePointers.Add(item);
    }

    public void Clear()
    {
        NodePointers.Clear();
    }

    public bool Contains(T item)
    {
        return NodePointers.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        NodePointers.CopyTo(array,arrayIndex);
    }

    public bool Remove(T item)
    {
        return NodePointers.Remove(item);
    }
}