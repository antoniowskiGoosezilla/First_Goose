using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe usata per aggiungere automaticamente tutti i componenti necessari
//per i personaggi controllati dal giocatore

namespace AntoNamespace
{
    [RequireComponent(typeof(PlayerAnimatorHandler))]
    [RequireComponent(typeof(PlayerInventoryHandler))]
    [RequireComponent(typeof(PlayerLocomotionHandler))]
    [RequireComponent(typeof(PlayerStatsHandler))]
    [RequireComponent(typeof(PlayerAimHandler))]
    [RequireComponent(typeof(ComboHandler))]
    [RequireComponent(typeof(CapsuleCollider))]
    
    public class PlayerCore : MonoBehaviour
    {
    }
}
