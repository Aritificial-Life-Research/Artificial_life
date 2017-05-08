using System.Collections;
using Common;
using UnityEngine;

public class Health : BaseBehaviour
{
    int _health;

    public int maxHealth;
    public int health
    {
        get
        {
            return _health;
        }
    }

    public override void Awake()
    {
        base.Awake();
        InnerBus.Damage.AddListener(Damage);
    }

    public override void Start()
    {
        _health = maxHealth;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}