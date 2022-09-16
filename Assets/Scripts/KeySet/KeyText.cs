using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyText : MonoBehaviour
{
    [SerializeField] string _keyName;
    private Text _keyText;
    private void Start()
    {
        _keyText = GetComponent<Text>();
    }
    void Update()
    {
        _keyText.text = KeyPreset.Instance.StringToKeyCode(_keyName).ToString();
    }
}
