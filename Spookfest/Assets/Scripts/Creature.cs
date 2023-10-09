using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    //feel free to add more boosts/perks these are all i could think of so far
    public string[] boosts = { "Health", "Speed", "Sneak", "Sight"};
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //creature behavior
    }
    public void tame()
    {
        //make creature follow player 
        Player_Inventory.addCreature(gameObject);
    }
}
