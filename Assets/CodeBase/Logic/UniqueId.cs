using System;
using UnityEngine;

namespace CodeBase.Logic
{
    public class UniqueId : MonoBehaviour
    {
        public string Id;
        public bool ShouldGenerate;

        private void Start()
        {
            if (ShouldGenerate)
                Generate();
        }
        
        private void Generate() => 
            Id = $"{gameObject.scene.name}_{Guid.NewGuid().ToString()}";
    }
}