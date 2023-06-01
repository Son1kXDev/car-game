using System.Reflection;
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class StatusIconAttribute : PropertyAttribute
{
    public string? str { get; private set; } = null;
    public int? num { get; private set; } = null;
    public float? flt { get; private set; } = null;
    public float offset { get; private set; } = 12;
    public StatusIconAttribute(float offset = 12) { this.offset = offset; }
    public StatusIconAttribute(string value, float offset = 12) { str = value; this.offset = offset; }
    public StatusIconAttribute(int value, float offset = 12) { num = value; this.offset = offset; }
    public StatusIconAttribute(float value, float offset = 12) { flt = value; this.offset = offset; }
}

