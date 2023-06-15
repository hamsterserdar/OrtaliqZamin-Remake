using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLevel1 : MonoBehaviour
{
    public CinemachineShake cinemachineShake;
    private bool DamageTake,isFacingRight,Die,Tullanma,DalincaDus,ShakeCamera = false;
    [HideInInspector]public bool playerInRange;
    Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private float ReadyNextShoot;
    [Header("Speed")]
    public float Speed;public float geriQayitma; public float Health; public float ShakeIslemeSayi;
    public float TullanmaGucu;public float TullanmaCounter;public float TullanmaVaxti;
    public float fireRate; public float CameraShakeIntensity; public float CameraShakeTime; public float CameraShakeFrequency;
    [Header("Game")]
    public GameObject Died;public GameObject Weapon;public GameObject Dusmen;public GameObject ParticleBlood;
    public GameObject KnifePrefab;
    public Image HealthBar;
    public Transform groundCheck,ShootPoint;
    public LayerMask atlamaLayer;
    public AudioClip audioClip;
    public AudioSource audioSource;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Dusmen.transform.position - ShootPoint.transform.position;
        ShootPoint.transform.right = dir;
        if(TullanmaCounter <= 0 && !playerInRange)
        {
            Tullanma = true;
        }
        if(!IsGrounded())
        {
            transform.position = Vector2.MoveTowards(transform.position,Dusmen.transform.position,Speed*Time.deltaTime);
            if(Time.time > ReadyNextShoot)
            {
                ReadyNextShoot = Time.time + 1/fireRate;
                Shoot();
            }
            ShakeCamera = true;
            ShakeIslemeSayi = 0;
        }
        else
        {
            if(ShakeCamera && ShakeIslemeSayi == 0)
            {
                Shake();
                audioSource.clip = audioClip;
                audioSource.Play();
                ShakeIslemeSayi++;
            }
            if(TullanmaCounter > 0f)
            {
                TullanmaCounter -= Time.deltaTime;
            }
        }
        if(transform.position.x < Dusmen.transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if(transform.position.x > Dusmen.transform.position.x && isFacingRight)
        {
            Flip();
        }                
    }
    private void FixedUpdate()
    {
        if(Tullanma)
        {
            rigid.AddForce(new Vector2(rigid.velocity.x,TullanmaGucu*100));
            TullanmaCounter = TullanmaVaxti;
            Tullanma = false;
            DalincaDus = true;
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,0.2f,atlamaLayer);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0,180,0);
    }
    private void Shoot()
    {
        if(transform.rotation.z > -66f)
        {
            GameObject gulle = Instantiate(KnifePrefab,ShootPoint.position,ShootPoint.rotation);
            Destroy(gulle,2f);               
        }
    }
    public void Shake()
    {
        cinemachineShake.ShakeCamera(CameraShakeIntensity,CameraShakeFrequency,CameraShakeTime);
    }
}
