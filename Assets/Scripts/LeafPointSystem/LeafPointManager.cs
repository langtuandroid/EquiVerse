using System;
using System.Collections;
using System.Net;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers {
    public class LeafPointManager : MonoBehaviour
    {
        public Image totalPointsBackground;
        private Color originalColor;
        public TextMeshProUGUI totalPointsText;
        public TextMeshProUGUI endOfLevelCostText;
        public static int totalPoints, visualPoints;
        
        [Header("LevelValues")]
        private int startingPoints = 395;
        public int endOfLevelCost;
        
        private DateTime lastVisualUpdate = DateTime.Now;
        private const float updateInterval = 0.01666667f;
        private const int speedFactor = 15;

        private void Start()
        {
            originalColor = totalPointsBackground.color;
            
            totalPoints = startingPoints + UpgradeVariableController.startingPointsBonus;
            visualPoints = totalPoints;
            UpdatePointText();

            endOfLevelCostText.text = endOfLevelCost.ToString();
        }

        public void Update() {
#if(UNITY_EDITOR)
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
            {
                IncrementPoints(300);
            }
#endif
            UpdateVisualPoints();
        }

        public void AddLowValuePoints() {
            IncrementPoints(UpgradeVariableController.lowValuePoints);
        }
        
        public void AddHighValuePoints() {
            IncrementPoints(UpgradeVariableController.highValuePoints);
        }

        public void AddGooseEggPoints() {
            IncrementPoints(UpgradeVariableController.gooseEggPoints);
        }
        
        public void AddCrystalShardPoints() {
            IncrementPoints(UpgradeVariableController.crystalShardPoints);
        }

        public void UpdateVisualPoints() {
            DateTime now = DateTime.Now;
            if ((now - lastVisualUpdate).TotalSeconds > updateInterval) {
                int difference = Math.Abs(visualPoints - totalPoints);
                int changeAmount = Math.Max(1, difference / speedFactor); 

                if (visualPoints < totalPoints) {
                    visualPoints = Math.Min(visualPoints + changeAmount, totalPoints);
                } else if (visualPoints > totalPoints) {
                    visualPoints = Math.Max(visualPoints - changeAmount, totalPoints);
                }

                lastVisualUpdate = now;
                UpdatePointText();
            }
        }

        void UpdatePointText() {
            if (totalPointsText != null) {
                totalPointsText.text = visualPoints.ToString();
            }
        }

        public void IncrementPoints(int amount) {
            totalPoints += amount;
            GameManager.totalLeafPointsCollected += amount;
        }

        public void DecrementPoints(int amount) {
            totalPoints -= amount;
        }

        public void FlickerTotalPointsElement()
        {
            totalPointsBackground.DOColor(new Color(1f, 0.3f, 0.35f, 0.8f), 0.25f)
                .SetLoops(5, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    totalPointsBackground.color = originalColor;
                });
        }
    }
}
