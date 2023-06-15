using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollow : MonoBehaviour
{
    public Transform transformCharacter;
    void Update()
    {
        transform.localPosition = transformCharacter.localPosition;
    }
}
