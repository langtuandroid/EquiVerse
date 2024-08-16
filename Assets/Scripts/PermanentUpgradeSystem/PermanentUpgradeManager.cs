using System;
using System.Collections.Generic;
using System.Net.Mime;
using DG.Tweening;
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

    private PermanentUpgrade currentlySelectedUpgrade;  // Reference to the currently selected upgrade

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
        EcoEssenceRewardsManager.IncrementEcoEssence(3000);
        
        explanationPanelTitleText.gameObject.SetActive(false);
        explanationPanelImage.gameObject.SetActive(false);
        explanationPanelDescriptionText.gameObject.SetActive(false);
        explanationPanelCostText.gameObject.SetActive(false);
        
        PopulateGrid();
    }

    private void PopulateGrid()
    {
        foreach (var upgrade in availableUpgrades)
        {
            CreateButton(upgrade);
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
        if (currentlySelectedUpgrade == null) return;  // Ensure an upgrade is selected

        if (EcoEssenceRewardsManager.totalEcoEssence >= currentlySelectedUpgrade.upgradeCost)
        {
            EcoEssenceRewardsManager.DecrementEcoEssence(currentlySelectedUpgrade.upgradeCost);

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
}

