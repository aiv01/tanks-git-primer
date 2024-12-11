using Aiv.Fast2D;
using OpenTK;

namespace ECS_Tankz_1
{
    public class SpriteRenderer : Component, IDrawable
    {
        private Texture texture;
        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        private Sprite sprite;
        public Sprite Sprite { get { return sprite; } }

        private DrawLayer layer;
        public DrawLayer Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        private Vector2 pivot;
        public Vector2 Pivot
        {
            get { return pivot; }
            set
            {
                pivot = value;
                sprite.pivot = new Vector2(sprite.Width * pivot.X, sprite.Height * pivot.Y);
            }
        }

        private float width;
        public float Width
        {
            get { return width * Transform.Scale.X; }
            set { width = value; }
        }

        private float height;
        public float Height
        {
            get { return height * Transform.Scale.Y; }
            set { height = value; }
        }

        private Vector2 textureOffset;
        public Vector2 TextureOffset
        {
            get { return textureOffset; }
            set { textureOffset = value; }
        }

        public Camera Camera
        {
            get { return sprite.Camera; }
            set { sprite.Camera = value; }
        }

        public SpriteRenderer(GameObject owner, DrawLayer layer) : base(owner)
        {
            Layer = layer;
            DrawManager.AddItem(this);
        }



        public SpriteRenderer(GameObject owner, string textureName, Vector2 pivot, DrawLayer layer = DrawLayer.Background) : base(owner)
        {
            texture = GfxManager.GetTexture(textureName);
            textureOffset = Vector2.Zero;
            Width = Game.PixelsToUnit(texture.Width);
            Height = Game.PixelsToUnit(texture.Height);
            sprite = new Sprite(Width, Height);
            this.layer = layer;
            Pivot = pivot;
            DrawManager.AddItem(this);
        }

        public SpriteRenderer(GameObject owner, string textureName, Vector2 pivot, float width, float height, DrawLayer layer = DrawLayer.Background) : base(owner)
        {
            texture = GfxManager.GetTexture(textureName);
            textureOffset = Vector2.Zero;
            this.Width = Game.PixelsToUnit(width);
            this.Height = Game.PixelsToUnit(height);
            sprite = new Sprite(this.Width, this.Height);
            this.layer = layer;
            Pivot = pivot;
            DrawManager.AddItem(this);
        }

        public void Draw()
        {
            sprite.position = Transform.Position; // in unità non in pixel
            sprite.EulerRotation = Transform.Rotation;
            sprite.scale = Transform.Scale;
            sprite.DrawTexture(texture, (int)textureOffset.X, (int)textureOffset.Y, Game.UnitToPixels(Width), Game.UnitToPixels(Height));
        }

        public static SpriteRenderer Factory(GameObject owner, string textureName, Vector2 pivot, DrawLayer layer)
        {
            return new SpriteRenderer(owner, textureName, pivot, layer);
        }

        public static SpriteRenderer Factory(GameObject owner, string textureName, Vector2 pivot, DrawLayer layer, Vector2 textureOffset, float width, float height)
        {
            SpriteRenderer sr = new SpriteRenderer(owner, textureName, pivot, width, height, layer);
            sr.textureOffset = textureOffset;
            return sr;
        }

        public override Component Clone(GameObject owner)
        {
            SpriteRenderer sr = new SpriteRenderer(owner, Layer);
            sr.texture = texture;
            sr.sprite = new Sprite(sprite.Width, sprite.Height);
            sr.Width = Width;
            sr.Height = Height;
            sr.pivot = pivot;
            sr.textureOffset = textureOffset;
            sr.Camera = Camera;
            return sr;
        }
    }
}
