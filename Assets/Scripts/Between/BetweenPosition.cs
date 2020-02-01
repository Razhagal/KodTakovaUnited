using UnityEngine;

/// <summary>
/// Transition position.
/// </summary>
[AddComponentMenu("BetweenUI/Between Position")]
public class BetweenPosition : BetweenBase
{
    public Vector3 From;
    public Vector3 To;

    public bool LockX = false;
    public bool LockY = false;
    public bool LockZ = false;

    private RectTransform trans;

    private RectTransform CachedTransform
    {
        get
        {
            if (this.trans == null)
            {
                this.trans = this.GetComponent<RectTransform>();
            }

            return this.trans;
        }
    }

    /// <summary>
    /// Transition's current value.
    /// </summary>
    private Vector3 Value
    {
        get
        {
            return this.CachedTransform.anchoredPosition;
        }

        set
        {
            this.CachedTransform.anchoredPosition = value;
        }
    }

    /// <summary>
    /// Transition the value.
    /// </summary>
    protected override void OnUpdate(float timeFactor)
    {
        float x = this.Value.x;
        float y = this.Value.y;
        float z = this.Value.z;

        if (!this.LockX)
        {
            x = this.From.x * (1f - timeFactor) + this.To.x * timeFactor;
        }

        if (!this.LockY)
        {
            y = this.From.y * (1f - timeFactor) + this.To.y * timeFactor;
        }

        if (!this.LockZ)
        {
            z = this.From.z * (1f - timeFactor) + this.To.z * timeFactor;
        }

        this.Value = new Vector3(x, y, z);
    }

    public void Reset()
    {
        ToForCurrent();
        FromForCurrent();
    }

    [ContextMenu("Current position for From")]
    public void FromForCurrent()
    {
        this.From = this.CachedTransform.anchoredPosition;
    }

    [ContextMenu("Current position for To")]
    public void ToForCurrent()
    {
        this.To = this.CachedTransform.anchoredPosition;
    }
}
