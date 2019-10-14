using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmManager : MonoBehaviour
{
    private Algorithm _algorithm;

    public void SetAlgorithm(Algorithm algorithm)
    {
        _algorithm = algorithm;
    }

    public void FindTheShortest()
    {
        _algorithm.FindTheShortest();
    }
}
