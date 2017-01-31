using UnityEngine;

public class Meteor : WeaponBehaviour {

    public ParticleSystem fire;
    public ParticleSystem smoke;

    public void ChangeParticleSystemSize()
    {
        fire.startSize = transform.localScale.x * 2f;
        smoke.startSize = transform.localScale.x * 2f;
        AudioController.INSTANCE.PlayAudio(AudioClipType.ROCKET_SOUND, transform.localScale.x * 2f);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Meteor>() == null)
        {
            explosionObject = PoolingSystem.Spawn(explosionPrefab, transform.position);
            DespawnExplosionAfterDelay(2f);
            CheckForEnemiesAndDealDamage();

            PoolingSystem.Despawn(gameObject);
        }
    }
}
