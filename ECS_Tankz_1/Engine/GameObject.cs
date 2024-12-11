using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS_Tankz_1
{
    public class GameObject
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = string.IsNullOrEmpty(value) ? "GameObject" : value; }
        }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (!isActive && value)
                { //quando entro qui, il gameobject è disattivato. Se vado a controllare tutti i componenti anch'essi sono disabilitati indipendentemente dal loro attributo enable. Perché la proprietà Enable controlla anche GO IsActive
                    isActive = true;
                    OnEnable();
                }
                else if (isActive && !value)
                {//quando entro qui, il gameobject è attivato. Se prima lo disattivo e poi vado  a controllare tutti i componenti anch'essi sono disabilitati indipendentemente dal loro attributo enable. Perché la proprietà Enable controlla anche GO IsActive
                    OnDisable();
                    isActive = false;
                }
                if (!isStarted && value)
                {
                    Start();
                }
            }
        }

        private bool isStarted;
        public Transform Transform { get; private set; }

        public List<Component> components;

        private string tag;
        public string Tag
        {
            get { return tag; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    tag = "Untagged";
                }
                else
                {
                    tag = value;
                }
            }
        }



        public GameObject()
        {
            Name = string.Empty;
            Transform = new Transform(Vector2.Zero, Vector2.One);
            components = new List<Component>();
            isActive = true;
            if (Game.CurrentScene.IsInizialized)
            { //qui significa che l'oggetto è stato creato dopo l'inizializzazione della scena e quindi la sua awake e la sua start non sarà mai chiamata
                Awake();
                Start();
            }
            RegisterToScene();
        }

        public GameObject(string name, Vector2 position)
        {
            Name = name;
            Transform = new Transform(position, Vector2.One);
            components = new List<Component>();
            isActive = true;
            if (Game.CurrentScene.IsInizialized)
            {//qui significa che l'oggetto è stato creato dopo l'inizializzazione della scena e quindi la sua awake e la sua start non sarà mai chiamata
                Awake();
                Start();
            }
            RegisterToScene();
        }

        public GameObject(string name, Vector2 position, Vector2 scale)
        {
            Name = name;
            Transform = new Transform(position, scale);
            components = new List<Component>();
            isActive = true;
            if (Game.CurrentScene.IsInizialized)
            {//qui significa che l'oggetto è stato creato dopo l'inizializzazione della scena e quindi la sua awake e la sua start non sarà mai chiamata
                Awake();
                Start();
            }
            RegisterToScene();
        }

        private void RegisterToScene()
        {
            Game.CurrentScene.RegisterGameObject(this);
        }

        public void OnEnable()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.OnEnable();
            }
        }

        public void OnDisable()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.OnDisable();
            }
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public Component AddComponent(Type type, params object[] args)
        {
            object[] parameters = new object[args.Length + 1];
            parameters[0] = this;

            for (int i = 1; i < parameters.Length; i++)
            {
                parameters[i] = args[i - 1];
            }

            Component component = Activator.CreateInstance(type, parameters) as Component;
            components.Add(component);

            return component;
        }

        public T AddComponent<T>(params object[] args) where T : Component
        {
            object[] parameters = new object[args.Length + 1];
            parameters[0] = this;
            for (int i = 1; i < parameters.Length; i++)
            {
                parameters[i] = args[i - 1];
            }
            Component component = Activator.CreateInstance(typeof(T), parameters) as Component; //Activator.CreateInstance --> crea un oggetto del tipo type, 
                                                                                                //cercando tra i costruttori della classe di tipo type, quello che matcha l'array di pararametri paramters
            components.Add(component);
            return (T)component;
        }

        public Component GetComponent(Type type)
        {
            foreach (Component component in components)
            { 
                if(component.GetType() == type)
                {
                    return component;
                }
            }

            Type currentType;
            foreach(Component component in components)
            {
                currentType = component.GetType().BaseType;

                while(currentType != typeof(object))
                {
                    if(currentType == type)
                    {
                        return component;
                    }

                    currentType = currentType.BaseType;
                }
            }

            return null;
        }

        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in components)
            {
                if (component is T)
                {
                    return (T)component;
                }
            }
            return null;
        }

        public void Awake()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.Awake();
                if (temp.Enabled)
                {
                    temp.OnEnable();
                }
            }
        }

        public void Start()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.Start();
            }
            isStarted = true;
        }

        public void Update()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.Update();
            }
        }

        public void LateUpdate()
        {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.LateUpdate();
            }
        }

        public void OnCollide(Collision CollisionInfo)
         {
            UserComponent temp;
            for (int i = 0; i < components.Count; i++)
            {
                if (!components[i].Enabled) continue;
                temp = components[i] as UserComponent;
                if (temp == null) continue;
                temp.OnCollide(CollisionInfo);
            }
        }

        private static int cloneCounter = 0;

        public static GameObject Clone(GameObject objectToClone)
        {
            GameObject clone = new GameObject(objectToClone.name + "_Clone_" + cloneCounter,
                                              objectToClone.Transform.Position,
                                              objectToClone.Transform.Scale);
            cloneCounter++;
            foreach (Component component in objectToClone.components)
            {
                clone.AddComponent(component.Clone(clone));
            }
            return clone;
        }

        public static GameObject Find(string name)
        {
            return Game.CurrentScene.FindGameObject(name);
        }

        public static GameObject[] FindWithTag(string tag)
        {
            return Game.CurrentScene.FindGameObjectsByTag(tag);
        }
    }
}
