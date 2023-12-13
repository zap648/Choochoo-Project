using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class RailNode : MonoBehaviour
{
    [Header("Neighbour node info")]
    [SerializeField] public List<RailNode> neighbours;    // Relevant rail nodes
    [SerializeField] public List<float> neighbourDistance;
    [SerializeField] public List<Quaternion> neighbourDirection;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        GetNeighbourInfo();
    }

    void GetNeighbourInfo()
    {
        foreach (RailNode node in neighbours)
        {
            neighbourDistance.Add(Vector3.Distance(this.transform.position, node.transform.position));
            neighbourDirection.Add(Quaternion.LookRotation(this.transform.position - node.transform.position, Vector3.up));
        }
    }

    private void OnDrawGizmos()
    {
        // GizmoDrawing the blue Lines
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up);

        // GizmoDrawing the red Lines
        Gizmos.color = Color.red;
        foreach (RailNode neighbour in neighbours.Skip(1))
            Gizmos.DrawLine(transform.position, neighbour.transform.position);

        // GizmoDrawing the cyan Lines
        Gizmos.color = Color.cyan;
        for (int j = 0; j < neighbours.Count; j++)
            Gizmos.DrawLine(transform.position, transform.position + (transform.position - neighbours[j].transform.position).normalized);
    }
}

