using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollectLeafPoints : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag("LowValue"))
                {
                    ECManager ecManager = FindObjectOfType<ECManager>();

                    if (ecManager != null)
                    {
                        ecManager.AddLowValuePoints();
                        Destroy((hitObject));
                    }
                }
            }
        }
    }
}
