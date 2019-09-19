using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Traveling : MonoBehaviour
{
    public Text walkingText;
    private float startTime;
    string minutes = "";
    string seconds = "";

    public Text ComputingText;
    public Text DistanceText;
    public Text algorithmText;
    public Text checkpointsText;


    List<int> remainingCheckpoints = new List<int>();
    List<int> finalShortest = new List<int>();
    List<int> currentShortest = new List<int>();
    List<int> currentLoopShortest = new List<int>();

    NavMeshAgent myNavMeshAgent;
    int currentCheckpointIndex;
    int finalShortestIndexer = 0;
    bool traveling;

    public void Start()
    {
        Heuristic();
        Debug.Log("Final Shortest is: ");
        foreach(int ele in finalShortest) Debug.Log(ele);

        myNavMeshAgent = this.GetComponent<NavMeshAgent>();

        if (myNavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component isn't attached to " + gameObject.name);
        }
        else
        {
            if (myCheckpoints != null && myCheckpoints.Count >= 2)
            {
                currentCheckpointIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Need more checkpoints");
            }
        }
    }

    public void Update()
    {
        if (traveling && myNavMeshAgent.remainingDistance <= 2.0f)
        {
            traveling = false;

            if (!myNavMeshAgent.isStopped)
            {
                NextCheckpoint();
                SetDestination();
            }
        }
    }

    private void SetDestination()
    {
        if (myCheckpoints != null)
        {
            Vector3 targetVector = myCheckpoints[currentCheckpointIndex].transform.position;
            myNavMeshAgent.SetDestination(targetVector);
            traveling = true;
        }
    }

    private void NextCheckpoint()
    {
        finalShortestIndexer++;

        if (finalShortestIndexer - 1 > myCheckpoints.Count) //final shortest path has 1 more element than Checkpoints, because we go back
        {
            myNavMeshAgent.isStopped = true;
        }
        else if (finalShortestIndexer == myCheckpoints.Count+1) // if about to go to the last Checkpoint, go to Checkpoint[0]
        {
            currentCheckpointIndex = 0;
        }
        else
        {
            currentCheckpointIndex = finalShortest[finalShortestIndexer];
        }
    }

    //private void ComputeDistances()
    //{
    //    distances = new float[myCheckpoints.Count, myCheckpoints.Count];

    //    for (int i=0; i<myCheckpoints.Count; i++)
    //    {
    //        for (int j = 0; j < myCheckpoints.Count; j++)
    //        {
    //            if (i == j) continue;

    //            distances[i, j] = Vector3.Distance(myCheckpoints[i].transform.position, myCheckpoints[j].transform.position);
    //        }
    //    }
    //}

    private double ComputeDistance(List<int> myList)
    {
        double distance = 0;

        for(int i=0; i<myList.Count-1; i++) //10 elements: 0-9, index [8] goes to [9] and we stop there
        {
            distance += Vector3.Distance(myCheckpoints[myList[i]].transform.position, myCheckpoints[myList[i+1]].transform.position);
        }

        return distance;
    }

    private void Heuristic()
    {
        for (int i=1; i<myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set below
        {
            remainingCheckpoints.Add(i);
        }

        int startIndex = 0;

        finalShortest.Add(startIndex);
        finalShortest.Add(startIndex);

        currentShortest.AddRange(finalShortest);

        System.Random random = new System.Random();

        while (finalShortest.Count <= myCheckpoints.Count) // because we have to go back, so finalShortest has 1 more Checkpoints than all Checkpoints
        {
            int randomIndex = random.Next(0, remainingCheckpoints.Count);
            currentShortest.Insert(1, remainingCheckpoints[randomIndex]);
            for (int i=2; i<finalShortest.Count; i++)
            {
                currentLoopShortest.Clear();
                currentLoopShortest.AddRange(finalShortest);
                currentLoopShortest.Insert(i, remainingCheckpoints[randomIndex]);
                if (ComputeDistance(currentLoopShortest) < ComputeDistance(currentShortest)) currentShortest = currentLoopShortest;
            }
            finalShortest.Clear();
            finalShortest.AddRange(currentShortest);
            remainingCheckpoints.RemoveAt(randomIndex);
        }
    }
}

