using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Could be expanded by adding other interfaces for hovering, dragging, etc...
/// </summary>
public class ButtonConfig : SerializedMonoBehaviour, IPointerClickHandler
{

    //This makes sure only the button that was actually clicked gets the new button assigned and its data is correct
    //OnGUI calls are executed for each object, so I need to account for that (this is still better than looping all keycodes in update)
    private bool wasClicked;
    private bool isAlternative;
    private KeyBind target;

    //internal values for later reference, no GetComponent outside Awake/Start
    private Image img;
    private RectTransform rT;
    
    //resize the rect transform to make the icons a bit bigger (temporary value)
    private float scale = 5;
    
    //custom "Constructor" to populate data from EntryElement.cs
    public void Initialize(KeyBind binding, bool isAlternative)
    {
        target = binding;
        this.isAlternative = isAlternative;

        img = GetComponent<Image>();
        rT = GetComponent<RectTransform>();
        
        SetImage();
    }

    /// <summary>
    /// Nicely handle click event. 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        wasClicked = true;
        KeyMapper.Instance.isRemapping = true;
    }

    /// <summary>
    /// Rebind the key when the pointer event is triggered.
    /// </summary>
    private void OnGUI()
    {
        if (wasClicked)
        {
            if (Event.current.type == EventType.KeyUp)
            {
                target.Remap(Event.current.keyCode, isAlternative);
                KeyMapper.Instance.isRemapping = false;
            }   
            else if (Event.current.type == EventType.MouseUp)
            {
                //Keycodes for mouse input need to be interpreted
                KeyCode mouseCode = KeyCode.None;
                
                switch (Event.current.button)
                {
                    case 0:
                        //left mouse button
                        mouseCode = KeyCode.Mouse0;
                        break;
                    case 1:
                        //right mouse button
                        mouseCode = KeyCode.Mouse1;
                        break;
                    case 2:
                        //middle mouse button
                        mouseCode = KeyCode.Mouse2;
                        break;
                }
                
                target.Remap(mouseCode, isAlternative);
                KeyMapper.Instance.isRemapping = false;
                wasClicked = false;
            } 
            
            //Evaluate the icon that needs to be rendered
            SetImage();
        }
    }

    /// <summary>
    /// (Re)set the image after rebinding
    /// </summary>
    public void SetImage()
    {
        //local reference for lookup
        var iconData = KeyMapper.Instance.iconData;

        img.sprite = iconData.GetIcon(target.GetKeyCode(isAlternative));

        if (img.sprite)
        {
            rT.sizeDelta = new Vector2(img.sprite.rect.width, img.sprite.rect.height) * scale;   
        }
        else
        {
            Debug.LogError($"No image was found for key {target.GetKeyCode(isAlternative)}");
        }
        
        wasClicked = false;
    }
}
