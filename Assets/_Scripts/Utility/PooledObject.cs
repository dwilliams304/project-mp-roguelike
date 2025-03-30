using System.Collections.Generic;
using UnityEngine;

namespace ContradictiveGames.Utility
{
    [System.Serializable]
    public class PooledObject
    {
        public GameObject ObjectToPool;
        [Min(1)]
        [SerializeField] private int AmountToPool = 50;
        [SerializeField] private bool PoolCanGrow = false;

        //Nothing is allowed to change this accept this script!
        private readonly List<GameObject> currentPool = new List<GameObject>();
        //To ensure that the pool was initialized and that there are objects in the pool
        private bool isInitialized = false;

        /// <summary>
        /// Set up the pool by instantiating the object and setting them to be inactive
        /// </summary>
        public void InitializePool(){
            if(ObjectToPool == null)
            {
                Debug.LogError("We don't have an object to pool and can not initialize it!");
                return;
            }
            isInitialized = true;
            for(int i = 0; i < AmountToPool; i++){
                CreateObject();
            }
        }

        /// <summary>
        /// Create an object by instantiating it, and addingthem to the pool list
        /// </summary>
        /// <param name="andRetrieve">Do we also want to retrieve the freshly created object?</param>
        /// <param name="target">If we are retrieving it, where we are changing the position/rotation to</param>
        /// <returns></returns>
        private GameObject CreateObject(bool andRetrieve = false, Transform target = null){
            GameObject pooledObj = GameObject.Instantiate(ObjectToPool);
            currentPool.Add(pooledObj);
            if(andRetrieve){
                SetObject(pooledObj, target);
            }
            else{
                pooledObj.SetActive(false);
            }
            return pooledObj;
        }

        /// <summary>
        /// Try to get a currently inactive object in the pool and return that
        /// </summary>
        /// <param name="target">Where we are sending the object to</param>
        /// <returns></returns>
        public GameObject Retrieve(Transform target){
            //Check to make sure that the pool has been initialized, and that there are objects
            //in the current pool. If not, return nothing and log it!
            if(!isInitialized){
                Debug.LogError($"Object pool for: {ObjectToPool.name} has not been initialized!");
                return null;
            }
            for(int i = 0; i < currentPool.Count; i++){
                if(!currentPool[i].activeInHierarchy){
                    GameObject obj = currentPool[i];
                    SetObject(obj, target);
                    return obj;
                }
            }
            if(PoolCanGrow){
                return CreateObject(true, target);
            }
            Debug.Log("All objects are active and pool can not grow!");
            return null;
        }


        /// <summary>
        /// Set the position and rotation of the object, as well as make it active
        /// </summary>
        /// <param name="obj">The object we are effecting</param>
        /// <param name="target">Where we are sending it to</param>
        private void SetObject(GameObject obj, Transform target){
            obj.transform.position = target.position;
            obj.transform.rotation = target.rotation;
            obj.SetActive(true);
        }
    }
}