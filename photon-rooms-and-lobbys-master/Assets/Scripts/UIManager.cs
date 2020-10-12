using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour{

    public static UIManager Instance = null;

    [SerializeField]
    private bool initUI = true;

    public GameObject ConnectPanel;
    public GameObject LabelProgress;
    public GameObject LobbyPanel;
    public GameObject RoomPanel;
    public GameObject ScrollPanel;
    //public GameObject PanelMatchOptions;
    public GameObject RoomOptionsPanel;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        if (initUI)
        {
            Clear();
            ConnectPanel.SetActive(true);            
        }
    }
    void Clear()
    {
        ConnectPanel.SetActive(false);
        LabelProgress.SetActive(false);
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        ScrollPanel.SetActive(false);
        RoomOptionsPanel.SetActive(false);
        //PanelMatchOptions.SetActive(false);
    }
    public  void GoToLobby()
    {
        Clear();
        LobbyPanel.SetActive(true);
    }
    public void ShowProgress()
    {
        Clear();
        LabelProgress.SetActive(true);
    }
    public void GotoRoom()
    {
        Clear();
        RoomPanel.SetActive(true);
    }
    
    public void ShowCreateMatchOptions()
    {
        RoomOptionsPanel.SetActive(true);
        MatchOptionsManager matchPanel = RoomOptionsPanel.GetComponent<MatchOptionsManager>();
        matchPanel.SetUpPanel(LobbyActionEnum.Create);
    }
    public void ShowFindMatchOptions()
    {
        RoomOptionsPanel.SetActive(true);
        MatchOptionsManager matchPanel = RoomOptionsPanel.GetComponent<MatchOptionsManager>();
        matchPanel.SetUpPanel(LobbyActionEnum.Find);
    }
    
    public void HideMatchOptions()
    {
        RoomOptionsPanel.SetActive(false);
    }
}
