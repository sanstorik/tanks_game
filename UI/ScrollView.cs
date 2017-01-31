using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour {

    public GameObject playerTank;

    public GameObject blackSquare;
    public Button scrollRight;
    public Button scrollLeft;
    public Button confirmWeapon;

    //name - damage - explosion range - force on start - description
    public Text[] textsForWeapon;

    WeaponType currentType = WeaponType.ROCKET;
    const float SPACING = 15;
    const float PREFFERED_WIDTH = 40;

    private void Start()
    {
        scrollLeft.interactable = false;
    }

    public void ScrollRight_OnClick()
    {
        currentType++;

        scrollLeft.interactable = true;
        if((int)currentType == Weapon.WEAPON_COUNT - 1)
        {
            scrollRight.interactable = false;
        }

        Vector3 squarePos = blackSquare.transform.localPosition;
        squarePos.x += (SPACING + PREFFERED_WIDTH);
        blackSquare.transform.localPosition = squarePos;

        ChangeTextForWeapons();
    }

    public void ScrollLeft_OnClick()
    {
        currentType--;

        scrollRight.interactable = true;
        if(currentType == 0)
        {
            scrollLeft.interactable = false;
        }

        Vector3 squarePos = blackSquare.transform.localPosition;
        squarePos.x -= (SPACING + PREFFERED_WIDTH);
        blackSquare.transform.localPosition = squarePos;

        ChangeTextForWeapons();

    }

    public void ConfirmWeapon_OnClick()
    {
        playerTank.GetComponent<TankController>().currentWeapon.weaponType = currentType;
        HideAndUnblockScreen();
    }

    private void HideAndUnblockScreen()
    {
        gameObject.SetActive(false);
        ScreenOpacity.INSTANCE.MakeSmoothNormal();
    }

    private void ChangeTextForWeapons()
    {
        string[] temp = new string[textsForWeapon.Length];
        switch (currentType)
        {
            case WeaponType.ROCKET:
                temp[0] = "ROCKET FLARE";
                temp[1] += "15"; // damage
                temp[2] += "3"; // explosive range
                temp[3] += "9"; // force on start
                temp[4] = "A rocket is launched from cannon and moves in one direction. Deals middle amount of damage.";
                break;
            case WeaponType.HOMMING_MISSILE:
                temp[0] = "HOMMING MISSILE";
                temp[1] += "11";
                temp[2] += "2";
                temp[3] += "10";
                temp[4] = "A rocket, that moves in one direction and after a delay stops and searches for target. After target is chosen - moves to it. (Note: you can be targeted by your own rocket)";
                break;
            case WeaponType.BANANA:
                temp[0] = "BANANA";
                temp[1] += "7";
                temp[2] += "2";
                temp[3] += "8";
                temp[4] = "Initiates a banana, which moves along a curve. After colliding it initiates 6 copies of it moving in different direction. (Note: copies deal 4 damage)";
                break;
            case WeaponType.LASOR:
                temp[0] = "LASOR";
                temp[1] += "10";
                temp[2] += "2";
                temp[3] += "10";
                temp[4] = "Instanties a lasor, which is not affected by gravity force. It bounces after colliding (9 bounces), spawning an explosion.";
                break;
            case WeaponType.POISON_ARROW:
                temp[0] = "POISON ARROW";
                temp[1] += "5 + 15";
                temp[2] += "-";
                temp[3] += "-";
                temp[4] = "Shoots an arrow not affected by gravity. Moves with constant speed. On hitting tank, deals 5 damage and poisons it for 3 turn (5 dmg each turn).";
                break;
        }

        for(int i=0; i < textsForWeapon.Length; i++)
        {
            textsForWeapon[i].text = temp[i];
        }
    }

}
