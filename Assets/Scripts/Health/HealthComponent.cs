using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField]
    public int mStartingHealth;
    [SerializeField]
    public int mCurrentHealth ;

    public int CurrentHealth => mCurrentHealth;
    public int StartingHealth => mStartingHealth;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        mCurrentHealth = mStartingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
        mCurrentHealth -= damage;

        if (mCurrentHealth <= 0)
            GetComponent<Animator>().Play("Death");
    }

    public void HealPlayer(int health)
    {
        if (mCurrentHealth + health >= mStartingHealth)
            mCurrentHealth = mStartingHealth;
        else
            mCurrentHealth += health;
    }
}
