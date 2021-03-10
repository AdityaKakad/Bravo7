using Assets.Scripts;
using UnityEngine;

public class ArrowKeysDetector : MonoBehaviour, IInputDetector
{
    public InputDirection? DetectInputDirection()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            return InputDirection.Top;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            return InputDirection.Bottom;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            return InputDirection.Right;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            return InputDirection.Left;
        else if (Input.GetKeyUp(KeyCode.Space))
            return InputDirection.Jump;
        else
            return null;
    }
}