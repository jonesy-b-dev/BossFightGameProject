using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private PlayerController player;

    private enum Facing { Up, Down, Left, Right }
    [SerializeField] private Facing facing;

    private float powerX;
    private float powerY;
    private const float time = 0.1f;

    private void Start()
    {
        switch (facing)
        {
            case Facing.Up:
                {
                    powerX = 0f;
                    powerY = 10f;
                } break;
            case Facing.Down:
                {
                    powerX = 0f;
                    powerY = -10f;
                } break;
            case Facing.Left:
                {
                    powerX = -10f;
                    powerY = 5f;
                } break;
            case Facing.Right:
                {
                    powerX = 10f;
                    powerY = 5f;
                } break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.Damage(1);
            player.Knockback(powerX, powerY, time);
        }
    }
}
