using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActivatorControllers : MonoBehaviour
{
    [SerializeField]
    List<ConditionalActivators> activators;
    [SerializeField]
    Scrollbar scroll;
    // Update is called once per frame
    void Update()
    {
        foreach(var obj in activators)
        {
            if(obj.EventConditionMet(scroll.value))
            {
                obj.setActive?.Invoke();
            } else
            {
                obj.setInactive?.Invoke();
            }
        }
    }
}
