using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{ 
    string userName, userId;
    bool masterClient;
    ColorEnum m_color;
    public PlayerManager(string _userId, string _userName, bool _masterClient)
    {
        this.userName = _userId;
        this.userId = _userName;
        this.masterClient = _masterClient;
    }

    public void SetDefaultColor(List<ColorEnum> pickedColors)
    {
        ColorEnum[] colors = { ColorEnum.Blue, ColorEnum.Red, ColorEnum.Green, ColorEnum.Yellow, ColorEnum.Black, ColorEnum.Purple };
        List<ColorEnum> availableColors = new List<ColorEnum>();
        foreach (var color in pickedColors)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if(color != colors[i])
                {
                    availableColors.Add(color);
                }
            }
        }
        for (int i = 0; i < 1; i++)
        {
            m_color = availableColors[Random.Range(0, availableColors.Count)];
            Debug.Log(userName + ": " + m_color.ToString());
            break;
        }
    }

    public void SetColor(ColorEnum _color)
    {
        m_color = _color;
    }

}
