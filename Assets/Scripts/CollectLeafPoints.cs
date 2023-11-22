using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollectLeafPoints : MonoBehaviour
{
    public GameObject particleEffectPrefab;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                        StartCoroutine(DestroyObjectWithEffect(hitObject));
                    }
                }
            }
        }
    }
    
    private IEnumerator DestroyObjectWithEffect(GameObject obj)
    {
        Destroy(obj);
        GameObject particleEffect = Instantiate(particleEffectPrefab, obj.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        Destroy(particleEffect);
    }
}
