using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Card : ScriptableObject
{
    public enum Category {Normal, Effect, Spell, Field};
    public Category category;
    public string CardName;
    public Material cardImage;
    public enum Type {Aqua, Beast, BeastWarrior, Dinosaur, Dragon, Fairy, Fiend, Fish, Insect, Machine, Plant, Reptile, Rock, SeaSerpent, Spellcaster, Thunder, Warrior, WingedBeast, Zombie};
    
}




