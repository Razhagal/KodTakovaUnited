using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 1f;
    public float LifeTime = 2f;

    protected bool isAlive = false;
    
    public virtual void FireAtTarget(Transform target) { }
}
