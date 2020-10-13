using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerContainerRoom : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI userName, ping;
    Image mImage;
    string userId;
    ColorEnum color = ColorEnum.None;
    Color mColor;

    Dictionary<ColorEnum, Color> colorValues;

    public void Start()
    {
        mImage = GetComponent<Image>();
        colorValues = new Dictionary<ColorEnum, Color>();
        colorValues.Add(ColorEnum.Blue, new Color(0f, 0.9596043f,1f));
        colorValues.Add(ColorEnum.Red, new Color(0.8396226f, 0.05940725f, 0.1013368f));
        colorValues.Add(ColorEnum.Green, new Color(0.2839868f, 0.8962264f, 0.1564169f));
        colorValues.Add(ColorEnum.Yellow,new Color(1f, 0.993843f, 0.08018869f));
        colorValues.Add(ColorEnum.Black, new Color(0.990566f, 0.5562041f, 0f));
        colorValues.Add(ColorEnum.Purple, new Color(0.6621118f, 0.2355375f, 0.745283f));
    }
    public string UserId { get => userId; set => userId = value; }

    public void SetMyUserInfo(string _userId, string _userName, int _ping)
    {
        userName.text = _userName;
        ping.text = _ping.ToString();
        userId = _userId;
    }

    //public void PaintMe(ColorEnum _color)
    //{
    //    mImage.color = colorValues[_color];
    //}


}
