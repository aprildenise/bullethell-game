using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Resources;

/// <summary>
/// Controls the UI that allows player to pick a weapon/Shooter based on Size and Type.
/// </summary>
public class WeaponPanel : MonoBehaviour
{

    #region Variables
    // Parts of the UI
    public CanvasGroup weaponPanel;
    public Button button1;
    public Button button2;
    public Button button3;
    /// <summary>
    /// Parent holding each subButton.
    /// </summary>
    public CanvasGroup subButtons1Group;
    public CanvasGroup subButtons2Group;
    public CanvasGroup subButtons3Group;
    public Button[] subButtons1Buttons;
    public Button[] subButtons2Buttons;
    public Button[] subButtons3Buttons;
    public TextMeshProUGUI currentShooterName;

    // To communicate with the player.
    private Size currentSize;
    private Type currentType;
    /// <summary>
    /// Previously selected Size from the player, saved in case the Player does not choose a Type/Size upon opening this weapon panel.
    /// </summary>
    private Size prevSize;
    private Type prevType;
    private PlayerController playerController;

    // For handling animations and inputs.
    public static bool panelEnabled;
    private CanvasGroup subButtonsEnabled;
    private Animator animator;


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


        playerController = PlayerController.GetInstance();

        animator = GetComponent<Animator>();

        DeactivateWeaponPanel();
        DeactivateSubButtons(subButtons1Group);
        DeactivateSubButtons(subButtons2Group);
        DeactivateSubButtons(subButtons3Group);
        panelEnabled = false;
        subButtonsEnabled = null;

