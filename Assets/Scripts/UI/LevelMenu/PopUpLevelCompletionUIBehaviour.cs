using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PopUpLevelCompletionUIBehaviour : MonoBehaviour
{
    public LevelStatManager levelStatManager;
    public AchievementManager achievementManager;
    public GameManager gameManager;

    [Header("GameObjects")]
    public GameObject popUpLevelCompletionPanelObject;
    public GameObject levelAchievementsPanelObject;
    public GameObject levelStatsPanelObject;
    public GameObject totalEcoEssencePanelObject;
    public GameObject nextSceneButtonObject;
    public GameObject retryLevelButtonObject;
    public GameObject returnToMainMenuButtonObject;

    [Header("Prefabs")]
    public GameObject achievementUIPrefab;
    public GameObject statUIPrefab;

    public Button skipButton; // Reference to the Skip button

    public Color achievedColor;
    public Color newlyAchievedColor;
    public Color notAchievedColor;

    private Dictionary<GameObject, Vector2> originalPositions;
    private bool skipRequested = false; // Flag to check if skip is requested

    private void Start()
    {
        totalEcoEssencePanelObject.SetActive(false);
        nextSceneButtonObject.SetActive(false);
        retryLevelButtonObject.SetActive(false);
        returnToMainMenuButtonObject.SetActive(false);
        originalPositions = new Dictionary<GameObject, Vector2>();

        skipButton.onClick.AddListener(OnSkipButtonClicked); // Add listener to the skip button
    }

    private void OnSkipButtonClicked()
    {
        skipRequested = true;
        StopAllCoroutines(); // Stop all ongoing coroutines
        DisplayAllImmediately(); // Display everything immediately
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
        yield return new WaitForSecondsRealtime(0.25f);
        yield return StartCoroutine(DisplayLevelStats(stats));
        yield return new WaitForSecondsRealtime(0.25f);
        PopInAnimation(totalEcoEssencePanelObject);
        yield return new WaitForSecondsRealtime(0.25f);
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
                    backGroundImage.color = notAchievedColor;
                    backGroundImage.DOColor(newlyAchievedColor, 0.1f).SetDelay(1f + achievementDelay).SetUpdate(true);
                    achievementUI.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.7f, 8, 0.8f).SetDelay(1f + achievementDelay).SetUpdate(true);
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

            rectTransform.SetAsLastSibling();
        }

        yield return new WaitForSecondsRealtime(0.1f);

        foreach (var achievementUI in achievementUIs)
        {
            if (skipRequested) yield break; // Skip animations if skip is requested

            RectTransform rectTransform = achievementUI.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = achievementUI.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                originalPositions[achievementUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence().SetUpdate(true);
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[achievementUI], 0.1f).SetEase(Ease.OutBack));

                yield return sequence.WaitForCompletion();
            }
        }
        yield return new WaitForSecondsRealtime(0.25f);
        if (AchievementChecker.firstTimeCompletion)
        {
            PopInAnimation(nextSceneButtonObject);
        }
        else
        {
            PopInAnimation(returnToMainMenuButtonObject);
        }

        yield return new WaitForSecondsRealtime(0.5f);
        PopInAnimation(retryLevelButtonObject);
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

            rectTransform.SetAsLastSibling();
        }

        yield return new WaitForSecondsRealtime(0.1f);

        foreach (var statUI in statUIs)
        {
            if (skipRequested) yield break; // Skip animations if skip is requested

            RectTransform rectTransform = statUI.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = statUI.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");
                originalPositions[statUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence().SetUpdate(true);
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[statUI], 0.1f).SetEase(Ease.OutBack))
                        .OnUpdate(() => LayoutRebuilder.ForceRebuildLayoutImmediate(levelStatsPanelObject.GetComponent<RectTransform>()));

                yield return sequence.WaitForCompletion();
            }
        }
    }

    private void DisplayAllImmediately()
    {
        popUpLevelCompletionPanelObject.transform.localScale = Vector3.one;
        popUpLevelCompletionPanelObject.SetActive(true);

        for (int i = 1; i < levelStatsPanelObject.transform.childCount; i++)
        {
            GameObject.Destroy(levelStatsPanelObject.transform.GetChild(i).gameObject);
        }

        foreach (var stat in levelStatManager.GetLevelStats())
        {
            GameObject statUI = Instantiate(statUIPrefab, levelStatsPanelObject.transform);
            TextMeshProUGUI statNameText = statUI.transform.Find("StatName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI statValueText = statUI.transform.Find("StatValue").GetComponent<TextMeshProUGUI>();
            statNameText.text = stat.statName;
            statValueText.text = stat.statValue;
            statUI.GetComponent<CanvasGroup>().alpha = 1f;
            statUI.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        for (int i = 1; i < levelAchievementsPanelObject.transform.childCount; i++)
        {
            GameObject.Destroy(levelAchievementsPanelObject.transform.GetChild(i).gameObject);
        }

        foreach (var achievement in achievementManager.GetLevelAchievements())
        {
            GameObject achievementUI = Instantiate(achievementUIPrefab, levelAchievementsPanelObject.transform);
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
                    break;
                case LevelAchievement.AchievementState.NotAchieved:
                    backGroundImage.color = notAchievedColor;
                    break;
            }

            achievementUI.GetComponent<CanvasGroup>().alpha = 1f;
            achievementUI.GetComponent<RectTransform>().localScale = Vector3.one;
        }

        totalEcoEssencePanelObject.SetActive(true);
        totalEcoEssencePanelObject.GetComponent<RectTransform>().localScale = Vector3.one;


        if (AchievementChecker.firstTimeCompletion)
        {
            nextSceneButtonObject.SetActive(true);
        }
        else
        {
            returnToMainMenuButtonObject.SetActive(true);
        }

        retryLevelButtonObject.SetActive(true);
        nextSceneButtonObject.GetComponent<RectTransform>().localScale = Vector3.one;
        returnToMainMenuButtonObject.GetComponent<RectTransform>().localScale = Vector3.one;
        retryLevelButtonObject.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void PopInAnimation(GameObject gameObject)
    {
        gameObject.SetActive(true);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = Vector3.zero;
            rectTransform.DOScale(1, 0.5f).SetEase(Ease.OutExpo).SetUpdate(true);
        }
    }
}
