using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Mappings for icons and which keycode they belong to
/// </summary>
[Serializable] public class IconWithKeyCode
{
    public KeyCode code;
    public Sprite icon;

    public IconWithKeyCode(KeyCode code, Sprite icon)
    {
        this.code = code;
        this.icon = icon;
    }
}


[CreateAssetMenu(menuName = "tobspr/IconTable")]
public class IconLookUp : SerializedScriptableObject
{
    [TableList] public List<IconWithKeyCode> iconTable = new();

    /// <summary>
    /// Find an icon in the database
    /// </summary>
    /// <param name="kc">the key code that needs to be looked for in the database.</param>
    /// <returns>A sprite for the key, or null if none was found.</returns>
    public Sprite GetIcon(KeyCode kc)
    {
        //LINQ is OK when not using repeatedly (as it creates copies constantly)
        return iconTable.FirstOrDefault(x => x.code == kc)?.icon;
    }

    private void OnValidate()
    {
        foreach (var kc in Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Where(x => !x.ToString().Contains("Joystick")))
        {
            if (iconTable.All(x => x.code != kc))
            {
                iconTable.Add(new IconWithKeyCode(kc, null));
            }
        }
        
        foreach (var iconWithKeyCode in iconTable.Where(x => !Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().Contains(x.code)))
        {
            iconTable.Add(iconWithKeyCode);
        }
    }
}
