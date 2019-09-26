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
    [SerializeField]
    private Text _walkingText;

    [SerializeField]
    private Text _ComputingText;

    [SerializeField]
    private Text _DistanceText;

    [SerializeField]
    private Text _algorithmText;

    [SerializeField]
    private Text _checkpointsText;

    private float _startTime;
    private string _minutes = "";
    private string _seconds = "";

    [SerializeField]
    private List<Checkpoint> _myCheckpoints;

    private List<int> _finalShortest = new List<int>();

    private NavMeshAgent _myNavMeshAgent;
    private int _currentCheckpointIndex;
    private int _finalShortestIndexer = 0;
    private bool _isTraveling;

    private void Start()
    {
        _checkpointsText.text += "\n" + MainMenu.checkpointsAmount;
        _algorithmText.text = MainMenu.algorithmName.ToUpper();
        _startTime = Time.time;
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Bruteforce();
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        string computingMinutes;
        string computingSeconds;

        if ((elapsedMs / 1000) / 60 < 10)
        {
            computingMinutes = "0" + ((elapsedMs / 1000) / 60).ToString();
        }
        else
        {
            computingMinutes = ((elapsedMs / 1000) / 60).ToString();
        }

        if ((elapsedMs / 1000) < 10)
        {
            computingSeconds = "0" + (((elapsedMs / 1000)) % 60).ToString("f2");
        }
        else
        {
            computingSeconds = (((elapsedMs / 1000)) % 60).ToString("f2");
        }

        _ComputingText.text += "\n" + computingMinutes + ":" + computingSeconds;

        _myNavMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_myNavMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component isn't attached to " + gameObject.name);
        }
        else
        {
            if (_myCheckpoints != null && _myCheckpoints.Count >= 2)
            {
                _currentCheckpointIndex = 0;
                MySetDestination();
            }
            else
            {
                Debug.Log("Need more checkpoints");
            }
        }
    }

    private void Update()
    {
        float t = Time.time - _startTime;
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
        
        if (_isTraveling) _walkingText.text = "Walk time:\n" + _minutes + ":" + _seconds;

        if (_isTraveling && _myNavMeshAgent.remainingDistance <= 2.0f)
        {
            _isTraveling = false;

            if (!_myNavMeshAgent.isStopped)
            {
                NextCheckpoint();
                MySetDestination();
            }
        }
    }

    private void MySetDestination()
    {
        if (_myCheckpoints != null)
        {
            Vector3 targetVector = _myCheckpoints[_currentCheckpointIndex].transform.position;
            _myNavMeshAgent.SetDestination(targetVector);
            _isTraveling = true;
        }
    }

    private void NextCheckpoint()
    {
        _finalShortestIndexer++;

        if (_finalShortestIndexer - 1 > _myCheckpoints.Count) //final shortest path has 1 more element than Checkpoints, because we go back
        {
            _myNavMeshAgent.isStopped = true;
        }
        else if (_finalShortestIndexer == _myCheckpoints.Count + 1) // if about to go to the last Checkpoint, go to Checkpoint[0]
        {
            _currentCheckpointIndex = 0;
        }
        else
        {
            _currentCheckpointIndex = _finalShortest[_finalShortestIndexer];
        }
    }

    private double ComputeDistance(List<int> myList)
    {
        double distance = 0;

        for (int i = 0; i < myList.Count - 1; i++) //10 elements: 0-9, index [8] goes to [9] and we stop there
        {
            distance += Vector3.Distance(_myCheckpoints[myList[i]].transform.position, _myCheckpoints[myList[i + 1]].transform.position);
        }

        return distance;
    }

    private void Insertion()
    {
        List<int> remainingCheckpoints = new List<int>();
        List<int> currentShortest = new List<int>();
        List<int> currentLoopShortest = new List<int>();
        int startIndex = 0;
        double totalDistance;
        System.Random random = new System.Random();

        for (int i = 1; i < _myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set below
        {
            remainingCheckpoints.Add(i);
        }

        _finalShortest = new List<int>();

        _finalShortest.Add(startIndex);
        _finalShortest.Add(startIndex);

        currentShortest.AddRange(_finalShortest);

        while (_finalShortest.Count <= _myCheckpoints.Count) // because we have to go back, so finalShortest has 1 more Checkpoints than all Checkpoints
        {
            int randomIndex = random.Next(0, remainingCheckpoints.Count);
            currentShortest.Insert(1, remainingCheckpoints[randomIndex]);
            for (int i = 2; i < _finalShortest.Count; i++)
            {
                currentLoopShortest.Clear();
                currentLoopShortest.AddRange(_finalShortest);
                currentLoopShortest.Insert(i, remainingCheckpoints[randomIndex]);
                if (ComputeDistance(currentLoopShortest) < ComputeDistance(currentShortest)) currentShortest = currentLoopShortest;
            }
            _finalShortest.Clear();
            _finalShortest.AddRange(currentShortest);
            remainingCheckpoints.RemoveAt(randomIndex);
        }

        totalDistance = ComputeDistance(_finalShortest);

        _DistanceText.text += "\n" + totalDistance.ToString("f2");
    }
     

    private void Bruteforce()
    {
        _finalShortest = new List<int>();
        List<int> listToPermutate = new List<int>();
        List<int> currentPermutation = new List<int>();
        int permutationsNumber;
        double shortestDistance;
        double currentPermutationDistance;

        for (int i = 1; i < _myCheckpoints.Count; i++)
        {
            listToPermutate.Add(i);
        }

        _finalShortest.Add(0);
        _finalShortest.AddRange(listToPermutate);
        _finalShortest.Add(0);
        shortestDistance = ComputeDistance(_finalShortest);

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

            if (currentPermutationDistance < shortestDistance)
            {
                shortestDistance = currentPermutationDistance;
                _finalShortest.Clear();
                _finalShortest.AddRange(currentPermutation);
            }

            currentPermutation.Clear();
        }

        shortestDistance = ComputeDistance(_finalShortest);
        _DistanceText.text += "\n" + shortestDistance.ToString("f2");
    }

    private void RandomCheckpoints()
    {
        List<int> remainingCheckpoints = new List<int>();
        int startIndex = 0;
        double totalDistance;
        System.Random random = new System.Random();
        int randomIndex;

        for (int i = 1; i < _myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set
        {
            remainingCheckpoints.Add(i);
        }

        _finalShortest = new List<int>();
        _finalShortest.Add(startIndex);
        
        while (_finalShortest.Count < _myCheckpoints.Count)
        {
            randomIndex = random.Next(0, remainingCheckpoints.Count);
            _finalShortest.Add(remainingCheckpoints[randomIndex]);
            remainingCheckpoints.RemoveAt(randomIndex);
        }

        _finalShortest.Add(startIndex);
        totalDistance = ComputeDistance(_finalShortest);
        _DistanceText.text += "\n" + totalDistance.ToString("f2");
    }
}

    

