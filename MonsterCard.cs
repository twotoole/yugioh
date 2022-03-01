using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Monster Card")]
public class MonsterCard : Card
{
    public int Attack;
    public int Defence;
    public int Level;
    public enum Attribute {Dark, Devine, Earth, Fire, Light, Water, Wind};
    public Attribute attribute;
    public Type type;


    public void Display(){
        
    }
}

