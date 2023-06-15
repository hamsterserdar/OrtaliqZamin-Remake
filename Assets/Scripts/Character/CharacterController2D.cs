using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    float vertical,originalspeed,coyotaTimeCounter,coyotaTime,offset;
    [HideInInspector]public float horizontal,rigidgravity;
    float jumpBufferCounter;
    bool isWallTouch,isSliding,Die,DamageTake,isLadder,Jump,isClimbing;
    private Rigidbody2D rigid;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    [Header("Vectors")]
    public Vector2 KnockBack;
    [Header("Speed")]
    public float speed;public float shiftSpeed;
    public float ColorChangeSpeed;public float geriQayitma;
    public float jumpingPower;public float slideSpeed;
    public float jumpBufferTime;public float wallJumpDuration;
    public float Health;public float DirmasmaSureti;
    public float geriTepmeVaxti,geriTepmeCounter;
    [Header("Transforms")]
    public Transform groundCheck;public Transform wallCheck;public Transform Child;public Transform SplashEffect;
    public Vector2 wallJumpForce;
    [Header("Layers")]
    public LayerMask atlamaLayer;public LayerMask wallJumpLayer;
    [Header("Bools")]
    public bool isFacingRight;public bool wallJumping;
    [Header("Game")]
    public GameObject BloodParticle;public GameObject Weapon; public AudioSource Walk;
    void Start()
    {
        coyotaTime = 0.2f;
        originalspeed = speed;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidgravity = rigid.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!DialogManager.Instance.dialogueIsPlaying && !Die && geriTepmeCounter <= 0)
        {
                Child.transform.position = this.transform.position;
                if(IsGrounded())
                {
                    coyotaTimeCounter = coyotaTime;
                }
                else
                {
                    coyotaTimeCounter -= Time.deltaTime;
                }
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
                Flip();
                if(isLadder && Mathf.Abs(vertical) > 0f)
                {
                    isClimbing = true;
                }
                if(horizontal != 0 && IsGrounded())
                {
                    if(!Walk.isPlaying && speed == originalspeed)
                    {
                        Walk.pitch = 1f;
                        Walk.Play();
                    }
                    else if(!Walk.isPlaying && speed != originalspeed)
                    {
                        Walk.pitch = 1.2f;
                        Walk.Play();
                    }
                    
                }
                if(Input.GetButtonDown("Jump"))
                {
                    jumpBufferCounter = jumpBufferTime;
                    if(coyotaTimeCounter > 0f && jumpBufferCounter > 0f)
                    {
                        Jump = true;
                        AudioManager.Instance.PlaySfx("Jump");
                        jumpBufferCounter = 0;
                    }
                    else if(isSliding)
                    {
                        wallJumping = true;
                        Invoke("WallJumpStop",wallJumpDuration);
                    }
                }
                else
                {
                    jumpBufferCounter -= Time.deltaTime;
                }
                if(Input.GetButtonUp("Jump"))
                {
                    coyotaTimeCounter = 0f;
                }
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    speed = shiftSpeed;
                }
                else
                {
                    speed = originalspeed;
                }
                if(Input.GetKey(KeyCode.Tab))
                {
                    Time.timeScale = 0.2f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
                else
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
                if(IsWall() && !IsGrounded() && horizontal != 0)
                {
                    isSliding = true;
                }
                else
                {
                    isSliding = false;
                }
                if(DamageTake)
                {
                    spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.red, ColorChangeSpeed*Time.deltaTime);
                }
                else
                {
                    spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.white, ColorChangeSpeed*Time.deltaTime);
                }
        }
        else if(geriTepmeCounter > 0)
        {
            geriTepmeCounter -= Time.deltaTime;
        }
        if(DamageTake)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.red, ColorChangeSpeed*Time.deltaTime);
        }
        else
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color,Color.white, ColorChangeSpeed*Time.deltaTime);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position,new Vector2(0.93f,0.096f),0,atlamaLayer);
    }
    private bool IsWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position,0.2f,wallJumpLayer);
    }
    void FixedUpdate()
    {
        if(!DialogManager.Instance.dialogueIsPlaying && !Die)
        {
            if(isSliding)
            {
                rigid.velocity = new Vector2(rigid.velocity.x,Mathf.Clamp(rigid.velocity.y,-slideSpeed,float.MaxValue));
            }
            if(wallJumping)
            {
                rigid.velocity = new Vector2(-horizontal*wallJumpForce.x,wallJumpForce.y);
            }
            if(Jump)
            {
                rigid.AddForce(new Vector2(rigid.velocity.x,jumpingPower*100));
                Jump = false;
            }
            else if(geriTepmeCounter <= 0)
            {
                rigid.velocity = new Vector2(horizontal*speed,rigid.velocity.y);
            }  
            if(isClimbing)
            {
                rigid.gravityScale = 0f;
                rigid.velocity = new Vector2(rigid.velocity.x,vertical*DirmasmaSureti);
            }
            else
            {
                rigid.gravityScale = rigidgravity;
            }            
        }
    }
    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0,180,0);
        }
    }
    void WallJumpStop()
    {
        wallJumping = false;
    }
    void KnockBackFunction()
    {
        AudioManager.Instance.PlaySfx("Jump");
        geriTepmeCounter = geriTepmeVaxti;
        if(isFacingRight)
        {
            rigid.velocity = new Vector2(-KnockBack.x,KnockBack.y);
        }
        else
        {
            rigid.velocity = new Vector2(KnockBack.x,KnockBack.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "Knife":
            DamageDown(10,20);
            if(Health <= 0 && !Die)
            {
                GameManager.Instance.PlayerDeath = true;
                GameManager.Instance.OlmeSebebiTextConfigure("Bıcağlandın");
                Ol();
            }
            else
            {
                StartCoroutine(ColorChange());
            }
            break;
            
            case "Merdiven":
            isLadder = true;
            break;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch(other.collider.tag)
        {
            case "KnockBackTikan":
            KnockBackFunction();
            DamageDown(10,20);
            Debug.Log("KnockBackTikan");
            if(Health <= 0 && !Die)
            {
                GameManager.Instance.PlayerDeath = true;
                GameManager.Instance.OlmeSebebiTextConfigure("Girdi Tikana Öldü");
                Ol();
            }
            else
            {
                StartCoroutine(ColorChange());
            }
            break;

            case "Enemy":
            DamageDown(100,120);
            Ol();
            break;

            case "Dusmen":
            KnockBackFunction();
            StartCoroutine(ColorChange());
            break;

            case "Qepik":
            AudioManager.Instance.PlaySfx("CollectCoin");
            GameManager.Instance.QepikRefresh();
            Destroy(other.gameObject.transform.parent.gameObject);
            break;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Merdiven"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
    public void DamageDown(int random1,int random2)
    {
        int canAl = Random.Range(random1,random2+1);
        Health -= canAl;
        GameManager.Instance.HealthBarConfigruation(Health);
        GameObject BloodSplash = Instantiate(BloodParticle,SplashEffect.transform.position,SplashEffect.transform.rotation);
        Destroy(BloodSplash,2f);
    }
    public void Ol()
    {
        Child.gameObject.SetActive(true);
        Destroy(rigid);
        animator.SetTrigger("Die");
        Weapon.SetActive(false);
        StartCoroutine(ColorChange());
        Die = true;
        AudioManager.Instance.PlayMusic("Death");
        GameManager.Instance.DeadPanel.SetActive(true);
        spriteRenderer.enabled = false;
    }
    IEnumerator ColorChange()
    {
        DamageTake = true;
        yield return new WaitForSeconds(geriQayitma);
        DamageTake = false;
    }
}
