using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelController : MonoBehaviour
{

    public List<Monster3D> Monsters;
    public List<Transform> FieldPositions;
    public List<GameObject> PlayedMonsters;


    public void SummonMonster(string _name, int _index)
    {
        for(int i = 0; i<Monsters.Count;i++)
        {
            if(_name == Monsters[i].CardName)
            {

                    if(FieldPositions[_index].GetComponent<CardPosition>().isOccupied == false && FieldPositions[_index].gameObject.tag == "MonsterZone")
                    {
                        GameObject newMonster;
                        newMonster = Instantiate(Monsters[i].Prefab);
                        newMonster.transform.position = FieldPositions[_index].transform.position;
                        newMonster.name = _name;
                        int ID = Animator.StringToHash("Summon");
                        if(newMonster.GetComponent<Animator>().HasState(0, ID)){
                            newMonster.GetComponent<Animator>().Play("Summon");
                        }
                        FieldPositions[_index].GetComponent<CardPosition>().isOccupied = true;
                        PlayedMonsters.Add(newMonster);
                        break;
                    }
                
            } 
        }
    }

    public void Attack(string _name){
        for(int i = 0; i<PlayedMonsters.Count;i++)
        {
            if(_name == PlayedMonsters[i].name)
            {
                int ID = Animator.StringToHash("Attack");
                if(PlayedMonsters[i].GetComponent<Animator>().HasState(0, ID))
                {
                    PlayedMonsters[i].GetComponent<Animator>().Play("Attack");
                }
            }
        }

    }


}
