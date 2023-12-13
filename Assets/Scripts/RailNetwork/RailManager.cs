using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    [Header("Neighbour node info")]
    [SerializeField] public List<RailNode> nodes;    // Relevant rail nodes

    [Header("Station node info")]
    [SerializeField] public List<StationNode> stationNodes;  // Relevant station nodes
}
