using UnityEngine;

public class MonkeyCS : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public Weapon weapon;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();

    }

    void Update()
    {
        if (true)
        {
            weapon.SetAttackState(true);
        }
        else
        {
            anim.SetBool("Attack", false);
            weapon.SetAttackState(false);
        }
    }
}
