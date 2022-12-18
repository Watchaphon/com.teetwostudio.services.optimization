﻿using System.Collections.Generic;
using UnityEngine;

namespace Services.Optimization.PoolingSystem
{
    /// <summary>
    /// Pooling system for target of initialized prefab object.
    /// </summary>
    public class PoolingSystem
    {
        private List<PoolingObject> objectPoolings = new List<PoolingObject>();

        private GameObject object_Ref;
        private PoolingObject objjectPool_Ref;

        private PoolingProfile profile;
        private Transform originalPerent;

        private string profileID;

        private int currentPooilingIndex = 0;

        public int ObjectCount { get => objectPoolings.Count; }

        public Transform root { get => originalPerent; }


        /// <summary>
        /// This method use for installation pooling system and init all pooling objects of target prefab width parent.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="amount"></param>
        public void Initialize(PoolingProfile profile, int amount, Transform parent)
        {
            this.profile = profile;

            originalPerent = parent;
            profileID = profile.ID;

            for (int i = 0; i < amount; i++)
            {
                AddNew();
            }
        }

        /// <summary>
        /// This metod use for add pooling count of original prefab.
        /// </summary>
        /// <param name="count"></param>
        public void AddCount(int count)
        {
            for (int i = 0; i < count; i++)
                AddNew();
        }

        private void AddNew()
        {
            object_Ref = Object.Instantiate(profile.Prefab, originalPerent);
            objjectPool_Ref = new PoolingObject(object_Ref, profile, profileID);

            if (!objjectPool_Ref.gameObject) 
            { 
                Debug.LogError("'ObjectPooing' can't create prefab <" + profile.name + ">");
                return; 
            }

            objjectPool_Ref.gameObject.SetActive(false);
            objjectPool_Ref.SetOriginalPerent(originalPerent);
            objectPoolings.Add(objjectPool_Ref);
        }

        /// <summary>
        /// Call for pool object prefab out width position and rotaion and enabled.
        /// </summary>
        /// <param name="poolPosition"></param>
        /// <param name="poolRotation"></param>
        /// <returns>Lasted object prefab index</returns>
        public PoolingObject PoolObject(Vector3 poolPosition, Quaternion poolRotation)
        {
            objjectPool_Ref = GetValidableObject();

            objjectPool_Ref.transform.position = poolPosition;
            objjectPool_Ref.transform.rotation = poolRotation;
            objjectPool_Ref.transform.localScale = profile.Prefab.transform.localScale;
            objjectPool_Ref.Enabled();

            SetNextIndex();

            return objjectPool_Ref;
        }

        /// <summary>
        /// Call for pool object prefab out width set parent and enabled.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>Lasted object prefab index</returns>
        public PoolingObject PoolObject(Transform parent)
        {
            objjectPool_Ref = GetValidableObject();

            objjectPool_Ref.transform.SetParent(parent);
            objjectPool_Ref.transform.localPosition = Vector3.zero;
            objjectPool_Ref.transform.localScale = profile.Prefab.transform.localScale;
            objjectPool_Ref.Enabled();

            return objjectPool_Ref;
        }

        /// <summary>
        /// Call for pool object prefab and enabled.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>Lasted object prefab index</returns>
        public PoolingObject PoolObject()
        {
            objjectPool_Ref = GetValidableObject();

            objjectPool_Ref.transform.localPosition = Vector3.zero;
            objjectPool_Ref.transform.localScale = profile.Prefab.transform.localScale;
            objjectPool_Ref.Enabled();

            return objjectPool_Ref;
        }

        /// <summary>
        /// Calculate validabe prefab to pool.
        /// </summary>
        /// <returns></returns>
        private PoolingObject GetValidableObject()
        {
            objjectPool_Ref = objectPoolings[currentPooilingIndex];

            if (objjectPool_Ref.IsActive)
            {
                AddNew();
                currentPooilingIndex = objectPoolings.Count - 1;
                objjectPool_Ref = objectPoolings[currentPooilingIndex];
                return objjectPool_Ref;
            }

            SetNextIndex();
            return objjectPool_Ref;
        }


        private void SetNextIndex()
        {
            if (currentPooilingIndex < objectPoolings.Count - 1) 
                currentPooilingIndex++;
            else 
                currentPooilingIndex = 0;
        }

        public void Dispose()
        {
            PoolingManager.RemoveFormRecycleDictionary(profileID);
        }

        public void DisabledAll()
        {
            currentPooilingIndex = 0;

            for (int i = 0; i < objectPoolings.Count; i++)
            {
                objectPoolings[i].Disabled();
            }
        }
    }
}