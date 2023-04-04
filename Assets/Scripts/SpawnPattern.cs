using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPattern
{
    public enum Pattern
    {
        single,
        school,
        stream
    }
    [Header("SpawnPattern")]
    public Pattern Type = Pattern.single;


    public int mincount, maxcount;


    public int spread_radius = 2;


    public float rate = 1, duration = 0;

}
