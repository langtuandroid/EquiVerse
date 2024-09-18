using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using DG.Tweening;
using Spawners;

[System.Serializable]
public class Enemy
{
    public String enemyName;
    public GameObject enemyPrefab;
    public int countToSpawn;
}

public class EnemySpawner : MonoBehaviour
{
    public GameManager gameManager;
    public LevelSoundController soundController;
    public float initialSpawnDelay;
    public float minSpawnDelay;
    public float maxSpawnDelay;
    public Transform enemySpawnLocation;
    public ParticleSystem portalOpeningParticleSystem;
    public List<Enemy> enemyTypes = new List<Enemy>();
    public TextMeshProUGUI newEnemyTypeWarningText;

    private bool firstEnemySpawned;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool firstEnemyTutorialStepCompleted = false;

    public static bool enemyDanger;

    private void Start()
    {
        firstEnemySpawned = false;
        enemyDanger = false;
    }

    public void SpawnEnemies()
    {
        StartCoroutine(EnemySpawnSequence());
    }

    IEnumerator EnemySpawnSequence()
    {
        if (!firstEnemySpawned)
        {
            yield return new WaitForSeconds(initialSpawnDelay);
            firstEnemySpawned = true;
        }

        if (gameManager.level2 && !firstEnemyTutorialStepCompleted)
        {
            TutorialManager.GoToNextStep();
            firstEnemyTutorialStepCompleted = true;
        }
        Enemy randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Count)];
        int enemyCount = randomEnemy.countToSpawn;
        string enemyName = randomEnemy.enemyName;
        
        newEnemyTypeWarningText.gameObject.SetActive(true);
        newEnemyTypeWarningText.text = $"ALERT! {enemyCount} <b>{enemyName}</b>(s) are about to invade!";
                    
        soundController.FadeAudioParameter("Music", "World1LevelMainMusicVolume", 0f, 1.2f);
        soundController.StartAudioEvent("BattleMusic");
        soundController.FadeAudioParameter("BattleMusic", "EnemyMusicVolume", 1f, 1.2f);

        enemyDanger = true;
                    
        yield return new WaitForSeconds(10f);
        newEnemyTypeWarningText.gameObject.SetActive(false);
        portalOpeningParticleSystem.Play();
        FMODUnity.RuntimeManager.PlayOneShot("event:/SpawnPortal/SpawnPortalCharge");
        yield return new WaitForSeconds(5f);
        FMODUnity.RuntimeManager.PlayOneShot("event:/World1/SwampGolem/Spawn");
        soundController.FadeAudioParameter("BattleMusic", "Suspense_Action_Transition", 1f, 0.5f);
        
        for (int i = 0; i < randomEnemy.countToSpawn; i++)
        {
            GameObject newEnemy = Instantiate(randomEnemy.enemyPrefab, enemySpawnLocation.position, Quaternion.identity);
            activeEnemies.Add(newEnemy);
            AnimateEnemySpawnIn(newEnemy, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        
        portalOpeningParticleSystem.Stop();
    }

    public void AnimateEnemySpawnIn(GameObject enemy, float duration)
    {
        Vector3 originalScale = enemy.transform.localScale;
        if (enemy.transform.localScale != Vector3.zero)
        {
            enemy.transform.localScale = Vector3.zero;
        }
        enemy.transform.DOKill();
        enemy.transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
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
            enemyDanger = false;
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
