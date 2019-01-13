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
        [MenuItem("BlueToolKit/导出模型/将选中模型分别导出（子物体会拆分导出）")]
        private static void ExportAllChild()
        {
            if (!ExportFile.CreateExportFolder())
                return;

            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportCount = 0;

            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    exportCount++;
                    ExportFile.ExportObj((MeshFilter)meshfilter[m], ExportFile.EXPORT_FOLDER, selection[i].name + "_" + i + "_" + m);
                }
            }

            if (exportCount > 0)
                EditorUtility.DisplayDialog("导出成功", "成功导出 " + exportCount + " 个模型", "关闭");
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }

        [MenuItem("BlueToolKit/导出模型/将选中模型导出成一个obj文件")]
        private static void ExportToSingleObj()
        {
            if (!ExportFile.CreateExportFolder())
                return;


            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportCount = 0;

            ArrayList mfList = new ArrayList();

            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    exportCount++;
                    mfList.Add(meshfilter[m]);
                }
            }

            if (exportCount > 0)
            {
                MeshFilter[] mf = new MeshFilter[mfList.Count];

                for (int i = 0; i < mfList.Count; i++)
                {
                    mf[i] = (MeshFilter)mfList[i];
                }

                string filename = SceneManager.GetActiveScene() + "_" + exportCount;

                int stripIndex = filename.LastIndexOf('/'); //FIXME: Should be Path.PathSeparator

                if (stripIndex >= 0)
                    filename = filename.Substring(stripIndex + 1).Trim();

                ExportFile.ExportObjs(mf, ExportFile.EXPORT_FOLDER, filename);


                EditorUtility.DisplayDialog("导出成功", "导出模型名称：" + filename, "关闭");
            }
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }

        [MenuItem("BlueToolKit/导出模型/将选中模型分别导出（子物体不拆分导出）")]
        private static void ExportParent()
        {
            if (!ExportFile.CreateExportFolder())
                return;

            Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

            if (selection.Length == 0)
            {
                EditorUtility.DisplayDialog("未选中模型", "请选中一个或多个模型", "关闭");
                return;
            }

            int exportCount = 0;


            for (int i = 0; i < selection.Length; i++)
            {
                Component[] meshfilter = selection[i].GetComponentsInChildren(typeof(MeshFilter));

                MeshFilter[] mf = new MeshFilter[meshfilter.Length];

                for (int m = 0; m < meshfilter.Length; m++)
                {
                    mf[m] = (MeshFilter)meshfilter[m];
                }

                exportCount++;

                ExportFile.ExportObjs(mf, ExportFile.EXPORT_FOLDER, selection[i].name + "_" + i);
            }

            if (exportCount > 0)
            {
                EditorUtility.DisplayDialog("导出成功", "成功导出 " + exportCount + " 个模型", "关闭");
            }
            else
                EditorUtility.DisplayDialog("导出失败", "导出模型必须含有Mesh Filter组件", "关闭");
        }
    }
}