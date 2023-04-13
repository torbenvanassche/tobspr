using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable] public class KeyBind
{
    //These can also be made read-only later if we don't want people to make changes in the editor
    
    //An additional abstraction layer could also be a solution, mapping the default and current keycode.
    //Seems a bit overkill for this, but could be changed depending on the scale of the project.
    
    [SerializeField] private KeyCode _mainDefaultKeyCode = 0;
    [SerializeField] private KeyCode _alternativeDefaultKeyCode = 0;
    
    [SerializeField] private KeyCode _mainKeyCode = 0;
    [SerializeField] private KeyCode _alternativeKeyCode = 0;

    //only try to save if the value has changed
    private bool _needsUpdate = false;

    /// <summary>
    /// Saves the values for this KeyBind. Potential optimization with string concatenation and splitting.
    /// </summary>
    /// <param name="keyId">the id to use to store in PlayerPrefs</param>
    public void Save(string keyId)
    {
        if (_needsUpdate)
        {
            //Could be combined to reduce lookup time
            PlayerPrefs.SetInt($"{keyId}/key", (int)_mainKeyCode);
            PlayerPrefs.SetInt($"{keyId}/alternativeKey", (int)_alternativeKeyCode);

            _needsUpdate = false;   
        }
    }

    public KeyCode GetKeyCode(bool isAlternative)
    {
        return isAlternative ? _alternativeKeyCode : _mainKeyCode;
    }

    /// <summary>
    /// Saves the values for this KeyBind. Potential optimization with string concatenation and splitting.
    /// </summary>
    /// <param name="keyId">the id to use to retrieve the data from PlayerPrefs</param>
    public void Load(string keyId)
    {
        //If for any reason there is no value defined in player prefs, set them to default
        _mainKeyCode = (KeyCode)PlayerPrefs.GetInt($"{keyId}/key", (int)_mainDefaultKeyCode);
        _alternativeKeyCode = (KeyCode)PlayerPrefs.GetInt($"{keyId}/alternativeKey", (int)_alternativeDefaultKeyCode);   
    }

    /// <summary>
    /// Checking both input methods (if they are defined), If either is not defined the value is KeyCode.None
    /// </summary>
    /// <returns></returns>
    public bool GetKey()
    {
        return Input.GetKey(_mainKeyCode) || Input.GetKey(_alternativeKeyCode);
    }

    /// <summary>
    /// Resets the key to its default settings
    /// </summary>
    public void Reset()
    {
        _mainKeyCode = _mainDefaultKeyCode;
        _alternativeKeyCode = _alternativeDefaultKeyCode;
    }

    /// <summary>
    /// Remaps the key and queue's it up to be saved.
    /// </summary>
    /// <param name="newKey">The new binding for this key</param>
    /// <param name="alternativeKey">is this an alternative binding?</param>
    public void Remap(KeyCode newKey, bool alternativeKey)
    {
        if (alternativeKey)
        {
            _alternativeKeyCode = newKey;
        }
        else
        {
            _mainKeyCode = newKey;
        }

        _needsUpdate = true;
    }
}
