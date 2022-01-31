using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Script_MainMenu : MonoBehaviour
{
    public string Title = "TITLE OF GAME";
    public int Version_Major = 0;
    public int Version_Minor = 1;

    float m_MouseSensitivity = 1;
    TMPro.TextMeshProUGUI m_TitleText;
    TMPro.TextMeshProUGUI m_VersionText;
    Slider MouseSenseSlider;
    Dropdown Resolutions;

    void Start()
    {
        Screen.fullScreen = true;
        Resolutions = GameObject.FindGameObjectWithTag("Resolutions").GetComponent<Dropdown>();
        MouseSenseSlider = GameObject.FindGameObjectWithTag("SenseSlider").GetComponent<Slider>();
        m_VersionText = GameObject.FindGameObjectWithTag("VersionText").GetComponent<TMPro.TextMeshProUGUI>();
        m_TitleText = GameObject.FindGameObjectWithTag("TitleText").GetComponent<TMPro.TextMeshProUGUI>();
    }
    void Update()
    {
        UpdateVersionText();
        UpdateTitleText();
    }

    void UpdateVersionText()
    {
        m_VersionText.text = "Version " + Version_Major + "." + Version_Minor;
    }
    void UpdateTitleText()
    {
        m_TitleText.text = Title;
    }
    public void SetResolution()
    {
        switch(Resolutions.value)
        {
            case 0:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1280, 1024, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 4:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 5:
                Screen.SetResolution(1920, 1200, Screen.fullScreen);
                break;
            case 6:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
            case 7:
                Screen.SetResolution(3440, 1440, Screen.fullScreen);
                break;
            case 8:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
            default:
                break;
        }
        
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = true;
    }
    public void ToggleWindowed()
    {
        Screen.fullScreen = false;
    }
    public void SetMouseSensitivity()
    {
        m_MouseSensitivity = MouseSenseSlider.value;
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
