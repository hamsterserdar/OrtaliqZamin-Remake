using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypads : MonoBehaviour
{
    public TMP_Text PasswordText;
    public string TrueAnswer;
    public void Send(string Key)
    {
        PasswordText.text += Key;
    }
    public void Remove()
    {
        PasswordText.text = null;
    }
    public void Dedect()
    {
        if(PasswordText.text == TrueAnswer)
        {
            Debug.Log("Duzdur");
        }
        else
        {
            Debug.Log("sehvdi");
        }
    }
}
