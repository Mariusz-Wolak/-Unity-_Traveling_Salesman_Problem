using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteforceAlgorithm : Algorithm
{
    public override void FindTheShortest()
    {
        Debug.Log("bruteforce");
        List<int> listToPermutate = new List<int>();
        List<int> currentPermutation = new List<int>();

        Debug.Log("MainSceneManager.myCheckpoints.Count: " + MainSceneManager.myCheckpoints.Count);

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++)
        {
            listToPermutate.Add(i);
            Debug.Log($"added {i} to listToPermutate");
        }

        finalShortest.Add(0);
        finalShortest.AddRange(listToPermutate);
        finalShortest.Add(0);

        int permutationsNumber = Permutations.Factorial(listToPermutate.Count);

        foreach (var permu in Permutations.Permutate(listToPermutate, listToPermutate.Count))
        {
            currentPermutation.Add(0);
            foreach (var i in permu)
            {
                //Debug.Log($"Checkpoint >{i}< added to tempList");
                currentPermutation.Add((int)i);
            }
            currentPermutation.Add(0);

            double currentPermutationDistance = MainSceneManager.ComputeDistance(currentPermutation);

            if (currentPermutationDistance < MainSceneManager.totalDistance)
            {
                MainSceneManager.totalDistance = currentPermutationDistance;
                finalShortest.Clear();
                finalShortest.AddRange(currentPermutation);
            }

            currentPermutation.Clear();
        }
    }
}
