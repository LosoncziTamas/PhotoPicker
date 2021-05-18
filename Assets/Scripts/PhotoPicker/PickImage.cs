using System;
using System.IO;
using System.Threading.Tasks;
using Plugins.Android.PhotoPicker.Source;
using UnityEngine;
using UnityEngine.UI;

namespace PhotoPicker
{
    public class PickImage : MonoBehaviour
    {
        [SerializeField] private Button _selectFromGalleryButton;
        [SerializeField] private Button _takePhotoButton;
        [SerializeField] private AndroidBridge _androidBridge;
        [SerializeField] private Image _image;

        private void OnEnable()
        {
            _selectFromGalleryButton.onClick.AddListener(OnSelectFromGalleryClick);
            _takePhotoButton.onClick.AddListener(OnTakePhoto);
        }
        
        private void OnDisable()
        {
            _selectFromGalleryButton.onClick.RemoveListener(OnSelectFromGalleryClick);
            _takePhotoButton.onClick.RemoveListener(OnTakePhoto);
        }

        private async void OnSelectFromGalleryClick()
        {
            var (success, result) = await CaptureImage(AndroidBridge.CaptureMethod.FromGallery);
            if (success)
            {
                _image.material.mainTexture = result;
            }
        }
        
        private async void OnTakePhoto()
        {
            var (success, result) = await CaptureImage(AndroidBridge.CaptureMethod.FromCamera);
            if (success)
            {
                _image.material.mainTexture = result;
            }        
        }

        private Task<Tuple<bool, Texture2D>> CaptureImage(AndroidBridge.CaptureMethod captureMethod)
        {
            var result = new TaskCompletionSource<Tuple<bool, Texture2D>>();

            _androidBridge.CaptureImage(async (path, orientation) =>
            {
                Debug.Log($"CaptureImage path {path} orientation {orientation}");

                if (path != null && File.Exists(path))
                {
                    var bitmapTexture = await ReadTextureAsync(path);
                    var rotation = TextureUtils.GetRotationByExifOrientation(orientation);
                    if (rotation == TextureRotation.Default)
                    {
                        result.SetResult(Tuple.Create(true, bitmapTexture));
                    }
                    else
                    {
                        var rotated = TextureUtils.RotateTexture(bitmapTexture, rotation);
                        Destroy(bitmapTexture);
                        result.SetResult(Tuple.Create(true, rotated));
                    }
                }
                else
                {
                    result.SetResult(Tuple.Create<bool, Texture2D>(false, default));
                }
            }, captureMethod);

            return result.Task;
        }
        
        private static Task<Texture2D> ReadTextureAsync(string path)
        {
            var imageData = File.ReadAllBytes(path);
            var bitmapTexture = new Texture2D(1, 1);
            bitmapTexture.LoadImage(imageData);
            return Task.FromResult(bitmapTexture);
        }
    }
}