using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StationNode : MonoBehaviour
{
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
        transform.rotation = neighbourDirection[0];

        // OBS! Planning to implement the station snapping on to the railway!
        for (int i = 0; i < neighbours.Count; i++)
            neighbourDistance.Add(Vector3.Distance(neighbours[i].transform.position, this.transform.position));

        FindStationRoutes();
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
        List<float> distance = new List<float>();
        List<bool> sptSet = new List<bool>();
        List<RailNode> SPT = new List<RailNode>();

        int start1 = 0, start2 = 0;

        
        for (int i = 0; i < neighbours.Count - 1; i++)
        {
            sptSet.Add(false);
            distance.Add(float.MaxValue);
            if (neighbours[i] == this.neighbours[0]) start1 = i;
            else if (neighbours[i] == this.neighbours[1]) start2 = i;
        }



        // Go in each direction
        // if the station was found in a previous iteration, save the station in the stations List, and the route in the stationRoutes List
        // if the current node is connected to more than two nodes, create another iteration for the second route
        // if there is a junction, split off until you find the station
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
