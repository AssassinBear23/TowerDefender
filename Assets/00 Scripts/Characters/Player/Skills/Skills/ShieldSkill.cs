using UnityEngine;

/// <summary>
/// Represents a shield skill that can be activated by the player.
/// </summary>
[RequireComponent(typeof(MeshCollider), typeof(MeshRenderer), typeof(Light))]
public class ShieldSkill : Skill
{
    #region Properties
    [Header("References")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Light lightComponent;

    [Header("Skill Values")]
    //[SerializeField] private float shieldDuration = 3f;
    [SerializeField] private float cooldownTime = 10f;

    [SerializeField] private bool active;
    private bool firstTimeActivation = true;

    #endregion Properties


    // =========================================== METHODS ================================================== //

    #region Methods
    #region SetupMethods

    private void Awake()
    {
        lastUseTime = Time.time;
    }

    private void Reset()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        lightComponent = GetComponent<Light>();
    }

    #endregion SetupMethods

    // ======================================= FUNCTIONAL METHODS ================================================== //

    #region Functional Methods



    /// <summary>
    /// Use the <see cref="ShieldSkill"/>.
    /// </summary>
    public override void UseSkill()
    {
        //If the shield key is not pressed, the skill is on cooldown or the skill is already active, don't do anything.
        //Debug.Log(IsOnCooldown());
        if (firstTimeActivation && InputManager.instance.shieldPressed)
        {
            firstTimeActivation = false;
            ToggleComponents();
            active = true;
        }
        if (!InputManager.instance.shieldPressed
            || IsOnCooldown() || active == true)
        {
            //Debug.Log("Skill is on cooldown: " + IsOnCooldown());
            return;
        }

        //Debug.Log("Entering UseSkill");

        ToggleComponents();
        active = true;
    }


    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// Updates the last use time of the shield skill.
    /// </summary>
    /// <param name="other">The other collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered Collision");

        // Guardblock
        if (!active) return;

        Destroy(other.gameObject);
        ToggleComponents();
        active = false;
        lastUseTime = Time.time;
    }

    private float lastUseTime;
    /// <summary>
    /// Checks if the skill is currently on cooldown.
    /// </summary>
    /// <returns>True if the skill is on cooldown, otherwise false.</returns>
    public override bool IsOnCooldown()
    {
        return Time.time <= lastUseTime + cooldownTime;
    }

    public void ToggleComponents()
    {
        lightComponent.enabled = !lightComponent.enabled;
        meshRenderer.enabled = !meshRenderer.enabled;
    }

    #endregion Functional Methods
    #endregion Methods
}
