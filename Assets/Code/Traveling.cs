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
    
    public static NavMeshAgent myNavMeshAgent;
    public static int currentCheckpointIndex = 0;
    public static bool isTraveling;
    private int _finalShortestIndexer = 0;
    private string _minutes = "";
    private string _seconds = "";

    [SerializeField]
    private Material _greenMaterial;

    [SerializeField]
    private Text _headerText;

    [SerializeField]
    private Text _ComputingText;

    [SerializeField]
    private GameObject _NPC;


    private void Start()
    {
        myNavMeshAgent = this.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        float t = Time.time - MainSceneManager.startTime;
        if (((int)t / 60) < 10)
        {
            _minutes = "0" + ((int)t / 60).ToString();
        }
        else
        {
            _minutes = ((int)t / 60).ToString();
        }
        
        if((t % 60) < 10)
        {
            _seconds = "0" + (t % 60).ToString("f2");
        }
        else
        {
            _seconds = (t % 60).ToString("f2");
        }
        
        if (isTraveling) _headerText.text = "Walk time:\n" + _minutes + ":" + _seconds;

        if (isTraveling && myNavMeshAgent.remainingDistance <= 2.0f)
        {
            isTraveling = false;
            if (currentCheckpointIndex != 0)
            {
                MainSceneManager.myCheckpoints[currentCheckpointIndex].GetComponent<Renderer>().material = _greenMaterial;
            }

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
        _finalShortestIndexer++;

        if (_finalShortestIndexer - 1 > MainSceneManager.myCheckpoints.Count) //final shortest path has 1 more element than Checkpoints, because we go back
        {
            myNavMeshAgent.isStopped = true;
            _NPC.GetComponent<Renderer>().material = _greenMaterial;
            MainSceneManager.myCheckpoints[currentCheckpointIndex].GetComponent<Renderer>().material = _greenMaterial;
        }
        else if (_finalShortestIndexer == MainSceneManager.myCheckpoints.Count + 1) // if about to go to the last Checkpoint, go to Checkpoint[0]
        {
            currentCheckpointIndex = 0;
        }
        else
        {
            currentCheckpointIndex = Algorithms.finalShortest[_finalShortestIndexer];
        }
    }
}

    

