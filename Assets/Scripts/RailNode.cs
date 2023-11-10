using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class RailNode : MonoBehaviour
{
    [Header("Neighbour node info")]
    [Tooltip("Neighbouring nodes")]
    public List<RailNode> neighbours;
    
    [Tooltip("Distance beetween neighbours")]
    [SerializeField] List<float> neighbourDistance;

    [Tooltip("Direction to neighbour")]
    [SerializeField] List<Quaternion> neighbourDirection;

    // Start is called before the first frame update
    void Start()
    {
        GetNeighbourInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetNeighbourInfo()
    {
        foreach (RailNode node in neighbours)
        {
            neighbourDistance.Add(Vector3.Distance(this.transform.position, node.transform.position));
            neighbourDirection.Add(Quaternion.LookRotation((transform.position - node.transform.position), Vector3.up));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, neighbours[neighbours.Count-1].transform.position);

        Gizmos.color = Color.cyan;
        for (int i = 0; neighbours.Count > i; i++)
        Gizmos.DrawLine(transform.position, transform.position+(transform.position - neighbours[i].transform.position).normalized);
    }
}
