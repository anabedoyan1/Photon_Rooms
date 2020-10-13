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
    

    GameModeEnum m_gameMode;
    string roomCode;
    bool visible;
    int maxPlayers, currentPlayersCount;

    public int CurrentPlayersCount { get => currentPlayersCount; set => currentPlayersCount = value; }
    public int MaxPlayers { get => maxPlayers; set => maxPlayers = value; }

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
        UpdateRoomData();
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
        UpdateRoomData();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomData();
        RemovePlayerFromList();
        FillPlayerList();
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
            roomUI.usersUpdate(PhotonNetwork.NetworkingClient.UserId, player.NickName, PhotonNetwork.GetPing());            
        }
    }

    void RemovePlayerFromList()
    {
        // Borra cada entrada de la lista de jugadores
        roomUI.DestroyUsers();
    }
    #endregion

    #region public Methods
    public void UpdateRoomData()
    {
        GetRoomGameMode((int)PhotonNetwork.CurrentRoom.CustomProperties[GAME_MODE_PROP_KEY]);
        roomCode = PhotonNetwork.CurrentRoom.Name;
        visible = PhotonNetwork.CurrentRoom.IsVisible;
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        currentPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(roomCode + " " + m_gameMode + " " + visible);
        roomUI.SetUpRoomUI(m_gameMode.ToString(), roomCode, visible, maxPlayers, currentPlayersCount, PhotonNetwork.IsMasterClient);
    }
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

    //public void ChooseColor(int _color)
    //{
    //    ColorEnum userColor; 
    //    ColorEnum[] colors = { ColorEnum.Blue, ColorEnum.Red, ColorEnum.Green, ColorEnum.Yellow, ColorEnum.Black, ColorEnum.Purple };
    //    for (int i = 0; i < colors.Length; i++)
    //    {
    //        if(i == _color)
    //        {
    //            userColor = colors[i];
    //            roomUI.PaintUser(PhotonNetwork.NetworkingClient.UserId, userColor);
    //            break;
    //        }
    //    }
    //}
}
