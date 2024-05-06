using System;

[Serializable]
public enum InputAction
{
    Move,
    Strafe,
    Jump,
    Attack,
}

[Serializable]
public enum MouseInput
{
    Left,
    Right,
    Middle
}

[Serializable]
public enum InputMethod
{
    Hold,
    OnPress,
    OnRelease
}

[Serializable]
public struct Range
{
    public float Min;
    public float Max;

    public void Clamp(ref float targetValue)
    {
        targetValue = Math.Clamp(targetValue, Min, Max);
    }
}