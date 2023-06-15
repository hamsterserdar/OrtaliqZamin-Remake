using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private Animator animator;
    public int knifeIndex;
    bool vurmaq = true;
    public enum IstifadeNovu
    {
        Dusmen,Zamin        
    }
    public IstifadeNovu istifadeNovu;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && istifadeNovu == IstifadeNovu.Zamin && !DialogManager.Instance.dialogueIsPlaying )
        {
            Bicaqla();
            WeaponSway.Instance.BoolDeactives(knifeIndex);
        }
    }
    public void VuraBilmek()
    {
        vurmaq = true;
        WeaponSway.Instance.BoolActivate();
    }
    public void Bicaqla()
    {
        if(vurmaq)
        {
            animator.SetTrigger("Attack");
            vurmaq = false;
        }
    }
    public void SoundPlay()
    {
        AudioManager.Instance.PlaySfx("KnifeAttack");
    }
    void OnDisable()
    {
        animator.Rebind();   
    }
}
