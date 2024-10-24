using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// Manages the UI elements in the game.
/// </summary>
public class UIManager : MonoBehaviour, IMoveHandler, IHandler
{
    #region Variables

    /// <summary>
    /// The pages (panels) managed by this UI Manager.
    /// </summary>
    [Header("Page Management")]
    [Tooltip("The pages (panels) managed by this UI Manager")]
    public List<UIPage> pages;

    /// <summary>
    /// The index of the currently active page/panel.
    /// </summary>
    [Tooltip("The index of the currently active page/panel")]
    public int activePageIndex = 0;

    /// <summary>
    /// The default page that is active when UI Manager starts up.
    /// </summary>
    [Tooltip("The default page that is active when UI Manager starts up")]
    public int defaultPageIndex = 0;

    /// <summary>
    /// The pause menu page's index.
    /// </summary>
    [Header("Pause Settings")]
    [Tooltip("The pause menu page's index\n defaults to 1")]
    public int pausePageIndex = 1;

    

    /// <summary>
    /// Whether or not the game can be paused.
    /// </summary>
    [Tooltip("Whether or not the game can be paused")]
    public bool allowPause = true;

    // Whether or not the game is paused
    private bool isPaused = false;

    // A list of all UIElement classes
    private List<UIElement> UIElements;

    // The NavigationAction that we listen to to reset the selected UI to default if none is selected
    //[SerializeField] private InputAction onNavigationAction;
    // The input manager to list for pausing
    [HideInInspector] public InputManager inputManager;

    #endregion Variables

    // ============================================= METHODS ===================================================

    #region Methods

    // =========================================== SETUP METHODS ===============================================

    #region SetupMethods

    /// <summary>
    /// Default Unity Method called when the script is enabled.
    /// </summary>
    private void OnEnable()
    {
        SetupUIManager();
    }

    /// <summary>
    /// Default Unity Method called when the script is first loaded.
    /// </summary>
    private void Start()
    {
        Debug.Log("UI Manager Started");
        SetupDebug();
        SetupInputManager();
        UpdateElements();
    }

    /// <summary>
    /// Method to get all UI elements in the scene.
    /// </summary>
    void GetUIElements()
    {
        UIElements = FindObjectsOfType<UIElement>().ToList();
    }

    /// <summary>
    /// Sets up the inputManager singleton reference.
    /// </summary>
    private void SetupInputManager()
    {
        if (inputManager == null)
        {
            inputManager = InputManager.instance;
        }
        if (inputManager == null)
        {
            Debug.LogWarning($"There is no {nameof(inputManager)} in the scene. Make sure to add one to the scene otherwise you cannot pause the game");
        }
    }

    /// <summary>
    /// Sets up the UIManager singleton instance in <see cref="GameManager.uIManager"/>.
    /// </summary>
    void SetupUIManager()
    {
        if (GameManager.instance != null && GameManager.instance.uiManager == null)
        {
            try
            {
                GameManager.instance.uiManager = this;
            }
            catch (System.Exception)
            {
                // Exception caught but not displayed
            }
        }

        //onNavigationAction.performed += _ => ResetSelectedUIToDefault();
    }
    #endregion Setup Methods

    //=========================================== FUNCTIONAL METHODS ===========================================

    #region Functional Methods

    /// <summary>
    /// Updates all UI elements in the <see cref="UIElements"/> list.
    /// </summary>
    public void UpdateElements()
    {
        GameDebug.UpdateDebug();
        GetUIElements();
        foreach (UIElement element in UIElements)
        {
            element.UpdateElement();
        }
    }

    /// <summary>
    /// Default Unity Method that is called every frame.
    /// </summary>
    private void Update()
    {
        CheckPauseInput();
        //ResetSelectedUIToDefault();
    }

    /// <summary>
    /// Checks for pause input.
    /// </summary>
    private void CheckPauseInput()
    {
        if (inputManager == null)
        {
            return;
        }
        if (inputManager.pausePressed)
        {
            TogglePause();
        }
    }

    #endregion Functional Methods
    #region UI Methods

    /// <summary>
    /// Turns all stored pages on or off depending on the passed parameter.
    /// </summary>
    /// <param name="activeState">The state to set all pages to, true to active them all, false to deactivate them all</param>
    private void SetActiveAllPages(bool activeState)
    {
        if (pages == null)
        {
            return;
        }
        foreach (UIPage page in pages)
        {
            if (page != null)
            {
                page.gameObject.SetActive(activeState);
            }
        }
    }

    /// <summary>
    /// Toggles the pause state of the game.
    /// </summary>
    public void TogglePause()
    {
        if (!allowPause)
        {
            return;
        }
        if (isPaused)
        {
            SetActiveAllPages(false);
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            GoToPage(pausePageIndex);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void ToggleAllowPause()
    {
        allowPause = !allowPause;
    }

    /// <summary>
    /// Goes to a page by that page's index.
    /// </summary>
    /// <param name="pageIndex">The index in the page list to go to</param>
    public void GoToPage(int pageIndex)
    {
        if (pageIndex < pages.Count && pages[pageIndex] != null)
        {
            SetActiveAllPages(false);
            pages[pageIndex].gameObject.SetActive(true);
            pages[pageIndex].SetSelectedUIToDefault();
        }
    }


    public GameObject LastSelected { private get; set; }

    public void OnMove(AxisEventData eventData)
    {
        Debug.Log("OnMove Called");
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(LastSelected);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Debug.Log($"{this.gameObject.name} got deselected");
        LastSelected = EventSystem.current.currentSelectedGameObject;
    }

    //========================================== DEBUGGING ========================================================

    #region Debugging

    [Header("Debugging")]
        [Tooltip("The text that will be used to display debugging information")]
        [SerializeField] private TMP_Text debuggingText;
        [Tooltip("Size of the text")]
        [SerializeField][Range(1f, 40f)] private int textSize = 24;

        [Space(10)]
        [Tooltip("The debug flags to display in the inspector")]
        [SerializeField] private GameDebug.DebugFlagsEnum debugFlags;

        /// <summary>
        /// Set up the debugging functionality.
        /// </summary>
        void SetupDebug()
        {
            //Debug.Log("Setting up Debugging" +
            //    "\nDebugger Text: " + debuggingText +
            //    "\nText size: " + textSize +
            //    "\nDebug Flags: " + debugFlags);
            GameDebug.SetupDebug(debuggingText, textSize, debugFlags);
        }

        #endregion Debugging

        #endregion UI Methods
        #endregion Methods
    }
