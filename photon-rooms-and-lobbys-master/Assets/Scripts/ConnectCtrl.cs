using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public  enum RegionsCodes
{
    AUTO,
    CAE,
    EU,
    US,
    USW
}

public class ConnectCtrl : MonoBehaviourPunCallbacks
{
    #region SerializeFields

    [SerializeField]
    private int gameVersion = 1;
    [SerializeField]
    private string regionCode = null;

    #endregion

    bool isConnecting;

    #region MonoBehaviour Callbacks

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion= null;
    }
    #endregion

    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected");
    }
    public override void OnConnectedToMaster()
    {
        
        if (isConnecting)
        {
            Debug.Log("Bienvenido Al servidor de Photon \n Region " + PhotonNetwork.CloudRegion + " PING: " + PhotonNetwork.GetPing() + " ms");
            Debug.Log("User ID: " + PhotonNetwork.NetworkingClient.UserId);
            isConnecting = false;
            UIManager.Instance.GoToLobby();
            PhotonNetwork.JoinLobby();
        }
    }
  
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Se perdio la conexion con el servidor\n Casua:\n {0}", cause);
    }
    #endregion   

    #region Public Methods    
    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        UIManager.Instance.ShowProgress();
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {

        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "" + gameVersion;
        }
    }
    public void SetRegion(int index)
    {
        RegionsCodes region = (RegionsCodes)index;
        if (region == RegionsCodes.AUTO)
            regionCode = null;
        else
            regionCode = region.ToString();        

        Debug.Log("Region: " + regionCode);

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = regionCode;
    }

    #endregion

}
