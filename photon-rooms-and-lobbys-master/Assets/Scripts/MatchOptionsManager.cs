using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MatchOptionsManager : MonoBehaviour
{
    [SerializeField] Button actionBtn;
    [SerializeField] Toggle privacyBtn;
    TextMeshProUGUI privacyText, actionText;
    LobbyActionEnum action;

    public void Awake()
    {
        privacyBtn.gameObject.SetActive(false);
        privacyBtn.isOn = true;
    }

    public void SetUpPanel(LobbyActionEnum _action)
    {
        
        actionText = actionBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (_action == LobbyActionEnum.Create)
        {
            privacyBtn.gameObject.SetActive(true);
            actionText.text = "Create";
            action = LobbyActionEnum.Create;
        }
        if (_action == LobbyActionEnum.Find)
        {
            actionText.text = "Find";            
            action = LobbyActionEnum.Find;
        }
    }
    public void ValidateAction()
    {       
        if (action == LobbyActionEnum.Create)
        {
            LobbyCtrl.Instance.CreateRoom();
        }
        if (action == LobbyActionEnum.Find)
        {
            LobbyCtrl.Instance.FindMatch();
            UIManager.Instance.HideMatchOptions();
        }
    }    

    public void ChangePrivacy()
    {
        privacyText = privacyBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (privacyBtn.isOn)
        {
            LobbyCtrl.Instance.SetRoomIsVisible(true);
            privacyText.text = "Public";

        }
        else
        {
            LobbyCtrl.Instance.SetRoomIsVisible(false);
            privacyText.text = "Private";
        }
    }

    public void SelectGameMode(int _gameMode)
    {
        switch (_gameMode)
        {
            case 0:
                //Debug.Log("Bomberman");
                LobbyCtrl.Instance.SetGameMode(0);
                break;
            case 1:
                //Debug.Log("Survive");
                LobbyCtrl.Instance.SetGameMode(1);
                break;
            case 2:
                //Debug.Log("Random");
                LobbyCtrl.Instance.SetGameMode(2);
                break;
        }
    }
}
