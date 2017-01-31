using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonListeners : MonoBehaviour {

    public Button mainActionButton;
    public Button superWeaponButton;

    public Slider cannonRotationSlider;
    public Slider projectileForceOnFireSlider;

    public GameObject actionButtons;
    public GameObject changeWeaponMenu;

    public GameObject firePanel;
    public GameObject loadLevelPanel;

    public GameObject player;

    Transform cannon;
    TankController playerController;

    bool actionButtonsAreActive = false;

    private void Start()
    {
        cannon = player.transform.GetChild(0);
        playerController = player.GetComponent<TankController>();
    }

    public void MainAction_OnClick()
    {
        if (actionButtonsAreActive)
            actionButtons.SetActive(false);
        else actionButtons.SetActive(true);

        actionButtonsAreActive = !actionButtonsAreActive;
    }

    public void ShootButton_OnClick()
    {
        firePanel.SetActive(true);
        MainAction_OnClick();
        playerController.FreezeRotation(true);

        mainActionButton.gameObject.SetActive(false);
        ScreenOpacity.INSTANCE.BlockScreen();
    }

    public void FirePanelOk_OnClick()
    {
        firePanel.SetActive(false);

        playerController.FreezeRotation(false);
        playerController.Shoot();
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);
    }

    public void MoveLeft_OnClick()
    {
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);
        playerController.MoveLeft();
        StartCoroutine(_DelayBeforeUsingButtonsAgain());
    }

    public void MoveRight_OnClick()
    {
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);
        playerController.MoveRight();
        StartCoroutine(_DelayBeforeUsingButtonsAgain());
    }

    public void ChangeWeapon_OnClick()
    {
        ScreenOpacity.INSTANCE.MakeSmoothDarker(0.5f);
        changeWeaponMenu.SetActive(true);
    }

    public void SuperWeaponOnFire_OnClick()
    {
        AudioController.INSTANCE.PlayAudio(AudioClipType.COUNTDOWN_321);
        playerController.UseSuperWeapon();

        mainActionButton.gameObject.SetActive(false);
        superWeaponButton.gameObject.SetActive(false);

        ScreenOpacity.INSTANCE.BlockScreen();
    }

    public void PauseButton_OnClick()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }

    public void MuteButton_OnClick()
    {
        AudioListener.volume = AudioListener.volume > 0.5 ? 0 : 1;
    }


    public void ChangeRotationOfCannon_OnValueChanged()
    {
        float onePercentOfRotation = 140f / 100f;
        float rotation = cannonRotationSlider.value * onePercentOfRotation * 100;

        Vector3 rot = cannon.rotation.eulerAngles;
        rot.z = rotation;

        playerController.RotateCannon(rot);
    }

    public void ChangeForceOfProjectile_OnValueChanged()
    {
        playerController.PowerOfFireProjectile = projectileForceOnFireSlider.value;
    }

    public void ReturnToMainMenuButton_OnClick()
    {
        PoolingSystem.ClearPools();
        loadLevelPanel.SetActive(true);
        SceneManager.LoadSceneAsync(0);
    }

    public void RestartLevelButton_OnClick()
    {
        PoolingSystem.ClearPools();
        loadLevelPanel.SetActive(true);
        SceneManager.LoadSceneAsync(1);
    }

    IEnumerator _DelayBeforeUsingButtonsAgain()
    {
        MainAction_OnClick();
        mainActionButton.interactable = false;
        superWeaponButton.interactable = false;
        yield return new WaitForSeconds(TankController.DELAY_BEFORE_ACTION + TankController.MOVING_TIME);
        mainActionButton.interactable = true;
        superWeaponButton.interactable = true;
    }
}
