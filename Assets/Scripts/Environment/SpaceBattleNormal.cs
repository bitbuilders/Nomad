using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBattleNormal : MonoBehaviour
{
    public enum Direction
    {
        UP,
        RIGHT,
        LEFT,
        DOWN
    }

    [SerializeField] Direction m_direction = Direction.RIGHT;

    public Vector3 Normal { get; private set; }

    private void Start()
    {
        switch (m_direction)
        {
            case Direction.UP:
                Normal = transform.up;
                break;
            case Direction.RIGHT:
                Normal = transform.right;
                break;
            case Direction.LEFT:
                Normal = -transform.right;
                break;
            case Direction.DOWN:
                Normal = -transform.up;
                break;
        }
    }
}
