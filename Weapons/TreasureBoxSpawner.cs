using UnityEngine;

static public class TreasureBoxSpawner
{

    static bool isTreasureOnLeft = false;
    static bool isTreasureOnRight = false;
    static bool rightTreasureIsPickedOnce = false;
    static bool leftTreasureIsPickedOnce = false;

    static public void SpawnTreasureBox()
    {
        bool isPlayerTurn = TurnController.INSTANCE.isPlayerTurn;
        bool couldBeCreated = true;

        float randomNumber = Random.Range(0, 3);
        if (randomNumber == 1)
        {
            Vector3 pos = CameraController.INSTANCE.spawnSpot.position;

            float posFromCenter = isPlayerTurn ? -4 : 4.5f;
            pos.x += posFromCenter;

            if (isPlayerTurn && leftTreasureIsPickedOnce)
                pos.x -= 3f;
            else if (!isPlayerTurn && rightTreasureIsPickedOnce)
                pos.x += 3f;

            couldBeCreated = isPlayerTurn ? !isTreasureOnLeft : !isTreasureOnRight;
            if (couldBeCreated)
            {
                PoolingSystem.Spawn(PoolManager.INSTANCE.treasurePrefab, pos);

                if (isPlayerTurn)
                    isTreasureOnLeft = true;
                else
                    isTreasureOnRight = true;
            }
        }
    }

    static public void PickLeftTreasure()
    {
        isTreasureOnLeft = false;
        leftTreasureIsPickedOnce = !leftTreasureIsPickedOnce;
    }

    static public void PickRightTreasure()
    {
        isTreasureOnRight = false;
        rightTreasureIsPickedOnce = !rightTreasureIsPickedOnce;
    }
}
