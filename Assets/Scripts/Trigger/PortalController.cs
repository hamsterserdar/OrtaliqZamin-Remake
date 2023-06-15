using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform Destination;
    GameObject Player;
    Rigidbody2D rigid;
    Animation anim;
    Animator thisanim;
    float teleporttimer = 0;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rigid = Player.GetComponent<Rigidbody2D>();
        anim = Player.GetComponent<Animation>();
        thisanim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(Vector2.Distance(Player.transform.position,transform.position) > 0.3f)
            {
                StartCoroutine(PortalIn());
            }
        }
    }
    IEnumerator PortalIn()
    {
        rigid.simulated = false;
        StartCoroutine(MoveInPortal());
        anim.Play("PortalIn");
        thisanim.SetTrigger("Destroy");
        yield return new WaitForSeconds(0.5f);
        Player.transform.position = Destination.transform.position;
        rigid.velocity = Vector2.zero;
        anim.Play("PortalOut");
        yield return new WaitForSeconds(0.2f);
        Destination.GetComponent<Animator>().SetTrigger("Destroy");
        yield return new WaitForSeconds(0.3f);
        rigid.simulated = true;
        teleporttimer = 0;
    }
    IEnumerator MoveInPortal()
    {
        float timer = 0;
        while (timer < 0.5f)
        {
            Player.transform.position = Vector2.MoveTowards(Player.transform.position,transform.position,3*Time.deltaTime);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
