using System.Collections;
using UnityEngine;

public class CoroutineTester : MonoBehaviour
{
    private void Start()
    {
        print($"Start Time {Time.time}");
        StartCoroutine(MyFirstCoroutine());
    }

    private void Update()
    {
        // Code Goes Here
    }

    private IEnumerator WhatAmI()
    {
        while(true)
        {
            yield return null;
            // Code Goes Here
        }
    }

    private IEnumerator MyFirstCoroutine()
    {
        print($"Time 0 -> {Time.time}");
        yield return null; // waits 1 frame
        print($"Time 1 -> {Time.time}");
        yield return null; // waits 1 frame
        print($"Time 2 -> {Time.time}");
        yield return new WaitForSeconds(3f);
        print($"Time 3 -> {Time.time}");
    }

}
