using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StationNode : MonoBehaviour
{
    [SerializeField] RailManager railManager;

    [Header("Neighbour node info")]
    [SerializeField] public List<RailNode> neighbours;    // Relevant rail nodes
    [SerializeField] public List<float> neighbourDistance;
    [SerializeField] public List<Quaternion> neighbourDirection;
    List<StationNode> stations; // Stations available to the current station
    List<List<RailNode>> stationRoutes; // Routes to the available stations

    // Start is called before the first frame update
    void Start()
    {
        GetNeighbourInfo();
        FindStationRoutes();

        transform.rotation = neighbourDirection[0];
    }

    void GetNeighbourInfo()
    {
        foreach (RailNode node in neighbours)
        {
            neighbourDistance.Add(Vector3.Distance(this.transform.position, node.transform.position));
            neighbourDirection.Add(Quaternion.LookRotation(this.transform.position - node.transform.position, Vector3.up));
        }
    }

    void FindStationRoutes()
    {
        // Attempt 1
        //float nodeDistance = float.MaxValue;

        //// Initialize an empty set of visited nodes and nodes to visit
        //List<RailNode> visited = new List<RailNode>();
        //Queue<RailNode> toVisit = new Queue<RailNode>();

        //RailNode target = stations[0].neighbours[0];

        //toVisit.Enqueue(target);

        //while (toVisit.Count > 0)
        //{
        //    RailNode currentNode = toVisit.Dequeue();
        //    // Calculate the node distance
        //    if (currentNode == target)
        //    {
        //        nodeDistance = 0;
        //    }

        //    // Add to queue
        //    visited.Add(currentNode);

        //    // Find available next nodes
        //    List<RailNode> nextNodes = currentNode.neighbours;
        //    List<RailNode> filteredNodes = nextNodes.Where(nodes => !visited.Contains(nodes)).ToList();

        //    // Enqueue them
        //    foreach (var node in filteredNodes)
        //    {
        //        // Visit node and get distance
        //        float distance;
        //        float newDistance;
        //        for (int i = 0; i < node.neighbours.Count; i++)
        //        {
        //            if (node.neighbours[i] == currentNode)
        //            {
        //                distance = neighbourDistance[i];
        //                newDistance = nodeDistance + distance;
        //                nodeDistance = Mathf.Min(nodeDistance, newDistance);
        //                break;
        //            }
        //        }

        //        // Schedule node to be visited
        //        toVisit.Enqueue(node);
        //    }
        //}

        //// Check these to see if we're reaching a station
        //List<List<RailNode>> alerterNodes = new List<List<RailNode>>();
        //for (int i = 0; i < railManager.stationNodes.Count; i++)
        //{
        //    alerterNodes.Add(railManager.stationNodes[i].neighbours);
        //}

        // Attempt 2
        //// Initialize an empty set of visited nodes and nodes to visit
        //List<RailNode> visited = new List<RailNode>();

        //// Initialize a list for distances and set all distances as float.max
        //List<float> distances = new List<float>();
        //foreach (RailNode node in railManager.nodes)
        //{ distances.Add(float.MaxValue); }

        //// Insert source itself in priority and initialize its distance as 0;
        //for (int i = 0; i < railManager.nodes.Count; i++)
        //{
        //    if (railManager.nodes[i] == neighbours[0])
        //    {
        //        visited.Add(railManager.nodes[i]);
        //        distances[i] = 0.0f;
        //        break;
        //    }
        //}

        //for (int i = 0; i < visited.Last().neighbours.Count; i++)
        //{
        //    if (visited.Last().neighbours.Contains(visited.Last().neighbours[i]))
        //    { continue; }


        //}

        //foreach (RailNode nodes in neighbours)
        //{
        //    for (int i = 0; i < nodes.neighbours.Count; i++)
        //    {
        //        if (nodes == neighbours[i])
        //        {
        //            visited.Add(nodes);
        //            startIndex = i;
        //        }
        //        else
        //            distance += neighbourDistance[i];


        //    }
        //}

        // Attempt 3?
        //// Set nodes as infinity
        //List<float> distance = new List<float>();
        //List<bool> sptSet = new List<bool>();

        //for (int i = 0; i < railManager.nodes.Count; i++)
        //{
        //    sptSet.Add(false);
        //    distance.Add(float.MaxValue);
        //}

        //visited.Add(neighbours[0]);
        //toVisit.Enqueue(neighbours[1]);

        //sptSet[0] = true;
        //distance[0] = 0.0f;

        //sptSet[1] = true;
        //distance[1] = neighbourDistance[1];

        //foreach (StationNode station in railManager.stationNodes)
        //{
        //    // No need to find the shortest path to itself
        //    if (station == this)
        //        continue;

        //    for (int i = 0; i < station.neighbours.Count; i++) 
        //    {
        //        if (visited.Contains(station.neighbours[i]))
                    
        //    }
        //}

        // Plan
        // Go in each direction (i.e: do twice)
        // if the station was found in a previous iteration, save the station in the stations List, and the route in the stationRoutes List
        // if the current node is connected to more than two nodes, create another iteration for the second route (nodes are only connected to two nodes, so far)
        // if there is a junction, split off until you find the station (what does "split off" mean?)
        // if you find the station, 

        // It will follow the Nodes until it reaches a Station
        // That Station it will put in the stations List, with the shortest Node Route to it in its respective Position in the station route List
        // If there is an other shortest Route using the other Direction, the Route will be placed in the double [index] of its Position in the stations List
        // (If the Train can't find a Route in front of It, It will turn around and use the Route behind It instead, be that the primary or secondary Route)
    }

    public List<RailNode> GetRoute(StationNode toNode)
    {
        int index = 0;

        // Find whether there exists a route to that node
        foreach (StationNode node in stations)
        {
            if (node == toNode)
                break;
            index++;
        }

        if (stations.Count - 1 <= index && stations[index] != null)
        {
                return stationRoutes[index];
        }

        return null;
    }

    public List<RailNode> GetOtherRoute(StationNode toNode)
    {
        int index = 0;

        // Find whether there exists a route to that node
        foreach (StationNode node in stations)
        {
            if (node == toNode)
                break;
            index++;
        }

        if (stations.Count - 1 <= index * 2 && stations[index * 2] != null)
        {
                return stationRoutes[index];
        }
        
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
