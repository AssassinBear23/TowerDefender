using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the UI elements in the game.
/// </summary>
public class UIManager : MonoBehaviour
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

    // A list of all UIElements classes
    private List<UIElement> UIElements;

    // The event system that manages UI navigation
    [HideInInspector] public EventSystem eventSystem;
    // The input manager to list for pausing
    [SerializeField] InputManager inputManager;

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
        SetupInputManager();
        SetupEventSystem();
        if (debugOn) SetupDebug();
        UpdateElements();
    }

    /// <summary>
    /// Method to get all UI elements in the scene.
    /// </summary>
    void GetUIElements()
    {
        UIElements = FindObjectsOfType<UIElement>().ToList();
        //Debug.Log("Amount of UI Elements:\t" + UIElements.Count);
        //for(int i = 0; i < UIElements.Count; i++)
        //{
        //    Debug.Log("Element " + i + ":\t" + UIElements[i].name);
        //}
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
    /// Sets the <see cref="eventSystem"/> variable to the EventSystem in the scene.
    /// </summary>
    private void SetupEventSystem()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogWarning($"There is no {nameof(eventSystem)} in the scene. Make sure to add one to the scene");
        }
    }

    /// <summary>
    /// Sets up the UIManager singleton instance in <see cref="GameManager.uIManager"/>.
    /// </summary>
    void SetupUIManager()
    {
        if (GameManager.instance.uiManager == null && GameManager.instance != null)
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
    }

    [Header("Debug")]
    [SerializeField] private bool debugOn;
    [Space(10)]
    [SerializeField] private GameDebug.DebugFlagsEnum debugFlags;
    [SerializeField] private TMPro.TMP_Text debugText;
    [SerializeField] private int debugTextSize = 24;

    /// <summary>
    /// 
    /// </summary>
    private void SetupDebug()
    {
        GameDebug.SetupDebug(debugText, debugTextSize, debugFlags);
    }

    public void ToggleDebug()
    {
        debugOn = !debugOn;
        if(debugOn) SetupDebug(); 
        else debugText.gameObject.SetActive(false);
    }
    #endregion Setup Methods

    //=========================================== FUNCTIONAL METHODS ===========================================

    #region FunctionalMethods

    /// <summary>
    /// Updates all UI elements in the <see cref="UIElements"/> list.
    /// </summary>
    public void UpdateElements()
    {
        if (debugOn) GameDebug.UpdateDebug();
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

    #endregion
    #endregion Methods
}
