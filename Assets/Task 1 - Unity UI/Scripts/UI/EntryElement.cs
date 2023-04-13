using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class EntryElement : SerializedMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private ButtonConfig _mainButton;
    [SerializeField] private ButtonConfig _alternateButton;

    //using Odin to more easily create a drawer for this, This allows the use of a dropdown instead of making mistakes typing the string manually
    [SerializeField, ValueDropdown("@KeyMapper.Instance.Bindings.Keys")] private string binding;

    /// <summary>
    /// Make sure the bindigs are in the dictionary when required for lookup
    /// </summary>
    private void OnValidate()
    {
        KeyMapper.Instance.PopulateBindingLookup();
    }

    //initialize underlying components, and set the text for the string
    private void Awake()
    {
        _label.text = binding;

        var keyBind = KeyMapper.Instance.GetBinding(binding);
        _mainButton.Initialize(keyBind, false);
        _alternateButton.Initialize(keyBind, true);
    }

    public void Evaluate()
    {
        _mainButton.SetImage();
        _alternateButton.SetImage();
    }
}
