using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : Ability
{
    [SerializeField] private float rollSpeed = 1.5f;

    protected override IEnumerator CastAbility()
    {
        if (player.animator.GetBool("Moving") && !isConcurrentActive)
        {
            isActive = true;
            isConcurrentActive = true;

            // Play the rolling animation.
            player.animator.SetTrigger("Roll");
            player.updateMovingDirection = false;

            // Deactivate the arms.
            player.leftArm.SetActive(false);
            player.rightArm.SetActive(false);

            player.rb.velocity = player.input * player.moveSpeed * rollSpeed;
        }

        yield return null;
    }

    protected override void StopAbility()
    {
        base.StopAbility();
        if (player.isAlive)
        {
            isConcurrentActive = false;
            player.updateMovingDirection = true;
            player.leftArm.SetActive(true);
            player.rightArm.SetActive(true);
        }
    }
}
