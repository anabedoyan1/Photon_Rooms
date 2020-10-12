using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField]
    private Text nameText; // texto con nombre de sala
    [SerializeField]
    private Text sizeText; // muestra tamaño de la sala

    private string roomName;
    private int roomSize;
    private int playerCount;

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void SetRoom(string name, int size, int players)
    {
        roomName = name;
        roomSize = size;
        playerCount = players;
        nameText.text = name;
        sizeText.text = players + "/" + size;
    }
}
