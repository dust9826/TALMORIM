using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioClip clip = null;
    [SerializeField] GameObject press = null;
    private bool isActive = false;
    private float a = 0;
    // Start is called before the first frame update
    void Start()
    {
        SoundController.Instance.PlayMusic(clip);
    }

    // Update is called once per frame
    void Update()
    {
        a += Time.deltaTime;
        if(a > 1f)
        {
            a = 0f;
            press.SetActive(isActive);
            isActive = !isActive;
        }
    }
}
