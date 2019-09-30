using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Traveling : MonoBehaviour
{
    public static List<int> finalShortest = new List<int>();
    public static NavMeshAgent myNavMeshAgent;
    public static int currentCheckpointIndex = 0;
    public static bool isTraveling;
    public int finalShortestIndexer = 0;

    [SerializeField]
    private Text _headerText;

    [SerializeField]
    private Text _ComputingText;


    private void Start()
    {
        myNavMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        float t = Time.time - MainSceneManager.startTime;
        if (((int)t / 60) < 10)
        {
            MainSceneManager.minutes = "0" + ((int)t / 60).ToString();
        }
        else
        {
            MainSceneManager.minutes = ((int)t / 60).ToString();
        }
        
        if((t % 60) < 10)
        {
            MainSceneManager.seconds = "0" + (t % 60).ToString("f2");
        }
        else
        {
            MainSceneManager.seconds = (t % 60).ToString("f2");
        }
        
        if (isTraveling) _headerText.text = "Walk time:\n" + MainSceneManager.minutes + ":" + MainSceneManager.seconds;

        if (isTraveling && myNavMeshAgent.remainingDistance <= 2.0f)
        {
            isTraveling = false;

            if (!myNavMeshAgent.isStopped)
            {
                NextCheckpoint();
                MySetDestination();
            }
        }
    }

    public static void MySetDestination()
    {
        if (MainSceneManager.myCheckpoints != null)
        {
            Vector3 targetVector = MainSceneManager.myCheckpoints[currentCheckpointIndex].transform.position;
            myNavMeshAgent.SetDestination(targetVector);
            isTraveling = true;
        }
    }

    private void NextCheckpoint()
    {
        finalShortestIndexer++;

        if (finalShortestIndexer - 1 > MainSceneManager.myCheckpoints.Count) //final shortest path has 1 more element than Checkpoints, because we go back
        {
            myNavMeshAgent.isStopped = true;
        }
        else if (finalShortestIndexer == MainSceneManager.myCheckpoints.Count + 1) // if about to go to the last Checkpoint, go to Checkpoint[0]
        {
            currentCheckpointIndex = 0;
        }
        else
        {
            currentCheckpointIndex = finalShortest[finalShortestIndexer];
        }
    }

    public static double ComputeDistance(List<int> myList)
    {
        double distance = 0;

        for (int i = 0; i < myList.Count - 1; i++) //10 elements: 0-9, index [8] goes to [9] and we stop there
        {
            distance += Vector3.Distance(MainSceneManager.myCheckpoints[myList[i]].transform.position, MainSceneManager.myCheckpoints[myList[i + 1]].transform.position);
        }

        return distance;
    }

    public static void Insertion()
    {
        List<int> remainingCheckpoints = new List<int>();
        List<int> currentShortest = new List<int>();
        List<int> currentLoopShortest = new List<int>();
        int startIndex = 0;
        System.Random random = new System.Random();

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set below
        {
            remainingCheckpoints.Add(i);
        }

        finalShortest = new List<int>();

        finalShortest.Add(startIndex);
        finalShortest.Add(startIndex);

        currentShortest.AddRange(finalShortest);

        while (finalShortest.Count <= MainSceneManager.myCheckpoints.Count) // because we have to go back, so finalShortest has 1 more Checkpoints than all Checkpoints
        {
            int randomIndex = random.Next(0, remainingCheckpoints.Count);
            currentShortest.Insert(1, remainingCheckpoints[randomIndex]);
            for (int i = 2; i < finalShortest.Count; i++)
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

        MainSceneManager.totalDistance = ComputeDistance(finalShortest);
    }


    public static void Bruteforce()
    {
        finalShortest = new List<int>();
        List<int> listToPermutate = new List<int>();
        List<int> currentPermutation = new List<int>();
        int permutationsNumber;
        double currentPermutationDistance;

        Debug.Log("MainSceneManager.myCheckpoints.Count: "+MainSceneManager.myCheckpoints.Count);

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++)
        {
            listToPermutate.Add(i);
            Debug.Log($"added {i} to listToPermutate");
        }

        finalShortest.Add(0);
        finalShortest.AddRange(listToPermutate);
        finalShortest.Add(0);
        Debug.Log("Final Shortest Count: "+finalShortest.Count);
        MainSceneManager.totalDistance = ComputeDistance(finalShortest);

        permutationsNumber = Permutations.Factorial(listToPermutate.Count);
 
        foreach (var permu in Permutations.Permutate(listToPermutate, listToPermutate.Count))
        {
            currentPermutation.Add(0);
            foreach (var i in permu)
            {
                //Debug.Log($"Checkpoint >{i}< added to tempList");
                currentPermutation.Add((int)i);
            }
            currentPermutation.Add(0);

            currentPermutationDistance = ComputeDistance(currentPermutation);

            if (currentPermutationDistance < MainSceneManager.totalDistance)
            {
                MainSceneManager.totalDistance = currentPermutationDistance;
                finalShortest.Clear();
                finalShortest.AddRange(currentPermutation);
            }

            currentPermutation.Clear();
        }

        MainSceneManager.totalDistance = ComputeDistance(finalShortest);
    }

    public static void RandomCheckpoints()
    {
        List<int> remainingCheckpoints = new List<int>();
        int startIndex = 0;
        System.Random random = new System.Random();
        int randomIndex;

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set
        {
            remainingCheckpoints.Add(i);
        }

        finalShortest = new List<int>();
        finalShortest.Add(startIndex);
        
        while (finalShortest.Count < MainSceneManager.myCheckpoints.Count)
        {
            randomIndex = random.Next(0, remainingCheckpoints.Count);
            finalShortest.Add(remainingCheckpoints[randomIndex]);
            remainingCheckpoints.RemoveAt(randomIndex);
        }

        finalShortest.Add(startIndex);
        MainSceneManager.totalDistance = ComputeDistance(finalShortest);
        
    }
}

    

