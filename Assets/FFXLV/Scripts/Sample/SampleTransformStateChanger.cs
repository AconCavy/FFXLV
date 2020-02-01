﻿using UnityEngine;

namespace FFXLV.Sample
{
    public class SampleTransformStateChanger : MonoBehaviour
    {
        [SerializeField] private TransformChanger transformChanger;
        [SerializeField] private float angularMagunitude = 0.2f;
        [SerializeField] private float distanceMagunitude = 0.4f;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Initialize();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.transformChanger.NextState();
            }
        }

        public void Initialize()
        {
            this.transformChanger.Initialize(this.angularMagunitude, this.distanceMagunitude);
        }
    }
}

