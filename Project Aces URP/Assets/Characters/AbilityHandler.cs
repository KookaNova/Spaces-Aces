using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary> Abilities inherit from Ability Handler. The ability handler holds virtual functions which the SpaceCraft Controller calls to activate abilities.
/// The Spacecraft controller can Start Activate Coroutines, things like second activate, and can run certain parts on Update while a bool is checked.
/// Ability Handler inherits from Scriptable Object to make it easy to apply to a character's ability list without creating needless prefabs or another more
/// complex form of data object.
///</summary>
public abstract class AbilityHandler : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public float cooldownTime = 3f, startUpTime = .2f;
    public ParticleSystem startUpParticle, endParticle;
    public GameObject player;

    public bool canUse = true;

    [HideInInspector] public bool isActive = false, isUpdating = false;
    

    #region Virtual Functions
    ///<summary> Call with StartCoroutine(Activate()). Abilities override this function to perform a specific action on a button press. </summary>
    public virtual IEnumerator Activate(){
        yield break;
    }

    ///<summary> Call in Update(), FixedUpdate(), or LateUpdate(). Should be called with the check "if(isUpdating)" or "while(isUpdating)."
    /// Abilities override the function and perform a specific function on Update() when called correctly. </summary>
    public virtual void RunOnUpdate() {}

    #endregion

}