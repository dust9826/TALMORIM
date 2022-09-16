using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject _door1 = null;
    [SerializeField] GameObject _door2 = null;

    public void CloseDoor(bool isOpen)
    {
        if (isOpen)
            StartCoroutine(Close(1));
        else
            StartCoroutine(Close(-1));
    }
    IEnumerator Close(int a)
    {
        for(int i=0;i<90;i++)
        {
            yield return new WaitForSeconds(0.01f);
            if (_door1 != null) _door1.transform.eulerAngles += new Vector3(0,1,0) * a;
            if (_door2 != null) _door2.transform.eulerAngles -= new Vector3(0, 1, 0) * a;
        }
    }
}
