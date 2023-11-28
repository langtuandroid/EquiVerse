using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public void CloseMessage(GameObject targetPopUp)
    {
        targetPopUp.SetActive(false);
    }

    public void GivePoints(int amountOfPoints)
    {
        ECManager.totalPoints = amountOfPoints;
    }
    
    public void ActivateObject(GameObject objectToActive){
        objectToActive.SetActive(true);
    }
}
