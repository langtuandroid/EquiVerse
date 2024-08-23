using System;
using System.Collections.Generic;
using System.Net.Mime;
using Behaviour;
using DG.Tweening;
using Managers;
using Spawners;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PermanentUpgradeCategory
{
    companionUpgrade,
    animalUpgrade,
    combatUpgrade,
    financialUpgrade
}

public enum PermanentUpgradeType
{
    increaseStartingCapitalUpgrade,
    increaseRabbitDeathThresholdUpgrade,
    increaseEggValueUpgrade,
    increaseEggSpawnFrequencyUpgrade,
    decreaseMoveSpeedLeafpointsUpgrade,
    increasePabloMoveSpeedUpgrade,
    increaseRabbitMoveSpeedUpgrade,
    increaseLeafpointValueUpgrade,
    increaseTobyThrowRateUpgrade,
    increaseTobyFoodQualityUpgrade,
    decreaseFoxHungerRate
}

[Serializable]
public class PermanentUpgrade
{
    public string upgradeName;
    public string upgradeDescription;
    public PermanentUpgradeCategory upgradeCategory;
    public PermanentUpgradeType upgradeType;
    public float effectValue;
    public int upgradeCost;
    public Sprite upgradeImage;
    public int requiredWorld;
    public int requiredLevel;

    public void ApplyUpgrade()
    {
        switch (upgradeType)
        {
            case PermanentUpgradeType.increaseStartingCapitalUpgrade:
                UpgradeVariableController.startingPointsBonus += (int)effectValue;
                break;

            case PermanentUpgradeType.increaseRabbitDeathThresholdUpgrade:
                UpgradeVariableController.deathThreshold += (int)effectValue;
                break;

            case PermanentUpgradeType.increaseEggValueUpgrade:
                UpgradeVariableController.gooseEggPoints += (int)effectValue;
                break;

            case PermanentUpgradeType.increaseEggSpawnFrequencyUpgrade:
                UpgradeVariableController.eggDropMinWait -= effectValue;
                UpgradeVariableController.eggDropMaxWait -= effectValue;
                break;

            case PermanentUpgradeType.decreaseMoveSpeedLeafpointsUpgrade:
                UpgradeVariableController.duration += effectValue;
                break;

            case PermanentUpgradeType.increasePabloMoveSpeedUpgrade:
                UpgradeVariableController.moveSpeed += effectValue;
                break;

            case PermanentUpgradeType.increaseRabbitMoveSpeedUpgrade:
                //Rabb.RabbitMoveSpeed += effectValue;
                break;

            case PermanentUpgradeType.increaseLeafpointValueUpgrade:
                UpgradeVariableController.lowValuePoints += (int)effectValue;
                UpgradeVariableController.highValuePoints += (int)effectValue;
                break;

            case PermanentUpgradeType.increaseTobyThrowRateUpgrade:
                UpgradeVariableController.minTimeTillNextThrow -= effectValue;
                UpgradeVariableController.maxTimeTillNextThrow -= effectValue;
                break;

            case PermanentUpgradeType.increaseTobyFoodQualityUpgrade:
                UpgradeVariableController.foodQualityUpgrade = true;
                break;

            case PermanentUpgradeType.decreaseFoxHungerRate:
                UpgradeVariableController.foxHungerDecrementValue = (int)effectValue;
                break;

            default:
                Debug.LogWarning($"Upgrade {upgradeName} does not have a valid upgrade type.");
                break;
        }
        UpgradeVariableController.SaveVariablesToPlayerPrefs();
    }
    
    public bool IsUnlocked()
    {
        string levelKey = $"WORLD_{requiredWorld}_LEVEL_{requiredLevel}";
        return AchievementManager.IsLevelPreviouslyCompleted(levelKey);
    }
}

public class PermanentUpgradeManager : MonoBehaviour
{
    public static PermanentUpgradeManager Instance;

    public Transform gridTransform;
    public GameObject upgradeButtonPrefab;
    public List<PermanentUpgrade> availableUpgrades;

    public TextMeshProUGUI ecoEssenceValueText;

    [Header("Explanation Panel Elements")]
    public TextMeshProUGUI explanationPanelTitleText;
    public Image explanationPanelImage;
    public TextMeshProUGUI explanationPanelDescriptionText;
    public TextMeshProUGUI explanationPanelCostText;

    private Dictionary<PermanentUpgrade, PermanentUpgradeButton> upgradeButtonDictionary =
        new Dictionary<PermanentUpgrade, PermanentUpgradeButton>();

    private PermanentUpgrade currentlySelectedUpgrade;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ecoEssenceValueText.text = EcoEssenceRewardsManager.totalEcoEssence.ToString();

        explanationPanelTitleText.gameObject.SetActive(false);
        explanationPanelImage.gameObject.SetActive(false);
        explanationPanelDescriptionText.gameObject.SetActive(false);
        explanationPanelCostText.gameObject.SetActive(false);

        PopulateGrid();
        LoadPurchasedUpgrades();
    }

    private void PopulateGrid()
    {
        foreach (var upgrade in availableUpgrades)
        {
            if (upgrade.IsUnlocked())
            {
                CreateButton(upgrade);
            }
        }
    }

    private void CreateButton(PermanentUpgrade upgrade)
    {
        GameObject buttonObj = Instantiate(upgradeButtonPrefab, gridTransform);
        PermanentUpgradeButton upgradeButton = buttonObj.GetComponent<PermanentUpgradeButton>();
        upgradeButton.Setup(upgrade);
        upgradeButtonDictionary[upgrade] = upgradeButton;
    }

    public void DisplayUpgrade(PermanentUpgrade upgrade)
    {
        explanationPanelTitleText.gameObject.SetActive(true);
        explanationPanelImage.gameObject.SetActive(true);
        explanationPanelDescriptionText.gameObject.SetActive(true);
        explanationPanelCostText.gameObject.SetActive(true);

        currentlySelectedUpgrade = upgrade;

        explanationPanelTitleText.text = upgrade.upgradeName;
        explanationPanelImage.sprite = upgrade.upgradeImage;
        explanationPanelDescriptionText.text = upgrade.upgradeDescription;
        explanationPanelCostText.text = upgrade.upgradeCost.ToString();
    }

    public void BuyUpgrade()
    {
        if (currentlySelectedUpgrade == null) return;

        if (EcoEssenceRewardsManager.totalEcoEssence >= currentlySelectedUpgrade.upgradeCost)
        {
            EcoEssenceRewardsManager.DecrementEcoEssence(currentlySelectedUpgrade.upgradeCost);

            currentlySelectedUpgrade.ApplyUpgrade();
            SaveUpgradeState(currentlySelectedUpgrade);

            if (upgradeButtonDictionary.TryGetValue(currentlySelectedUpgrade, out PermanentUpgradeButton button))
            {
                button.UpgradeBought();
            }

            explanationPanelTitleText.gameObject.SetActive(false);
            explanationPanelImage.gameObject.SetActive(false);
            explanationPanelDescriptionText.gameObject.SetActive(false);
            explanationPanelCostText.gameObject.SetActive(false);

            currentlySelectedUpgrade = null;
        }
    }

    private void SaveUpgradeState(PermanentUpgrade upgrade)
    {
        string key = $"UPGRADE_{upgrade.upgradeName}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }
    
    private void LoadPurchasedUpgrades()
    {
        foreach (var upgrade in availableUpgrades)
        {
            string key = $"UPGRADE_{upgrade.upgradeName}";
            if (PlayerPrefs.GetInt(key, 0) == 1)
            {
                upgradeButtonDictionary[upgrade].UpgradeBought();
            }
        }
    }
}


