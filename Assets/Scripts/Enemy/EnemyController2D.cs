 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController2D : MonoBehaviour
{
    private bool DamageTake,isFacingRight,Die,movement = false;
    private int i;
    private SpriteRenderer spriteRenderer;
    private GameObject JumpTransform;
    public enum DusmenNovu{Dialog,BicaqliDusmen,SilahliDusmen}
    [Header("Dusmen")]
    public DusmenNovu dusmenNovu;
    public bool playerInRange;
    [Header("Speed")]
    public float Speed;public float geriQayitma; public float Health;
    public float geriDonmeVaxt1;public float geriDonmeVaxt2;public float geriDonmeCounter;public float movementSpeed;
    [Header("Dialog")]
    public GameObject VisualCue;
    public TextAsset DialogFile;
    [Header("Game")]
    public GameObject Died;public GameObject Weapon;public GameObject Dusmen;public GameObject ParticleBlood;
    public GameObject QepikDrop,BloodSplash;
    public Image HealthBar;
    [Header("baslangicNoqte")]
    public int startingPoint;
    [Header("Yerler")]
    public Transform[] points;
    void Start()
    {
        JumpTransform = transform.Find("JumpTransform").gameObject;
        movement = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(!Die)
        {
            if(dusmenNovu != DusmenNovu.Dialog && Vector2.Distance(transform.position,points[i].position) < 0.02f)
            {
                i++;
                if(i == points.Length)
                {
                    i = 0;
                }
                Flip();
            }
            if (!playerInRange && dusmenNovu != DusmenNovu.Dialog)
            {
                geriDonmeCounter = 0;
                if(transform.position.x < points[i].position.x && !isFacingRight)
                {
                    Flip();
                }
                else if(transform.position.x > points[i].position.x && isFacingRight)
                {
                    Flip();
                }
                transform.position = Vector2.MoveTowards(transform.position,points[i].position,movementSpeed*Time.deltaTime);           
            }
            else if(playerInRange && dusmenNovu != DusmenNovu.Dialog)
            {
                if(geriDonmeCounter <= 0f)
                {
                    if(dusmenNovu != DusmenNovu.Dialog)
                    {
                        if(transform.position.x < Dusmen.transform.position.x && !isFacingRight)
                        {
                            Flip();
                            geriDonmeCounter = Random.Range(geriDonmeVaxt1,geriDonmeVaxt2);
                        }
                        else if(transform.position.x > Dusmen.transform.position.x && isFacingRight)
                        {
                            Flip();
                            geriDonmeCounter = Random.Range(geriDonmeVaxt1,geriDonmeVaxt2);
                        }
                        movement = false;
                    }
                }
                else
                {
                    geriDonmeCounter -= Time.deltaTime;
                }
            }
            switch(dusmenNovu)
            {
                case DusmenNovu.Dialog:
                if(playerInRange && !DialogManager.Instance.dialogueIsPlaying)
                {
                    VisualCue.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.E) && !DialogManager.Instance.dialogueIsPlaying)
                    {
                        VisualCue.SetActive(false);
                        DialogManager.Instance.EnterDialogueMode(DialogFile);
                        Destroy(this);
                    }
                }
                else
                {
                    VisualCue.SetActive(false);
                }
                break;
                case DusmenNovu.BicaqliDusmen:
                if(playerInRange)
                {
                    Weapon.GetComponent<Knife>().Bicaqla();
                }
                break;
            }
            if(DamageTake)
            {
                spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.red, Speed*Time.deltaTime);
            }
            else
            {
                spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.white, Speed*Time.deltaTime);
            }
        }
    }
    public void EnterDialogueMode()
    {
        playerInRange = true;
    }
    public void ExitDialogueMode()
    {
        playerInRange = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Knife"))
        {
            AudioManager.Instance.PlaySfx("KnifeStab");
            StartCoroutine(ColorChange());
            CanAl(Random.Range(30,40));
            GameObject BloodSplash = Instantiate(ParticleBlood,other.transform.position,transform.rotation);
            Destroy(BloodSplash,2f);
        }   
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Gulle"))
        {
            StartCoroutine(ColorChange());
            CanAl(Random.Range(30,40));
            GameObject BloodSplash = Instantiate(ParticleBlood,other.transform.position,transform.rotation);
            Destroy(BloodSplash,2f);          
        }      
    }
    IEnumerator ColorChange()
    {
        DamageTake = true;
        yield return new WaitForSeconds(geriQayitma);
        DamageTake = false;
    }
    public void CanAl(int damage)
    {
        Health -= damage;
        HealthBar.fillAmount = Health/100;
        if(Health <= 0)
        {
            if(QepikDrop != null)
            {
                Instantiate(QepikDrop,new Vector2(transform.position.x,transform.position.y + 0.8f),Quaternion.identity);
            }
            GameObject BloodSplin = Instantiate(BloodSplash,transform.position,Quaternion.identity);
            BloodSplin.transform.parent = gameObject.transform;
            DiedFunction();
        }
    }
    private void DiedFunction()
    {
        JumpTransform.SetActive(false);
        Die = true;
        Died.SetActive(true);
        spriteRenderer.enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        Weapon.SetActive(false);
        Destroy(this);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0,180,0);
    }
}
