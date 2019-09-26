using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Permutations
{
    public static void RotateRight(IList sequence, int count)
    {
        object tmp = sequence[count - 1];
        sequence.RemoveAt(count - 1);
        sequence.Insert(0, tmp);
    }

    public static IEnumerable<IList> Permutate(IList sequence, int count)
    {
        if (count == 1) yield return sequence;
        else
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var perm in Permutate(sequence, count - 1))
                    yield return perm;
                RotateRight(sequence, count);
            }
        }
    }

    public static int Factorial(int n)
    {
        if (n == 0)
            return 1;
        else
            return n * Factorial(n - 1);
    }
}
