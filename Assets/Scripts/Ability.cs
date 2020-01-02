using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected float cooldown = 5f;
    [SerializeField] protected string buttonName = "";
    [SerializeField] protected float damage;
    private float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        if ((timer += Time.deltaTime) >= cooldown && Input.GetButtonDown(buttonName) && GetComponent<PlayerController>().Mana >= manaCost)
        {
            StartCoroutine(CastAbility());
            GetComponent<PlayerController>().Mana -= manaCost;
            timer = 0f;
        }
    }

    protected abstract IEnumerator CastAbility();
}
