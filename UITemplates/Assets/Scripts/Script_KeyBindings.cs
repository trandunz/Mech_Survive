using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class Script_KeyBindings : MonoBehaviour
{
    Dictionary<string, KeyCode> KeyBindings;

    private void OnEnable()
    {
        KeyBindings = new Dictionary<string, KeyCode>();

        KeyBindings["Forward"] = KeyCode.W;
        KeyBindings["Backward"] = KeyCode.S;
        KeyBindings["Left"] = KeyCode.A;
        KeyBindings["Right"] = KeyCode.D;
    }
    public bool GetButtonDown(string buttonName)
    {
        if (!KeyBindings.ContainsKey(buttonName))
        {
            Debug.LogError("InputManager::GetKeyDown - No Button Named '" + buttonName + "'");
        }
        return Input.GetKeyDown(KeyBindings[buttonName]);
    }
    public string[] GetButtonNames()
    {
        return KeyBindings.Keys.ToArray();
    }

    public string GetKeyNameForButton(string buttonName)
    {
        if (!KeyBindings.ContainsKey(buttonName))
        {
            Debug.LogError("InputManager::GetKeyDown - No Button Named '" + buttonName + "'");
        }
        return KeyBindings[buttonName].ToString();
    }
}
