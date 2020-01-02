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

            Debug.Log("Roll");

            // Deactivate the arms.
            player.leftArm.SetActive(false);
            player.rightArm.SetActive(false);

            player.rb.velocity = player.input * player.moveSpeed * rollSpeed;

            //yield return new WaitForSeconds(player.animator.GetCurrentAnimatorStateInfo(0).length);
            yield return null;
        }
    }

    protected void StopRoll()
    {
        StopAbility();
        if (player.isAlive)
        {
            Debug.Log("Stop Roll");
            isConcurrentActive = false;
            player.updateMovingDirection = true;
            player.leftArm.SetActive(true);
            player.rightArm.SetActive(true);
        }
    }
}
