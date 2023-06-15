using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private int i;
    [Header("Suret")]
    public float rotationspeed;
    [Header("baslangicNoqte")]
    public int startingPoint;
    [Header("Yerler")]
    public Transform[] points;
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,rotationspeed);
        if(Vector2.Distance(transform.position,points[i].position) < 0.02f)
        {
            i++;
            if(i == points.Length)
            {
                i = 0;
            }
            Flip();
        }
        transform.position = Vector2.MoveTowards(transform.position,points[i].position,rotationspeed*Time.deltaTime);
    }
    private void Flip()
    {
        transform.Rotate(0,180,0);
    }
}
