package com.photopicker.nativeWrapper

import android.app.Activity
import android.content.Intent
import android.media.ExifInterface
import android.os.Bundle
import android.util.Log
import pl.aprilapps.easyphotopicker.DefaultCallback
import pl.aprilapps.easyphotopicker.EasyImage
import pl.aprilapps.easyphotopicker.MediaFile
import pl.aprilapps.easyphotopicker.MediaSource

class OnResultCallback : Activity() {

    companion object {
        @JvmStatic
        var imageCaptureCallback: CaptureImagePlugin.ImageCallback? = null

        private const val CAPTURE_IMAGE_ARG = "image_arg"

        fun createImageCaptureIntent(activity: Activity, captureMethod: Int) = Intent().apply {
            setClass(activity, OnResultCallback::class.java)
            putExtra(CAPTURE_IMAGE_ARG, captureMethod)
        }
    }

    private val easyImage: EasyImage = EasyImage.Builder(this)
            .setCopyImagesToPublicGalleryFolder(true)
            .allowMultiple(false)
            .build()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        if (intent == null) {
            finishSelf(null, -1)
        } else {
            when (val captureArg = intent.getIntExtra(CAPTURE_IMAGE_ARG, 0)){
                CaptureImagePlugin.FROM_CAMERA -> {
                    easyImage.openCameraForImage(this)
                }
                CaptureImagePlugin.FROM_GALLERY -> {
                    easyImage.openGallery(this)
                }
                else -> {
                    Log.e("OnResultCallback", "Invalid intent args $captureArg")
                    finishSelf(null, -1)
                }
            }
        }
    }

    fun finishSelf(path: String?, orientation: Int) {
        if (path != null) {
            imageCaptureCallback?.onImageCaptured(path, orientation)
        } else {
            imageCaptureCallback?.onImageCaptured("", -1)
        }
        CaptureImagePlugin.mainActivity = null
        imageCaptureCallback = null
        finish()
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        Log.d("onActivityResult", "requestCode $requestCode resultCode $resultCode ")

        easyImage.handleActivityResult(requestCode, resultCode, data, this, object : DefaultCallback() {

            override fun onMediaFilesPicked(imageFiles: Array<MediaFile>, source: MediaSource) {
                if (imageFiles.isNotEmpty()) {
                    val path = imageFiles[0].file.path
                    finishSelf(path, getOrientation(path))
                } else {
                    finishSelf(null, -1)
                    Log.e("onMediaFilesPicked", "No image files returned")
                }
            }

            override fun onImagePickerError(error: Throwable, source: MediaSource) {
                finishSelf(null, -1)
                Log.e("onImagePickerError", "Unknown picker error")
            }

            override fun onCanceled(source: MediaSource) {
                finishSelf(null, -1)
                Log.e("onImagePickerError", "onCanceled")
            }
        })
    }

    private fun getOrientation(path: String) = ExifInterface(path).getAttributeInt("Orientation", 1)
}