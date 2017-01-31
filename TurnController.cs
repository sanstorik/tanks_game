using UnityEngine;
using DG.Tweening;

public class TurnController : MonoBehaviour {
    static private TurnController instance;

    // player --  enemy
    [SerializeField]
    TankController[] tanks;
    [SerializeField]
    GameObject mainActionButton;
    [SerializeField]
     GameObject superWeaponButton;
    [SerializeField]
    UnityEngine.UI.Text mainText;
    [SerializeField]
    GameObject endGamePanel;

    [HideInInspector]
    public bool isPlayerTurn = true;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        tanks[0].FreezeRotation(true);
        tanks[1].FreezeRotation(true);
        mainActionButton.SetActive(true);
    }

    void EndGame(string looserTag)
    {
        var endGameText = endGamePanel.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>();
        ScreenOpacity.INSTANCE.BlockScreen();
        ScreenOpacity.INSTANCE.MakeSmoothDarker(0.5f, 5f);

        if(looserTag == "TankPlayer")
        {
            endGameText.text = "YOU LOST!";
            AudioController.INSTANCE.PlayAudio(AudioClipType.RUSSIAN_ANTHEM);
        }
        else
        {
            endGameText.text = "YOU WON!";
            AudioController.INSTANCE.PlayAudio(AudioClipType.UKRAINE_ANTHEM);
        }

        Vector3 posToSpawn = Camera.main.transform.position;
        posToSpawn.z = 0;
        posToSpawn.x -= 4f;
        PoolingSystem.Spawn(PoolManager.INSTANCE.starsPrefab, posToSpawn);
        posToSpawn.x += 8;
        PoolingSystem.Spawn(PoolManager.INSTANCE.starsPrefab, posToSpawn);
        posToSpawn.y += 7f;
        posToSpawn.x -= 5f;
        PoolingSystem.Spawn(PoolManager.INSTANCE.confettiPrefab, posToSpawn);

        endGamePanel.SetActive(true);
        endGamePanel.transform.DOMoveY(Camera.main.transform.position.y + 1f, 6f).OnComplete(() =>
        {
            endGamePanel.transform.DOShakePosition(4f, 20f, 10, 90);
        });
    }

    public void SwitchTurn()
    {
        DamageOverTurn.DealPoisonDamageToAllPoisoned();

        string looserTag;
        if (CheckIfGameEnded(out looserTag))
        {
            EndGame(looserTag);
            return;
        }

        TreasureBoxSpawner.SpawnTreasureBox();
        GiveTurnAndShowText();
    }


    void GiveTurnAndShowText()
    {
        mainText.text = isPlayerTurn ? "YOUR TURN ENDED" : "YOUR TURN";
        mainText.gameObject.SetActive(true);

        mainText.DOFade(1, 2f);
        mainText.DOFade(0, 2f).SetDelay(3f).OnComplete(() =>
        {
            mainText.gameObject.SetActive(false);
            GiveTurnToNeededPlayer();
        });
    }

    void GiveTurnToNeededPlayer()
    {
        if (isPlayerTurn)
            GiveTurnToEnemy();
        else
            GiveTurnToPlayer();

        isPlayerTurn = !isPlayerTurn;
    }

    private void GiveTurnToEnemy()
    {
        AI_Tank.INSTANCE.StartTurnAI();
    }

    private void GiveTurnToPlayer()
    {
        mainActionButton.SetActive(true);
        ScreenOpacity.INSTANCE.UnblockScreen();
    }

    private bool CheckIfGameEnded(out string looserTag)
    {
        looserTag = null;
        for(int i=0; i < tanks.Length; i++)
        {
            if (!tanks[i].gameObject.activeSelf)
            {
                looserTag = tanks[i].tag;
                return true;
            }
        }
        return false;
    }

    public void SuperWeaponButton_SetActive(bool active)
    {
        superWeaponButton.SetActive(active);
    }

    static public TurnController INSTANCE
    {
        get { return instance; }
    }
}
