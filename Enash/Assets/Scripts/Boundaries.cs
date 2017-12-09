using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public float _topBoundary;
    public float _bottomBoundary;
    public float _leftBoundary;
    public float _rightBoundary;

    public static Boundaries Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _topBoundary = GetComponent<SpriteRenderer>().bounds.max.y;
        _bottomBoundary = GetComponent<SpriteRenderer>().bounds.min.y;
        _leftBoundary = GetComponent<SpriteRenderer>().bounds.min.x;
        _rightBoundary = GetComponent<SpriteRenderer>().bounds.max.x;
    }

    public Vector3 GetBoundaryPosition(Vector3 posToLook)
    {
        // if the player reached the top boundary
        if (posToLook.y >= _topBoundary)
        {
            if (posToLook.x >= _rightBoundary)
                posToLook.x = _rightBoundary;
            else if (posToLook.x <= _leftBoundary)
                posToLook.x = _leftBoundary;
            posToLook.y = _topBoundary;
            return posToLook;
        }

        // if the player reached the bottom boundary
        if (posToLook.y <= _bottomBoundary)
        {
            if (posToLook.x >= _rightBoundary)
                posToLook.x = _rightBoundary;
            else if (posToLook.x <= _leftBoundary)
                posToLook.x = _leftBoundary;
            posToLook.y = _bottomBoundary;
            return posToLook;
        }

        // if the player reached the right boundary
        if (posToLook.x >= _rightBoundary)
        {
            posToLook.x = _rightBoundary;
            return posToLook;
        }

        // if the player reached the left boundary
        if (posToLook.x <= _leftBoundary)
        {
            posToLook.x = _leftBoundary;
            return posToLook;
        }

        return posToLook;
    }
}
