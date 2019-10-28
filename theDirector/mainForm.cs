using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.IO;
using System.Activities.Core.Presentation;
using System.Activities.Hosting;
using System.Activities.Core.Presentation.Factories;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Statements;
using System.Activities.Validation;
using System.Activities.XamlIntegration;
using System.Windows;
using System.Windows.Forms.Design;
using Parallel = System.Activities.Statements.Parallel;
using TestMessageboxActivity;
using MessageBox = System.Windows.Forms.MessageBox;

namespace TestingHosting
{
    public partial class mainForm : Form
    {
        private WorkflowDesigner wfDesigner;
        private string fileName;

        public mainForm()
        {
            InitializeComponent();
            var wfDesignerMetadata = new DesignerMetadata();
            wfDesignerMetadata.Register();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCanvas(1);
            LoadActivityToolbox();
            LoadPropertyBox();
        }

        private void LoadPropertyBox()
        {
            ElementHost host = new ElementHost(){Dock =DockStyle.Fill};
            host.Child = wfDesigner.PropertyInspectorView;
            rightSplitContainer.Panel2.Controls.Add(host);
        }

        private void LoadCanvas(Int32 workflowType)
        {
            ElementHost host = new ElementHost(){Dock = DockStyle.Fill};
            wfDesigner = new WorkflowDesigner();
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .TargetFrameworkName = new System.Runtime.Versioning.FrameworkName(
                ".NETFramework", new Version(4, 5));
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .AnnotationEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .AutoConnectEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .AutoSplitEnabled = true;
            
            // enable AutoSurroundWithSequence 
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .AutoSurroundWithSequenceEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .BackgroundValidationEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .MultipleItemsContextMenuEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .MultipleItemsDragDropEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .NamespaceConversionEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .PanModeEnabled = true;
            wfDesigner.Context.Services.GetService<DesignerConfigurationService>()
                .RubberBandSelectionEnabled = true;
            
            if (string.IsNullOrWhiteSpace(fileName))
            {
                //wfDesigner.Load(new Sequence());
                switch (workflowType)
                {
                    case 1: //sequence
                        wfDesigner.Load(new Sequence());
                        break;
                    case 2: //flowchart
                        wfDesigner.Load(new Flowchart());
                        break;
                    case 3: //state machine
                        wfDesigner.Load(new StateMachine());
                        break;
                }
            }
            else
            {
                wfDesigner.Load(fileName);
            }
            
            host.Child = wfDesigner.View;
            centerSplitContainer.Panel1.Controls.Add(host);
            
        }
        private void LoadActivityToolbox()
        {
            var wfToolbox = new ToolboxControl();

            ToolboxCategory controlFlow = new ToolboxCategory("Control Flow");

            wfToolbox.Categories.Add(LoadControlFlow());
            wfToolbox.Categories.Add(LoadVariableTools());
            wfToolbox.Categories.Add(LoadErrorHandling());
            wfToolbox.Categories.Add(LoadLooping());
            wfToolbox.Categories.Add(LoadDecisions());
            wfToolbox.Categories.Add(LoadWorkflow());

            ElementHost host = new ElementHost() { Dock = DockStyle.Fill };
            host.Child = wfToolbox;
            leftSplitContainer.Panel1.Controls.Add(host);
        }


