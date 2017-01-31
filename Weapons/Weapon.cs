using UnityEngine;


public class Weapon {

    public TankController caster;

    public WeaponType weaponType = WeaponType.ROCKET;
    public SuperWeaponType superWeaponType = SuperWeaponType.NONE;
    public const float SUPER_WEAPON_COUNT = 3;
    public const float WEAPON_COUNT = 5;


    public void Fire(Vector3 startingPosition, Quaternion rotation, float powerOfProjectile)
    {
        var go = PoolingSystem.Spawn(PoolManager.INSTANCE.GetWeaponPrefab(weaponType), startingPosition);
        go.transform.position = startingPosition;
        go.transform.rotation = rotation;

        WeaponBehaviour weaponBehaviour = go.GetComponent<WeaponBehaviour>();
        PoisonWeapon poisonWeapon = go.GetComponent<PoisonWeapon>();
        HommingMissile missile = go.GetComponent<HommingMissile>();

        //IF IT IS A SINGLE WEAPON AND IS RIGIDBODY
        if(weaponBehaviour != null)
              weaponBehaviour.FireAndAddForce(powerOfProjectile);
        // ELSE IF ITS PARTICLE SYSTEM SET NEEDED ROTATION IN SCRIPT
        else if(poisonWeapon != null)
        {
            poisonWeapon.SetRotation();
        }
        if (missile != null)
            missile.CasterIsPlayer(caster.CompareTag("TankPlayer"));


    }

    public void UseSuperWeapon(Quaternion rotation)
    {
        switch (superWeaponType)
        {
            case SuperWeaponType.NONE:
                return;
            case SuperWeaponType.HEAL:
                caster.GainHealth(20);
                TurnController.INSTANCE.SwitchTurn();
                break;
            case SuperWeaponType.METEORITE_SWARM:
                PoolingSystem.Spawn(PoolManager.INSTANCE.GetWeaponPrefab(superWeaponType), 
                    CameraController.INSTANCE.spawnSpot.position);
                break;
            case SuperWeaponType.HAMMER:
                PoolingSystem.Spawn(PoolManager.INSTANCE.GetWeaponPrefab(superWeaponType),
                    FindPositionForHammerSpawn());
                break;
        }

        superWeaponType = SuperWeaponType.NONE;
    }


    Vector3 FindPositionForHammerSpawn()
    {
        TankController target = FindEnemyTank();
        Vector3 pos = target.transform.position;
        pos.y += 11f;
        return pos;
    }

    TankController FindEnemyTank()
    {
        TankController enemyTank;
        GameObject tankEnemy;

        if (caster.CompareTag("TankPlayer"))
            tankEnemy = GameObject.FindGameObjectWithTag("TankEnemy");
        else
            tankEnemy = GameObject.FindGameObjectWithTag("TankPlayer");
        
        enemyTank = tankEnemy.GetComponent<TankController>();

        return enemyTank;
    }
}
