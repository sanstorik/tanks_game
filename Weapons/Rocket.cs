using System;
using UnityEngine;

public class Rocket : WeaponBehaviour {

    // Update is called once per frame
    void FixedUpdate () {
        RotateObjectTheWayItFacing();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        explosionObject = PoolingSystem.Spawn(explosionPrefab, transform.position);
        DespawnExplosionAfterDelay(2f);
        CheckForEnemiesAndDealDamage();

        PoolingSystem.Despawn(gameObject);
    }


}
