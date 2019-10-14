using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCheckpointsAlgorithm : Algorithm
{
    public override void FindTheShortest()
    {
        List<int> remainingCheckpoints = new List<int>();
        int startIndex = 0;
        System.Random random = new System.Random();
        int randomIndex;

        for (int i = 1; i < MainSceneManager.myCheckpoints.Count; i++) //starting at 1, because startIndex = 0 is set
        {
            remainingCheckpoints.Add(i);
        }

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
