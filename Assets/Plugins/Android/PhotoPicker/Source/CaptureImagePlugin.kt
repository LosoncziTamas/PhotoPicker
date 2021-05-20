package com.photopicker.nativeWrapper

import android.app.Activity
import android.content.Intent
import android.util.Log

class CaptureImagePlugin {

    companion object {
        @JvmStatic
        var mainActivity: Activity? = null

        const val FROM_GALLERY = 1
        const val FROM_CAMERA = 2

        @JvmStatic
        fun captureImage(callback: ImageCallback, captureMethod: Int) {
            if (captureMethod != FROM_GALLERY && captureMethod != FROM_CAMERA) {
                Log.e("CaptureImagePlugin", "Invalid capture method $captureMethod")
                OnResultCallback.imageCaptureCallback = null
                return
            }
            mainActivity?.runOnUiThread {
                mainActivity?.let {
                    val photoIntent = OnResultCallback.createImageCaptureIntent(it, captureMethod)
                    OnResultCallback.imageCaptureCallback = callback
                    it.startActivity(photoIntent)
                }
            }
        }
    }

    interface ImageCallback {
        fun onImageCaptured(path: String?, orientation: Int)
    }
}