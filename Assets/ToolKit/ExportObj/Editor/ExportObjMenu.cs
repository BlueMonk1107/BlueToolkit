using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine.SceneManagement;

namespace BlueToolkit
{
    public class ExportObjMenu : ScriptableObject
    {
        //导出文件夹名称 路径为工程根目录
        private const string EXPORT_FOLDER = "ExportedObj";


        [MenuItem("BlueToolKit/导出模型/将选中模型分别导出（子物体会拆分导出）")]
        private static void ExportAllChild()
        {
            if (!CreateExportFolder())
                return;

            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportedObjects = 0;

            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    exportedObjects++;
                    MeshToFile((MeshFilter)meshfilter[m], EXPORT_FOLDER, selection[i].name + "_" + i + "_" + m);
                }
            }

            if (exportedObjects > 0)
                EditorUtility.DisplayDialog("导出成功", "成功导出 " + exportedObjects + " 个模型", "关闭");
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }

        [MenuItem("BlueToolKit/导出模型/将选中模型导出成一个obj文件")]
        private static void ExportToSingleObj()
        {
            if (!CreateExportFolder())
                return;


            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportedObjects = 0;

            ArrayList mfList = new ArrayList();

            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    exportedObjects++;
                    mfList.Add(meshfilter[m]);
                }
            }

            if (exportedObjects > 0)
            {
                MeshFilter[] mf = new MeshFilter[mfList.Count];

                for (int i = 0; i < mfList.Count; i++)
                {
                    mf[i] = (MeshFilter)mfList[i];
                }

                string filename = SceneManager.GetActiveScene() + "_" + exportedObjects;

                int stripIndex = filename.LastIndexOf('/'); //FIXME: Should be Path.PathSeparator

                if (stripIndex >= 0)
                    filename = filename.Substring(stripIndex + 1).Trim();

                MeshesToFile(mf, EXPORT_FOLDER, filename);


                EditorUtility.DisplayDialog("导出成功", "导出模型名称：" + filename, "关闭");
            }
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }

        [MenuItem("BlueToolKit/导出模型/将选中模型分别导出（子物体不拆分导出）")]
        private static void ExportParent()
        {
            if (!CreateExportFolder())
                return;

            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportedObjects = 0;


            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                MeshFilter[] mf = new MeshFilter[meshfilter.Length];

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    mf[m] = (MeshFilter)meshfilter[m];
                }

                exportedObjects++;

                MeshesToFile(mf, EXPORT_FOLDER, selection[i].name + "_" + i);
            }

            if (exportedObjects > 0)
            {
                EditorUtility.DisplayDialog("导出成功", "成功导出 " + exportedObjects + " 个模型", "关闭");
            }
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }

        //保存材质
        private static void MaterialsToFile(Dictionary<string, MaterialData> materialList, string folder, string filename)
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
        //保存单个模型
        private static void MeshToFile(MeshFilter mf, string folder, string filename)
        {
            Dictionary<string, MaterialData> materialList = new Dictionary<string, MaterialData>();
            MeshData data = new MeshData(mf, materialList);

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                sw.Write(data.ToString());
            }

            MaterialsToFile(materialList, folder, filename);
        }
        //保存多个模型
        private static void MeshesToFile(MeshFilter[] mf, string folder, string filename)
        {
            Dictionary<string, MaterialData> materialList = new Dictionary<string, MaterialData>();

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                foreach (MeshFilter mesh in mf)
                {
                    MeshData data = new MeshData(mesh, materialList);
                    sw.Write(data.ToString());
                }
            }

            MaterialsToFile(materialList, folder, filename);
        }
        //创建导出目录
        private static bool CreateExportFolder()
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