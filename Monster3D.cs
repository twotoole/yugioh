
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "3DMonster")]
public class Monster3D : ScriptableObject
{
    public string CardName;
    public GameObject Prefab;
    public Animator AnimatonController;
    public bool Defence = false;


    public void DefenceMode(){
        AnimatonController.SetBool("DefenceMode", Defence);
    }

    public void Attack(){
        AnimatonController.Play("Attack");
    }


}
