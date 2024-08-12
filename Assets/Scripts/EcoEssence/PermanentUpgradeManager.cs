using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PermanentUpgradeType
{
    companionupgrade,
    financialUpgrade,
    combatUpgrade
    //another upgrade
}

[Serializable]
public class PermanentUpgrade
{
    public string upgradeName;
    public PermanentUpgradeType upgradeType;
    public int upgradeCost;
    public Image upgradeImage;
}

public class PermanentUpgradeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
