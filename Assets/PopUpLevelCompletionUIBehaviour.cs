using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpLevelCompletionUIBehaviour : MonoBehaviour
{
    public LevelStatManager levelStatManager;
    
    [Header("GameObjects")]
    public GameObject popUpLevelCompletionPanelObject;
    public GameObject levelAchievementsPanelObject;
    public GameObject levelStatsPanelObject;
    public GameObject totalEcoEssencePanelObject;
    public GameObject buttonsObject;

    [Header("Prefabs")]
    public GameObject achievementUIPrefab;
    public GameObject statUIPrefab;

    public Color achievedColor;
    public Color newlyAchievedColor;
    public Color notAchievedColor;

    private Dictionary<GameObject, Vector2> originalPositions;

    private void Start()
    {
        totalEcoEssencePanelObject.SetActive(false);
        buttonsObject.SetActive(false);
        originalPositions = new Dictionary<GameObject, Vector2>();
    }

    public void DisplayAchievements(List<LevelAchievement> achievements)
    {
        StartCoroutine(OpenPopUpSequence(achievements, levelStatManager.GetLevelStats()));
    }

    private IEnumerator OpenPopUpSequence(List<LevelAchievement> achievements, List<LevelStat> stats)
    {
        float achievementDelay = 0;
        popUpLevelCompletionPanelObject.transform.localScale = Vector3.zero;
        PopInAnimation(popUpLevelCompletionPanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
        yield return new WaitForSecondsRealtime(0.5f);

        yield return StartCoroutine(DisplayLevelStats(stats));
        yield return new WaitForSecondsRealtime(0.5f);

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

            switch (achievement.achievementState)
            {
                case LevelAchievement.AchievementState.AlreadyAchieved:
                    backGroundImage.color = achievedColor;
                    break;
                case LevelAchievement.AchievementState.NewlyAchieved:
                    backGroundImage.color = newlyAchievedColor;
                    backGroundImage.DOColor(newlyAchievedColor, 0.1f).SetDelay(1f + achievementDelay);
                    achievementUI.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.7f, 8, 0.8f).SetDelay(1f + achievementDelay);
                    achievementDelay += 0.5f;
                    break;
                case LevelAchievement.AchievementState.NotAchieved:
                    backGroundImage.color = notAchievedColor;
                    break;
            }
        

            achievement.backGroundImage = backGroundImage;

            CanvasGroup canvasGroup = achievementUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = achievementUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            RectTransform rectTransform = achievementUI.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;

            // Ensure new elements appear on top
            rectTransform.SetAsLastSibling();
        }

        // Force layout rebuild if necessary
        yield return new WaitForEndOfFrame(); 

        foreach (var achievementUI in achievementUIs)
        {
            RectTransform rectTransform = achievementUI.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = achievementUI.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                originalPositions[achievementUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[achievementUI], 0.1f).SetEase(Ease.OutBack));

                yield return sequence.WaitForCompletion();
            }
        }

        yield return new WaitForSecondsRealtime(2f);
        PopInAnimation(totalEcoEssencePanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
        yield return new WaitForSecondsRealtime(1f);
        PopInAnimation(buttonsObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
    }

    private IEnumerator DisplayLevelStats(List<LevelStat> stats)
    {
        List<GameObject> statUIs = new List<GameObject>();

        foreach (var stat in stats)
        {
            GameObject statUI = Instantiate(statUIPrefab, levelStatsPanelObject.transform);
            statUIs.Add(statUI);

            TextMeshProUGUI statNameText = statUI.transform.Find("StatName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI statValueText = statUI.transform.Find("StatValue").GetComponent<TextMeshProUGUI>();

            statNameText.text = stat.statName;
            statValueText.text = stat.statValue;

            CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = statUI.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
            RectTransform rectTransform = statUI.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.zero;

            // Ensure new elements appear on top
            rectTransform.SetAsLastSibling();
        }

        yield return new WaitForEndOfFrame(); 

        foreach (var statUI in statUIs)
        {
            RectTransform rectTransform = statUI.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                originalPositions[statUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[statUI], 0.1f).SetEase(Ease.OutBack))
                        .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(levelStatsPanelObject.GetComponent<RectTransform>()));

                yield return sequence.WaitForCompletion();
            }
        }
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
