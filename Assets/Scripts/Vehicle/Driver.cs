using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Train))]
public class Driver : MonoBehaviour
{
    [SerializeField] Train train;

    [Header("Route Info")]
    [SerializeField] public bool isDriving;  // Whether this driver is driving
    [SerializeField] StationNode fromStation; // The StationNode this driver is chugging from
    [SerializeField] StationNode toStation; // The StationNode this driver is chugging towards
    [SerializeField] float stationDistance;
    [SerializeField] int routeNr;
    [SerializeField] List<StationNode> stationPlan; // The list of stations the train is planning to go through
    [SerializeField] List<RailNode> railRoute; // The list of railNodes the train is planning to go through
    [SerializeField] float waitTime;
    [SerializeField] float storedWaitTime;  // How long will the train wait at its current station (in seconds)
    [SerializeField] bool isLooping;    // Is the train repeating its trip?

    // Start is called before the first frame update
    void Start()
    {
        train = GetComponent<Train>();
        train.machine = new TrainMachine(train);
        train.machine.Initialize(train.machine.accelerateState);

        routeNr = 0;
        waitTime = storedWaitTime;
        isDriving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (toStation == null)
        {
            toStation = stationPlan[routeNr];
            stationDistance = toStation.neighbourDistance[1];
            fromStation = stationPlan[stationPlan.Count - 1];
        }

        if (train.distFromNode > train.nodeDistance && train.nextNode.neighbours.Count > 1 || train.distFromNode < 0 && train.prevNode.neighbours.Count > 1)
        {
            train.Turn(null);
            CheckNearStation();
        }

        if (isDriving)
        {
            // Checks if the train is at the train station
            if (train.distFromNode > stationDistance && toStation.neighbours[0] == train.prevNode ||
                train.distFromNode > stationDistance && toStation.neighbours[1] == train.prevNode)
                Stop();
        }
        else
        {
            if (waitTime > 0) // Decrease the timer
                waitTime -= Time.deltaTime;

            else if (waitTime < 0) // If the timer is up
                Go();
        }
    }

    void Stop()
    {
        // Start stopping the train (Deceleration)
        train.machine.TransitionTo(train.machine.stationState);
        isDriving = false;
    }

    void Go()
    {
        // Start moving the train
        train.machine.TransitionTo(train.machine.accelerateState);
        waitTime = storedWaitTime; // + reset the Clock

        // Set the next station in the station plan
        if (routeNr < stationPlan.Count - 1) routeNr++;
        else if (isLooping) routeNr = 0;

        fromStation = toStation;
        toStation = stationPlan[routeNr];

        // Set the rail route to the next station
        //fromStation.GetRoute(toStation);

        isDriving = true;
    }

    void CheckNearStation()
    {
        // If the train is on the same lane as a station, find the distance
        if (train.prevNode == toStation.neighbours[0] && train.nextNode == toStation.neighbours[1])
            stationDistance = toStation.neighbourDistance[0];

        else if (train.prevNode == toStation.neighbours[1] && train.nextNode == toStation.neighbours[0])
            stationDistance = toStation.neighbourDistance[1];

        return;
    }
}
