using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TrainState
{
    public void Enter()
    {

    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}

[Serializable]
public class TrainMachine
{
    public TrainState CurrentState { get; private set; }

    public AccelerateState accelerateState;
    public CruiseState cruiseState;
    public DecelerateState decelerateState;
    public StationState stationState;

    public TrainMachine(Train train)
    {
        this.accelerateState = new AccelerateState(train);
        this.cruiseState = new CruiseState(train);
        this.decelerateState = new DecelerateState(train);
        this.stationState = new StationState(train);
    }

    public void Initialize(TrainState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void TransitionTo(TrainState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update()
    {
        if (CurrentState != null)
            CurrentState.Update();
    }
}

public class AccelerateState : TrainState
{
    private Train train;

    public AccelerateState(Train train)
    {
        this.train = train;
    }

    public void Enter()
    {
        Debug.Log("Train has entered Acceleration stage");
    }

    public void Update()
    {
        // Accelerates the train
        train.mps += train.acceleration * Time.deltaTime;
        // & its wagons
        foreach (Wagon wagon in train.wagons)
            wagon.mps = train.mps;

        if (train.mps > train.speedLimit)
            train.machine.TransitionTo(train.machine.cruiseState);
    }

    public void Exit()
    {
        Debug.Log("Train has exited Acceleration stage");
    }
}

public class CruiseState : TrainState
{
    private Train train;

    public CruiseState(Train train)
    {
        this.train = train;
    }

    public void Enter()
    {
        Debug.Log("Train has entered Cruise stage");
        train.mps = train.speedLimit;
        foreach (Wagon wagon in train.wagons)
            wagon.mps = train.mps;
    }

    public void Update()
    {
        // Train be cruising along the rails unless explicitly otherwise
    }

    public void Exit()
    {
        Debug.Log("Train has exited Cruise stage");
    }
}

public class DecelerateState : TrainState
{
    private Train train;

    public DecelerateState(Train train)
    {
        this.train = train;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        // Decelerates the train
        train.mps -= train.acceleration * Time.deltaTime;
        // & its wagons
        foreach (Wagon wagon in train.wagons)
            wagon.mps = train.mps;

        if (train.mps <= 0)
            train.machine.TransitionTo(train.machine.stationState);
    }

    public void Exit()
    {

    }
}

public class StationState : TrainState
{
    private Train train;

    public StationState(Train train)
    {
        this.train = train;
    }

    public void Enter()
    {
        Debug.Log("Train has entered Station stage");
        train.mps = 0.0f;
        foreach (Wagon wagon in train.wagons)
            wagon.mps = train.mps;
    }

    public void Update()
    {
        // Train be stationed in place unless told explicitly otherwise (TransitionTo())
    }

    public void Exit()
    {
        Debug.Log("Train has exited Station stage");
    }
}