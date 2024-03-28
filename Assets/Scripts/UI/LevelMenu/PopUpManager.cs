using UnityEngine;

namespace Managers
{
    public class PopUpManager : MonoBehaviour
    {
        public void CloseMessage(GameObject targetPopUp)
        {
            targetPopUp.SetActive(false);
        }

        public void GivePoints(int amountOfPoints)
        {
            LeafPointManager.totalPoints = amountOfPoints;
        }
    
        public void ActivateObject(GameObject objectToActive){
            objectToActive.SetActive(true);
        }
    }
}
