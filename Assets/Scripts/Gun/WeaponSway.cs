using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public static WeaponSway Instance;
    [Header("Silahlar")]
    public GameObject[] Guns;
    [Header("Key")]
    public KeyCode[] Key;
    public bool[] GunsBool;
    int evvelkisilah;
    void Start()
    {
        Instance = this;
    }
    public void Update()
    {   
        for(int i = 0; i < Key.Length;i++)
        {
            if(Input.GetKeyDown(Key[i]) && GunsBool[i])
            {
                for(int k = 0 ;k < Guns.Length; k++)
                {
                    Guns[k].SetActive(k == i);
                }
            }
        }
    }
    public void BoolDeactives(int Bool)
    {
        for(int i = 0 ; i < GunsBool.Length ; i++)
        {
            GunsBool[i] = (i == Bool);
        }
    }
    public void BoolActivate()
    {
        for(int i = 0; i < GunsBool.Length; i++) 
        {
            GunsBool[i] = true;
        }
    }
}
