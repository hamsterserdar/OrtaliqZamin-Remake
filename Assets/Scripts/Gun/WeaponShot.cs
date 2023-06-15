using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class WeaponShot : MonoBehaviour
{
    [Header("Objects")]
    public GameObject Bullet;
    Animator anim;
    [Header("Transforms")]
    public Transform shootPoint;
    [Header("Sounds")]
    public AudioSource Sound;
    public AudioClip[] audioClip;
    [Header("Text")]
    public TMP_Text Gulle;
    float ReadyNextShoot,elaveedilenGulle,reloadVaxti;
    [Header("Silah Deyerleri")]
    public float fireRate;public float gulleSureti;public float movcudGulle;
    public float silahdakiGulle;public float maxGulle;public float ReloadTime;
    [Header("Bools")]
    public bool AtesBool;public bool ReloadBool;
    [Header("Camera Shake")]
    public CinemachineShake cinemachineShake;
    public float ShakeIntensity,ShakeFrequency,ShakeTime;
    [Header("Weapon Sway")]
    public int gunIndex;
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("ReloadSpeed",ReloadTime);
    }
    void Update()
    {
        Gulle.text = movcudGulle + " / " + silahdakiGulle;
        elaveedilenGulle = maxGulle - movcudGulle;
        if(elaveedilenGulle > silahdakiGulle)
        {
            elaveedilenGulle = silahdakiGulle;
        }
        if(Input.GetMouseButton(0) && !ReloadBool)
        {
            if(movcudGulle <= 0)
            {
                anim.SetTrigger("Reload");
            }
            else{
                if(Time.time > ReadyNextShoot)
                {
                    ReadyNextShoot = Time.time + 1/fireRate;
                    Shoot();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.R) && elaveedilenGulle > 0 && silahdakiGulle > 0 && !ReloadBool)
        {
            if(Time.time > reloadVaxti)
            {
                anim.SetTrigger("Reload");
                reloadVaxti = Time.time;
            }
        }
    }
    public void Shoot()
    {
        movcudGulle--;
        cinemachineShake.ShakeCamera(ShakeIntensity,ShakeIntensity,ShakeTime);
        anim.SetTrigger("Fire");
        GameObject gulle = Instantiate(Bullet,shootPoint.position,shootPoint.rotation);
        Sound.PlayOneShot(audioClip[0]);
        gulle.GetComponent<Bullet>().bulletSpeed = gulleSureti;
        Destroy(gulle,3f);
    }
    private void OnTriggerEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Ground"))
        {
            Vector2 silah = new Vector2(transform.position.x-other.transform.position.x,0);
            transform.position = Vector2.Lerp(transform.position,silah,1);
            Debug.Log("Salam");
        }
    }
    public void ReloadOn()
    {
        Sound.PlayOneShot(audioClip[1]);
        WeaponSway.Instance.BoolDeactives(gunIndex);
        AtesBool = false;
        ReloadBool = true;
    }
    public void ReloadOff()
    {
        movcudGulle += elaveedilenGulle;
        silahdakiGulle -= elaveedilenGulle;
        AtesBool = true;
        ReloadBool = false; 
        WeaponSway.Instance.BoolActivate();
    }
    public void Reset()
    {
        ReloadBool = false;
    }
    void OnDisable()
    {
        anim.Rebind();
        anim.keepAnimatorStateOnDisable = true;
    }
}
