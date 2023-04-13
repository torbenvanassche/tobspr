using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

//Singleton, we only ever want one of these to exist
public class KeyMapper : Singleton<KeyMapper>
{
    //I could also inherit this from ScriptableObjects so that the data exists in the editor and not on scripts 
    //For the purpose of showcasing the rebind I don't think its necessary
    [Serializable] private class KeyBindingPair
    {
        public string id;
        public KeyBind bind;
    }
    
    public IconLookUp iconData;
    
    //Odin also adds a searchable enum field. While I don't use the serialization to JSON from it.
    //I think it makes it a lot faster to define different keybinds.
    [SerializeField] private List<KeyBindingPair> _keyBinds = new();
    
    //This is technically not needed, but prevents duplicate key bindings.
    //This way I can still edit the data in the editor, while using it as a dictionary internally
    private readonly Dictionary<string, KeyBind> _bindings = new();
    
    //we need to know if anything is being remapped, so we don't have faulty button triggers
    [HideInInspector] public bool isRemapping = false;

    //Either store from editor or in Awake, marginal performance difference if performed only once.
    [SerializeField] private List<EntryElement> uiElements = new();

    public Dictionary<string, KeyBind> Bindings
    {
        get
        {
            PopulateBindingLookup();
            return _bindings;
        }
    }

    private void Awake()
    {
        foreach (var keyBind in _keyBinds)
        {
            keyBind.bind.Load(keyBind.id);
            Bindings.TryAdd(keyBind.id, keyBind.bind);
        }
    }

    /// <summary>
    /// Saves all key binds to player prefs
    /// </summary>
    public void Save()
    {
        foreach (var keyBind in Bindings)
        {
            keyBind.Value.Save(keyBind.Key);
        }
        
        //Because I make sure to only update and not save before actually wanting to save.
        //I can reduce calls to save to just once.
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads all key binds from player prefs
    /// </summary>
    public void Load()
    {
        foreach (var keyBind in Bindings)
        {
            keyBind.Value.Load(keyBind.Key);
        }
        
        EvaluateButtonImages();
    }

    /// <summary>
    /// Resets all key bindings to the default
    /// </summary>
    public void ResetBindings()
    {
        foreach (var keyBind in Bindings)
        {
            keyBind.Value.Reset();
        }

        EvaluateButtonImages();
    }

    private void EvaluateButtonImages()
    {
        foreach (var entryElement in uiElements)
        {
            entryElement.Evaluate();
        }
    }

    public KeyBind GetBinding(string id)
    {
        if (Bindings.TryGetValue(id, out var binding))
        {
            return binding;   
        }

        return null;
    }

    public void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!isRemapping)
            {
                LogKeyPressed();
            }
        }
    }

    private void LogKeyPressed()
    {
        foreach (var keyBind in Bindings)
        {
            if (keyBind.Value.GetKey())
            {
                Debug.Log($"{keyBind.Key} is pressed.");   
            }
        } 
    }

    public void PopulateBindingLookup()
    {
        foreach (var keyBind in _keyBinds)
        {
            _bindings.TryAdd(keyBind.id, keyBind.bind);
        }
    }

    private void OnEnable()
    {
        PopulateBindingLookup();
    }
}
