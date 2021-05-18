using System;
using UnityEngine;

namespace PhotoPicker
{
    public enum TextureRotation
    {
        Default,
        RotateBy90,
        RotateBy180,
        RotateBy270
    }

    public static class TextureUtils
    {
        public static TextureRotation GetRotationByExifOrientation(int exifOrientation)
        {
            if (exifOrientation == 3)
            {
                return TextureRotation.RotateBy180;
            }

            if (exifOrientation == 6)
            {
                return TextureRotation.RotateBy90;
            }

            if (exifOrientation == 8)
            {
                return TextureRotation.RotateBy270;
            }

            return TextureRotation.Default;
        }

        public static Texture2D RotateTexture(Texture2D source, TextureRotation rotation)
        {
            if (rotation == TextureRotation.Default)
            {
                return source;
            }

            if (rotation == TextureRotation.RotateBy90)
            {
                return RotateTexture(source, clockwise: true);
            }

            if (rotation == TextureRotation.RotateBy270)
            {
                return RotateTexture(source, clockwise: false);
            }

            if (rotation == TextureRotation.RotateBy180)
            {
                var pixels = source.GetPixels32();
                Array.Reverse(pixels);
                var destTex = new Texture2D(source.width, source.height);
                destTex.SetPixels32(pixels);
                destTex.Apply();

                return destTex;
            }

            throw new InvalidOperationException($"Unhandled rotation value {rotation}");
        }

        private static Texture2D RotateTexture(Texture2D source, bool clockwise)
        {
            var original = source.GetPixels32();
            var rotated = new Color32[original.Length];
            var w = source.width;
            var h = source.height;

            for (var j = 0; j < h; ++j)
            {
                for (var i = 0; i < w; ++i)
                {
                    var iRotated = (i + 1) * h - j - 1;
                    var iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                    rotated[iRotated] = original[iOriginal];
                }
            }

            var rotatedTexture = new Texture2D(h, w);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }
    }
}
    