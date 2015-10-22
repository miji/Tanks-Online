using UnityEngine;
using UnityEditor;

/// <summary>
/// AssetPostprocessor for map snapshots, makes sure the snapshot is imported as Sprite and put in the right Atlas
/// </summary>
public class MMapSnapshotPostProcessor : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        if (assetPath.Contains(MMapSnapshot.FolderPath))
        {
            TextureImporter importer = assetImporter as TextureImporter;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePackingTag = "ABMM";
            importer.spritePixelsPerUnit = 100;
            importer.mipmapEnabled = false;
            importer.filterMode = FilterMode.Trilinear;
            importer.npotScale = TextureImporterNPOTScale.None;

            Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
            if (asset)
            {
                EditorUtility.SetDirty(asset);
            }
        }
    }
}