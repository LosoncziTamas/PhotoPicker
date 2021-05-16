package com.photopicker.nativeWrapper;

import android.app.Activity;

class AndroidNative {
    
    public static void takePhoto(Activity activity, CaptureImagePlugin.CaptureImageCallback callback) {
         CaptureImagePlugin.setMainActivity(activity);
         CaptureImagePlugin.captureImage(callback);
    }
    
    public static void selectFromGallery() {
    
    }
}