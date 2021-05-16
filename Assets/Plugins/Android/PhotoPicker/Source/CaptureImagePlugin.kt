package com.photopicker.nativeWrapper

import android.app.Activity
import android.content.Intent

class CaptureImagePlugin {

    companion object {
        @JvmStatic
        var mainActivity: Activity? = null

        @JvmStatic
        fun captureImage(callback: CaptureImageCallback) {
            mainActivity?.runOnUiThread {
                mainActivity?.let {
                    val photoIntent = Intent().apply { setClass(it, OnResultCallback::class.java) }
                    OnResultCallback.callback = callback
                    it.startActivity(photoIntent)
                }
            }
        }
    }

    interface CaptureImageCallback {
        fun onImageCaptured(path: String?, orientation: Int)
    }

}