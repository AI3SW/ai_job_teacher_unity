using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollbarMaxEdgeActivator : ConditionalActivators
{
    [SerializeField]
    float ActivationMax = 1;
    public override bool EventConditionMet(float val)
    {
        return (ActivationMax >= val);
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
