using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qutu : MonoBehaviour
{
    public GameObject[] Qutular;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Gulle"))
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            foreach(GameObject qutu in Qutular)
            {
                qutu.SetActive(true);
                qutu.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0,5),Random.Range(0,4));
            }
            Destroy(this.gameObject.GetComponent<SpriteRenderer>());
        }
    }
}
