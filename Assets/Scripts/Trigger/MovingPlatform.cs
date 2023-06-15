using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private int i;
    public enum EngelPlatformNovu{Parent,NoParent}
    public EngelPlatformNovu engelPlatformNovu;
    [Header("Suret")]
    public float speed;
    public float waitDuration;
    int speedMultiplier = 1;

    [Header("baslangicNoqte")]
    public int startingPoint;
    [Header("Yerler")]
    public Transform[] points;
    public GameObject Main;
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,points[i].position) < 0.02f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
            StartCoroutine(WaitNextPosition());
        }
        var suret = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position,points[i].position,suret);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(engelPlatformNovu == EngelPlatformNovu.Parent)
        {
            other.transform.SetParent(transform,true);
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {   
        if(engelPlatformNovu == EngelPlatformNovu.Parent)
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if(engelPlatformNovu == EngelPlatformNovu.Parent)
        {
            other.transform.SetParent(Main.transform);
        }
    }
    IEnumerator WaitNextPosition()
    {
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitDuration);
        speedMultiplier = 1;
    }
}
