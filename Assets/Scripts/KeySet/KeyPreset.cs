using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyPreset
{
    private KeyPreset() 
    {
        if (_keyCodes.Count == 0)
        {
            KeyCode[] codes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
            foreach(KeyCode code in codes)
            {
                _keyCodes.Add(code);
            }
        }
        _keyDictionary = new Dictionary<KeyCode, UnityEvent>();
        _eventDictionary = new Dictionary<UnityEvent, KeyCode>();
        _stringToEvent = new Dictionary<string, UnityEvent>();
        IsChanging = false;
    }
    private static KeyPreset _instace;
    public static KeyPreset Instance
    {
        get
        {
            if (_instace == null)
            {
                _instace = new KeyPreset();
            }
            return _instace;
        }
    }

    List<KeyCode> _keyCodes = new List<KeyCode>();

    public bool IsChanging { get; set; }

    private List<KeyCode> _keys = new List<KeyCode>();
    private List<string> _strings = new List<string>();
    private Dictionary<KeyCode, UnityEvent> _keyDictionary;
    private Dictionary<UnityEvent, KeyCode> _eventDictionary;
    private Dictionary<string, UnityEvent> _stringToEvent;
    public Dictionary<KeyCode, UnityEvent> KeyDictionary
    {
        get { return _keyDictionary; }
        set { _keyDictionary = value; }
    }
    public Dictionary<UnityEvent, KeyCode> EventDictionary
    {
        get { return _eventDictionary; }
    }
    public UnityEvent StringToEvent(string event_str)
    {
        return _stringToEvent[event_str];
    }
    public KeyCode StringToKeyCode(string event_str)
    {
        return _eventDictionary[_stringToEvent[event_str]];
    }

    public void RemoveAll()
    {
        while(_strings.Count != 0)
        {
            _stringToEvent.Remove(_strings[0]);
            _strings.RemoveAt(0);
        }
        for(int i=0;i<_keys.Count;i++)
        {
            _eventDictionary.Remove(_keyDictionary[_keys[i]]);
        }
        while(_keys.Count != 0)
        {
            _keyDictionary.Remove(_keys[0]);
            _keys.RemoveAt(0);
        }
    }

    public void AddKeyDictionary(KeyCode code, UnityEvent keyEvent, string event_str)
    {
        if (_keyDictionary.ContainsKey(code) || _eventDictionary.ContainsKey(keyEvent) || _stringToEvent.ContainsKey(event_str))
            return;
        _keyDictionary.Add(code, keyEvent);
        _eventDictionary.Add(keyEvent, code);
        _stringToEvent.Add(event_str, keyEvent);
        _keys.Add(code);
        _strings.Add(event_str);
    }

    public IEnumerator ChangeKey(UnityEvent keyEvent)  //테스트중
    {
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        Debug.Log("Changing...");
        bool isChange = true;
        yield return wait;

        while (true)
        {
            if(Input.anyKeyDown)
            {
                Debug.Log("A");
                foreach (KeyCode keyCode in _keyCodes)
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        isChange = false;
                        if (_keyDictionary.ContainsKey(keyCode))
                        {
                            KeyCode codeTemp1 = keyCode;
                            KeyCode codeTemp2 = _eventDictionary[keyEvent];
                            UnityEvent eventTemp1 = _keyDictionary[keyCode];
                            UnityEvent eventTemp2 = keyEvent;
                            _keyDictionary.Remove(codeTemp1);
                            _keyDictionary.Remove(codeTemp2);
                            _keyDictionary.Add(codeTemp1, eventTemp2);
                            _keyDictionary.Add(codeTemp2, eventTemp1);
                            _eventDictionary.Remove(eventTemp1);
                            _eventDictionary.Remove(eventTemp2);
                            _eventDictionary.Add(eventTemp1, codeTemp2);
                            _eventDictionary.Add(eventTemp2, codeTemp1);
                            break;
                        }
                        else
                        {
                            _keyDictionary.Remove(_eventDictionary[keyEvent]);
                            _keys.Remove(_eventDictionary[keyEvent]);
                            _keyDictionary.Add(keyCode, keyEvent);
                            _eventDictionary.Remove(keyEvent);
                            _eventDictionary.Add(keyEvent, keyCode);
                            _keys.Add(keyCode);
                            break;
                        }
                    }
                }
                if (!isChange)
                    break;
            }
/*  
          if (Input.anyKeyDown)
            {
                string s = Input.inputString.ToUpper();
                if (s.Length != 0)
                {
                    s = char.ToString(s[0]);
                }
                if (first == KeyCode.None)
                {
                    first = (KeyCode)System.Enum.Parse(typeof(KeyCode), s);
                    Debug.Log(first);
                    if (!_keyDictionary.ContainsKey(first))
                    {
                        Debug.Log("No Key exist");
                        break;
                    }
                }
                else
                {
                    second = (KeyCode)System.Enum.Parse(typeof(KeyCode), s);
                    if (_keyDictionary.ContainsKey(second))
                    {
                        Debug.Log("Key is exist");
                        break;
                    }

                    UnityEvent temp = _keyDictionary[first];
                    _keyDictionary.Remove(first);
                    _keyDictionary.Add(second, temp);

                    Debug.Log(first + " is Change to " + second);
                    break;
                }
            }
*///입력 string을 받아서 처리
            yield return wait;
        }

        Debug.Log("Changing End");
    }
}
