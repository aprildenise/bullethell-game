using UnityEngine;


/// <summary>
/// Used by any gameobjects that are meant to "activate" and become enabled due to 
/// a certain condition defined by ActivateOn and its children classes.
/// </summary>
public interface IActivator
{

    /// <summary>
    /// Called by ActivateOn class. Will perform whatever move specified by the 
    /// class that implements this.
    /// </summary>
    void Activate();
}
