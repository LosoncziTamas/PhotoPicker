package com.photopicker.nativeWrapper;

import android.app.Activity;

class AndroidNative {
    
    public static void captureImage(Activity activity, CaptureImagePlugin.ImageCallback callback, int captureMethod) {
        CaptureImagePlugin.setMainActivity(activity);
        CaptureImagePlugin.captureImage(callback, captureMethod);
    }
}