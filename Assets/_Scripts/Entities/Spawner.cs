using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ContradictiveGames
{
    public class Spawner : NetworkBehaviour
    {
        [SerializeField] private List<Transform> spawnPositions = new();
        [SerializeField] private GameObject enemyPrefab;

        public override void OnNetworkSpawn(){
            if(IsServer){
                StartCoroutine(SpawnEntities(2f));
            }
        }

        private IEnumerator SpawnEntities(float delay){
            yield return new WaitForSeconds(delay);

            GameObject entity = Instantiate(enemyPrefab);
            entity.transform.position = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            entity.GetComponent<NetworkObject>().Spawn();
        }

        
    }
}
