﻿using UnityEngine;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Scripts.Share
{
    public class FixShaderQueue : MonoBehaviour
    {
        public int AddQueue = 1;
        private Renderer rend;
        // Use this for initialization
        private void Start()
        {
            rend = GetComponent<Renderer>();
            if (rend != null)
                rend.sharedMaterial.renderQueue += AddQueue;
            else
                Invoke("SetProjectorQueue", 0.1f);
        }

        void SetProjectorQueue()
        {
            GetComponent<Projector>().material.renderQueue += AddQueue;
        }

        // Update is called once per frame
        void OnDisable()
        {
            if (rend != null)
                rend.sharedMaterial.renderQueue = -1;
        }
    }
}
