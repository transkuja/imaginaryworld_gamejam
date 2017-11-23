using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanels : MonoBehaviour {
    [SerializeField]
    GameObject MenuPanel;
    [SerializeField]
    GameObject CreditsPanel;
    
    public void Switch()
    {
        MenuPanel.SetActive(!MenuPanel.activeSelf);
        CreditsPanel.SetActive(!CreditsPanel.activeSelf);
    }
}
