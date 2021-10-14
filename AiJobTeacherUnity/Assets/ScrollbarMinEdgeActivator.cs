using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollbarMinEdgeActivator : ConditionalActivators
{
    [SerializeField]
    float ActivationMin = 0;
    public override bool EventConditionMet(float val)
    {
        return (val >= ActivationMin);
    }
    protected override void Activate()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
    protected override void Deactivate()
    {
        if (gameObject.activeSelf)
         gameObject.SetActive(false);
    }
}
