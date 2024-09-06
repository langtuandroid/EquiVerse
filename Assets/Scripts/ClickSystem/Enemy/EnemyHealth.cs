using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyHealth : Clickable
{
    public int enemyHealth;
    public GameObject reward;
    public GameObject deathParticles;
    public FMODUnity.EventReference deathSound;

    private Tween punchTween;
    private bool isShooting = false;
    private bool isFiring = false; // Flag to prevent multiple coroutines
    private float shootInterval = 0.2f;

    public override void OnClick(Vector3 point)
    {
        if (!isFiring) // Start firing only if it's not already firing
        {
            StartCoroutine(HoldFire());
        }
    }

    public void Die()
    {
        transform.DOScale(0, 0.2f).SetEase(Ease.OutQuint);
        FMODUnity.RuntimeManager.PlayOneShot(deathSound);
        GameObject particlesInstance = Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
        Instantiate(reward, gameObject.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);

        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySpawner.RemoveEnemy(gameObject);

        Destroy(particlesInstance, 5f);
        Destroy(gameObject);
    }

    private IEnumerator HoldFire()
    {
        isFiring = true; // Mark as firing
        while (UnityEngine.Input.GetMouseButton(0))
        {
            // Recalculate the point where the mouse is currently pointing
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Vector3 point = hit.point;
                    OnHitByGun(point); // Update the particle spawn position
                }
                else
                {
                    break; // Exit the coroutine if the mouse moves away from the enemy
                }
            }

            yield return new WaitForSeconds(shootInterval);
        }
        isFiring = false; // Mark as not firing after the loop ends
    }

    private void OnHitByGun(Vector3 point)
    {
        GunUpgrade currentGunUpgrade = FindObjectOfType<GunUpgradeManager>().GetCurrentGunUpgrade();
        if (enemyHealth > currentGunUpgrade.gunDamage)
        {
            if (punchTween != null && punchTween.IsActive())
            {
                punchTween.Complete();
                punchTween = null;
            }

            enemyHealth -= currentGunUpgrade.gunDamage;

            punchTween = transform.DOPunchScale(Vector3.one * 0.05f, 0.5f, 8, 1);

            ParticleSystem particleSystem = Instantiate(currentGunUpgrade.gunParticles, point, Quaternion.identity);
            particleSystem.Play();
            Destroy(particleSystem.gameObject, 1f);

            FMODUnity.RuntimeManager.PlayOneShot(currentGunUpgrade.gunImpactSoundEventPath);

            if (InputOptionsMenu.screenShakeEnabled)
            {
                CameraShake.Instance.ShakeCamera(2f, 0.15f);
            }
        }
        else
        {
            Die();
        }
    }
}
