using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static ModelManager G_ModelManager = null;
    public static ModelManager GetGlobalModelManager()
    {
        if(G_ModelManager == null)
        {
            G_ModelManager = GameObject.Find("G_Models").GetComponent<ModelManager>();
        }
        return G_ModelManager;
    }

    public GameObject Player1Pokemon;
    public GameObject Player2Pokemon;
    public GameObject Player3Pokemon;
    public GameObject Player4Pokemon;
    public GameObject Player5Pokemon;
    public GameObject Player6Pokemon;

    public GameObject Enemy1Pokemon;
    public GameObject Enemy2Pokemon;
    public GameObject Enemy3Pokemon;
    public GameObject Enemy4Pokemon;
    public GameObject Enemy5Pokemon;
    public GameObject Enemy6Pokemon;
 
}
