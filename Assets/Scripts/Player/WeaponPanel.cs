using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{

    #region Variables
    // Parts of the UI
    public CanvasGroup weaponPanel;
    public Button button1;
    public Button button2;
    public Button button3;
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
    private Size prevSize;
    private Type prevType;
    private PlayerController playerController;

    // For handling animations and inputs.
    private bool panelEnabled;
    private CanvasGroup subButtonsEnabled;
    private Animator animator;
    //private Plane plane; // TODO Make this private eventually. 
    //private float screenHeight;
    //private float screenWidth;
    //private Button previouslySelected;


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

        playerController = PlayerController.GetPlayerController();
        animator = GetComponent<Animator>();

        DeactivateWeaponPanel();
        DeactivateSubButtons(subButtons1Group);
        DeactivateSubButtons(subButtons2Group);
        DeactivateSubButtons(subButtons3Group);
        panelEnabled = false;
        subButtonsEnabled = null;



    }


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
        animator.SetBool("panelEnabled", false);

    }

    public void ActivateWeaponPanel()
    {
        //weaponPanel.blocksRaycasts = true;
        weaponPanel.alpha = 1;
        panelEnabled = true;

        // Pre-select a button.
        //EventSystem.current.SetSelectedGameObject(previouslySelected.gameObject);
        animator.SetInteger("buttonSelected", -1);
        animator.SetBool("panelEnabled", true) ;
    }

    public void DeactivateSubButtons(CanvasGroup subButtons)
    {
        //subButtons.blocksRaycasts = false;
        subButtons.alpha = 0;
        animator.SetInteger("buttonSelected", -1);
        

    }

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


    //public void SetPreviouslySelected(Button button)
    //{
    //    this.previouslySelected = button; 
    //    EventSystem.current.SetSelectedGameObject(previouslySelected.gameObject);
    //}

    public void SetSize(int size)
    {
        currentSize = (Size)size;
        animator.SetInteger("buttonSelected", size);
    }

    public void SetType(int type)
    {
        currentType = (Type)type;

        Debug.Log(currentSize + "," + currentType);

        // Since this it the final input, send the info to the player.
        SetCurrentShooter(currentType, currentSize);

        // Set the previous Type and Size
        prevSize = currentSize;
        prevType = currentType;
    }


    private void SetCurrentShooter(Type type, Size size)
    {
        playerController.SetCurrentShooter(type, size);
        currentShooterName.text = playerController.GetCurrentShooterName();
    }

    private void Update()
    {
        if (!panelEnabled) return;

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distanceToPlane;
        
        //if (plane.Raycast(ray, out distanceToPlane))
        //{
        //    Debug.Log(ray.GetPoint(distanceToPlane));

        //}

        // Listen for input in order to control the event of the buttons.
        if (subButtonsEnabled != null)
        {

            //Debug.Log("subbuttoned enabled");
            if (Input.GetButton("Cancel"))
            {
                //Debug.Log("Hit cancel");
                ActivateSubButtons(subButtonsEnabled);
                DeactivateSubButtons(subButtonsEnabled); // TODO fix this
                subButtonsEnabled = null;
                return;
            }
            else if (subButtonsEnabled.Equals(subButtons1Group))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    //subButtons1Buttons[0].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons1Buttons[0].gameObject);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    //subButtons1Buttons[1].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons1Buttons[1].gameObject);
                }
            }
            else if (subButtonsEnabled.Equals(subButtons2Group))
            {
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    //subButtons2Buttons[0].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons2Buttons[0].gameObject);
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    //subButtons2Buttons[1].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons2Buttons[1].gameObject);
                }
            }
            else if (subButtonsEnabled.Equals(subButtons3Group))
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    //subButtons3Buttons[0].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons3Buttons[0].gameObject);
                }
                else if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    //subButtons3Buttons[1].onClick.Invoke();
                    EventSystem.current.SetSelectedGameObject(subButtons1Buttons[1].gameObject);
                }
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                button1.onClick.Invoke();
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                button2.onClick.Invoke();
            }
            else if (Input.GetAxisRaw("Vertical") < 0)
            {
                button3.onClick.Invoke();
            }
        }


    }




}
