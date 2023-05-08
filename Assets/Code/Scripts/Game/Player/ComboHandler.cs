using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AntoNamespace
{    
    public class ComboHandler : MonoBehaviour
    {
        public enum ComboGrade
        {
            None,
            Good,
            Gooder,
            Fantastic,
            Great,
            Superb,
            DeadEye
        }

        //PUBLIC

        public float comboPoints = 0; //{get; private set;}
        public ComboGrade comboGrade = ComboGrade.None;

        public float globalComboPoints;

        public void AddPoints(float points)
        {
            comboPoints += points;
            comboTimer = comboInterval;
            UpdateComboGrade();

            if(!comboStarted)
                StartCoroutine("StartComboCooldown");
        }

        public void EndCombo()
        {
            StopCoroutine(StartComboCooldown());
            ResetCombo();
        }

        public void SetNewTotalPointPercentage(float percetage)
        {
            percentageTotalPointGain = percetage;
        }




        //PRIVATE

        private bool comboStarted = false;
        private float comboTimer;
        private float comboInterval = 10;

        private float percentageTotalPointGain = 0.07f;

        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        private void UpdateComboGrade()
        {
            switch(comboPoints)
            {
                case var expression when (comboPoints > 0 && comboPoints <= 1000 && comboGrade != ComboGrade.Good):
                    comboGrade = ComboGrade.Good;
                    break;
                case var expression when (comboPoints > 1000 && comboPoints <= 2500 && comboGrade != ComboGrade.Gooder):
                    comboGrade = ComboGrade.Gooder;
                    break;
                case var expression when (comboPoints > 2500 && comboPoints <= 5000 && comboGrade != ComboGrade.Fantastic):
                    comboGrade = ComboGrade.Fantastic;
                    break;
                case var expression when (comboPoints > 5000 && comboPoints <= 10000 && comboGrade != ComboGrade.Great):
                    comboGrade = ComboGrade.Great;
                    break;
                case var expression when (comboPoints > 10000 && comboPoints <= 20000 && comboGrade != ComboGrade.Superb):
                    comboGrade = ComboGrade.Superb;
                    break;
                case var expression when (comboPoints > 20000 && comboGrade != ComboGrade.DeadEye):
                    comboGrade = ComboGrade.DeadEye;
                    break;
                default:
                    break;
            }
        }

        private void ResetCombo()
        {
            globalComboPoints += comboPoints*percentageTotalPointGain;              //7%

            comboTimer = 0;
            comboPoints = 0;
            comboGrade = ComboGrade.None;
            comboStarted = false;
        }

        private IEnumerator StartComboCooldown()
        {
            comboStarted = true;
            while(comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
                yield return null;
            }

            ResetCombo();
        }

    }
}
