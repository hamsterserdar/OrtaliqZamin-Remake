using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caynik : MonoBehaviour
{
    public GameObject Bullet,ShootPoint;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Shoot()
    {
        GameObject BulletObject = Instantiate(Bullet,ShootPoint.transform.position,ShootPoint.transform.rotation);
        
        BulletObject.GetComponent<Bullet>().bulletSpeed = 3;
    }
}
