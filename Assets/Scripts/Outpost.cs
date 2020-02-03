using System.Collections.Generic;
using UnityEngine;

public class Outpost : MonoBehaviour
{
    public static List<Outpost> OutpostList = new List<Outpost>();

    public static Outpost GetRandomOutpost()
    {
        if (OutpostList.Count == 0) return null;

        var randomIndex = Random.Range(0, OutpostList.Count);
        return OutpostList[randomIndex];
    }

    private void OnEnable()
    {
        OutpostList.Add(this);
    }

    private void OnDisable()
    {
        OutpostList.Remove(this);
    }
}