        SetText();

    }


    /// <summary>
    /// Deactivate the entire weapon panel by hiding it through the CanvasGroup, along with the other sub buttons. 
    /// </summary>
    public void DeactivateWeaponPanel()
    {
        //weaponPanel.blocksRaycasts = false;
        weaponPanel.alpha = 0;

        // If the player closes the weapon panel, reset the current Size and Type to the prev ones.
        currentSize = prevSize;
        currentType = prevType;

        // Deactivate all the other subButtons
        DeactivateSubButtons(subButtons1Group);
        DeactivateSubButtons(subButtons2Group);
        DeactivateSubButtons(subButtons3Group);
        panelEnabled = false;
        subButtonsEnabled = null;

        // Set the animator.
        animator.SetBool("panelEnabled", false);

        Input.ResetInputAxes();

    }

    /// <summary>
    /// Activate the entire weapon by showing it through the CanvasGroup.
    /// </summary>
    public void ActivateWeaponPanel()
    {
        Input.ResetInputAxes();

        weaponPanel.alpha = 1;
        panelEnabled = true;

        animator.SetInteger("buttonSelected", -1);
        animator.SetBool("panelEnabled", true) ;
    }

    /// <summary>
    /// Deactivate a specific group of sub buttons.
    /// </summary>
    /// <param name="subButtons">subButtons to deactivate.</param>
    public void DeactivateSubButtons(CanvasGroup subButtons)
    {
        subButtons.alpha = 0;
        animator.SetInteger("buttonSelected", -1);

        // Reset the Button Animator.
        if (subButtons.Equals(subButtons1Group)) ResetSubButtonAnimator(subButtons1Buttons);
        else if (subButtons.Equals(subButtons2Group)) ResetSubButtonAnimator(subButtons2Buttons);
        else ResetSubButtonAnimator(subButtons3Buttons);

    }


    /// <summary>
    /// Activate a specific group of sub buttons. By doing so, also deactivate any other sub buttons
    /// that are currently activated.
    /// </summary>
    /// <param name="subButtons">subButtons to activate.</param>
    public void ActivateSubButtons(CanvasGroup subButtons)
    {
        // Activate the given subButtons.
        //subButtons.blocksRaycasts = true;
        subButtons.alpha = 1;
        subButtonsEnabled = subButtons;

        // Deactivate the other subButtons.
        if (subButtons.Equals(subButtons1Group))
        {
            DeactivateSubButtons(subButtons2Group);
            DeactivateSubButtons(subButtons3Group);
        }
        else if (subButtons.Equals(subButtons2Group))
        {
            DeactivateSubButtons(subButtons1Group);
            DeactivateSubButtons(subButtons3Group);
        }
        else
        {
            DeactivateSubButtons(subButtons1Group);
            DeactivateSubButtons(subButtons2Group);
        }
    }


    /// <summary>
    /// Save the Size that the player picked from the UI.
    /// Called by a button's onClick().
    /// </summary>
    /// <param name="size">Size chosen by the player.</param>
    public void SetSize(int size)
    {
        currentSize = (Size)size;
        animator.SetInteger("buttonSelected", size);
    }

    /// <summary>
    /// Save the Type that the player picked from the UI. Since this is the last choice from the player, 
    /// save what the player chose and set the player's Shooter.
    /// Called by a button's onClick().
    /// </summary>
    /// <param name="type">Type chosen by the player.</param>
    public void SetType(int type)
    {
        currentType = (Type)type;

        // Since this it the final input, send the info to the player.
        SetCurrentShooter(currentType, currentSize);

        // Set the previous Type and Size
        prevSize = currentSize;
        prevType = currentType;
    }

    /// <summary>
    /// Based on Type and Size selected by the player, change the player's Shooter by sending the
    /// given Type and Size to the PlayerController.
    /// </summary>
    /// <param name="type">Type of the Shooter.</param>
    /// <param name="size">Size of the Shooter.</param>
    private void SetCurrentShooter(Type type, Size size)
    {
        playerController.SetCurrentShooter(type, size);
        SetText();
    }

    private void SetText()
    {
        currentShooterName.text = playerController.GetCurrentWeaponName();
    }

    private void ResetSubButtonAnimator(Button[] subButtons)
    {
        foreach (Button button in subButtons)
        {
            ResetSubButtonAnimator(button);
        }
    }

    private void ResetSubButtonAnimator(Button button)
    {
        Animator anim = button.gameObject.GetComponent<Animator>();
        anim.SetTrigger("Normal");
    }

    private void SetInteractible(Button[] subButtons, int interactable, int notInteractible)
    {
        subButtons[interactable].interactable = true;
        subButtons[notInteractible].interactable = false;
    }

    /// <summary>
    /// Control inputs from the player, if the panel is enabled.
    /// </summary>
    private void Update()
    {
        if (!panelEnabled) return;

        // Get input.
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        bool left = hInput < 0;
        bool right = hInput > 0;
        bool up = vInput > 0;
        bool down = vInput < 0;


        // Listen for input in order to control the event of the buttons.
        if (subButtonsEnabled != null)
        {
            // If we are activating sub buttons, then button inputs depend on the Inputs from the player.
            // A button will be highlighted based on the Inputs, NOT immediately selected.
            if (subButtonsEnabled.Equals(subButtons1Group))
            {
                if (up)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons1Buttons[0].gameObject);
                    SetInteractible(subButtons1Buttons, 0, 1);
                    Input.ResetInputAxes();
                }
                else if (left)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons1Buttons[1].gameObject);
                    SetInteractible(subButtons1Buttons, 1, 0);
                    Input.ResetInputAxes();
                }
                // When the player chooses the "opposite button", "Cancel" the selected sub button and hide all other sub buttons.
                else if (right || down)
                {
                    DeactivateSubButtons(subButtonsEnabled);
                    subButtonsEnabled = null;
                    Input.ResetInputAxes();
                }
            }
            else if (subButtonsEnabled.Equals(subButtons2Group))
            {
                if (up)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons2Buttons[0].gameObject);
                    SetInteractible(subButtons2Buttons, 0, 1);
                    Input.ResetInputAxes();
                }
                else if (right)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons2Buttons[1].gameObject);
                    SetInteractible(subButtons2Buttons, 1, 0);
                    Input.ResetInputAxes();
                }
                // When the player chooses the "opposite button", "Cancel" the selected sub button and hide all other sub buttons.
                else if (left || down)
                {
                    DeactivateSubButtons(subButtonsEnabled);
                    subButtonsEnabled = null;
                    Input.ResetInputAxes();
                }
            }
            else if (subButtonsEnabled.Equals(subButtons3Group))
            {
                if (left)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons3Buttons[0].gameObject);
                    SetInteractible(subButtons3Buttons, 0, 1);
                    Input.ResetInputAxes();
                }
                else if (right)
                {
                    EventSystem.current.SetSelectedGameObject(subButtons3Buttons[1].gameObject);
                    SetInteractible(subButtons3Buttons, 1, 0);
                    Input.ResetInputAxes();
                }
                else if (up)
                {
                    DeactivateSubButtons(subButtonsEnabled);
                    subButtonsEnabled = null;
                    Input.ResetInputAxes();
                }
            }
        }
        else
        {
            if (left)
            {
                button1.onClick.Invoke();
                Input.ResetInputAxes();
            }
            else if (right)
            {
                button2.onClick.Invoke();
                Input.ResetInputAxes();
            }
            else if (down)
            {
                button3.onClick.Invoke();
                Input.ResetInputAxes();
            }
        }


    }




}
