﻿using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BetweenUI/Between Alpha")]
public class BetweenAlpha : BetweenBase
{
    [Range(0f, 1f)]
    public float From = 0f;
    [Range(0f, 1f)]
    public float To = 1f;

    public bool childIncluding = true;

    private bool isParent;
    private bool cached;
    private CanvasGroup canvasGroup;
    private Graphic graphic;
    private Color graphicAlpha = Color.white;

    private void Cache()
    {
        this.cached = true;
        this.canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        if (this.childIncluding || this.canvasGroup != null)
        {
            this.isParent = true;

            if (this.canvasGroup == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning(string.Format(MessageNotCreatedComponent, this.name, "CanvasGroup"));
#endif
                this.canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }

            this.canvasGroup.alpha = this.From;
        }
        else
        {
            this.graphic = this.GetComponent<Graphic>();
            this.graphicAlpha = this.graphic.color;
        }
    }

    /// <summary>
    /// Transition's current value.
    /// </summary>
    private float Value
    {
        set
        {
            if (!this.cached)
            {
                Cache();
            }

            if (this.isParent && this.childIncluding)
            {
                this.canvasGroup.alpha = value;
            }
            else
            {
                this.graphicAlpha.a = value;
                this.graphic.color = this.graphicAlpha;
            }
        }
    }

    /// <summary>
    /// Transit the value.
    /// </summary>
    protected override void OnUpdate(float timeFactor)
    {
        this.Value = Mathf.Lerp(this.From, this.To, timeFactor);
    }

    public void Reset()
    {
        ToForCurrent();
        FromForCurrent();
    }

    [ContextMenu("Current alpha for To")]
    public void ToForCurrent()
    {
        Cache();
        if (this.childIncluding && this.isParent)
        {
            this.To = this.canvasGroup.alpha;
        }
        else
        {
            this.To = this.GetComponent<Graphic>().color.a;
        }
    }

    [ContextMenu("Current alpha for From")]
    public void FromForCurrent()
    {
        Cache();
        if (this.childIncluding && this.isParent)
        {
            this.To = this.canvasGroup.alpha;
        }
        else
        {
            this.To = this.GetComponent<Graphic>().color.a;
        }
    }
}