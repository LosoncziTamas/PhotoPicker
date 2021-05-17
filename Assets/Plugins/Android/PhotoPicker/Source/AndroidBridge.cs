using System;
using UnityEngine;

namespace Plugins.Android.PhotoPicker.Source
{
    public class AndroidBridge : MonoBehaviour
    {
        public enum CaptureMethod
        {
            FromGallery = 1,
            FromCamera = 2,
        }
        
        private const string WrapperName = "com.photopicker.nativeWrappper.AndroidNative";
        private const string CaptureImagePluginName = "com.photopicker.photopicker.nativeWrappper.CaptureImagePlugin";
    
        private AndroidJavaClass _androidNative;
    
        private AndroidJavaClass AndroidNative
        {
            get
            {
                return _androidNative ??= new AndroidJavaClass(WrapperName);
            }
        }
    
        private AndroidJavaObject _currentActivityContext;
    
        private AndroidJavaObject CurrentActivityContext
        {
            get
            {
                if (_currentActivityContext == null)
                {
                    var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _currentActivityContext = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }

                return _currentActivityContext;
            }
        }

        class CaptureImageCallback: AndroidJavaProxy
        {
            private Action<string, int> captureHandler;
            
            public CaptureImageCallback(Action<string, int> captureHandlerIn) : base ($"{CaptureImagePluginName}$ImageCallback")
            {
                captureHandler = captureHandlerIn;
            }

            public void onImageCaptured(string path, int orientation)
            {
                captureHandler?.Invoke(path, orientation);
            }
        }

        public void CaptureImage(Action<string, int> onCaptureImage, CaptureMethod captureMethod)
        {
            Debug.Log($"[AndroidBridge] CaptureImage");
            var captureMethodInt = (int) captureMethod;
            AndroidNative.CallStatic("captureImage", CurrentActivityContext, new CaptureImageCallback(onCaptureImage), captureMethodInt);
        }
    }
}
