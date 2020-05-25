using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPanel : MonoBehaviour
{

    // Variables
    public CanvasGroup weaponPanel;
    public CanvasGroup subButtons1;
    public CanvasGroup subButtons2;
    public CanvasGroup subButtons3;


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
    /// Set up the UI by hiding it all in th beginning.
    /// </summary>
    private void Start()
    {
        DeactivateWeaponPanel();
        DeactivateSubButtons(subButtons1);
        DeactivateSubButtons(subButtons2);
        DeactivateSubButtons(subButtons3);
    }


    public void DeactivateWeaponPanel()
    {
        weaponPanel.blocksRaycasts = true;
        weaponPanel.alpha = 0;
    }

    public void ActivateWeaponPanel()
    {
        weaponPanel.blocksRaycasts = false;
        weaponPanel.alpha = 1;
    }

    public void DeactivateSubButtons(CanvasGroup subButtons)
    {
        subButtons.blocksRaycasts = true;
        subButtons.alpha = 0;

    }

    public void ActivateSubButtons(CanvasGroup subButtons)
    {
        // Activate the given subButtons.
        subButtons.blocksRaycasts = false;
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


}
