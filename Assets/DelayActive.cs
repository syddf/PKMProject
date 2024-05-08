using UnityEngine;
using System.Collections;

public class DelayActive : MonoBehaviour
{
    public GameObject Obj;
    public void Delay()
    {
        StartCoroutine(DisableObjectAfterDelay());
    }

    IEnumerator DisableObjectAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        Obj.SetActive(true);
    }
}