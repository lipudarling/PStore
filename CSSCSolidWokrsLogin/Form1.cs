using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using SolidWorks.Interop.swcommands;
using View = SolidWorks.Interop.sldworks.View;
using SolidWorks.Interop.swdocumentmgr;
using Microsoft.VisualBasic;
using System.Linq;
using Attribute = SolidWorks.Interop.sldworks.Attribute;

namespace CSSCSolidWokrsLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //1.连接SolidWorks
        private void Btn_Connect_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();

            if (swApp != null)
            {
                string msg = "This message from C#. solidworks version is " + swApp.RevisionNumber();
                //发一个消息给solidworks用户
                swApp.SendMsgToUser(msg);
            }


        }
        //2.打开和创建
        private void Btn_OpenAndCreate_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();
            if (swApp != null)
            {
                //通过GetDocumentTemplate 获取默认模板的路径 ,第一个参数可以指定类型
                string partDefaultTemplate = swApp.GetDocumentTemplate((int)swDocumentTypes_e.swDocPART, "", 0, 0, 0);
                //也可以直接指定slddot asmdot drwdot
                //partDefaultTemplate = @"xxx\..prtdot";
                var newDoc = swApp.NewDocument(partDefaultTemplate, 0, 0, 0);

                if (newDoc != null)
                {
                    swApp.SendMsgToUser("Creating Done!");
                    //下面获取当前文件
                    ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                    //选择对应的草图基准面
                    bool boolstatus = swModel.Extension.SelectByID2("Plane1", "PLANE", 0, 0, 0, false, 0, null, 0);

                    //创建一个2d草图
                    swModel.SketchManager.InsertSketch(true);

                    //画一条线 长度100mm  (solidworks 中系统单位是米,所以这里写0.1)
                    swModel.SketchManager.CreateLine(0, 0, 0, 0, 0.1, 0);

                    //关闭草图
                    swModel.SketchManager.InsertSketch(true);

                    //设定保存文件的完整路径
                    string myNewPartPath = @"E:\myNewPart.SLDPRT";

                    //保存零件.
                    int longstatus = swModel.SaveAs3(myNewPartPath, 0, 1);

                    //关闭零件
                    //swApp.CloseDoc(myNewPartPath);
                    //swApp.SendMsgToUser("Closed");
                    ////重新打开零件.
                    //swApp.OpenDoc(myNewPartPath, (int)swDocumentTypes_e.swDocPART);

                    //swApp.SendMsgToUser("Open completed.");
                }
            }
        }
        //3.读取零件属性
        private void Btn_GetPartAttribute_Click(object sender, EventArgs s)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();
            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc; //当前零件

                if (swModel != null)
                {
                    string PathOfModel = swModel.GetPathName();
                    string TitleOfModel = swModel.GetTitle();
                    bool boolstatus;
                    //获取通用属性值
                    string date_value = swModel.GetCustomInfoValue("", "Project");
                    string name_value = swModel.GetCustomInfoValue("", "Qty");
                    string code_value = swModel.GetCustomInfoValue("", "Description");

                    //修改通用属性值
                    swModel.set_CustomInfo2("", "Project", "project_value");

                    //新增通用属性
                    boolstatus = swModel.AddCustomInfo3("", "NewAttr1", 30, "NewAttr1_value");
                    //boolstatus = swModel.AddCustomInfo3("Default", "PartCode", 30, "DC0025");




                    //boolstatus = swModel.DeleteCustomInfo2("", "Qty"); //删除指定属性项
                    //boolstatus = swModel.DeleteCustomInfo2("", "Project"); //删除指定属性项
                    //boolstatus = swModel.AddCustomInfo3("", "Project", 30, "1"); //增加通用属性值

                    var ConfigNames = (string[])swModel.GetConfigurationNames(); //所有配置名称

                    Configuration swConfig = null;

                    foreach (var configName in ConfigNames)//遍历所有配置
                    {
                        swConfig = (Configuration)swModel.GetConfigurationByName(configName);

                        var manger = swModel.Extension.CustomPropertyManager[configName];
                        //删除当前配置中的属性
                        manger.Delete2("Code");
                        //增加一个属性到配置
                        manger.Add3("Code", (int)swCustomInfoType_e.swCustomInfoText, "A-" + configName, (int)swCustomPropertyAddOption_e.swCustomPropertyReplaceValue);
                        //获取此配置中的Code属性
                        string tempCode = manger.Get("Code");
                        //获取此配置中的Description属性

                        var tempDesc = manger.Get("Description");
                        Debug.Print("  Name of configuration  ---> " + configName + " Desc.=" + tempCode);
                    }
                }
                else
                {
                    MessageBox.Show("Please open a part first!");
                }
            }
        }
        //4.修改零件
        private void Btn_ChangeDim_Click(object sender, EventArgs s)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();

            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
                if (swModel != null)
                {
                    string NewConfigName = "NewConfig";
                    bool boolstatus = swModel.AddConfiguration2(NewConfigName, "", "", true, false, false, true, 256);

                    swModel.ShowConfiguration2(NewConfigName);

                    //2.增加特征(选择一条边，加圆角)
                    ModelDocExtension activeDocExtension = swModel.Extension;
                    boolstatus = activeDocExtension.SelectByID2("", "EDGE", 3.75842546947069E-03, 3.66350829162911E-02, 1.23295158888936E-03, false, 1, null, 0);

                    if (boolstatus == false)
                    {
                        ErrorMsg(swApp, "Failed to select edge");
                        return;
                    }
                    FeatureManager activeDocFeature_Manager = swModel.FeatureManager;
                    Feature feature = (Feature)activeDocFeature_Manager.FeatureFillet3(195, 0.000508, 0.01, 0, 0, 0, 0, null, null, null, null, null, null, null);

                    if (feature == null)
                    {
                        ErrorMsg(swApp, "Error:Feature == nullptr!");
                        return;
                    }
                    //3.压缩特征

                    feature.Select(false);

                    swModel.EditSuppress();

                    //4.修改尺寸
                    Dimension dimension = (Dimension)swModel.Parameter("D1@Fillet7");
                    dimension.SystemValue = 0.000254; //0.001英寸

                    swModel.EditRebuild3();

                    //5.删除特征

                    feature.Select(false);
                    swModel.EditDelete();
                }
                else
                {
                    MessageBox.Show("Please open a part first!");
                }
            }
        }
        //5.遍历零件特征
        private void Btn_Traverse_Feature_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();

            //加速读取
            swApp.CommandInProgress = true;

            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                //第一个特征
                Feature swFeat = (Feature)swModel.FirstFeature();

                //遍历
                Program.TraverseFeatures(swFeat, true);
            }
            swApp.CommandInProgress = false;
        }
        //6.遍历装配体
        private void Btn_Traverse_Assembly_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();

            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                Configuration swConf = (Configuration)swModel.GetActiveConfiguration();

                Component2 swRootComp = (Component2)swConf.GetRootComponent();

                //遍历
                Program.TraverseCompXform(swRootComp, 0);
            }
        }
        //7.遍历视图与球标
        private void Btn_Traverse_Drawing_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();

            if (swApp != null)
            {
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                DrawingDoc drawingDoc = (DrawingDoc)swModel;

                //获取当前工程图中的所有图纸名称
                var sheetNames = (object[])drawingDoc.GetSheetNames();

                //遍历并找出包含k3 的工程图名称
                string k3Name = "";
                foreach (var kName in sheetNames)
                {
                    if (((String)kName).Contains("k3"))
                    {
                        k3Name = (String)kName;
                    }
                }
                //切换图纸
                bool bActSheet = drawingDoc.ActivateSheet(k3Name);

                // 获取当前工程图对象
                Sheet drwSheet = default(Sheet);
                drwSheet = (Sheet)drawingDoc.GetCurrentSheet();

                //获取所有的视图
                object[] views = null;
                views = (object[])drwSheet.GetViews();

                foreach (object vView in views)
                {
                    var ss = (View)vView;
                    Debug.Print(ss.GetName2());
                }

                //选中新的视图，移动位置。
                bool boolstatus = swModel.Extension.SelectByID2("Drawing View1", "DRAWINGVIEW", 0, 0, 0, false, 0, null, 0);
                //切换视图方向
                swModel.ShowNamedView2("*Front", (int)swStandardViews_e.swFrontView);
                //修改视图的名称
                swModel.SelectedFeatureProperties(0, 0, 0, 0, 0, 0, 0, true, false, "主视图-1");

                SelectionMgr modelSel = swModel.ISelectionManager;

                //该视图对象
                View actionView = (View)modelSel.GetSelectedObject5(1);

                //位置 actionView.Position

                //获取注释
                var noteCount = actionView.GetNoteCount();

                List<Note> AllNotes = new List<Note>();
                if (noteCount > 0)
                {
                    Note note = (Note)actionView.GetFirstNote();

                    Debug.Print(noteCount.ToString());

                    //这里要判断类型之后 才能转成组件，才能获取 名字
                    //var anno = (Annotation)note.GetAnnotation();

                    //var types= (int[])anno.GetAttachedEntityTypes();

                    //var attOjbect = (object[])anno.GetAttachedEntities3();

                    //var attEntity = (Entity)attOjbect[0];

                    //var attComp = (Component2)(attEntity.GetComponent());

                    //Debug.Print(attComp.Name2);

                    // note.GetBalloonStyle
                    Debug.Print(note.GetText());

                    AllNotes.Add(note);

                    var leaderInfo = note.GetLeaderInfo();

                    for (int k = 0; k < noteCount - 1; k++)
                    {
                        note = (Note)note.GetNext();
                        Debug.Print(note.GetText());

                        AllNotes.Add(note);
                    }

                    swModel.EditRebuild3();

                    swModel.EditDelete();
                }
            }
        }
        //8.从装配体制作工程图
        private void Btn_MakeDrawingFromAssembly_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            DrawingDoc swDrawing;
            View swView;
            ModelDocExtension swModelDocExt;
            string fileName;
            bool status;
            int errors = 1;
            //int warnings;
            int numViews;
            object[] viewNames;
            //string viewName;
            string viewPaletteName;
            int i;
            //AssemblyDoc swAssembly;
            //PartDoc swPart;

            ISldWorks swApp = Program.ConnectToSolidWorks();

            if (swApp != null)
            {
                swDrawing = (DrawingDoc)swApp.ActiveDoc;

                // Get number of views on View Palette
                numViews = 0;
                viewNames = (object[])swDrawing.GetDrawingPaletteViewNames();

                // Iterate through views on View Palette
                // When view name equals *Current, drop
                // that view in drawing
                if (!((viewNames == null)))
                {
                    numViews = (viewNames.GetUpperBound(0) - viewNames.GetLowerBound(0));
                    for (i = 0; i <= numViews; i++)
                    {
                        viewPaletteName = (string)viewNames[i];
                        if ((viewPaletteName == "*Current"))
                        {
                            swView = (View)swDrawing.DropDrawingViewFromPalette2(viewPaletteName, 0.0, 0.0, 0.0);
                        }
                    }
                }

                // Activate the part document and
                // select two faces for the relative drawing view
                swApp.ActivateDoc3("maingrip.sldprt", false, (int)swRebuildOnActivation_e.swUserDecision, ref errors);
                swModel = (ModelDoc2)swApp.ActiveDoc;
                swModelDocExt = (ModelDocExtension)swModel.Extension;
                swModel.ClearSelection2(true);
                status = swModelDocExt.SelectByID2("", "FACE", 0.0466263268498324, 0.00558799999987514, -0.00617351393179888, false, 1, null, 0);
                status = swModelDocExt.SelectByID2("", "FACE", 0.0504738910727269, 0.00167315253537481, -0.00496149996774875, true, 2, null, 0);

                // Activate the drawing document
                // Create and insert the relative drawing view using
                // the selected faces
                // Activate the relative drawing view
                swApp.ActivateDoc3("maingrip - Sheet1", false, (int)swRebuildOnActivation_e.swUserDecision, ref errors);
                //swDrawing = (DrawingDoc)swApp.ActiveDoc;
                fileName = "C:\\Users\\Public\\Documents\\SOLIDWORKS\\SOLIDWORKS 2018\\samples\\tutorial\\api\\maingrip.sldprt";
                swView = swDrawing.CreateRelativeView(fileName, 0.203608914116486, 0.493530187561698, (int)swRelativeViewCreationDirection_e.swRelativeViewCreationDirection_FRONT, (int)swRelativeViewCreationDirection_e.swRelativeViewCreationDirection_RIGHT);
                status = swDrawing.ActivateView("Drawing View2");

            }
        }
        //9.新建工程图，插入零件视图
        private void Btn_NewDrawing_Click(object sender, EventArgs e)
        {
            //int errors = 1;

            ISldWorks swApp = Program.ConnectToSolidWorks();
            if (swApp != null)
            {
                string FileName = "C:\\Users\\Public\\Documents\\SOLIDWORKS\\SOLIDWORKS 2019\\samples\\tutorial\\api\\chair.sldprt";
                //ModelDoc2 swOpenedDoc = swApp.OpenDoc(FileName, (int)swDocumentTypes_e.swDocPART);

                string drawingTemplate = swApp.GetUserPreferenceStringValue((int)swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
                ModelDoc2 designProject = swApp.NewDocument(drawingTemplate, (int)swDwgPaperSizes_e.swDwgPaperBsize, 0, 0);
                DrawingDoc designProjectDrawing = (DrawingDoc)designProject;
                int errors = 0;
                swApp.ActivateDoc2(designProject.GetTitle(), false, ref errors);
                designProjectDrawing.SetupSheet5("", (int)swDwgPaperSizes_e.swDwgPapersUserDefined, (int)swDwgTemplates_e.swDwgTemplateCustom, 1, 100, false, "a4 - iso.slddrt", 0.297, 0.21, "", true);

                //插入零件视图
                View swPartView = designProjectDrawing.CreateDrawViewFromModelView3(/*swOpenedDoc.GetPathName()*/FileName, "*Front", 0.1, 0.2, 0);

                Debug.Print("Create DrawingView Succeeded!");

            }

        }
        //10.装配零件
        private void Btn_InsertPart_Click(object sender, EventArgs e)
        {
            //step1:生成一个新装配并保存.
            ISldWorks swApp = Program.ConnectToSolidWorks();
            int errors = 0;
            int warinings = 0;
            if (swApp != null)
            {
                //通过GetDocumentTemplate 获取默认模板的路径 ,第一个参数可以指定类型
                string partDefaultTemplate = swApp.GetDocumentTemplate((int)swDocumentTypes_e.swDocASSEMBLY, "", 0, 0, 0);
                //也可以直接指定slddot asmdot drwdot
                //partDefaultTemplate = @"xxx\..prtdot";

                var newDoc = swApp.NewDocument(partDefaultTemplate, 0, 0, 0);

                if (newDoc != null)
                {
                    //下面获取当前文件
                    ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

                    bool boolstatus = swModel.Extension.SaveAs(@"E:\lipu2021\SLDWModel\TempAssembly.sldasm", 0, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, "", ref errors, ref warinings);

                    //step2:打开已有零件
                    string myNewPartPath = @"E:\lipu2021\SLDWModel\clamp1.sldprt";
                    swApp.OpenDoc(myNewPartPath, (int)swDocumentTypes_e.swDocPART);

                    //step3:切换到装配体中,利用面配合来装配零件.

                    AssemblyDoc assemblyDoc = (AssemblyDoc)swApp.ActivateDoc3("TempAssembly.sldasm", true, 0, errors);
                    swApp.ActivateDoc("TempAssembly.sldasm");

                    Component2 InsertedComponent = assemblyDoc.AddComponent5(myNewPartPath, 0, "", false, "", 0, 0, 0);

                    InsertedComponent.Select(false);

                    assemblyDoc.UnfixComponent();

                    //step4: 配合:

                    boolstatus = swModel.Extension.SelectByID2("Plane1", "PLANE", 0, 0, 0, false, 0, null, 0);

                    boolstatus = swModel.Extension.SelectByID2("Front Plane@clamp1-1@TempAssembly", "PLANE", 0, 0, 0, true, 0, null, 0);
                    int longstatus = 0;
                    //重合
                    assemblyDoc.AddMate5(0, 0, false, 0, 0.001, 0.001, 0.001, 0.001, 0, 0, 0, false, false, 0, out longstatus);

                    swModel.EditRebuild3();
                    swModel.ClearSelection();

                    //距离配合 :
                    boolstatus = swModel.Extension.SelectByID2("Plane2", "PLANE", 0, 0, 0, false, 0, null, 0);
                    boolstatus = swModel.Extension.SelectByID2("Top Plane@clamp1-1@TempAssembly", "PLANE", 0, 0, 0, true, 0, null, 0);

                    assemblyDoc.AddMate5((int)swMateType_e.swMateDISTANCE, (int)swMateAlign_e.swMateAlignALIGNED, true, 0.01, 0.01, 0.01, 0.01, 0.01, 0, 0, 0, false, false, 0, out longstatus);
                }
            }
        }
        //11.转换成对应文档
        private void Btn_TransToDoc_Click(object sender, EventArgs e)
        {
            ISldWorks swApp = Program.ConnectToSolidWorks();
            int errors = 0;
            int warinings = 0;
            if (swApp != null)
            {
                //swApp.RemoveMenu();
                ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc; //当前零件
                if(swModel != null)
                {
                    bool boolstatus = swModel.Extension.SaveAs(@"E:\lipu2021\SLDWModel\TmpPart.tif", 0, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, "", ref errors, ref warinings);
                    Debug.Print("Transform successfully!");
                }
            }
        }

        private TaskpaneView taskpaneView;
        private void Btn_Filter_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //删除新加的控件
                // taskpaneView = null;
                taskpaneView?.DeleteView();
                Marshal.FinalReleaseComObject(taskpaneView);
                taskpaneView = null;
            }
            catch (Exception exception)
            {

            }
        }

        public void ErrorMsg(ISldWorks SwApp, string Message)
        {
            SwApp.SendMsgToUser2(Message, 0, 0);
            SwApp.RecordLine("'*** WARNING - General");
            SwApp.RecordLine("'*** " + Message);
            SwApp.RecordLine("");
        }

    }
}



