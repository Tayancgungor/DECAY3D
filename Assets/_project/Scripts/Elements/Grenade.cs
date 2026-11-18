using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NUnit.Framework;


public class Grenade : MonoBehaviour
{
    private GameDirector _gameDirector;
    public List<Enemy> enemiesInRange;
    public void StartGrenade(GameDirector gameDirector) 
    {
        _gameDirector = gameDirector;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Wall"))
        {
            Explode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy !=null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void Explode()
    {
        _gameDirector.fxManager.PlayGrenadeExplosionFX(transform.position);

        foreach (var enemy in enemiesInRange)
        {
            if (enemy !=null)
            {
                enemy.TakeDamage(10);
            }
        }
        Destroy(gameObject);

    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
