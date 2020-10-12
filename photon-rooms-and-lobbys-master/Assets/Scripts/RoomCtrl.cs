using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections;

public class RoomCtrl : MonoBehaviourPunCallbacks
{
    public static RoomCtrl Instance = null;

    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";
    public const string ROOM_CODE_KEY = "cd";

    [SerializeField] private Transform playersContainer;
    [SerializeField] private GameObject playerListItemPrefab;
    [SerializeField] private GameObject MasterPanel;
    [SerializeField] private RoomUIManager roomUI;
    //[SerializeField]
    //private TextMeshProUGUI gameModeText, gameCodeText;
    //[SerializeField]
    //private Dropdown DropdownMap;
    GameModeEnum m_gameMode;
    string roomCode;
    bool visible;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

    }    
    #region Photon Callbacks

    public override void OnJoinedRoom()
    {
        GetRoomGameMode((int)PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE_PROP_KEY]);
        roomCode = PhotonNetwork.CurrentRoom.Name;
        visible = PhotonNetwork.CurrentRoom.IsVisible;
        Debug.Log(roomCode + " " + m_gameMode + " " + visible);
        roomUI.SetUpRoomUI(m_gameMode.ToString(), roomCode, visible);


        //int map = (int)PhotonNetwork.CurrentRoom.CustomProperties[MAP_PROP_KEY];
        //Debug.Log("Map : " + map);
        //DropdownMap.value = map;        
        //RoomName.text = PhotonNetwork.CurrentRoom.Name;
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    MasterPanel.SetActive(true);
        //}
        //else
        //{
        //    MasterPanel.SetActive(false);
        //}
        RemovePlayerFromList();
        FillPlayerList();
    }

    public void GetRoomGameMode(int _gameMode)
    {
        GameModeEnum[] gameModes = { GameModeEnum.Bomberman, GameModeEnum.Survive, GameModeEnum.Random };
        for (int i = 0; i < gameModes.Length; i++)
        {
            if((int)gameModes[i] == _gameMode)
            {
                m_gameMode = gameModes[i];
                break;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RemovePlayerFromList();
        FillPlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemovePlayerFromList();
        FillPlayerList();
        if (PhotonNetwork.IsMasterClient)
        {
            MasterPanel.SetActive(true);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("El Master Cliente Se fue \nEl nuevo Master Client es :" + newMasterClient.NickName);     
;    }

    #endregion

    #region private methods
    void FillPlayerList()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListItemPrefab, playersContainer);
            //Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            //tempText.text = player.NickName;            
        }
    }

    void RemovePlayerFromList()
    {
        // Borra cada entrada de la lista de jugadores
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }
    #endregion

    #region public Methods
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void LeaveRoom() // Retorna al lobby
    {
        UIManager.Instance.GoToLobby();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
    #endregion

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(2);
        // para forzar la actualización de la lista de salas 
        PhotonNetwork.JoinLobby();
    }

    public void ChangePrivacy(bool _visible)
    {
        PhotonNetwork.CurrentRoom.IsVisible = _visible;
        _visible = visible;
    }

}
