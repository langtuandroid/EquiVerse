using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpLevelCompletionUIBehaviour : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject titleText;
    public GameObject popUpLevelCompletionPanelObject;
    public GameObject levelAchievementsPanelObject;
    public GameObject totalEcoEssencePanelObject;
    public GameObject buttonsObject;

    [Header("Prefabs")]
    public GameObject achievementUIPrefab;
    
    public Color achievedColor = Color.yellow;
    public Color notAchievedColor = Color.green;

    private Dictionary<GameObject, Vector2> originalPositions;

    private void Start()
    {
        totalEcoEssencePanelObject.SetActive(false);
        buttonsObject.SetActive(false);
        originalPositions = new Dictionary<GameObject, Vector2>();
    }

    public void DisplayAchievements(List<LevelAchievement> achievements)
    {
        StartCoroutine(OpenPopUpSequence(achievements));
    }

    private IEnumerator OpenPopUpSequence(List<LevelAchievement> achievements)
    {
        popUpLevelCompletionPanelObject.transform.localScale = Vector3.zero;
        PopInAnimation(popUpLevelCompletionPanelObject);
        yield return new WaitForSeconds(0.2f);
        PopInAnimation(titleText);
        yield return new WaitForSeconds(0.5f);
        PopInAnimation(levelAchievementsPanelObject);
        yield return new WaitForSeconds(0.25f);

        List<GameObject> achievementUIs = new List<GameObject>();

        foreach (var achievement in achievements)
        {
            GameObject achievementUI = Instantiate(achievementUIPrefab, levelAchievementsPanelObject.transform);
            achievementUIs.Add(achievementUI);

            Image achievementImage = achievementUI.transform.Find("AchievementImage").GetComponent<Image>();
            Image backGroundImage = achievementUI.GetComponent<Image>();
            TextMeshProUGUI descriptionText = achievementUI.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI rewardText = achievementUI.transform.Find("Reward").GetComponentInChildren<TextMeshProUGUI>();

            achievementImage.sprite = achievement.achievementImage;
            descriptionText.text = achievement.achievementDescription;
            rewardText.text = achievement.achievementReward.ToString();
            
            backGroundImage.color = achievement.isAchieved ? achievedColor : notAchievedColor;

            achievement.backGroundImage = backGroundImage;

            CanvasGroup canvasGroup = achievementUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = achievementUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            RectTransform rectTransform = achievementUI.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;
        }

        yield return null;

        foreach (var achievementUI in achievementUIs)
        {
            RectTransform rectTransform = achievementUI.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = achievementUI.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                originalPositions[achievementUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[achievementUI], 0.1f).SetEase(Ease.OutBack));

                yield return sequence.WaitForCompletion();
            }
        }

        yield return new WaitForSeconds(0.5f);
        PopInAnimation(totalEcoEssencePanelObject);
        yield return new WaitForSeconds(1f);
        PopInAnimation(buttonsObject);
    }

    public void PopInAnimation(GameObject gameObject)
    {
        gameObject.SetActive(true);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.zero;
            rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo);
        }
    }
}
