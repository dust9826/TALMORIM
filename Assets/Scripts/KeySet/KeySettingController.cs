using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class KeySettingController : MonoBehaviour
{
    [SerializeField] GameObject KeySettingWindow = null;
    // Start is called before the first frame update
    void Start()
    {
        KeySettingWindow.SetActive(false);
        KeyPreset.Instance.IsChanging = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenWindow()
    {
        KeyPreset.Instance.IsChanging = true;
        KeySettingWindow.SetActive(true);
    }

    public void CloseWindow()
    {
        KeyPreset.Instance.IsChanging = false;
        KeySettingWindow.SetActive(false);
    }

    public void KeySet(string event_str)
    {
        UnityEvent keyEvent = KeyPreset.Instance.StringToEvent(event_str);
        Debug.Log("Change " + event_str + " Key");
        StartCoroutine(KeyPreset.Instance.ChangeKey(keyEvent));
    }
}
