using System;
using System.Collections;
using UnityEngine;

public static class Helper
{
    public static IEnumerator PerformActionInSeconds(float time, Action action)
    {        
        yield return new WaitForSeconds(time);
        action();
    }
}
