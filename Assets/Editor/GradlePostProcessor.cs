// Script from Cross Platform Native Plugins - http://u3d.as/bV0

#if UNITY_2018_2_OR_NEWER
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor.Android;
using UnityEngine;

namespace Editor
{
    class GradlePostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            EnableJetifierIfRequired(path);
        }
        private void EnableJetifierIfRequired(string path)
        {
            string[] files = Directory.GetFiles(Application.dataPath + "/Plugins/Android" , "androidx.*.aar");
 
            if(files.Length > 0)
            {
                string gradlePropertiesPath = path + "/gradle.properties";
 
                string[] lines = File.ReadAllLines(gradlePropertiesPath);
 
                // Need jetifier patch process
                bool hasAndroidXProperty = lines.Any(text => text.Contains("android.useAndroidX"));
                bool hasJetifierProperty = lines.Any(text => text.Contains("android.enableJetifier"));
 
                StringBuilder builder = new StringBuilder();
 
                foreach(string each in lines)
                {
                    builder.AppendLine(each);
                }
 
                if (!hasAndroidXProperty)
                {
                    builder.AppendLine("android.useAndroidX=true");
                }
 
                if (!hasJetifierProperty)
                {
                    builder.AppendLine("android.enableJetifier=true");
                }
 
                File.WriteAllText(gradlePropertiesPath, builder.ToString());
            }
        }
    }
}
 
#endif