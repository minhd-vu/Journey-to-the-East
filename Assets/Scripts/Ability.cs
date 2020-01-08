using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected float manaCost = 10f;
    [SerializeField] protected float cooldown = 5f;
    [SerializeField] protected string buttonName = "";
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected string sound = "";

    protected static GameObject lowManaText = null;
    protected static PlayerController player;
    protected static bool isConcurrentActive = false;
    protected bool isActive;
    private float timer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
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

    protected virtual bool CanCast()
    {
        return timer >= cooldown && Input.GetButtonDown(buttonName);
    }

    // Update is called once per frame
    public virtual void OnUpdate()
    {
        timer += Time.deltaTime;

        if (cooldown > 0)
        {
            GameObject cooldownUI = GameObject.FindWithTag(buttonName + " Cooldown");

            if (cooldownUI != null)
            {
                float cooldownPercent = Mathf.Clamp(timer / this.cooldown, 0f, 1f);

                if (cooldownPercent >= 1f)
                {
                    cooldownPercent = 0;
                }

                cooldownUI.GetComponent<Image>().fillAmount = cooldownPercent;
            }
        }

        if (player.Mana >= manaCost)
        {
            if (CanCast())
            {
                GameObject offHand = GameObject.FindWithTag("Off Hand");
                if (offHand != null)
                {
                    offHand.GetComponent<Animator>().SetTrigger("Cast Spell");
                }

                StartCoroutine(CastAbility());
                player.Mana -= manaCost;
                timer = 0f;

                if (lowManaText != null)
                {
                    lowManaText.SetActive(false);
                }
            }
        }

        else if (lowManaText != null && !isActive)
        {
            lowManaText.SetActive(true);
        }
    }

    protected abstract IEnumerator CastAbility();
    protected virtual void StopAbility()
    {
        isActive = false;
    }
}
