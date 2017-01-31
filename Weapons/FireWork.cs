using UnityEngine;

public class FireWork : WeaponBehaviour {

    public override void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public override void Spawned()
    {
        base.Spawned();
        Invoke("FireworkExplode", 2f);
    }
    public override void Despawned()
    {
    }

    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rigidBody.velocity.y, rigidBody.velocity.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FireworkExplode()
    {
        explosionObject = PoolingSystem.Spawn(explosionPrefab, transform.position);
        DespawnExplosionAfterDelay(4f);
        AudioController.INSTANCE.PlayAudio(AudioClipType.FIREWORK);

        PoolingSystem.Despawn(gameObject);
    }
}
