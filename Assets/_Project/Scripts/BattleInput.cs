using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public enum PadType { Keyboard, Xbox, Playstation, Generic }
public static class BattleInput
{

    public static PadType CurrentPad()
    {
        if (Gamepad.current == null)
            return PadType.Keyboard;

        if (Gamepad.current is DualShockGamepad)
            return PadType.Playstation;

        if (Gamepad.current is XInputController)
            return PadType.Xbox;

        return PadType.Generic;
    }
    public static bool Confirm()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            return true;
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            return true;

        return false;
    }

    public static bool Cancel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            return true;
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            return true;

        return false;
    }

    public static int Horizontal()
    {
        // Right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            return 1;
        if (Gamepad.current != null && (Gamepad.current.leftStick.right.wasPressedThisFrame || Gamepad.current.dpad.right.wasPressedThisFrame))
            return 1;

        // Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Q))
            return -1;
        if (Gamepad.current != null && (Gamepad.current.leftStick.left.wasPressedThisFrame || Gamepad.current.dpad.left.wasPressedThisFrame))
            return -1;

        return 0;
    }

    public static int Vertical()
    {
        // Up
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z))
            return -1;
        if (Gamepad.current != null && (Gamepad.current.leftStick.up.wasPressedThisFrame || Gamepad.current.dpad.up.wasPressedThisFrame))
            return -1;

        // Down (+1 vers le bas de la list)
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            return 1;
        if (Gamepad.current != null && (Gamepad.current.leftStick.down.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame))
            return 1;

        return 0;
    }


}
