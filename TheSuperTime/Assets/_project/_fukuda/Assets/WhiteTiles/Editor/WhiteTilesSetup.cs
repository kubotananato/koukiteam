using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace WhiteTilesPackage
{
    [InitializeOnLoad]
    public static class WhiteTilesSetup
    {
        const string Root = "Assets/WhiteTiles";
        const string BasePath = Root + "/Textures/WhiteTiles_BaseColor.png";
        const string NormalPath = Root + "/Textures/WhiteTiles_Normal.png";
        const string MaterialPath = Root + "/WhiteTiles.mat";

        static WhiteTilesSetup()
        {
            EditorApplication.delayCall += () =>
            {
                if (!EditorApplication.isPlayingOrWillChangePlaymode &&
                    File.Exists(BasePath) && !File.Exists(MaterialPath)) Create();
            };
        }

        [MenuItem("Tools/White Tiles/Create Material")]
        public static void Create()
        {
            if (!File.Exists(BasePath))
            {
                Debug.LogError("Place the WhiteTiles folder inside Assets first.");
                return;
            }
            var pipeline = GraphicsSettings.currentRenderPipeline;
            bool urp = pipeline != null && pipeline.GetType().Name.Contains("Universal");
            if (pipeline != null && !urp)
            {
                Debug.LogError("White Tiles supports Built-in and URP. For HDRP, use the base color texture in an HDRP/Lit material.");
                return;
            }
            Shader shader = Shader.Find(urp ? "Universal Render Pipeline/Lit" : "Standard");
            if (shader == null)
            {
                Debug.LogError("The required Lit shader could not be found.");
                return;
            }
            AssetDatabase.ImportAsset(BasePath, ImportAssetOptions.ForceSynchronousImport);
            ConfigureTexture(BasePath, false);
            if (!File.Exists(NormalPath))
            {
                File.Copy(BasePath, NormalPath);
                AssetDatabase.ImportAsset(NormalPath, ImportAssetOptions.ForceSynchronousImport);
            }
            ConfigureTexture(NormalPath, true);
            var material = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            bool isNew = material == null;
            if (isNew) material = new Material(shader);
            else Undo.RecordObject(material, "Configure White Tiles");
            material.shader = shader;
            material.name = "WhiteTiles";
            material.SetTexture(urp ? "_BaseMap" : "_MainTex", AssetDatabase.LoadAssetAtPath<Texture2D>(BasePath));
            material.SetColor(urp ? "_BaseColor" : "_Color", Color.white);
            material.SetFloat("_Metallic", 0f);
            material.SetFloat(urp ? "_Smoothness" : "_Glossiness", 0.55f);
            material.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(NormalPath));
            material.SetFloat("_BumpScale", 0.35f);
            material.EnableKeyword("_NORMALMAP");
            material.enableInstancing = true;
            if (isNew) AssetDatabase.CreateAsset(material, MaterialPath);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            Debug.Log("White Tiles material ready: " + MaterialPath);
        }

        static void ConfigureTexture(string path, bool normal)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) throw new IOException("Texture import failed: " + path);
            importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
            importer.sRGBTexture = !normal;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Trilinear;
            importer.mipmapEnabled = true;
            importer.anisoLevel = 8;
            importer.maxTextureSize = 2048;
            importer.npotScale = TextureImporterNPOTScale.ToNearest;
            if (normal)
            {
                importer.convertToNormalmap = true;
                importer.heightmapScale = 0.025f;
                importer.normalmapFilter = TextureImporterNormalFilter.Sobel;
            }
            importer.SaveAndReimport();
        }
    }
}
