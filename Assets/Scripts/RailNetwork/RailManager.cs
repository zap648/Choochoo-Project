using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    [Header("Neighbour node info")]
    [SerializeField] public List<RailNode> neighbours;    // Relevant rail nodes
    [SerializeField] public List<float> neighbourDistance;
    [SerializeField] public List<Quaternion> neighbourDirection;
    [SerializeField] public bool looping;  // Is the network closed?

    [Header("Station node info")]
    [SerializeField] public List<StationNode> stationNodes;  // Relevant
}
