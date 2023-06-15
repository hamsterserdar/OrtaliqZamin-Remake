using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qepik : MonoBehaviour
{
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(Random.Range(2,-2),300));
    }
}
