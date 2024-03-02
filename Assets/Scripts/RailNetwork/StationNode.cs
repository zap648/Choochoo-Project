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
    List<StationRoute> stationRoutes; // Routes to the available stations

    public void GetNeighbourInfo()
    {
        stations = new List<StationNode>();
        stationRoutes = new List<StationRoute>();
        foreach (RailNode node in neighbours)
        {
            neighbourDistance.Add(Vector3.Distance(this.transform.position, node.transform.position));
            neighbourDirection.Add(Quaternion.LookRotation(this.transform.position - node.transform.position, Vector3.up));
        }
    }

    public void FindStationRoutes()
    {
        stations = railManager.stationNodes;
        foreach (StationNode station in stations)
        {
            if (station != this)
            {
                stationRoutes.Add(new StationRoute(this, station));
            }
        }
    }

    public StationRoute GetRoute(StationNode toNode)
    {
        if (toNode == null)
        {
            return null;
        }

        foreach (StationRoute route in stationRoutes)
        {

            if (route.endStation == toNode)
            {
                Debug.Log($"Route to {route.endStation} found!");
                return route;
            }
        }

        Debug.Log($"ERROR: Route to {toNode.name} not found!");
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
