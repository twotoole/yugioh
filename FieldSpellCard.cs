using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Field Spell Card")]
public class FieldSpellCard : Card
{
    public GameObject FieldPrefab;
    public int Modifier;
    public List<Type> AffectedTypesList;
    
    
    public void ChangeStats(MonsterCard _monster){
        for(int i=0;i<AffectedTypesList.Count;i++){
            if(_monster.type == AffectedTypesList[i]){
                _monster.Attack += Modifier;
                _monster.Defence += Modifier;
            }
        }
    }
}
