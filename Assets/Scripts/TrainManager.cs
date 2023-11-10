using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [Header("Train info")]
    public float mps;   // Metres per second
    public List<TrainManager> wagons;   // Following wagons (not implemented!)

    [Header("Node info")]
    [SerializeField] RailNode prevNode; // The previous node this went through
    [SerializeField] RailNode nextNode; // The next node this went through
    [SerializeField] float nodeDist;    // Distance between the last passed node and its next node
    [SerializeField] float distFromNode;    // Distance between this and its last passed node
    [SerializeField] Vector3 unitVector;    // Unit Vector pointing this in the direction it is chugging towards

    [Header("Station info")]
    [SerializeField] StationNode toStation; // The stationNode this is chugging towards
    [SerializeField] List<RailNode> railRoute; // The list of railNodes this is planning to go through
    public List<StationNode> stationPlan; // The list of stations this is planning to go through
    public float waitTime;  // How long will the train wait at its current station (in seconds)
    public bool b_repeating;    // Is the train repeating this trip?

    // Start is called before the first frame update
    void Start()
    {
        nodeDist = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);
        distFromNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Chugga();
    }

    void Chugga()
    {
        distFromNode += mps * Time.deltaTime;

        if (distFromNode > nodeDist && nextNode.neighbours.Count > 1 || distFromNode < 0 && prevNode.neighbours.Count > 1)
            Turn();

        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);

        if (prevNode && nextNode)
            transform.position = prevNode.transform.position + unitVector * distFromNode;
        else
            transform.position = prevNode.transform.position;
    }

    void Turn()
    {
        if (distFromNode > nodeDist)
        {
            foreach (RailNode node in nextNode.neighbours)
            {
                if (node != prevNode)
                {
                    prevNode = nextNode;
                    nextNode = node;
                    break;
                }
            }
            distFromNode -= nodeDist;
        }
        else
        {
            foreach (RailNode node in prevNode.neighbours)
            {
                if (node != nextNode)
                {
                    nextNode = prevNode;
                    prevNode = node;
                    break;
                }
            }
            distFromNode += Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        }

        nodeDist = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up);
    }
}
