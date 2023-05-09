using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AntoNamespace;

public class testHUDHandler : MonoBehaviour
{
    //PUBLIC
    [SerializeField] GameObject health;
    [SerializeField] GameObject actionStacks;
    [SerializeField] GameObject ammo;


    //PRIVATE

    private List<GameObject> actionStacksArray;

    private void Awake()
    {
        actionStacksArray = new List<GameObject>();

        //Eventi
        PlayerStatsHandler.OnUpdateStacks += UpdateStacks;
        PlayerStatsHandler.OnUpdateCooldown += UpdateStackValue;

        foreach(Transform child in actionStacks.transform)
        {
            actionStacksArray.Add(child.gameObject);
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void UpdateStacks(int index, bool add)
    {
        if(index == -1)
        {
            foreach (GameObject actionStack in actionStacksArray)
            {
                Slider slider = actionStack.GetComponent<Slider>();
                slider.value = add ? 1 : 0;
            }
            return;
        }

        int realIndex = index-1;
        Slider stack = actionStacksArray[realIndex].GetComponent<Slider>();
        
        if(!add)
        {
            stack.value = 0;
            return;
        }

        stack.value = 1;

        
    }

    private void UpdateStackValue(int index, float newValue)
    {
        int realIndex = index-1;
        //Cotrollo per vedere se si tratta della prima azione o meno
        if(index < actionStacksArray.Count)
        {
            Slider nextStack = actionStacksArray[index].GetComponent<Slider>();
            nextStack.value = 0;
        }
        Slider stack = actionStacksArray[realIndex].GetComponent<Slider>();
        stack.value = newValue;
    }
}
