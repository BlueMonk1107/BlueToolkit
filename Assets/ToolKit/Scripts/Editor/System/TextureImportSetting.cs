//=======================================================
// 作者：BlueMonk
// 描述：A set of tools designed to increase the efficiency of unity development. 
//=======================================================
using UnityEngine;
using UnityEditor;

namespace BlueToolkit
{
    /// <summary>
    /// 导入图片时自动设置图片的参数
    /// </summary>
    public class TextureImportSetting : AssetPostprocessor
    {

        /// <summary>
        /// 图片导入之前调用
        /// </summary>
        void OnPreprocessTexture()
        {
            TextureImporter texture = (TextureImporter) assetImporter;
            texture.textureType = TextureImporterType.Sprite;
            texture.mipmapEnabled = false;

            Debug.Log("已自动设置图片格式");
        }

        /// <summary>
        /// 图片已经被压缩、保存到指定目录下之后调用
        /// </summary>
        /// <param name="texture"></param>
        void OnPostprocessTexure(Texture2D texture)
        {
        }

        /// <summary>
        /// 所有资源被导入、删除、移动完成之后调用
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            //foreach (string asset in importedAssets)
            //{
            //}
            //foreach (string asset in deletedAssets)
            //{
            //}

            //for (int i = 0; i < movedAssets.Length; i++)
            //{
            //}
        }
    }
}