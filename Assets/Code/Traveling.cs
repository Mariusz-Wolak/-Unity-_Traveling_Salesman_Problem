using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Traveling : MonoBehaviour
{
    [SerializeField]
    List<Checkpoint> myCheckpoints;

    // float[,] distances;


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
        currentCheckpointIndex++;

        if (currentCheckpointIndex >= myCheckpoints.Count)
        {
            myNavMeshAgent.isStopped = true;
        }
    }

    private void ComputeDistances()
    {
        distances = new float[myCheckpoints.Count, myCheckpoints.Count];

        for (int i=0; i<myCheckpoints.Count; i++)
        {
            for (int j = 0; j < myCheckpoints.Count; j++)
            {
                if (i == j) continue;

                distances[i, j] = Vector3.Distance(myCheckpoints[i].transform.position, myCheckpoints[j].transform.position);
            }
        }
    }

}

