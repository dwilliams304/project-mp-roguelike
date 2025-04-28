using UnityEngine;

namespace ContradictiveGames
{
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Custom/Player/Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Movement")]
        public AnimationCurve MovementSlowdownCurve;
        public float MoveSpeed = 6f;

        [Header("Looking")]
        [Range(0, 3)] public float LookSmoothing = 0.15f;
        public LayerMask MouseHitLayer;

        [Header("Misc")]
        public LayerMask EnemyHitLayers;
        public float InteractionRadius = 3f;

        [Header("DEBUG SETTINGS")]
        public GameObject ProjectilePrefab;
    }
}