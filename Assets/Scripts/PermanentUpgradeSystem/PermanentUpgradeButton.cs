using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PermanentUpgradeButton : MonoBehaviour
{
    public Button buttonComponent;
    public TextMeshProUGUI titleText;
    public Image upgradeImage;
    public TextMeshProUGUI costText;
    public GameObject boughtBanner;

    private PermanentUpgrade upgrade;

    public void Setup(PermanentUpgrade upgrade)
    {
        this.upgrade = upgrade;
        titleText.text = upgrade.upgradeName;
        upgradeImage.sprite = upgrade.upgradeImage;
        costText.text = upgrade.upgradeCost.ToString();

        string key = $"UPGRADE_{upgrade.upgradeName}";
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            UpgradeBought();
        }
        else
        {
            buttonComponent.interactable = true;
            boughtBanner.SetActive(false);
        }
    }

    public void OnClick()
    {
        PermanentUpgradeManager.Instance.DisplayUpgrade(upgrade);
    }

    public void UpgradeBought()
    {
        buttonComponent.interactable = false;
        boughtBanner.SetActive(true);
    }
}
