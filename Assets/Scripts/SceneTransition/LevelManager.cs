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
            currentLevelText.text = "Level " + GameManager.LEVEL_INDEX + " - " + GameManager.WORLD_INDEX;
            
        }
    }
}
