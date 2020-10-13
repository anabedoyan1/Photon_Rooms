using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCtrl : MonoBehaviourPunCallbacks
{
    public static LobbyCtrl Instance = null;

    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";
    public const string ROOM_CODE_KEY = "cd";


    public int maxPlayers = 4;
    public bool EstrictSearch { get; set; }
    public int GameMode { get => gameMode; set => gameMode = value; }

    private bool flag ;
    #region SerializeFields
    [SerializeField]
    private Transform RoomsContainer; 
    [SerializeField]
    private GameObject prefabRoomItem; 

    #endregion

    private Dictionary<RoomInfo,GameObject> RoomsList; //lista de salas

    //Match Options
    private RoomOptions roomOptions;
    private string RoomName;
    private int Map = 0;
    private int gameMode = 2;
    private string roomCode;


    private int expectedMap = -1;
    private int expectedGameMode = -1;
    private int expectedMaxPlayers = 0;



    #region MonoBehaviour Callbacks 
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        RoomsList = new Dictionary<RoomInfo, GameObject>();
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)maxPlayers;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
    }

    #endregion

    #region Photon Callbacks 

    public override void OnJoinedLobby()
    {
        Debug.Log("Se unio al lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("update room list of " + roomList.Count);
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.CustomProperties);
            ListRoom(room);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No se encontro partida con ese codigo");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No se encontro partida aleatoria");
        if (EstrictSearch  || flag)
        {
            flag = false;
            CreateRoom();
        }
        else
        {
            flag = true;
            PhotonNetwork.JoinRandomRoom();
        }
        
    }
    public override void OnCreatedRoom()
    {        
        Debug.Log("Creaste el Cuarto de nombre : "+ PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnJoinedRoom()
    {

        Debug.Log("Se unio a un cuarto");
        UIManager.Instance.GotoRoom();

    }
    public override void OnCreateRoomFailed(short returnCode, string message) //si la sala existe
    {
        Debug.Log("Fallo en crear una nueva sala \n"+message);
        CreateRoom();
    }
    #endregion

    #region Private Methods

    void RemoveRoomsFromList()
    {
        for (int i = RoomsContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(RoomsContainer.GetChild(i).gameObject);
        }
    }
    void ListRoom(RoomInfo room) 
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject roomListItem;

            if (!RoomsList.ContainsKey(room))
            {
                roomListItem = Instantiate(prefabRoomItem, RoomsContainer);
                RoomsList.Add(room, roomListItem);
            }
            else
            {
                roomListItem = RoomsList[room];
                if (room.RemovedFromList)
                {
                    RoomsList.Remove(room);
                    Destroy(roomListItem);
                }
            }

            RoomItem tempButton = roomListItem.GetComponent<RoomItem>();
            tempButton.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);
        }
    }

    #endregion
  
    public void CreateRoom()
    {
        RoomName = GenerateRoomCode();
        Debug.Log("Creando nueva sala: " + RoomName);
        roomOptions.CustomRoomPropertiesForLobby = new string[] { GAME_MODE_PROP_KEY};
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {{ GAME_MODE_PROP_KEY, gameMode }};
        SetGameMode(gameMode);
        //roomOptions.CustomRoomPropertiesForLobby = new string[] { GAME_MODE_PROP_KEY, ROOM_CODE_KEY };
        //roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {{ GAME_MODE_PROP_KEY, gameMode }, { ROOM_CODE_KEY, roomCode } };       
        PhotonNetwork.CreateRoom(RoomName, roomOptions);        
    }
    public void FindMatch()
    {
        UIManager.Instance.ScrollPanel.SetActive(true);
        var expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable();

        if (gameMode >= 0)
        {
            Debug.Log("El game mode seleccionado es: " + gameMode);
            expectedCustomRoomProperties.Add(GAME_MODE_PROP_KEY, gameMode);
            Debug.Log("Buscando partida con filtros|  Game mode: " + (int)expectedCustomRoomProperties["gm"]);
            PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, (byte)maxPlayers);
        }
    }

    public void FindRoomByCode(string _code)
    {
        Debug.Log("Looking for room : " + _code);
        PhotonNetwork.JoinRoom(_code);
    }

    public string GenerateRoomCode(int stringLength = 5)
    {
        int _stringLength = stringLength - 1;
        string randomString = "";
        string[] characters = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        for (int i = 0; i <= _stringLength; i++)
        {
            randomString = randomString + characters[Random.Range(0, characters.Length)];
        }
        randomString.ToUpper();
        return randomString.ToUpper();
    }

    public void LeaveLobby() //Se enlace al botón cancelar. Se usa para retornar al menú principal
    {
        UIManager.Instance.LobbyPanel.SetActive(false);
        UIManager.Instance.ConnectPanel.SetActive(false);

        PhotonNetwork.LeaveLobby();
    }


    public void SetGameMode(int _gameMode)
    {
        if(_gameMode >= 2)
        {
            gameMode = (byte)Random.Range(0, 1);
        }
        else
        {
            gameMode = (byte)_gameMode;
        }
    }    
    
    public void SetRoomIsVisible(bool visible)
    {
        roomOptions.IsVisible = visible;
    }
        
}
