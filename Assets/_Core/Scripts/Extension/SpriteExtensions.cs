using System;
using UnityEngine;

using Vector2 = System.Numerics.Vector2;
using Vector4 = System.Numerics.Vector4;

namespace MoonlitMixes.Extensions
{
    using SaveSystems;
    
    public static class SpriteExtensions
    {
        private struct SpriteSerialized
        {
            public string base64Texture;
            public Vector4 rect;
            public Vector2 pivot;
        }

        public static string Serialize(this Sprite sprite)
        {
            Texture2D texture = sprite.ExtractReadableTexture();
            byte[] pngData = texture.EncodeToPNG();
            SpriteSerialized spriteSerialized = new()
            {
                base64Texture = Convert.ToBase64String(pngData),
                rect = new Vector4(sprite.rect.x, sprite.rect.y, sprite.rect.width, sprite.rect.height),
                pivot = new Vector2(sprite.pivot.x, sprite.pivot.y),
            };
            
            return SaveSystem.Instance.Serialize(spriteSerialized);
        }

        public static void Deserialize(this Sprite sprite, string json)
        {
            SpriteSerialized spriteSerialized = SaveSystem.Instance.Deserialize<SpriteSerialized>(json);
            
            byte[] pngData = Convert.FromBase64String(spriteSerialized.base64Texture);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(pngData);
            sprite = Sprite.Create(texture, 
                new Rect(spriteSerialized.rect.X, spriteSerialized.rect.Y, spriteSerialized.rect.W, spriteSerialized.rect.Z), 
                new UnityEngine.Vector2(spriteSerialized.pivot.X, spriteSerialized.pivot.Y));
        }
        
        private static Texture2D ExtractReadableTexture(this Sprite sprite)
        {
            Rect spriteRect = sprite.rect;
            Texture2D srcTex = sprite.texture;
            
            RenderTexture rt = RenderTexture.GetTemporary(
                (int)spriteRect.width,
                (int)spriteRect.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear
            );
            
            RenderTexture previous = RenderTexture.active;
            
            Texture2D cropped = new Texture2D((int)spriteRect.width, (int)spriteRect.height, TextureFormat.RGBA32, false);
            
            Color[] pixels = srcTex.GetPixels(
                (int)spriteRect.x,
                (int)spriteRect.y,
                (int)spriteRect.width,
                (int)spriteRect.height
            );

            cropped.SetPixels(pixels);
            cropped.Apply();

            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = previous;

            return cropped;
        }
    }
}