using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public TextMeshProUGUI currentLevelText;

        private void Start()
        {
            //Erwin: Niet belangrijk voor deze schaal maar leuk om te weten. Het volgende performt beter en kan de leesbaarheid verbeteren
            //currentLevelText.text = $"Level {GameManager.LEVEL_INDEX} - {GameManager.WORLD_INDEX}";
            currentLevelText.text = "Level " + GameManager.LEVEL_INDEX + " - " + GameManager.WORLD_INDEX;
        }
    }
}
