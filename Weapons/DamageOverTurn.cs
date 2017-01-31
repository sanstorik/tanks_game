using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;


public class Poison
{
    TankController tank;
    GameObject poisonedObject;
    float poisonDamage;
    float poisonTurnsLeft;

    GameObject poisonHitObject;

    public Poison(GameObject poisonedObject, float poisonDamage, float poisonTurnsLeft)
    {
        this.poisonDamage = poisonDamage;
        this.poisonedObject = poisonedObject;
        this.poisonTurnsLeft = poisonTurnsLeft;
        tank = poisonedObject.GetComponent<TankController>();
    }

    public override bool Equals(object obj)
    {
        Poison poison = (Poison)obj;
        return poisonedObject.Equals(poison.poisonedObject);
    }

    public override int GetHashCode()
    {
        return poisonedObject.GetHashCode() ^ 1000;
    }

    public void ExecutePoison()
    {
        tank.RecieveDamage(poisonDamage);
        poisonHitObject = PoolingSystem.Spawn(PoolManager.INSTANCE.poisonHitPrefab, tank.transform.position);

        // despawn object in delay using Tween
        poisonedObject.transform.DOShakePosition(0.01f, 0, 0, 0).SetDelay(3f).OnComplete(()=> PoolingSystem.Despawn(poisonHitObject));
        poisonTurnsLeft--;

        if (poisonTurnsLeft == 0)
        {
            Vector3 pos = tank.transform.position;
            pos.y += 0.2f;
            pos.x -= 1.5f;

            var text = PoolingSystem.Spawn(PoolManager.INSTANCE.GetDamageTextPrefab(), pos);
            text.GetComponent<DamageText>().SetEffect("-POISONED");

            DamageOverTurn.poisonToDelete = this;
        }
    }
}


static class DamageOverTurn {
    static HashSet<Poison> poisonOverTurn;
    static public Poison poisonToDelete = null;

    static DamageOverTurn()
    {
        poisonOverTurn = new HashSet<Poison>();
    }

    static public void AddPoisonToTank(Poison poison)
    {
        poisonOverTurn.Add(poison);
    }

    static public void DeletePoisonFromTank ()
    {
        poisonOverTurn.Remove(poisonToDelete);
        poisonToDelete = null;
    }

    static public void DealPoisonDamageToAllPoisoned()
    {
        foreach ( var x in poisonOverTurn)
        {
            x.ExecutePoison();
        }

        if (poisonToDelete != null)
            DeletePoisonFromTank();
    }
}
