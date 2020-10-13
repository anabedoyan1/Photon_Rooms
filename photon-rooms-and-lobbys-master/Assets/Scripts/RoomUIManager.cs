using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomUIManager : MonoBehaviour
{   
    [SerializeField] GameObject masterPanel;
    [SerializeField] TextMeshProUGUI roomGameModeText, roomCodeText, playersCountText, waitingPlayersMSG, waitingHostMSG;
    [SerializeField] Toggle privacyBtn;
    [SerializeField] Button startBtn;
    [SerializeField] GameObject usersContainer;
    List<GameObject> playerUIContainers = new List<GameObject>();
    TextMeshProUGUI privacyText;// userNameText, pingInfoText;

    public void Awake()
    {
        //userNameText = playerUIContainer.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //pingInfoText = playerUIContainer.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetUpRoomUI(string _roomGM, string _code, bool _visible, int _maxPlayers, int _currentPlayers, bool _masterClient)
    {        
        roomGameModeText.text = _roomGM;
        roomCodeText.text = _code;
        playersCountText.text = _currentPlayers.ToString() + "/" + _maxPlayers.ToString();
        Debug.Log(privacyBtn);
        if (_masterClient)
        {
            masterPanel.SetActive(true);
            SetUpMasterClientUI(_visible);
        }
        else
        {
            masterPanel.SetActive(false);
            if (RoomCtrl.Instance.CurrentPlayersCount == RoomCtrl.Instance.MaxPlayers)
            {
                waitingPlayersMSG.gameObject.SetActive(false);
                waitingHostMSG.gameObject.SetActive(true);
            }
            else
            {
                waitingPlayersMSG.gameObject.SetActive(true);
                waitingHostMSG.gameObject.SetActive(false);
            }
        }
    }
    public void ChangePrivacy()
    {
        if (privacyBtn.isOn)
        {
            RoomCtrl.Instance.ChangePrivacy(true);
            privacyText.text = "Public";

        }
        else
        {
            RoomCtrl.Instance.ChangePrivacy(false);
            privacyText.text = "Private";
        }
    }

    public void SetUpMasterClientUI(bool _visible)
    {
        masterPanel.SetActive(true);
        privacyText = privacyBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (_visible)
        {
            privacyBtn.isOn = true;
            privacyText.text = "Public";
        }
        else
        {
            privacyBtn.isOn = false;
            privacyText.text = "Private";
        }
        if(RoomCtrl.Instance.CurrentPlayersCount == RoomCtrl.Instance.MaxPlayers)
        {
            startBtn.gameObject.SetActive(true);
            waitingPlayersMSG.gameObject.SetActive(false);
            waitingHostMSG.gameObject.SetActive(false);
        }
        else
        {
            waitingPlayersMSG.gameObject.SetActive(true);
            startBtn.gameObject.SetActive(false);
            waitingHostMSG.gameObject.SetActive(false);
        }
    }

    public void usersUpdate(string _userId, string _userName, int _ping)
    {
        PlayerContainerRoom player = Instantiate(Resources.Load<PlayerContainerRoom>("Prefabs/PlayerContainer"));
        player.SetMyUserInfo(_userId, _userName, _ping);
        player.transform.SetParent(usersContainer.transform);
        playerUIContainers.Add(player.gameObject);
    }

    public void DestroyUsers()
    {
        foreach (var player in playerUIContainers)
        {
            Destroy(player);
        }
    }

    //public void PaintUser(string userId, ColorEnum _color)
    //{
    //    foreach (var player in playerUIContainers)
    //    {
    //        PlayerContainerRoom playerInfo = player.GetComponent<PlayerContainerRoom>();
    //        if(playerInfo.UserId == userId)
    //        {
    //            playerInfo.PaintMe(_color);
    //        }
    //    }
    //}


}
