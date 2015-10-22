using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Assign this component on a GameObject that should be listening to events coming from the user, such as clicking etc.
/// </summary>
public class MEventSystem : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler,
    IDropHandler,
    IScrollHandler,
    IMoveHandler,
    ISelectHandler,
    IDeselectHandler,
    IUpdateSelectedHandler,
    ISubmitHandler,
    ICancelHandler
{
    /// <summary>
    /// The list of registered handlers related to this event system
    /// </summary>
    public List<MInteractionEvent> registeredHandlers = new List<MInteractionEvent>();

    private bool _isDebugOn = false;
    private MController _controller;

    public MController Controller { get { return _controller; } set { _controller = value; } }

    private void Start()
    {
        //has to be in Start, not Awake
        if (_controller == null) _controller = transform.GetComponentInParentRecursively<MController>();
    }

    public void OnMove(AxisEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnMove : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Move, eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnPointerClick : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.PointerClick, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnPointerDown : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.PointerDown, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnPointerEnter : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.PointerEnter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnPointerExit : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.PointerExit, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnPointerUp : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.PointerUp, eventData);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnBeginDrag : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.BeginDrag, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnEndDrag : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.EndDrag, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnDrag : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Drag, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnDrop : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Drop, eventData);
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnScroll : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Scroll, eventData);
    }

    bool _UpdateSelected = false;

    public void OnSelect(BaseEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnSelect : " + eventData);

        _UpdateSelected = false;
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Select, eventData);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnDeselect : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Deselect, eventData);
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        if (!_UpdateSelected)
        {
            if (_isDebugOn) Debug.Log("OnUpdateSelected : " + eventData);
            _UpdateSelected = true;
            if (enabled && _controller == null) return;
            _controller.InteractionWithMap(this, MInteractionType.UpdateSelected, eventData);
        }
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnSubmit : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Submit, eventData);
    }

    public void OnCancel(BaseEventData eventData)
    {
        if (_isDebugOn) Debug.Log("OnCancel : " + eventData);
        if (enabled && _controller == null) return;
        _controller.InteractionWithMap(this, MInteractionType.Cancel, eventData);
    }

    public void OnInteractionEvent(MInteractionType interactionType, BaseEventData e)
    {
        foreach (MInteractionEvent handler in registeredHandlers)
        {
            if (handler.mapInteractionType == interactionType && handler.methodName != string.Empty)
            {
                handler.target.GetType().GetMethod(handler.methodName).Invoke(handler.target, new System.Object[] { e });
            }
        }
    }
}
