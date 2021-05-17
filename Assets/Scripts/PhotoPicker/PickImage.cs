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

        private void OnSelectFromGalleryClick()
        {
            _androidBridge.CaptureImage(OnImageCaptured, AndroidBridge.CaptureMethod.FromGallery);
        }
        
        private void OnTakePhoto()
        {
            _androidBridge.CaptureImage(OnImageCaptured, AndroidBridge.CaptureMethod.FromCamera);
        }

        private void OnImageCaptured(string path, int orientation)
        {
            
        }
    }
}