using System;
using UnityEngine;

public class Lasor : WeaponBehaviour {

    short countOfHits;
    const short MAX_COUNT_OF_HITS = 9;

    short explosionToDestroy;

    GameObject[] explosions = new GameObject[MAX_COUNT_OF_HITS];

    public override void Spawned()
    {
        base.Spawned();
        countOfHits = 0;
        explosionToDestroy = 0;
        CameraController.INSTANCE.SetActiveAdditionalGround(true);
    }

    public override void Despawned()
    {
        base.Despawned();
        CameraController.INSTANCE.SetActiveAdditionalGround(false);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        explosions[countOfHits] = PoolingSystem.Spawn(explosionPrefab, transform.position);

        DespawnExplosionAfterDelay(2f);
        countOfHits++;

        CheckForEnemiesAndDealDamage();
        if (countOfHits == MAX_COUNT_OF_HITS-1)
            PoolingSystem.Despawn(gameObject);

    }

    public override void DespawnExplosionAfterDelay(float delay)
    {
        Invoke("Despawn", delay);
    }

    private void Despawn()
    {
        PoolingSystem.Despawn(explosions[explosionToDestroy]);
        explosionToDestroy++;
    }
}
