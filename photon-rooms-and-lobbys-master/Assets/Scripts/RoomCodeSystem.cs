using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomCodeSystem : MonoBehaviour
{
    [SerializeField] private TMP_InputField codeField;
    private TextMeshProUGUI userString;
    // Start is called before the first frame update
    void Start()
    {
        userString = codeField.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void SendCode()
    {
        Debug.Log("Send code " + codeField.text);
        LobbyCtrl.Instance.FindRoomByCode(codeField.text);
    }
    
}
