using System.Collections.Generic;
using UnityEngine;

public class CompanionActivator : MonoBehaviour
{
    void Start()
    {
        ActivateSelectedCompanions();
    }

    void ActivateSelectedCompanions()
    {
        foreach (Transform companion in transform)
        {
            companion.gameObject.SetActive(false);
        }

        foreach (var selectedCompanion in CompanionSelectionManager.selectedCompanions)
        {
            Transform companionPrefab = transform.Find(selectedCompanion.companionTitle);

            if (companionPrefab != null)
            {
                companionPrefab.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Companion prefab not found for: " + selectedCompanion.companionTitle);
            }
        }
    }
}



