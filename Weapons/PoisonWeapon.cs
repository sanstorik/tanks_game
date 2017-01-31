using UnityEngine;
using System;

public class PoisonWeapon : MonoBehaviour, IPoolableComponent {

    public ParticleSystem arrow;
    public AudioClip fireSound;
    public float hitDamage;
    public float poisonDamage;
    public float poisonTurns;

    GameObject hitEffect;

    public void SetRotation()
    {
        var x = transform.localEulerAngles;
        x.z += 90;

        transform.localRotation = Quaternion.Euler(x);

        arrow.startRotation = (45 - transform.localRotation.eulerAngles.z) / Mathf.Rad2Deg;
    }

    private void OnParticleCollision(GameObject other)
    {
        TankController tank = other.GetComponent<TankController>();
        if (tank != null)
        {
            var pos = other.transform.position;
            pos.x -= 1.7f;
            pos.y += 0.2f;
            tank.RecieveDamage(hitDamage);

            var text = PoolingSystem.Spawn(PoolManager.INSTANCE.GetDamageTextPrefab(), pos);
            text.GetComponent<DamageText>().SetEffect("+POISONED");

            DamageOverTurn.AddPoisonToTank(new Poison(other.gameObject, poisonDamage, poisonTurns));

            hitEffect = PoolingSystem.Spawn(PoolManager.INSTANCE.poisonHitPrefab, other.transform.position);
            Invoke("DespawnCurrObjectAndExplosion", 3f);
        }
    }

    void DespawnCurrObjectAndExplosion()
    {
        PoolingSystem.Despawn(gameObject);
        PoolingSystem.Despawn(hitEffect);
    }

    void DespawnIfNotHitAnything()
    {
        if (gameObject.activeSelf)
            PoolingSystem.Despawn(gameObject);
    }

    public void Spawned()
    {
            CameraController.INSTANCE.ResetTarget();
            AudioController.INSTANCE.PlayAudio(fireSound);

            Invoke("DespawnIfNotHitAnything", 6f);
    }

    public void Despawned()
    {
            TurnController.INSTANCE.SwitchTurn();
    }
}
