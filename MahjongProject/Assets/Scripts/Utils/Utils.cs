using UnityEngine;
using System.Collections;

public class Utils 
{
    public static int GetRandomNum(int min, int max) {
        return Random.Range(min, max);
    }
}