        private ToolboxCategory LoadWorkflow()
        {
            ToolboxCategory workflowActivities = new ToolboxCategory("Workflow");

            workflowActivities.Add(new ToolboxItemWrapper(typeof(Flowchart), "Flowchart"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(FlowDecision), "Flow Decision"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(FlowSwitch<>), "Flow Switch"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(Sequence), "Sequence"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(StateMachine), "State Machine"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(State), "State"));
            workflowActivities.Add(new ToolboxItemWrapper(typeof(FinalState), "Final State"));

            return workflowActivities;
        }
        private ToolboxCategory LoadVariableTools()
        {
            ToolboxCategory variableTools = new ToolboxCategory("Variable Tools");
            variableTools.Add(new ToolboxItemWrapper(typeof(AddToCollection<>),"Add To Collection"));
            variableTools.Add(new ToolboxItemWrapper(typeof(Assign),"Assign"));
            variableTools.Add(new ToolboxItemWrapper(typeof(ClearCollection<>),"Clear Collection"));
            variableTools.Add(new ToolboxItemWrapper(typeof(ExistsInCollection<>),"Exists In Collection"));
            variableTools.Add(new ToolboxItemWrapper(typeof(RemoveFromCollection<>),"Remove From Collection"));
            variableTools.Add(new ToolboxItemWrapper(typeof(TestMessageboxActivity.Messagebox), "Test Messagebox"));

            return variableTools;
        }
        private ToolboxCategory LoadControlFlow()
        {
            ToolboxCategory controlFlow = new ToolboxCategory("Control Flow");
            controlFlow.Add(new ToolboxItemWrapper(typeof(Pick), "Pick"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(PickBranch), "PickBranch"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(WriteLine),"Write Line"));
            return controlFlow;
        }
        private ToolboxCategory LoadErrorHandling()
        {
            ToolboxCategory errorHandling = new ToolboxCategory("Error Hanlding");

            errorHandling.Add(new ToolboxItemWrapper(typeof(TryCatch), "TryCatch"));
            errorHandling.Add(new ToolboxItemWrapper(typeof(Rethrow), "Rethrow"));
            errorHandling.Add(new ToolboxItemWrapper(typeof(Throw), "Throw"));

            return errorHandling;
        }
        private ToolboxCategory LoadLooping()
        {
            ToolboxCategory looping = new ToolboxCategory("Looping");

            looping.Add(new ToolboxItemWrapper(typeof(DoWhile),"Do While"));
            looping.Add(new ToolboxItemWrapper(typeof(ForEach<>), "ForEach<T>"));
            looping.Add(new ToolboxItemWrapper(typeof(While), "While"));

            return looping;
        }
        private ToolboxCategory LoadDecisions()
        {
            ToolboxCategory decisions = new ToolboxCategory("Decisions");

            decisions.Add(new ToolboxItemWrapper(typeof(If),"If"));
            decisions.Add(new ToolboxItemWrapper(typeof(Switch<>), "Switch<>"));

            return decisions;
        }

        private void ToolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            wfDesigner.Flush();
            byte[] byteArray = Encoding.ASCII.GetBytes(wfDesigner.Text);
            MemoryStream wf = new MemoryStream(byteArray);
            var program = ActivityXamlServices.Load(wf);
            var results = ActivityValidationServices.Validate(program);
            WorkflowApplication instance = new WorkflowApplication(program);
            instance.Run();
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            throw new Exception("Not Implemented");
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new Exception("Not Implemented");
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                DialogResult result = saveDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    fileName = saveDialog.FileName;
                    wfDesigner.Flush();
                    wfDesigner.Save(fileName);
                    MessageBox.Show(fileName);
                }
            }
            else
            {
                wfDesigner.Flush();
                wfDesigner.Save(fileName);
            }
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openDialog.ShowDialog();
            
            if (!string.IsNullOrWhiteSpace(openDialog.FileName))
            {
                fileName = openDialog.FileName;
                foreach (Control x in centerSplitContainer.Panel1.Controls)
                {
                    centerSplitContainer.Panel1.Controls.Remove(x);
                }

                foreach (Control y in rightSplitContainer.Panel2.Controls)
                {
                    rightSplitContainer.Panel2.Controls.Remove(y);
                }
                LoadCanvas(0);
                LoadPropertyBox();
            }
        }

        private void FlowChartToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newWorkflow wfForm = new newWorkflow();
            DialogResult result = wfForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                foreach (Control x in centerSplitContainer.Panel1.Controls)
                {
                    centerSplitContainer.Panel1.Controls.Remove(x);
                }

                foreach (Control y in rightSplitContainer.Panel2.Controls)
                {
                    rightSplitContainer.Panel2.Controls.Remove(y);
                }
                LoadCanvas(wfForm.WorkFlowType);
                LoadPropertyBox();
            }
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
