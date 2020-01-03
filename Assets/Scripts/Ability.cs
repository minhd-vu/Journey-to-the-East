using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected float cooldown = 5f;
    [SerializeField] protected string buttonName = "";
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected string sound = "";

    protected static GameObject lowManaText = null;
    protected static bool isConcurrentActive = false;
    protected PlayerController player;
    protected bool isActive;
    private float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        timer = cooldown;
        isActive = false;

        if (lowManaText != null)
        {
            lowManaText.SetActive(false);
        }

        else
        {
            lowManaText = GameObject.FindWithTag("Low Mana Indicator");
        }
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown && !isActive && Input.GetButtonDown(buttonName))
        {
            if (player.Mana >= manaCost)
            {
                StartCoroutine(CastAbility());
                player.Mana -= manaCost;
                timer = 0f;

                if (lowManaText != null)
                {
                    lowManaText.SetActive(false);
                }
            }

            else if (lowManaText != null)
            {
                lowManaText.SetActive(true);
            }
        }
    }

    protected abstract IEnumerator CastAbility();
    protected virtual void StopAbility()
    {
        isActive = false;
    }
}
