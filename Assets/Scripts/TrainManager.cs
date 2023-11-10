using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    [Header("Train info")]
    public float mps;   // Metres per second
    [SerializeField] bool b_going;  // Whether the train is going
    public List<TrainManager> wagons;   // Following wagons (not implemented!)

    [Header("Node info")]
    [SerializeField] RailNode prevNode; // The previous node this went through
    [SerializeField] RailNode nextNode; // The next node this went through
    [SerializeField] float nodeDist;    // Distance between the last passed node and its next node
    [SerializeField] float distFromNode;    // Distance between this and its last passed node
    [SerializeField] Vector3 unitVector;    // Unit Vector pointing this in the direction it is chugging towards

    [Header("Station info")]
    [SerializeField] StationNode toStation; // The stationNode this is chugging towards
    [SerializeField] float stationDistance;
    [SerializeField] List<RailNode> railRoute; // The list of railNodes this is planning to go through
    public List<StationNode> stationPlan; // The list of stations this is planning to go through
    public float waitTime;  // How long will the train wait at its current station (in seconds)
    [SerializeField] int routeNr;
    public bool b_repeating;    // Is the train repeating this trip?

    // Start is called before the first frame update
    void Start()
    {
        nodeDist = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);
        routeNr = 0;
        toStation = stationPlan[routeNr];
        stationDistance = toStation.nodeDist[0];
        distFromNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Chugga();
    }

    void Chugga()
    {
        if (b_going)
        {
            distFromNode += mps * Time.deltaTime;

            if (distFromNode > nodeDist && nextNode.neighbours.Count > 1 || distFromNode < 0 && prevNode.neighbours.Count > 1)
                Turn();

            else if (distFromNode > stationDistance && (toStation.railNodes[0] == prevNode || toStation.railNodes[1] == prevNode))
                Stop();
        }

        else
        {
            if (waitTime > 0)
                waitTime -= Time.deltaTime;

            else if (waitTime < 0)
                Go();
        }

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
            // Find and change the previous and next railnodes
            foreach (RailNode node in nextNode.neighbours)
            {
                if (node != prevNode)
                {
                    prevNode = nextNode;
                    nextNode = node;
                    break;
                }
            }
            // Uses the train's extra distance from the bypassed node as its starting position towards the next node
            distFromNode -= nodeDist;
        }
        else
        {
            // Find and change the previous and next railnodes
            foreach (RailNode node in prevNode.neighbours)
            {
                if (node != nextNode)
                {
                    nextNode = prevNode;
                    prevNode = node;
                    break;
                }
            }
            // Uses the train's extra distance from the bypassed node as its starting position towards the next node
            distFromNode += Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        }

        // Distance between the two nodes
        nodeDist = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
    }

    void Stop()
    {
        // Start stopping the train
        b_going = false;
    }

    void Go()
    {
        // Start moving the train
        b_going = true;
        waitTime = 3;

        // Set next station in station plan
        if (routeNr < stationPlan.Count - 1)
            routeNr++;
        else if (b_repeating)
            routeNr = 0;
        toStation = stationPlan[routeNr];
        stationDistance = toStation.nodeDist[0];

        // Set rail route
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(0.5f, 0.5f, 1.0f));
    }
}
