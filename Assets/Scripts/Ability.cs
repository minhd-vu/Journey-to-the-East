using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected float cooldown = 5f;
    [SerializeField] protected string buttonName = "";
    [SerializeField] protected float damage;

    protected PlayerController player;
    protected bool isActive;
    private float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GetComponent<PlayerController>();
        timer = cooldown;
        isActive = false;
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        if ((timer += Time.deltaTime) >= cooldown && !isActive && Input.GetButtonDown(buttonName) && GetComponent<PlayerController>().Mana >= manaCost)
        {
            StartCoroutine(CastAbility());
            player.Mana -= manaCost;
            timer = 0f;
        }
    }

    protected abstract IEnumerator CastAbility();
    protected virtual void StopAbility()
    {
        isActive = false;
    }
}
