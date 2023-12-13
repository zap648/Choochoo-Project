using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RailVehicle : MonoBehaviour
{
    [Header("Rail vehicular Info")]
    [SerializeField] public float mps;   // Metres per second
    [SerializeField] public RailNode prevNode; // The previous node this went through
    [SerializeField] public RailNode nextNode; // The next node this went through
    [SerializeField] public float nodeDistance;    // Distance between the last passed node and its next node
    [SerializeField] public float distFromNode;    // Distance between this and its last passed node
    [SerializeField] public Vector3 unitVector;    // Unit Vector pointing this in the direction it is chugging towards

    public abstract void Initialize();
    public abstract void Move();

    private void Start()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Turn(RailNode toNode)
    {
        if (toNode == null)
        {
            // If the Vehicle has passed the (length of the) nextNode, It will turn
            if (distFromNode > nodeDistance)
            {
                // Find and change the previous and next railnodes
                foreach (RailNode node in nextNode.neighbours)
                    if (node != prevNode)
                    {
                        prevNode = nextNode;
                        nextNode = node;
                        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
                        break;
                    }
                // Uses the train's extra distance from the bypassed node as its starting position towards the next node
                distFromNode -= nodeDistance;
            }
            // If the Vehicle has passed the 
            else if (distFromNode <= 0)
            {
                // Find and change the previous and next railnodes
                foreach (RailNode node in prevNode.neighbours)
                    if (node != nextNode)
                    {
                        nextNode = prevNode;
                        prevNode = node;
                        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
                        break;
                    }
                // Uses the train's extra distance from the bypassed node as its starting position towards the next node
                distFromNode += Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
            }

            // Distance between the two nodes
            nodeDistance = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        }
    }
}
