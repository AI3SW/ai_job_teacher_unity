using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
public abstract class ConditionalActivators : MonoBehaviour
{
    [SerializeField]
    public UnityEvent setActive;
    [SerializeField]
    public UnityEvent setInactive;
    public abstract bool EventConditionMet(float val);

    protected abstract void Activate();
    protected abstract void Deactivate();

    private void Awake()
    {
        setActive.AddListener(Activate);
        setInactive.AddListener(Deactivate);
    }
}
