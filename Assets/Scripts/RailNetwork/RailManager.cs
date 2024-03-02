using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.CoreUtils;

public class RailManager : MonoBehaviour
{
    [Header("Neighbour node info")]
    [SerializeField] public List<RailNode> nodes;    // Relevant rail nodes

    [Header("Station node info")]
    [SerializeField] public List<StationNode> stationNodes;  // Relevant station nodes

    void Start()
    {
        foreach (RailNode rNode in nodes)
        {
            rNode.GetNeighbourInfo();
        }

        foreach (StationNode sNode in stationNodes)
        {
            sNode.GetNeighbourInfo();
        }
        foreach (StationNode sNode in stationNodes)
        {
            sNode.FindStationRoutes();
        }
        foreach (StationNode sNode in stationNodes)
        {
            sNode.transform.rotation = sNode.neighbourDirection[0];
        }
    }
}

public class StationRoute
{
    StationNode startStation;
    public StationNode endStation;
    public List<RailNode> railRoute;

    public StationRoute(StationNode _startStation, StationNode _endStation)
    {
        startStation = _startStation;
        endStation = _endStation;
        railRoute = new List<RailNode>();
        FindRoute();
    }

    void FindRoute()
    {
        List<float> distance = new List<float>();

        // Initialize an empty set of visited nodes and nodes to visit
        List<RailNode> visited = new List<RailNode>();
        List<RailNode> toVisit = new List<RailNode>();

        // Start with the neighbouring nodes of the start node
        toVisit.Add(startStation.neighbours[0]);
        distance.Add(startStation.neighbourDistance[0]);
        Debug.Log($"{startStation.name}-{endStation.name}: distance[{distance.Count - 1}] which is {toVisit.Last().name} is {distance.Last()} units from {startStation.name}");

        toVisit.Add(startStation.neighbours[1]);
        distance.Add(startStation.neighbourDistance[1]);
        Debug.Log($"{startStation.name}-{endStation.name}: distance[{distance.Count - 1}] which is {toVisit.Last().name} is {distance.Last()} units from {startStation.name}");

        for (int i = 0; i < startStation.neighbours.Count; i++)
        {
            // Add the unvisited neighbours of the last visited node to the toVisit list
            for (int x = 0; x < startStation.neighbours[i].neighbours.Count; x++)
            {
                if (!visited.Contains(startStation.neighbours[i].neighbours[x]) &&
                    !toVisit.Contains(startStation.neighbours[i].neighbours[x]))
                {
                    toVisit.Add(startStation.neighbours[i].neighbours[x]);
                    distance.Add(startStation.neighbours[i].neighbourDistance[x] + startStation.neighbourDistance[i]);
                    Debug.Log($"{startStation.name}-{endStation.name}: distance[{distance.Count - 1}] which is {toVisit.Last().name} is {distance.Last()} units from {startStation.name}");
                }
            }
            visited.Add(toVisit[i]);
        }

        // Find the distance from the startNode to all other nodes
        for (int i = 0; i < toVisit.Count; i++)
        {
            // First, check if this node has been visited
            if (visited.Contains(toVisit[i]))
            {
                continue;
            }

            // Add the unvisited neighbours of the last visited node to the toVisit list
            for (int x = 0; x < toVisit[i].neighbours.Count; x++)
            {
                if (!visited.Contains(toVisit[i].neighbours[x]) &&
                    !toVisit.Contains(toVisit[i].neighbours[x]))
                {
                    toVisit.Add(toVisit[i].neighbours[x]);
                    distance.Add(toVisit[i].neighbourDistance[x] + distance[i]);
                    Debug.Log($"{startStation.name}-{endStation.name}: distance[{distance.Count - 1}] which is {toVisit.Last().name} is {distance.Last()} units from {startStation.name}");
                }
            }

            // Confirm our visit to this node
            visited.Add(toVisit[i]);
        }

        // Find the shortest route
        // Get the first two neighbours of the endnode
        List<RailNode> shortList = new List<RailNode>();

        // Sets up the first nodes
        RailNode last = endStation.neighbours[0];
        float longestDist = float.MaxValue;
        for (int i = 0; i < toVisit.Count; i++)
        {
            if (endStation.neighbours[0] == toVisit[i])
            {
                longestDist = distance[i];
            }
        }

        for (int i = 0; i < toVisit.Count; i++)
        {
            for (int j = 0; j < endStation.neighbours.Count; j++)
            {
                if (toVisit[i] == endStation.neighbours[j] &&
                    longestDist < distance[i])
                {
                    last = endStation.neighbours[j];
                    longestDist = distance[i] + endStation.neighbourDistance[j];
                    break;
                }
            }
        }

        shortList.Add(last);
        for (int i = 0; i < endStation.neighbours.Count; i++)
        {
            for (int j = 0; j < toVisit.Count; j++)
            {
                if (endStation.neighbours[i] == toVisit[j] && toVisit[j] != shortList[0])
                {
                    //Debug.Log($"{startStation.name}-{endStation.name}: shortList[0] is {shortList[0].name} while shortList[1] is {toVisit[j]}");
                    shortList.Add(toVisit[j]);
                    longestDist = distance[j];
                }
            }
        }

        // Navigates to a neighbour of the start node
        RailNode tempNode = shortList.Last();
        int iteration = 0;
        do
        {
            if (iteration > 50)
            {
                Debug.Log("ERROR, reached max iteration");
                break;
            }

            for (int i = 0; i < shortList.Last().neighbours.Count; i++)
            {
                if (shortList.Contains(shortList.Last().neighbours[i]) == false)
                {
                    for (int j = 0; j < toVisit.Count; j++)
                    {
                        if (toVisit[j] == shortList.Last().neighbours[i] &&
                            shortList.Contains(toVisit[j]) == false &&
                            distance[j] < longestDist)
                        {
                            tempNode = toVisit[j];
                            longestDist = distance[j];
                            //Debug.Log($"{startStation}-{endStation}: Added {toVisit[j].name} with a distance of {distance[j]} to shortList");
                        }
                    }
                }
            }
            if (shortList.Contains(tempNode) == false)
            {
                shortList.Add(tempNode);
            }
            iteration++;
        } while (startStation.neighbours.Contains(tempNode) == false);

        // Place the last node in the planner
        for (int i = 0; i < startStation.neighbours.Count; i++)
        {
            for (int j = 0; j < toVisit.Count; j++)
            {
                if (startStation.neighbours[i] == toVisit[j] && shortList.Contains(toVisit[j]) == false)
                {
                    //Debug.Log($"{startStation.name}-{endStation.name}: shortList[{shortList.Count - 2}] is {shortList[shortList.Count - 2].name} while shortList.Last() is {toVisit[j]}");
                    shortList.Add(toVisit[j]);
                    longestDist = distance[j];
                }
            }
        }

        // Then we put it in the main node list
        for (int i = shortList.Count - 1; i >= 0; i--)
        {
            railRoute.Add(shortList[i]);
        }

        string s = $"RailRoute {startStation.name}-{endStation.name} goes like this: ";
        for (int i = 0; i < railRoute.Count; i++)
        {
            s += $"{railRoute[i].name} -> ";
        }
        Debug.Log(s);
    }
}