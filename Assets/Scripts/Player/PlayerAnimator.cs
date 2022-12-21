using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private float horizontal;

    private Animator animator;
    private PlayerController pc;

    [SerializeField] private float jumpTime;

    private void Start()
    {
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        #region Running
        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        if (horizontal > 0) GetComponent<SpriteRenderer>().flipX = false;
        if (horizontal < 0) GetComponent<SpriteRenderer>().flipX = true;
        #endregion

        #region Jumping / Falling
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("isJumping", true);
            Invoke(nameof(SetJumpingToFalse), jumpTime);
        }
        if (!pc.IsGrounded()) animator.SetBool("isGrounded", false);
        if (pc.IsGrounded()) animator.SetBool("isGrounded", true);
        #endregion

        #region Wall
        if (pc.TouchingL() || pc.TouchingR()) animator.SetBool("isTouching", true);
        if (!pc.TouchingL() && !pc.TouchingR()) animator.SetBool("isTouching", false);
        #endregion
    }

    void SetJumpingToFalse()
    {
        animator.SetBool("isJumping", false);
    }
}
