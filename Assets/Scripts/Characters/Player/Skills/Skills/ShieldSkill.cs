using UnityEngine;

/// <summary>
/// Represents a shield skill that can be activated by the player.
/// </summary>
[RequireComponent(typeof(MeshCollider), typeof(MeshRenderer))]
public class ShieldSkill : Skill
{
    #region Properties
    [Header("References")]
    [SerializeField] private Renderer shieldRenderer;

    [Header("Skill Values")]
    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private float cooldown = 10f;

    private bool active;

    #endregion Properties


    // =========================================== METHODS ================================================== //

    #region Methods
    #region SetupMethods

    private void Reset()
    {
        shieldRenderer = GetComponent<MeshRenderer>();
    }

    #endregion SetupMethods

    // ======================================= FUNCTIONAL METHODS ================================================== //

    #region Functional Methods

    /// <summary>
    /// Use the <see cref="ShieldSkill"/>.
    /// </summary>
    public override void UseSkill()
    {
        // If the shield key is not pressed, the skill is on cooldown or the skill is already active, don't do anything.
        if (!InputManager.instance.shieldPressed
            || IsOnCooldown())
            return;

        shieldRenderer.enabled = true;
    }


    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// Updates the last use time of the shield skill.
    /// </summary>
    /// <param name="other">The other collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Guardblock
        if (!active) return;

        shieldRenderer.enabled = false;
        active = false;
        lastUseTime = Time.time;
    }

    private float lastUseTime;
    /// <summary>
    /// Checks if the skill is currently on cooldown.
    /// </summary>
    /// <returns>True if the skill is on cooldown, otherwise false.</returns>
    private bool IsOnCooldown()
    {
        return lastUseTime + cooldown < Time.time;
    }

    #endregion Functional Methods

    // ======================================= UTIL METHODS ================================================== //

    #region Util

    /// <summary>
    /// Gets the skill component.
    /// </summary>
    /// <returns>The skill component.</returns>
    public override Component GetSkill()
    {
        return this;
    }

    #endregion Util
    #endregion Methods
}
