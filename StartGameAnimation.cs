using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour {

    public TankController[] tanks;
    public Text mainText;

    WeaponType[] typesOfTank;

	void Start () {
        typesOfTank = new WeaponType[tanks.Length];
        StartCoroutine(ShootAndStartGame());
	}
	
    IEnumerator ShootAndStartGame()
    {
        yield return new WaitForSeconds(2);
        AudioController.INSTANCE.PlayAudio(AudioClipType.BATTLE_HORN);

        for (int i=0; i < tanks.Length; i++)
        {
            typesOfTank[i] = tanks[i].currentWeapon.weaponType;
            tanks[i].currentWeapon.weaponType = WeaponType.FIREWORK;
            tanks[i].PowerOfFireProjectile = 0.6f;
            tanks[i].Shoot();
        }
        CameraController.INSTANCE.ResetTarget();

        yield return new WaitForSeconds(7f);

        mainText.gameObject.SetActive(true);
        mainText.text = "LET THE BATTLE BEGINS!";
        mainText.DOFade(1, 2f);
        mainText.DOFade(0, 2f).SetDelay(2f).OnComplete(() =>
        {
            mainText.gameObject.SetActive(false);
            TurnController.INSTANCE.StartGame();
        });

        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].PowerOfFireProjectile = 0.3f;
            tanks[i].currentWeapon.weaponType = typesOfTank[i];
        }

        tanks[1].canon.DORotateQuaternion(Quaternion.Euler(0, 0, 160), 3f);
        tanks[0].canon.DORotateQuaternion(Quaternion.Euler(0, 0, 20), 3f);

    }
}
