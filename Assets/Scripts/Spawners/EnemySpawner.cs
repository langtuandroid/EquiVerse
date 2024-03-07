using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;
    public World1LevelSoundController soundController;
    
    public float initialSpawnDelay;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 5f;
    public Transform enemySpawnLocation;
    public ParticleSystem portalOpeningParticleSystem;
    public GameObject enemyPrefab;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public TextMeshProUGUI newEnemyTypeWarningText;

    public void SpawnEnemies()
    {
        StartCoroutine(EnemySpawnSequence());
    }

    IEnumerator EnemySpawnSequence()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        newEnemyTypeWarningText.gameObject.SetActive(true);
        newEnemyTypeWarningText.text = "Alert! Portal fluctuations indicate a <b>Swamp Golem</b> is about to gatecrash the party!";
                    
        soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 1.2f);
        soundController.StartAudioEvent("BattleMusic");
        soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 1f, 1.2f);
                    
        yield return new WaitForSeconds(10f);
        newEnemyTypeWarningText.gameObject.SetActive(false);
        portalOpeningParticleSystem.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SpawnPortal/SpawnPortalCharge");
        yield return new WaitForSeconds(5f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/Spawn");
        soundController.FadeAudioParameter("BattleMusic", "Suspense_Action_Transition", 1f, 0.5f);
        GameObject newEnemy = Instantiate(enemyPrefab, enemySpawnLocation.position, Quaternion.identity);
        activeEnemies.Add(newEnemy); // Add the newly instantiated enemy to the active list.
        AnimateEnemySpawnIn(newEnemy, 0.5f);
        portalOpeningParticleSystem.Stop();
    }

    public void AnimateEnemySpawnIn(GameObject enemy, float duration)
    {
        enemy.transform.localScale = Vector3.zero;
        enemy.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    public void RemoveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
            Destroy(enemyToRemove); // Destroy the enemy game object.
        }
        
        if (activeEnemies.Count == 0)
        {
            StartCoroutine(StartSpawnDelayCountdown());
            soundController.FadeAudioParameter("BattleMusic", "Suspense_Action_Transition", 0f, 0.5f);
            soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 0f, 1.2f);
            soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 1f, 1.2f);
        }
    }

    IEnumerator StartSpawnDelayCountdown()
    {
        float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(spawnDelay);
        
        StartCoroutine(EnemySpawnSequence());
    }
}
