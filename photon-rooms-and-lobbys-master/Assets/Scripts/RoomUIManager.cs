using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomUIManager : MonoBehaviour
{
    #region Value_Variables
    string roomGameMode, roomCode;
    bool visible;
    #endregion

    #region UI_Variables
    [SerializeField] Toggle privacyBtn;
    TextMeshProUGUI privacyText;
    [SerializeField] TextMeshProUGUI roomGameModeText, roomCodeText;
    #endregion    

    public void SetUpRoomUI(string _roomGM, string _code, bool _visible)
    {
        roomGameMode = _roomGM;
        roomCode = _code;
        visible = _visible;
        roomGameModeText.text = roomGameMode;
        roomCodeText.text = roomCode;
        privacyText = privacyBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (visible)
        {
            privacyBtn.isOn = true;
            privacyText.text = "Public";
        }
        else
        {
            privacyBtn.isOn = false;
            privacyText.text = "Private";
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
}
