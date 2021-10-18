using System;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace CSSCSolidWokrsLogin
{
    public class Program
    {

        public static ISldWorks SwApp { get; private set; }

        public static ISldWorks ConnectToSolidWorks()
        {
            if (SwApp != null)
            {
                return SwApp;
            }
            else
            {
                //Process pr = new Process();
                //pr.StartInfo.FileName = "C:\\Program Files\\SOLIDWORKS Corp\\SOLIDWORKS\\SLDWORKS.exe";
                //pr.Start();

                try
                {
                    SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application");
                }
                catch (COMException)
                {
                    try
                    {
                        SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application.23");//2015
                    }
                    catch (COMException)
                    {
                        try
                        {
                            SwApp = (SldWorks)Marshal.GetActiveObject("SldWorks.Application.26");//2018
                        }
                        catch (COMException)
                        {
                            MessageBox.Show("Could not connect to SolidWorks.", "SolidWorks", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            SwApp = null;
                        }
                    }
                }

                return SwApp;
            }
        }

        /// <summary>
        /// 遍历特征
        /// </summary>
        /// <param name="thisFeat"></param>
        /// <param name="isTopLevel"></param>
        public static void TraverseFeatures(Feature thisFeat, bool isTopLevel, bool isShowDimension = false)
        {
            Feature curFeat = default(Feature);
            curFeat = thisFeat;

            while ((curFeat != null))
            {
                //输出特征名称
                Debug.Print(curFeat.Name);
                if (isShowDimension == true) ShowDimensionForFeature(curFeat);

                Feature subfeat = default(Feature);
                subfeat = (Feature)curFeat.GetFirstSubFeature();

                while ((subfeat != null))
                {
                    //if (isShowDimension == true) ShowDimensionForFeature(subfeat);
                    TraverseFeatures(subfeat, false);
                    Feature nextSubFeat = default(Feature);
                    nextSubFeat = (Feature)subfeat.GetNextSubFeature();
                    subfeat = nextSubFeat;
                    nextSubFeat = null;
                }

                subfeat = null;

                Feature nextFeat = default(Feature);

                if (isTopLevel)
                {
                    nextFeat = (Feature)curFeat.GetNextFeature();
                }
                else
                {
                    nextFeat = null;
                }

                curFeat = nextFeat;
                nextFeat = null;
            }
        }

        /// <summary>
        /// 遍历零件中的所有特征
        /// </summary>
        /// <param name="feature"></param>
        public static void ShowDimensionForFeature(Feature feature)
        {
            var thisDisplayDim = (DisplayDimension)feature.GetFirstDisplayDimension();

            while (thisDisplayDim != null)
            {
                var dimen = (Dimension)thisDisplayDim.GetDimension();

                Debug.Print($"---特征 {feature.Name} 尺寸-->" + dimen.GetNameForSelection() + "-->" + dimen.Value);

                thisDisplayDim = (DisplayDimension)feature.GetNextDisplayDimension(thisDisplayDim);
            }
        }

        /// <summary>
        /// 遍历装配体零件
        /// </summary>
        /// <param name="swComp"></param>
        /// <param name="nLevel"></param>
        public static void TraverseCompXform(Component2 swComp, long nLevel, bool setcolor = false)
        {
            object[] vChild;
            Component2 swChildComp;
            string sPadStr = "";
            MathTransform swCompXform;
            //  object vXform;
            long i;

            for (i = 0; i < nLevel; i++)
            {
                sPadStr = sPadStr + "  ";
            }
            swCompXform = swComp.Transform2;
            if (swCompXform != null)
            {
                ModelDoc2 swModel;
                swModel = (ModelDoc2)swComp.GetModelDoc2();

                try
                {
                    //子零件文件名
                    //Debug.Print(sPadStr + swComp.Name2);

                    if (swComp.GetSelectByIDString() == "")
                    {
                        //选择id
                        //Debug.Print(swComp.GetSelectByIDString());
                    }
                    else
                    {
                    }
                }
                catch
                {
                }
                if (swModel != null)
                {
                    Debug.Print("Loading:" + swComp.Name2);
                    //获取零件的一些信息，如属性，名字路径。
                    string tempPartNum = swModel.get_CustomInfo2(swComp.ReferencedConfiguration, "Project");
                    string tempName2 = swComp.Name2;
                    string tempName = swModel.GetPathName();
                    string tempConfigName = swComp.ReferencedConfiguration;
                    string tempComponentRef = swComp.ComponentReference;

                    //如果要设定颜色
                    if (setcolor == true)
                    {
                        double[] matPropVals = (double[])swModel.MaterialPropertyValues;
                        var tempC = GetRadomColor(System.IO.Path.GetFileNameWithoutExtension(swModel.GetPathName()));
                        matPropVals[0] = Convert.ToDouble(tempC.R) / 255;
                        matPropVals[1] = Convert.ToDouble(tempC.G) / 255;
                        matPropVals[2] = Convert.ToDouble(tempC.B) / 255;
                        swModel.MaterialPropertyValues = matPropVals;

                        swModel.WindowRedraw();
                    }
                }
            }
            else
            {
                ModelDoc2 swModel;
                swModel = (ModelDoc2)swComp.GetModelDoc2();
            }

            vChild = (object[])swComp.GetChildren();
            for (i = 0; i <= (vChild.Length - 1); i++)
            {
                swChildComp = (Component2)vChild[i];
                TraverseCompXform(swChildComp, nLevel + 1, setcolor);
            }
        }

        public static System.Drawing.Color GetRadomColor(string name)
        {
            Random rnd = new Random();

            //这里可以根据需要指定颜色。
            if (name.Contains("m1"))
            {
                return System.Drawing.Color.Red;
            }

            return System.Drawing.Color.FromArgb(
                 rnd.Next(0, 255), /*红色*/
                 rnd.Next(0, 255), /*绿色*/
                 rnd.Next(0, 255)  /*蓝色*/ );
        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}


