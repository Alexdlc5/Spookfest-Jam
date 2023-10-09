using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering;

public static class Player_Inventory 
{
    //inventorys
    private static HashSet<GameObject> creatures = new HashSet<GameObject>();
    //stats (creatures in inventory will effect this by checking the creatures script when it is added to the array of cratures)
    public static float health_boost;
    public static float speed_boost;  
    public static float sneak_boost;
    public static float sight_boost;
    public static void addCreature(GameObject creature)
    {
        creatures.Add(creature);
        applyAndUpdateBoosts(true);
    }
    public static void removeCreature(GameObject creature)
    {
        creatures.Remove(creature);
        applyAndUpdateBoosts(false);
    }
    private static void applyAndUpdateBoosts(bool creatureAdded)
    { 
        //check creature script for stat boosts and apply/remove them to the boost fields.
        //apply updated boosts to player              ^-based on creatureAdded t/f
    }
}
