using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Traveling : MonoBehaviour
{
    [SerializeField]
    List<Checkpoint> myCheckpoints;

    NavMeshAgent myNavMeshAgent;
    int currentCheckpointIndex;
    bool traveling;

    public void Start()
    {
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

            NextCheckpoint();

            if (!myNavMeshAgent.isStopped)
            {
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

}

