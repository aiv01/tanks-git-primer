using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    public abstract class Scene
    {
        protected List<GameObject> sceneObjects = new List<GameObject>();
        protected bool isInizialized;
        public bool IsInizialized
        {
            get { return isInizialized; }
            set { isInizialized = value; }
        }

        public virtual void InitializeScene()
        {
            LoadAssets();
        }

        protected virtual void LoadAssets()
        {

        }

        public void Awake()
        {
            foreach (GameObject go in sceneObjects)
            {
                if (!go.IsActive) continue;
                go.Awake();
            }
        }

        public void Start()
        {
            foreach (GameObject go in sceneObjects)
            {
                if (!go.IsActive) continue;
                go.Start();
            }
        }

        public void Update()
        {
            foreach (GameObject go in sceneObjects)
            {
                if (!go.IsActive) continue;
                go.Update();
            }
        }

        public void LateUpdate()
        {
            foreach (GameObject go in sceneObjects)
            {
                if (!go.IsActive) continue;
                go.LateUpdate();
            }
        }

        public GameObject FindGameObject(string name)
        {
            foreach (GameObject obj in sceneObjects)
            {
                if(obj.Name == name)
                {
                    return obj;
                }
            }

            return null;
        }

        public GameObject[] FindGameObjectsByTag(string tag)
        {
            List<GameObject> gameObjectsTagged = new List<GameObject>();
            foreach (GameObject obj in sceneObjects)
            {
                if (obj.Tag == tag)
                {
                    gameObjectsTagged.Add(obj);
                }
            }
            return gameObjectsTagged.ToArray();
        }

        public void RegisterGameObject(GameObject go)
        {
            sceneObjects.Add(go);
        }

        public virtual void DestroyScene()
        {
            sceneObjects.Clear();
            PhysicsManager.ClearAll();
            DrawManager.ClearAll();
            GfxManager.ClearAll();
            FontManager.ClearAll();
            CameraManager.ClearAll();
        }
    }
}
