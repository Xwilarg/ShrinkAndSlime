using System;
using UnityEngine;

namespace LudumDare56.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Range(0f, 10f)]
        [Tooltip("Speed of the player")]
        public float ForceMultiplier = 1f;

        [Range(0f, 10f)]
        [Tooltip("Speed multiplicator when the player is running")]
        public float SpeedRunningMultiplicator;

        [Range(0f, 10f)]
        [Tooltip("Vertical force used to make the player jump")]
        public float JumpForce;

        [Tooltip("Gravity multiplier to make the player fall")]
        public float GravityMultiplicator;
    }
}