using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureDrop : MonoBehaviour, IPoolableComponent {

    Transform parachute;
    bool falling = true;
    SuperWeaponType type = SuperWeaponType.NONE;
    static List<SuperWeaponType> weaponsReady = weaponsReady = new List<SuperWeaponType>();

    void Start () {
        parachute = transform.GetChild(0);

        FillListWithWeapons();
    }

    private void FillListWithWeapons()
    {
        weaponsReady.Add(SuperWeaponType.HAMMER);
        weaponsReady.Add(SuperWeaponType.HEAL);
        weaponsReady.Add(SuperWeaponType.METEORITE_SWARM);
    }

    IEnumerator  Move()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        float changeValue = 0.5f;

        while (falling)
        {
            rot.z += changeValue;
            var rotate = Quaternion.Euler(rot);
            transform.rotation = rotate;

            if (rot.z < -20 || rot.z > 20)
                changeValue *= -1;

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (falling)
        {
            parachute.gameObject.SetActive(false);
            falling = false;
        }

        TankController controller = collision.gameObject.GetComponent<TankController>();
        if(controller != null)
        {
            controller.currentWeapon.superWeaponType = type;
            if (collision.gameObject.CompareTag("TankPlayer"))
            {
                TurnController.INSTANCE.SuperWeaponButton_SetActive(true);
                TreasureBoxSpawner.PickLeftTreasure();
            }
            else
                TreasureBoxSpawner.PickRightTreasure();

            PoolingSystem.Despawn(gameObject);
        }
    }

    public void Spawned()
    {
            int typeToChoose;
            transform.localEulerAngles = Vector3.zero;
            StartCoroutine(Move());

            if(weaponsReady.Count == 0)
            {
                FillListWithWeapons();
            }

            typeToChoose = Random.Range(0, weaponsReady.Count);

            type = weaponsReady[typeToChoose];
            weaponsReady.Remove(type);
    }

    public void Despawned()
    {
            parachute.gameObject.SetActive(true);
            falling = true;
    }
}
