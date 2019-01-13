using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace BlueToolkit
{
    public static class ExportFile     
    {
        /// <summary>
        /// 导出文件夹名称 路径为工程根目录
        /// </summary>
        public const string EXPORT_FOLDER = "ExportedObj";

        //保存材质
        private static void ExportMaterials(Dictionary<string, MaterialData> materialList, string folder, string filename)
        {
            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".mtl"))
            {
                foreach (KeyValuePair<string, MaterialData> kvp in materialList)
                {
                    sw.Write("\n");
                    sw.Write("newmtl {0}\n", kvp.Key);
                    sw.Write("Ka  0.6 0.6 0.6\n");
                    sw.Write("Kd  0.6 0.6 0.6\n");
                    sw.Write("Ks  0.9 0.9 0.9\n");
                    sw.Write("d  1.0\n");
                    sw.Write("Ns  0.0\n");
                    sw.Write("illum 2\n");

                    if (kvp.Value.TextureName != null)
                    {
                        string destinationFile = kvp.Value.TextureName;


                        int stripIndex = destinationFile.LastIndexOf('/');

                        if (stripIndex >= 0)
                            destinationFile = destinationFile.Substring(stripIndex + 1).Trim();


                        string relativeFile = destinationFile;

                        destinationFile = folder + "/" + destinationFile;

                        try
                        {
                            File.Copy(kvp.Value.TextureName, destinationFile);
                        }
                        catch
                        {

                        }


                        sw.Write("map_Kd {0}", relativeFile);
                    }

                    sw.Write("\n\n\n");
                }
            }
        }
        /// <summary>
        /// 导出单个模型为一个文件
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        public static void ExportObjToOne(MeshFilter mf, string folder, string filename)
        {
            MeshData data = new MeshData();
            data.SaveData(mf);

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                sw.Write(data.ToString());
            }

            ExportMaterials(data.GetMaterialDic(), folder, filename);
        }
        /// <summary>
        /// 导出多个模型为一个文件
        /// </summary>
        /// <param name="mf"></param>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        public static void ExportObjsToOne(MeshFilter[] mf, string folder, string filename)
        {
            MeshData data = new MeshData();

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                foreach (MeshFilter mesh in mf)
                {
                    data.SaveData(mesh);
                    sw.Write(data.ToString());
                }
            }

            ExportMaterials(data.GetMaterialDic(), folder, filename);
        }
        /// <summary>
        /// 创建导出目录
        /// </summary>
        /// <returns></returns>
        public static bool CreateExportFolder()
        {
            try
            {
                System.IO.Directory.CreateDirectory(EXPORT_FOLDER);
            }
            catch
            {
                EditorUtility.DisplayDialog("错误", "创建导出文件夹失败", "关闭");
                return false;
            }

            return true;
        }
    }
}
