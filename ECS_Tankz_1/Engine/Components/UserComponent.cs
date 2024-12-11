using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public abstract class UserComponent : Component, IStartable, IUpdatable, ICollidable
    {
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (GameObject.IsActive)
                {
                    if (!Enabled && value)
                    { //non è attivo e lo voglio abilitare (value = true)
                        OnEnable();
                    }
                    else if (Enabled && !value)
                    { //è attivo e lo voglio disabilitare (value = false)
                        OnDisable();
                    }
                }
                base.Enabled = value;
            }
        }

        public UserComponent(GameObject owner) : base(owner)
        {
            
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }


        public virtual void LateUpdate()
        {

        }

        public virtual void OnCollide(Collision CollisionInfo)
        {

        }

        public virtual void OnEnable()
        {

        }

        public virtual void OnDisable()
        {

        }
    }
}
