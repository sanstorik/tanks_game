using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

class WeaponAI
{
    public WeaponType weaponType;
    public float weaponForce;
    public float currentPriority;
    public float minusPriorityOnShoot;
    public float priorityAddOnTurn;

    public WeaponAI(WeaponType weaponType, float weaponForce, float currentPriority, float minusPriorityOnShoot,
        float priorityAddOnTurn)
    {
        this.weaponType = weaponType;
        this.weaponForce = weaponForce;
        this.currentPriority = currentPriority;
        this.minusPriorityOnShoot = minusPriorityOnShoot;
        this.priorityAddOnTurn = priorityAddOnTurn;
    }
}

[RequireComponent(typeof(TankController))]
public class AI_Tank : MonoBehaviour {
    private static AI_Tank instance;

    public Transform playerTank;
    List<WeaponAI> weaponsList;
    Rigidbody2D rigidBody;
    TankController tankController;

    bool turnStarted = false;
    bool shouldMoveToTreasure = false;
    Vector3 directionToMove = Vector3.left;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        tankController = GetComponent<TankController>();

        weaponsList = new List<WeaponAI>((int)Weapon.WEAPON_COUNT);

        weaponsList.Add(new WeaponAI(WeaponType.ROCKET, 9, 55, 20, 7));
        weaponsList.Add(new WeaponAI(WeaponType.HOMMING_MISSILE, 10, 65, 25, 8)); 
        weaponsList.Add(new WeaponAI(WeaponType.BANANA, 8, 50, 15, 5)); 
        weaponsList.Add(new WeaponAI(WeaponType.LASOR, 10, 44, 20, 5));
        weaponsList.Add(new WeaponAI(WeaponType.POISON_ARROW, 0, 80, 50, 12));
	}
	
	void FixedUpdate () {
        if (turnStarted && shouldMoveToTreasure)
        {
            rigidBody.velocity = directionToMove * TankController.SPEED * 23/30f  * Time.deltaTime;

            if (tankController.currentWeapon.superWeaponType != SuperWeaponType.NONE)
            {
                shouldMoveToTreasure = false;
                UseSuperWeapon();
            }
        }
    }

    public void StartTurnAI()
    {
        turnStarted = true;

        TreasureDrop treasureDrop;
        var colliders = Physics2D.OverlapCircleAll(transform.position, 5f);

        foreach(var x in colliders)
        {
            if (x.GetComponent<TreasureDrop>() != null)
            {
                tankController.FreezeRotation(false);

                treasureDrop = x.GetComponent<TreasureDrop>();
                directionToMove = treasureDrop.transform.position.x < transform.position.x ? Vector3.left : Vector3.right;
                shouldMoveToTreasure = true;
                break;
            }
        }


        if (!shouldMoveToTreasure && tankController.currentWeapon.superWeaponType != SuperWeaponType.NONE)
        {
            UseSuperWeapon();
            return;
        }

        if (!shouldMoveToTreasure)
            Shoot();
    }

    private void Shoot()
    {
        WeaponAI currentWeapon = weaponsList[0];
        for(int i=0; i < weaponsList.Count; i++)
        {
            if (weaponsList[i].currentPriority > currentWeapon.currentPriority)
                currentWeapon = weaponsList[i];
        }

        for(int i=0; i < weaponsList.Count; i++)
        {
            weaponsList[i].currentPriority += weaponsList[i].priorityAddOnTurn;
        }

        currentWeapon.currentPriority -= currentWeapon.minusPriorityOnShoot;

        SetRotationOfCanon(currentWeapon);
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);

        tankController.currentWeapon.weaponType = currentWeapon.weaponType;
        tankController.Shoot();
        
        EndTurnAI();
    }

    private void SetRotationOfCanon(WeaponAI weapon)
    {
        Quaternion cannonRotation = new Quaternion();

        // look into enemy
        var dir = playerTank.position - tankController.transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        switch (weapon.weaponType)
        {
            case WeaponType.ROCKET:
                angle += 48;
                tankController.PowerOfFireProjectile = 0.69f;
                break;

            case WeaponType.HOMMING_MISSILE:
                angle += 50;
                tankController.PowerOfFireProjectile = 0.8f;
                break;

            case WeaponType.BANANA:
                angle += 60;
                tankController.PowerOfFireProjectile = 0.85f;
                break;

            case WeaponType.LASOR:
                angle += 43;
                tankController.PowerOfFireProjectile = 0.9f;
                break;

            case WeaponType.POISON_ARROW:
                break;
        }

        cannonRotation = Quaternion.AngleAxis(angle, -Vector3.forward);
        tankController.canon.DORotateQuaternion(cannonRotation, 2f);
    }

    private void UseSuperWeapon()
    {
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);
        tankController.UseSuperWeapon();
    }


    void EndTurnAI()
    {
        turnStarted = false;
        shouldMoveToTreasure = false;
    }

    public static AI_Tank INSTANCE
    {
        get { return instance; }
    }
}
