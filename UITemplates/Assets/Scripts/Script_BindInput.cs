using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Script_BindInput : MonoBehaviour
{
    Script_KeyBindings Bindings;
    public GameObject KeyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Bindings = GameObject.FindObjectOfType<Script_KeyBindings>();

        string[] buttonNames = Bindings.GetButtonNames();

        foreach(string button in buttonNames)
        {
            string bn = button;

            GameObject temp = (GameObject)Instantiate(KeyPrefab);
            temp.transform.SetParent(GameObject.FindGameObjectWithTag("KeyBindContext").transform);
            temp.transform.localScale = Vector3.one;

            Text buttonNameText = temp.transform.Find("MenuBind").GetComponent<Text>();
            buttonNameText.text = button;

            Text keyNameText = temp.transform.Find("Button/Text").GetComponent<Text>();
            keyNameText.text = Bindings.GetKeyNameForButton(button);

            Button bindButton = temp.transform.Find("Button").GetComponent<Button>();
            bindButton.onClick.AddListener(() => { StartRebindFor(bn); });
        }
    }

    void StartRebindFor(string _button)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
