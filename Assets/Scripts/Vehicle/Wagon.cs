using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wagon : RailVehicle
{
    //[Header("Wagon Info")]
    //[SerializeField] RailVehicle following; // What Vehicle the Wagon is following
    //[SerializeField] float connectionDistance; // The Distance between this Wagon and its leading vehicle

    override public void Initialize() 
    {
        
    }

    override public void Move()
    {
        distFromNode += mps * Time.deltaTime;

        if (distFromNode > nodeDistance && nextNode.neighbours.Count > 1 || distFromNode < 0 && prevNode.neighbours.Count > 1)
            Turn(null);

        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);

        if (prevNode && nextNode)
            transform.position = prevNode.transform.position + unitVector * distFromNode;
        else
            transform.position = prevNode.transform.position;
    }

    public override void Flip()
    {
        
    }

    private void OnDrawGizmos()
    {
        // To Gizmos-draw the Wagon
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(0.5f, 0.5f, 1.0f));
    }
}
