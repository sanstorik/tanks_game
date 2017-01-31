using UnityEngine;


public class HammerWeapon : WeaponBehaviour {

    public override void Spawned()
    {
            ScreenOpacity.INSTANCE.MakeSmoothDarker(0.5f, 2f);
            AudioController.INSTANCE.PlayAudio(AudioClipType.DIE_INSECT);
    }

    public override void Despawned()
    {
            ScreenOpacity.INSTANCE.MakeSmoothNormal(3f);
            TurnController.INSTANCE.SwitchTurn();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        explosionObject = PoolingSystem.Spawn(explosionPrefab, collision.transform.position);
        DespawnExplosionAfterDelay(2f);

        CheckForEnemiesAndDealDamage();
        PoolingSystem.Despawn(gameObject);
    }
}
