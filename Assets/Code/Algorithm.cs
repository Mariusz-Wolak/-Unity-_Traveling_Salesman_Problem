using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Algorithm
{
    public static List<int> finalShortest;

    public abstract void FindTheShortest();


    public static void Bruteforce()
    {
        finalShortest = new List<int>();
        List<int> listToPermutate = new List<int>();
        List<int> currentPermutation = new List<int>();
        int permutationsNumber;
        double currentPermutationDistance;

        Debug.Log("MainSceneManager.myCheckpoints.Count: " + MainSceneManager.myCheckpoints.Count);

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++)
        {
            listToPermutate.Add(i);
            Debug.Log($"added {i} to listToPermutate");
        }

        finalShortest.Add(0);
        finalShortest.AddRange(listToPermutate);
        finalShortest.Add(0);

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

            currentPermutationDistance = MainSceneManager.ComputeDistance(currentPermutation);

            if (currentPermutationDistance < MainSceneManager.totalDistance)
            {
                MainSceneManager.totalDistance = currentPermutationDistance;
                finalShortest.Clear();
                finalShortest.AddRange(currentPermutation);
            }

            currentPermutation.Clear();
        }
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
    }
}
