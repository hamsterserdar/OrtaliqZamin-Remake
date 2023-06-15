using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunShop : MonoBehaviour
{
    private int index = 0;
    public GunsScriptable[] Guns;
    public Image WeaponSprite;
    [Header("Weapon Info Text")]
    public TMP_Text WeaponName;
    public TMP_Text WeaponBulletSpeed,Damage,Reload,GunPrice,Qepik,Notice;
    [Header("Weapon Upgrade Price Text")]
    public TMP_Text GunUpgradeFireRatePrice;
    public TMP_Text GunUpgradeDamagePrice,GunUpgradeReloadPrice;
    [Header("Weapon Upgrade Text")]
    public TMP_Text GunFireRateText;
    public TMP_Text GunDamageText,GunReloadText;
    [Header("Weapon Button")]
    public Button GunUpgradeFireRateButton;
    public Button GunUpgradeDamageButton,GunUpgradeReloadButton,GunPriceButton;
    [Header("Money")]
    public int qepik;
    public void AllScriptableObjectSave()
    {
        foreach(GunsScriptable guns in Guns)
        {
            Save(guns.Ad,guns);
        }
    }
    public void AllScriptableObjectLoad()
    {
        foreach(GunsScriptable guns in Guns)
        {
            Load(guns.Ad,guns);
        }
    }
    void Start()
    {
        CheckUpgradeBool();
        AudioManager.Instance.PlayMusic("GunShopTheme");
    }
    void Update()
    {
        if(!Guns[index].isUnlocked)
        {
            GunPriceButton.gameObject.SetActive(false);
            GunUpgradeFireRateButton.gameObject.SetActive(true);
            GunUpgradeDamageButton.gameObject.SetActive(true);
            GunUpgradeReloadButton.gameObject.SetActive(true);
            GunUpgradeFireRatePrice.text = Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].UpgradePrice + " Qepik";
            GunUpgradeDamagePrice.text = Guns[index].DamageLevels[Guns[index].DamageLevelIndex].UpgradePrice + " Qepik";
            GunUpgradeReloadPrice.text = Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].UpgradePrice + " Qepik";
            GunFireRateText.text = "Atəş Sürəti +<color=green>" + Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].GunAtesSuretiUpgradeFark;
            GunDamageText.text = "Xəsarət +<color=green>" + Guns[index].DamageLevels[Guns[index].DamageLevelIndex].GunDamageUpgradeFark;
            GunReloadText.text = "Maqazin Dəyişmə +<color=green>" + Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].GunReloadUpgradeFark;
        }
        else
        {
            GunPriceButton.gameObject.SetActive(true);
            GunPrice.text = Guns[index].Price.ToString() + " Qepik";
            GunUpgradeFireRateButton.gameObject.SetActive(false);
            GunUpgradeDamageButton.gameObject.SetActive(false);
            GunUpgradeReloadButton.gameObject.SetActive(false);
        }
        Qepik.text = "Qəpik : " + qepik.ToString();
        WeaponName.text = "Ad : " + Guns[index].Ad;
        WeaponBulletSpeed.text = "Sürətli Atəş : " + Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].SuretliAtes.ToString();
        Damage.text = "Xəsarət : " + Guns[index].DamageLevels[Guns[index].DamageLevelIndex].Damage.ToString();
        Reload.text = "Maqazin Dəyişmə Sürəti : " + Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].ReloadTime.ToString();
        WeaponSprite.sprite = Guns[index].sprite;
    }
    public void NextWeapon()
    {
        AudioManager.Instance.PlaySfx("NextPreviousSound");
        if(index != Guns.Length - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        CheckUpgradeBool();
    }
    public void BuyWeapon()
    {
        if(qepik >= Guns[index].Price)
        {
            AudioManager.Instance.PlaySfx("BuySound");
            Guns[index].isUnlocked = false;
            qepik -= Guns[index].Price;
            StartCoroutine(TextSetting("<color=green>Silah Alindi"));
        }
        else
        {
            AudioManager.Instance.PlaySfx("NotBuySound");
            StartCoroutine(TextSetting("<color=red>Pulunuz Catmir"));
        }
    }
    public void UpgradeWeaponFireRate()
    {
        if(qepik >= Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].UpgradePrice)
        {
            AudioManager.Instance.PlaySfx("UpgradeSound");
            if(Guns[index].SuretliAtesLevelIndex != Guns[index].suretliAtesLevels.Length - 1)
            {
                qepik -= (int)Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].UpgradePrice;
                Guns[index].Upgrade("SuretliAtesLevel");
                StartCoroutine(TextSetting("<color=green>Silah Atəş Sürəti Gücləndirildi"));
                CheckUpgradeBool();
            }
            else
            {
                GunUpgradeFireRateButton.interactable = false;
                StartCoroutine(TextSetting("<color=red>Silah Atəş Sürəti Son Səviyyədədir"));
            }
        }
        else
        {
            AudioManager.Instance.PlaySfx("NotBuySound");
            StartCoroutine(TextSetting("<color=red>Pulunuz Çatmır"));
        }
    }
    public void UpgradeWeaponDamage()
    {
        if(qepik >= Guns[index].DamageLevels[Guns[index].DamageLevelIndex].UpgradePrice)
        {
            AudioManager.Instance.PlaySfx("UpgradeSound");
            if(Guns[index].DamageLevelIndex != Guns[index].DamageLevels.Length - 1)
            {
                qepik -= (int)Guns[index].DamageLevels[Guns[index].DamageLevelIndex].UpgradePrice;
                Guns[index].Upgrade("DamageLevel");
                StartCoroutine(TextSetting("<color=green>Silah Xəsarəti Gücləndirildi"));
                CheckUpgradeBool();
            }
            else
            {
                GunUpgradeDamageButton.interactable = false;
                StartCoroutine(TextSetting("<color=red>Silah Xəsarəti Son Səviyyədədir"));
            }
        }
        else
        {
            AudioManager.Instance.PlaySfx("NotBuySound");
            StartCoroutine(TextSetting("<color=red>Pulunuz Çatmır"));
        }
    }
    public void UpgradeWeaponReload()
    {
        if(qepik >= Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].UpgradePrice)
        {
            AudioManager.Instance.PlaySfx("UpgradeSound");
            if(Guns[index].ReloadLevelIndex != Guns[index].ReloadLevels.Length - 1)
            {
                qepik -= (int)Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].UpgradePrice;
                Guns[index].Upgrade("ReloadLevel");
                StartCoroutine(TextSetting("<color=green>Silah Maqazin Dəyişmə Vaxtı Gücləndirildi"));
                CheckUpgradeBool();
            }
            else
            {
                GunUpgradeReloadButton.interactable = false;
                StartCoroutine(TextSetting("<color=red>Silah Maqazin Dəyişmə Vaxtı Son Səviyyədədir"));
            }
        }
        else
        {
            AudioManager.Instance.PlaySfx("NotBuySound");
            StartCoroutine(TextSetting("<color=red>Pulunuz Çatmır"));
        }
    }

    public void PreviousWeapon()
    {
        AudioManager.Instance.PlaySfx("NextPreviousSound");
        if(index != 0)
        {
            index--;
        }
        else
        {
            index = Guns.Length-1;
        }
        CheckUpgradeBool();
    }
    public void CheckUpgradeBool()
    {
        if(!Guns[index].suretliAtesLevels[Guns[index].SuretliAtesLevelIndex].isFireRateUpgrade)
        {
            GunUpgradeFireRateButton.interactable = false;
        }
        else
        {
            GunUpgradeFireRateButton.interactable = true;
        }
        if(!Guns[index].DamageLevels[Guns[index].DamageLevelIndex].isDamageUpgrade)
        {
            GunUpgradeDamageButton.interactable = false;
        }
        else
        {
            GunUpgradeDamageButton.interactable = true;
        }
        if(!Guns[index].ReloadLevels[Guns[index].ReloadLevelIndex].isReloadUpgrade)
        {
            GunUpgradeReloadButton.interactable = false;
        }
        else
        {
            GunUpgradeReloadButton.interactable = true;
        }
    }
    public bool IsSaveFile()
    {
        return Directory.Exists(Application.persistentDataPath + "/SaveGame");
    }
    public void Save(string key,object Scriptable)
    {
        if(!IsSaveFile())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveGame");
        }
        if(!Directory.Exists(Application.persistentDataPath + "/SaveGame/GunShopData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveGame/GunShopData");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/SaveGame/GunShopData/" + key);
        var json = JsonUtility.ToJson(Scriptable);
        binaryFormatter.Serialize(file,json);
        file.Close();
    }
    public void Load(string key,object Scriptable)
    {
        if(!Directory.Exists(Application.persistentDataPath + "/SaveGame/GunShopData"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveGame/GunShopData/");
        }
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/SaveGame/GunShopData/" + key))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/SaveGame/GunShopData/" + key,FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)binaryFormatter.Deserialize(file),Scriptable);
        }
    }
    public IEnumerator TextSetting(string Text)
    {
        Notice.text = Text;
        yield return new WaitForSeconds(1);
        Notice.text = null;
    }
}
