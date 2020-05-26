using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanel : MonoBehaviour
{

    #region Variables
    // Parts of the UI
    public CanvasGroup weaponPanel;
    public CanvasGroup subButtons1;
    public CanvasGroup subButtons2;
    public CanvasGroup subButtons3;

    // To communicate with the player.
    private Size currentSize;
    private Type currentType;
    private Size prevSize;
    private Type prevType;
    private PlayerController playerController;

    // Test
    public TextMeshProUGUI placeHolderText;

    #endregion

    #region Singleton
    private static WeaponPanel instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public static WeaponPanel GetWeaponPanel()
    {
        return instance;
    }
    #endregion

    /// <summary>
    /// Set up the UI by hiding it all in the beginning and setting other variables.
    /// </summary>
    private void Start()
    {
        DeactivateWeaponPanel();
        DeactivateSubButtons(subButtons1);
        DeactivateSubButtons(subButtons2);
        DeactivateSubButtons(subButtons3);

        playerController = PlayerController.GetPlayerController();

    }


    public void DeactivateWeaponPanel()
    {
        weaponPanel.blocksRaycasts = false;
        weaponPanel.alpha = 0;

        // If the player closes the weapon panel, reset the current Size and Type to the prev ones.
        currentSize = prevSize;
        currentType = prevType;

        // Deactivate all the other subButtons
        DeactivateSubButtons(subButtons1);
        DeactivateSubButtons(subButtons2);
        DeactivateSubButtons(subButtons3);
    }

    public void ActivateWeaponPanel()
    {
        weaponPanel.blocksRaycasts = true;
        weaponPanel.alpha = 1;
    }

    public void DeactivateSubButtons(CanvasGroup subButtons)
    {
        subButtons.blocksRaycasts = false;
        subButtons.alpha = 0;

    }

    public void ActivateSubButtons(CanvasGroup subButtons)
    {
        // Activate the given subButtons.
        subButtons.blocksRaycasts = true;
        subButtons.alpha = 1;

        // Deactivate the other subButtons.
        if (subButtons.Equals(subButtons1))
        {
            DeactivateSubButtons(subButtons2);
            DeactivateSubButtons(subButtons3);
        }
        else if (subButtons.Equals(subButtons2))
        {
            DeactivateSubButtons(subButtons1);
            DeactivateSubButtons(subButtons3);
        }
        else
        {
            DeactivateSubButtons(subButtons1);
            DeactivateSubButtons(subButtons2);
        }
    }


    public void SetSize(int size)
    {
        currentSize = (Size)size;
    }

    public void SetType(int type)
    {
        currentType = (Type)type;

        // Since this it the final input, send the info to the player.
        SetCurrentShooter(currentType, currentSize);

        // Set the previous Type and Size
        prevSize = currentSize;
        prevType = currentType;
    }


    private void SetCurrentShooter(Type type, Size size)
    {
        playerController.SetCurrentShooter(type, size);

        //test
        placeHolderText.text = type + "," + size;
    }





}
