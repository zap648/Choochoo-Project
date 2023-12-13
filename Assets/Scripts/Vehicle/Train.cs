using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Driver))]
public class Train : RailVehicle
{
    [SerializeField] Driver driver;
    [SerializeField] public TrainMachine machine;

    [Header("Train Info")]
    [SerializeField] public float speedLimit;  // The speed limit of the locomotive
    [SerializeField] public float acceleration; // The acceleration (meters per second per second) of the locomotive
    [SerializeField] public List<Wagon> wagons;   // Following wagons
    [SerializeField] public float wagonDist;   // Distance between the wagons

    override public void Initialize() 
    {
        driver = GetComponent<Driver>();

        unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
        nodeDistance = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);
        transform.position = prevNode.transform.position + unitVector * distFromNode;

        if (wagons.Count > 0)
        {
            int i = 0;
            foreach (Wagon wagon in wagons)
            {
                i++;

                wagon.prevNode = prevNode;
                wagon.nextNode = nextNode;
                wagon.distFromNode = distFromNode - wagonDist * i;
                wagon.nodeDistance = Vector3.Distance(prevNode.transform.position, nextNode.transform.position);
                wagon.unitVector = (nextNode.transform.position - prevNode.transform.position).normalized;
                wagon.transform.position = prevNode.transform.position + unitVector * distFromNode;
                wagon.transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);
            }
        }
    }

    override public void Move()
    {
        distFromNode += mps * Time.deltaTime;

        machine.Update();

        transform.rotation = Quaternion.LookRotation(unitVector, Vector3.up);

        if (prevNode && nextNode)
            transform.position = prevNode.transform.position + unitVector * distFromNode;
        else
            transform.position = prevNode.transform.position;
    }

    private void OnDrawGizmos()
    {
        // To Gizmos-draw the Train
        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, new Vector3(0.5f, 0.5f, 1.0f));
    }
}
