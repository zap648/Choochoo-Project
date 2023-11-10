using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;

public class StationNode : MonoBehaviour
{
    [Header("Node info")]
    public List<RailNode> railNodes;    // The railNodes this station is placed inbetween
    public List<float> nodeDist;    // The distance between this station and its respective rails

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.LookRotation((railNodes[0].transform.position - railNodes[1].transform.position).normalized, Vector3.up);

        // OBS! Planning to implement the station snapping on to the railway!
        for (int i = 0; i < railNodes.Count; i++)
            nodeDist.Add(Vector3.Distance(railNodes[i].transform.position, transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
