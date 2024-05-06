using CustomUtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = CustomUtilityScripts.Logger;

[CreateAssetMenu(menuName = "CustomAssets/InputMap", fileName = "NewInputMap")]
public class InputMap : ScriptableObject
{
    [SerializeField]
    private List<ActionKeys> _actionKeys;

    private Dictionary<InputAction, MappedKeys> _inputActionMap;
    private Dictionary<InputAction, InputStatus> _inputKeyStatus = new();

    public Dictionary<InputAction, MappedKeys> InputActionMap => _inputActionMap;

    public void Sanitize()
    {
        _inputActionMap = new();

        foreach (ActionKeys actionKeys in _actionKeys)
        {
            if (_inputActionMap.ContainsKey(actionKeys.Action))
            {
                _inputActionMap[actionKeys.Action] = actionKeys.MappedKey;
            }
            else
            {
                _inputActionMap.Add(actionKeys.Action, actionKeys.MappedKey);
            }
        }
    }

    public void SaveKeyStatus(InputAction action, bool status, bool reverseKeyMotion = false)
    {
        if (_inputKeyStatus == null)
        {
            _inputKeyStatus = new();
        }

        InputStatus inputStatus = new();
        inputStatus.Status = status;
        inputStatus.ReverseMotion = reverseKeyMotion;

        if (!_inputKeyStatus.ContainsKey(action))
        {
            _inputKeyStatus.Add(action, inputStatus);
            return;
        }

        if (_inputKeyStatus.ContainsKey(action))
        {
            _inputKeyStatus[action] = inputStatus;
        }
    }

    public InputStatus GetInputStatus(InputAction action)
    {
        if (_inputKeyStatus == null
            || !_inputKeyStatus.ContainsKey(action))
        {
            return new();
        }

        return _inputKeyStatus[action];
    }

    public InputStatus RefreshInputStatus(InputAction context)
    {
        foreach (KeyCode key in InputActionMap[context].PrimaryKeys)
        {
            switch (InputActionMap[context].Method)
            {
                case InputMethod.OnPress:
                    if (Input.GetKeyDown(key))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.OnRelease:
                    if (Input.GetKeyUp(key))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.Hold:
                    if (Input.GetKey(key))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;
            }
        }

        foreach (KeyCode key in InputActionMap[context].MotionReverseKeys)
        {
            switch (InputActionMap[context].Method)
            {
                case InputMethod.OnPress:
                    if (Input.GetKeyDown(key))
                    {
                        SaveKeyStatus(context, true, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.OnRelease:
                    if (Input.GetKeyUp(key))
                    {
                        SaveKeyStatus(context, true, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.Hold:
                    if (Input.GetKey(key))
                    {
                        SaveKeyStatus(context, true, true);
                        return _inputKeyStatus[context];
                    }
                    break;
            }
        }

        foreach (KeyCode mouseButton in InputActionMap[context].MouseButtons)
        {
            switch (InputActionMap[context].Method)
            {
                case InputMethod.OnPress:
                    if (Input.GetMouseButtonDown((int)mouseButton))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.OnRelease:
                    if (Input.GetMouseButtonUp((int)mouseButton))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;

                case InputMethod.Hold:
                    if (Input.GetMouseButton((int)mouseButton))
                    {
                        SaveKeyStatus(context, true);
                        return _inputKeyStatus[context];
                    }
                    break;
            }
        }

        SaveKeyStatus(context, false);
        Logger.LogMessage("action = " + context.ToString()+" , status"+ _inputKeyStatus[context].Status);
        return _inputKeyStatus[context];
    }
}

[Serializable]
public struct ActionKeys
{
    public InputAction Action;
    public MappedKeys MappedKey;    
}

[Serializable]
public struct InputStatus
{
    public bool Status;
    public bool ReverseMotion;
}

[Serializable]
public struct MappedKeys
{
    public KeyCode CombinationKey;
    public List<KeyCode> PrimaryKeys;
    public List<KeyCode> MotionReverseKeys;
    public List<MouseInput> MouseButtons;
    public InputMethod Method;
}
