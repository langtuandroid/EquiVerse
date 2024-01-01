using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class CollectEggPoints : MonoBehaviour
{
    public GameObject particleEffectPrefab;
    
    private void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag("GooseEgg"))
                {
                    ECManager ecManager = FindObjectOfType<ECManager>();

                    if (ecManager != null)
                    {
                        ecManager.AddGooseEggPoints();
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
