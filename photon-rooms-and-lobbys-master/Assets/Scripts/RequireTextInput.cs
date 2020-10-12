using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]

public class RequireTextInput : MonoBehaviour
{
    [SerializeField]
    private int Requiredlength;

    Button _button;


    private void Awake()
    {
        _button = this.GetComponent<Button>();

    }
    public void CheckInput(string textInput)
    {
        Debug.Log("Checking Input :" +textInput);
        if (textInput.Length < Requiredlength)
            _button.interactable = false;
        else
            _button.interactable = true;
    }
}
