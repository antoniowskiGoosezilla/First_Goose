using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AntoNamespace;
using TMPro;

public class testHUDHandler : MonoBehaviour
{
    //PUBLIC
    [Header("HEALTH, STACKS AND AMMO")]
    [SerializeField] GameObject health;
    [SerializeField] GameObject actionStacks;
    [SerializeField] GameObject ammo;
    [Space]
    [Header("COMBO METER")]
    [SerializeField] GameObject comboMeter;



    //PRIVATE

    private List<GameObject> actionStacksArray;
    private Color activeStackColor = Color.yellow;
    private Color disactiveStackColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.33f);

    private TextMeshProUGUI comboTitle;
    private TextMeshProUGUI comboPoints;
    private Slider comboTimer;

    private TextMeshProUGUI currentAmmoText;
    private TextMeshProUGUI totalAmmoText;
    private TextMeshProUGUI maxTotalAmmoText;

    private void Awake()
    {
        //Eventi
        PlayerStatsHandler.OnUpdateStacks += UpdateStacks;
        PlayerStatsHandler.OnUpdateCooldown += UpdateStackValue;
        ComboHandler.OnUpdateCombo += UpdateComboMeter;
        ComboHandler.OnUpadateComboTimer += UpdateTimer;
        PlayerAimHandler.OnUpdateWeaponAmmo += UpdateAmmoCounter;
        PlayerInventoryHandler.OnUpdateWeaponAmmo += UpdateAmmoCounter;

        actionStacksArray = new List<GameObject>();
        foreach(Transform child in actionStacks.transform)
        {
            actionStacksArray.Add(child.gameObject);
        }

        comboTitle = comboMeter.transform.Find("Combo Name").gameObject.GetComponent<TextMeshProUGUI>();
        comboPoints = comboMeter.transform.Find("Points").gameObject.GetComponent<TextMeshProUGUI>();
        comboTimer = comboMeter.transform.Find("Timer").gameObject.GetComponent<Slider>();

        currentAmmoText = ammo.transform.Find("CurrentAmmo").gameObject.GetComponent<TextMeshProUGUI>();
        totalAmmoText = ammo.transform.Find("Total").gameObject.GetComponent<TextMeshProUGUI>();
        maxTotalAmmoText = ammo.transform.Find("MaxTotal").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        comboMeter.SetActive(false);
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
                actionStack.transform.Find("Fill").GetComponent<Image>().color = add ? activeStackColor : disactiveStackColor; //Aggiorna il colore
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

        stack.transform.Find("Fill").GetComponent<Image>().color = activeStackColor;
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
        actionStacksArray[realIndex].transform.Find("Fill").GetComponent<Image>().color = disactiveStackColor;
        stack.value = newValue;
    }

    private void UpdateComboMeter(float points, float timer, string comboName)
    {
        if(comboMeter.activeInHierarchy == false)
            comboMeter.SetActive(true);

        comboTitle.text = comboName;
        comboPoints.text = points.ToString();
        comboTimer.value = timer;

        if(timer == 0)
        {
            comboMeter.SetActive(false);
        }
    }

    private void UpdateTimer(float newValue)
    {
        comboTimer.value = newValue;
    }

    private void UpdateAmmoCounter(float currentAmmo, float totalAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString();
        totalAmmoText.text = totalAmmo.ToString();
    }

    private void UpdateAmmoCounter(float currentAmmo, float totalAmmo, float maxTotalAmmo)
    {
        currentAmmoText.text = currentAmmo.ToString();
        totalAmmoText.text = totalAmmo.ToString();
        maxTotalAmmoText.text = maxTotalAmmo.ToString();
    }

}
