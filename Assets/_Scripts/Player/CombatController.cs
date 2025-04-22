using ContradictiveGames.Input;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames.Player
{
    public class CombatController : NetworkBehaviour
    {

        private Transform firePoint;
        private InputReader inputReader;

        private AttackData primaryAttack;
        private AttackData secondaryAttack;

        private float lastPrimaryAttack;
        private float lastSecondaryAttack;

        [Header("Settings")]
        [SerializeField] private LayerMask enemyHitLayers;

        [Header("DEBUG")]
        public GameObject projectilePrefab;

        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                Deinitialize();
            }
        }

        public void SetUpInput(InputReader _inputReader, Transform _firePoint)
        {
            inputReader = _inputReader;
            firePoint = _firePoint;

            if (inputReader != null)
            {
                inputReader.MainAttack += MainAttack;
                inputReader.SecondaryAttack += SecondaryAttack;
            }
        }

        public void InitializeCombatController(PlayerClassData classData)
        {
            primaryAttack = classData.PrimaryAttack;
            secondaryAttack = classData.SecondaryAttack;
        }

        private void Deinitialize()
        {
            if (inputReader != null)
            {
                inputReader.MainAttack -= MainAttack;
                inputReader.SecondaryAttack -= SecondaryAttack;
            }
        }

        private bool CanDoPrimary()
        {
            return Time.time >= lastPrimaryAttack + primaryAttack.AttackCooldown;
        }
        private bool CanDoSecondary()
        {
            return Time.time >= lastSecondaryAttack + secondaryAttack.AttackCooldown;
        }

        private void MainAttack()
        {
            if (CanDoPrimary())
            {
                lastPrimaryAttack = Time.time;
                HandleAttack(primaryAttack);
            }
        }

        private void SecondaryAttack()
        {
            if (CanDoSecondary())
            {
                lastSecondaryAttack = Time.time;
                HandleAttack(secondaryAttack);
            }
        }

        /// <summary>
        /// Handle the attack logic checks
        /// </summary>
        /// <param name="attack">Attack data needed</param>
        private void HandleAttack(AttackData attack)
        {
            //Early exit
            if (attack == null)
            {
                CustomDebugger.LogWarning($"No attack supplied!", ConsoleLogLevel.PreBuild);
                return;
            }
            if (attack is MeleeAttack)
            {
                DoMeleeAttack(attack as MeleeAttack);
            }
            else if (attack is RangedAttack)
            {
                DoRangedAttack(attack as RangedAttack);
            }
            HandleEffects();
        }

        private void DoMeleeAttack(MeleeAttack attack)
        {
            Vector3 origin = firePoint.position;
            Vector3 meleeDir = firePoint.up;

            Collider[] hits = Physics.OverlapSphere(origin, attack.SwingLength, enemyHitLayers);
            bool _hasHit = false;
            foreach (var hit in hits)
            {
                Vector3 directionToHit = (hit.transform.position - origin).normalized;
                float angle = Vector3.Angle(meleeDir, directionToHit);

                if (angle <= attack.SwingArc * 0.5f)
                {
                    _hasHit = true;
                }
            }
            AttackGizmoDrawer.Instance.DrawMeleeSwing(
                origin,
                meleeDir,
                attack.SwingLength,
                attack.SwingArc,
                _hasHit ? Color.yellow : Color.red,
                1f
            );
        }
        private void DoRangedAttack(RangedAttack attack)
        {
            Vector3 origin = firePoint.position;
            Vector3 direction = firePoint.up;

            if (attack.IsProjectile)
            {
                GameObject projectile = Instantiate(projectilePrefab, origin, Quaternion.identity);

            }
            else
            {
                if (Physics.Raycast(origin, direction, out RaycastHit hit, attack.MaxDistance, enemyHitLayers))
                {
                    AttackDebugger.DrawDebugLine(origin, hit.point, Color.yellow);
                    
                }
                else
                {
                    // MISS
                    Vector3 endPoint = origin + direction * attack.MaxDistance;
                    Debug.DrawLine(origin, endPoint, Color.red, 1f); // RED on miss
                }
            }
        }

        private void HandleEffects()
        {

        }

    }
}