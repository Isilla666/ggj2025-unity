using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BaseAnimation: MonoBehaviour
{
    [Button]
    public abstract void Animate();
    
    [Button]
    public abstract void StopAnimation();
}