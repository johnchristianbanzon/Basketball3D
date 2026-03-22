using System;
using UnityEngine;

public class TriggerBehavior : MonoBehaviour
{
    private Action<GameObject> _onHighlightObject;
    private Action<GameObject> _onDeHighlightObject;
    private Action<GameObject> _onPassObject;

    public void SetTriggerEvent(Action<GameObject> OnHighlightObject, Action<GameObject> OnDeHighlightObject)
    {
        _onHighlightObject = OnHighlightObject;
        _onDeHighlightObject = OnDeHighlightObject;
    }
    
    public void SetOnPassTrigger(Action<GameObject> OnPassObject)
    {
        _onPassObject = OnPassObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        _onHighlightObject?.Invoke(other.gameObject);
        _onPassObject?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        _onDeHighlightObject?.Invoke(other.gameObject);
    }
}
