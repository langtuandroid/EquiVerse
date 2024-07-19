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
    public GameObject levelStatsPanelObject;
    public GameObject totalEcoEssencePanelObject;
    public GameObject buttonsObject;

    [Header("Prefabs")]
    public GameObject achievementUIPrefab;
    public GameObject statUIPrefab;
    
    public Color achievedColor = Color.yellow;
    public Color notAchievedColor = Color.green;

    private Dictionary<GameObject, Vector2> originalPositions;

    private void Start()
    {
        totalEcoEssencePanelObject.SetActive(false);
        buttonsObject.SetActive(false);
        originalPositions = new Dictionary<GameObject, Vector2>();
    }

    public void DisplayAchievements(List<LevelAchievement> achievements, List<LevelStat> levelStats)
    {
        StartCoroutine(OpenPopUpSequence(achievements, levelStats));
    }
    
    public void DisplayLevelStats(List<LevelStat> stats)
    {
        foreach (Transform child in levelStatsPanelObject.transform)
        {
            Destroy(child.gameObject);  // Clear existing stats
        }

        foreach (var stat in stats)
        {
            GameObject statUI = Instantiate(statUIPrefab, levelStatsPanelObject.transform);
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
        }

        StartCoroutine(OpenStatsSequence(stats));
    }
    
    private IEnumerator OpenStatsSequence(List<LevelStat> stats)
    {
        foreach (Transform child in levelStatsPanelObject.transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();

            if (rectTransform != null && canvasGroup != null)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                    .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                    .SetUpdate(true);

                yield return sequence.WaitForCompletion();
            }
        }
    }


    private IEnumerator OpenPopUpSequence(List<LevelAchievement> achievements, List<LevelStat> levelStats)
    {
        popUpLevelCompletionPanelObject.transform.localScale = Vector3.zero;
        PopInAnimation(popUpLevelCompletionPanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
        yield return new WaitForSecondsRealtime(0.2f);
        PopInAnimation(titleText);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
        yield return new WaitForSecondsRealtime(0.5f);
        PopInAnimation(levelStatsPanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
        yield return new WaitForSecondsRealtime(0.5f);
        PopInAnimation(levelAchievementsPanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
        yield return new WaitForSecondsRealtime(0.25f);
        
        DisplayLevelStats(levelStats);
        

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
                FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
                originalPositions[achievementUI] = rectTransform.anchoredPosition;

                Sequence sequence = DOTween.Sequence();
                sequence.Append(rectTransform.DOScale(1, 0.1f).SetEase(Ease.OutExpo))
                        .Join(canvasGroup.DOFade(1, 0.1f).SetEase(Ease.InOutQuad))
                        .Append(rectTransform.DOAnchorPos(originalPositions[achievementUI], 0.1f).SetEase(Ease.OutBack));

                yield return sequence.WaitForCompletion();
            }
        }

        yield return new WaitForSecondsRealtime(0.5f);
        PopInAnimation(totalEcoEssencePanelObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
        yield return new WaitForSecondsRealtime(1f);
        PopInAnimation(buttonsObject);
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/OpeningUIElement");  
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
