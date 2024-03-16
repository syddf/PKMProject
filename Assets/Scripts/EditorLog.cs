using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLog : MonoBehaviour
{
    public static void DebugLog(string Log)
    {
        #if UNITY_EDITOR
            Debug.Log(Log);
        #endif
    }
}
