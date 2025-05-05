using ContradictiveGames.Input;
using FishNet.Object;
using UnityEngine;

namespace ContradictiveGames.Player
{
    [DisallowMultipleComponent]
    public class CombatController : NetworkBehaviour
    {
        private PlayerBase player;

        private Transform firePoint;
        private InputReader inputReader;

        private AttackData primaryAttack;
        private AttackData secondaryAttack;

        private float lastPrimaryAttack;
        private float lastSecondaryAttack;


        private LayerMask enemyHitLayers;


        // public override void OnNetworkDespawn()
        // {
        //     if (IsOwner)
        //     {
        //         Deinitialize();
        //     }
        // }

        public void Initialize(PlayerBase _player)
        {
            player = _player;
            enemyHitLayers = player.PlayerSettings.DamageableLayers;

            inputReader = player.InputReader;
            firePoint = player.FirePoint;

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
                    if(hit.TryGetComponent<NetworkObject>(out NetworkObject entity))
                    {
                        // ulong targetNetworkId = entity.NetworkObjectId;

                        // RequestToDoDamageServerRpc(targetNetworkId, attack.Damage);
                    }
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
                GameObject projectile = Instantiate(player.PlayerSettings.ProjectilePrefab, origin, Quaternion.identity);

            }
            else
            {
                if (Physics.Raycast(origin, direction, out RaycastHit hit, attack.MaxDistance, enemyHitLayers))
                {
                    AttackDebugger.DrawDebugLine(origin, hit.point, Color.yellow);
                    if(hit.collider.TryGetComponent<NetworkObject>(out NetworkObject entity))
                    {
                        // ulong targetNetworkId = entity.NetworkObjectId;

                        // RequestToDoDamageServerRpc(targetNetworkId, attack.Damage);
                    }
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