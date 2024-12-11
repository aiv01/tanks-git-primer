using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public abstract class Component
    {
        private bool enabled;

        public Transform Transform { get { return GameObject.Transform; } }

        public virtual bool Enabled { get { return GameObject.IsActive && enabled; } set {  enabled = value; } }
        public virtual bool EnableSelf
        {
            get { return enabled; }
        }
        public GameObject GameObject { get; private set; } 

        public Component(GameObject owner)
        {
            this.GameObject = owner;
            enabled = true;
        }

        public T GetComponent<T>() where T : Component
        {
            return GameObject.GetComponent<T>();
        }

        public abstract Component Clone(GameObject owner);
    }
}
