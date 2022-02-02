using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Script_CharacterInfo : MonoBehaviour
{
    [SerializeField] Canvas m_Canvas;
    [SerializeField] Text m_PlayerName;
    [SerializeField] List<Image> m_BackpackSlots = new List<Image>();
    Script_Player player;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Script_Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            player.ToggleFunctionallity();
            m_Canvas.enabled = !m_Canvas.enabled;
            m_Canvas.GetComponent<CanvasScaler>().enabled = !m_Canvas.GetComponent<CanvasScaler>().enabled;
            m_Canvas.GetComponent<GraphicRaycaster>().enabled = !m_Canvas.GetComponent<GraphicRaycaster>().enabled;
        }
        if (player != null)
        {
            m_PlayerName.text = "Title: " + player.GamerTag;
            for (int i = 0; i < player.GetAllWeapons().Count; i++)
            {
                m_BackpackSlots[i].sprite = player.GetAllWeapons()[i].GetComponent<Script_Weapon>().HotBarIcon;
            }
        }
        
    }
}
