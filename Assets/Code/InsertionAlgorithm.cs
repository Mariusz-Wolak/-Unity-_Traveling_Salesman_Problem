using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionAlgorithm : Algorithm
{
    public override void FindTheShortest()
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
                if (MainSceneManager.ComputeDistance(currentLoopShortest) < MainSceneManager.ComputeDistance(currentShortest)) currentShortest = currentLoopShortest;
            }
            finalShortest.Clear();
            finalShortest.AddRange(currentShortest);
            remainingCheckpoints.RemoveAt(randomIndex);
        }
    }
}
