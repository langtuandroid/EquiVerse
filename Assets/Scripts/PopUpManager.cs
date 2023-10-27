using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public void CloseMessage(GameObject targetPopUp)
    {
        targetPopUp.SetActive(false);
    }
}
