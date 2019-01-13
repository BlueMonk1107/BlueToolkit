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
    struct ObjMaterial
    {
        public string name;
        public string textureName;
    }

    public class EditorObjExporter : ScriptableObject
    {
        private static int _vertexOffset = 0;
        private static int _normalOffset = 0;
        private static int _uvOffset = 0;
        //导出文件夹名称 路径为工程根目录
        private const string EXPORT_FOLDER = "ExportedObj";

        //把网格数据保存成文本信息
        private static string MeshToString(MeshFilter meshFilter, Dictionary<string, ObjMaterial> materialDic)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Material[] materialArray = meshFilter.GetComponent<Renderer>().sharedMaterials;

            StringBuilder data = new StringBuilder();

            //保存网格名称
            data.Append("g ").Append(meshFilter.name).Append("\n");

            //保存顶点数据
            foreach (Vector3 ver in mesh.vertices)
            {
                Vector3 worldPos = meshFilter.transform.TransformPoint(ver);

                //因为坐标系的区别，x分量需要反转
                data.Append(string.Format("v {0} {1} {2}\n", -worldPos.x, worldPos.y, worldPos.z));
            }
            data.Append("\n");

            //保存法线数据
            foreach (Vector3 normal in mesh.normals)
            {
                Vector3 directionWorld = meshFilter.transform.TransformDirection(normal);

                data.Append(string.Format("vn {0} {1} {2}\n", -directionWorld.x, directionWorld.y, directionWorld.z));
            }
            data.Append("\n");

            //保存uv数据
            foreach (Vector3 uv in mesh.uv)
            {
                data.Append(string.Format("vt {0} {1}\n", uv.x, uv.y));
            }

            //保存材质数据
            string materialName = "";
            for (int materialIndex = 0; materialIndex < mesh.subMeshCount; materialIndex++)
            {
                materialName = materialArray[materialIndex].name;
                data.Append("\n");

                data.Append("usemtl ")
                    .Append(materialName)
                    .Append("\n");

                data.Append("usemap ")
                    .Append(materialName)
                    .Append("\n");

                //筛选同名材质，不重复添加
                if (!materialDic.ContainsKey(materialName))
                {
                    ObjMaterial objMaterial = new ObjMaterial();

                    objMaterial.name = materialName;

                    objMaterial.textureName =
                        materialArray[materialIndex].mainTexture ?
                        AssetDatabase.GetAssetPath(materialArray[materialIndex].mainTexture) : null;

                    materialDic[objMaterial.name] = objMaterial;
                }

                //保存三角形数据
                int[] triangles = mesh.GetTriangles(materialIndex);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    data.Append(string.Format("f {1}/{1}/{1} {0}/{0}/{0} {2}/{2}/{2}\n",
                        triangles[i] + 1 + _vertexOffset, triangles[i + 1] + 1 + _normalOffset,
                        triangles[i + 2] + 1 + _uvOffset));
                }
            }

            _vertexOffset += mesh.vertices.Length;
            _normalOffset += mesh.normals.Length;
            _uvOffset += mesh.uv.Length;

            return data.ToString();
        }
        //清空数据
        private static void Clear()
        {
            _vertexOffset = 0;
            _normalOffset = 0;
            _uvOffset = 0;
        }
        //重置数据函数
        private static Dictionary<string, ObjMaterial> ResetData()
        {
            Clear();

            return new Dictionary<string, ObjMaterial>();
        }
        //保存材质
        private static void MaterialsToFile(Dictionary<string, ObjMaterial> materialList, string folder, string filename)
        {
            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".mtl"))
            {
                foreach (KeyValuePair<string, ObjMaterial> kvp in materialList)
                {
                    sw.Write("\n");
                    sw.Write("newmtl {0}\n", kvp.Key);
                    sw.Write("Ka  0.6 0.6 0.6\n");
                    sw.Write("Kd  0.6 0.6 0.6\n");
                    sw.Write("Ks  0.9 0.9 0.9\n");
                    sw.Write("d  1.0\n");
                    sw.Write("Ns  0.0\n");
                    sw.Write("illum 2\n");

                    if (kvp.Value.textureName != null)
                    {
                        string destinationFile = kvp.Value.textureName;


                        int stripIndex = destinationFile.LastIndexOf('/');

                        if (stripIndex >= 0)
                            destinationFile = destinationFile.Substring(stripIndex + 1).Trim();


                        string relativeFile = destinationFile;

                        destinationFile = folder + "/" + destinationFile;

                        try
                        {
                            File.Copy(kvp.Value.textureName, destinationFile);
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
            Dictionary<string, ObjMaterial> materialList = ResetData();

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                sw.Write(MeshToString(mf, materialList));
            }

            MaterialsToFile(materialList, folder, filename);
        }
        //保存多个模型
        private static void MeshesToFile(MeshFilter[] mf, string folder, string filename)
        {
            Dictionary<string, ObjMaterial> materialList = ResetData();

            using (StreamWriter sw = new StreamWriter(folder + "/" + filename + ".obj"))
            {
                sw.Write("mtllib ./" + filename + ".mtl\n");

                foreach (MeshFilter mesh in mf)
                {
                    sw.Write(MeshToString(mesh, materialList));
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

    }
}