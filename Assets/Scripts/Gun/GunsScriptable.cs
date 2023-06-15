using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GunName", menuName = "GunsScriptable/Guns")]
public class GunsScriptable : ScriptableObject
{
    public bool isUnlocked;
    public Sprite sprite;
    public string Ad;
    public int Price;
    public int SuretliAtesLevelIndex,DamageLevelIndex,ReloadLevelIndex;
    public SuretliAtesLevel[] suretliAtesLevels;
    public DamageLevel[] DamageLevels;
    public ReloadLevel[] ReloadLevels;
    public void Upgrade(string objectname)
    {
        switch(objectname)
        {
            case "SuretliAtesLevel":
            SuretliAtesLevelIndex++;
            break;
            case "DamageLevel":
            DamageLevelIndex++;
            break;
            case "ReloadLevel":
            ReloadLevelIndex++;
            break;
        }
    }
}
[System.Serializable]
public class SuretliAtesLevel
{
    public float Level,SuretliAtes,UpgradePrice,GunAtesSuretiUpgradeFark;
    public bool isFireRateUpgrade;
}
[System.Serializable]
public class DamageLevel
{
    public float Level,Damage,UpgradePrice,GunDamageUpgradeFark;
    public bool isDamageUpgrade;
}
[System.Serializable]
public class ReloadLevel
{
    public float Level,ReloadTime,UpgradePrice,GunReloadUpgradeFark;
    public bool isReloadUpgrade;
}
