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
using DirectorAPI.IO;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Runtime.Remoting.Contexts;
using System.Activities.Presentation.Hosting;
using System.Reflection;
using RabbitMQActivities;
using DataActivities;

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

            //var refs = wfDesigner.Context.Items.GetValue<AssemblyContextControlItem>() ?? new AssemblyContextControlItem();
            ////refs.ReferencedAssemblyNames.Add(new System.Reflection.AssemblyName("System.Forms.Windows"));

            ////Console.Write(refs.AllAssemblyNamesInContext.Count.ToString());
            //Assembly asm = Assembly.Load(typeof(System.Windows.Forms.MessageBox).Assembly.FullName);

            ////refs = new List<AssemblyName>();
            //refs.ReferencedAssemblyNames = new List<AssemblyName>();
            //refs.ReferencedAssemblyNames.Add(asm.GetName());
            //wfDesigner.Context.Items.SetValue(refs);

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
            wfToolbox.Categories.Add(LoadFlowchart());
            wfToolbox.Categories.Add(LoadStateMachine());
            wfToolbox.Categories.Add(LoadRabbitMQ());
            wfToolbox.Categories.Add(LoadData());
            wfToolbox.Categories.Add(LoadRuntime());
            wfToolbox.Categories.Add(LoadPrimitives());
            wfToolbox.Categories.Add(LoadTransaction());
            wfToolbox.Categories.Add(LoadCollection());
            wfToolbox.Categories.Add(LoadErrorHandling());
            wfToolbox.Categories.Add(LoadIOStuff());

            ElementHost host = new ElementHost() { Dock = DockStyle.Fill };
            host.Child = wfToolbox;
            leftSplitContainer.Panel1.Controls.Add(host);
        }

        private ToolboxCategory LoadData()
        {
            ToolboxCategory controlFlow = new ToolboxCategory("Data");

            controlFlow.Add(new ToolboxItemWrapper(typeof(CSV), "CSV"));
            return controlFlow;
        }

        private ToolboxCategory LoadControlFlow()
        {
            ToolboxCategory controlFlow = new ToolboxCategory("Control Flow");

            controlFlow.Add(new ToolboxItemWrapper(typeof(DoWhile), "Do While"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(ForEach<>), "ForEach<T>"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(If), "If"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(Parallel), "Parallel"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(ParallelForEach<>), "ParallelForEach<T>"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(Pick), "Pick"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(PickBranch), "PickBranch"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(Sequence), "Sequence"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(Switch<>), "Switch<>"));
            controlFlow.Add(new ToolboxItemWrapper(typeof(While), "While"));
            return controlFlow;
        }
        private ToolboxCategory LoadFlowchart()
        {
            ToolboxCategory flowChart = new ToolboxCategory("Flowchart");

            flowChart.Add(new ToolboxItemWrapper(typeof(Flowchart), "Flowchart"));
            flowChart.Add(new ToolboxItemWrapper(typeof(FlowDecision), "Flow Decision"));
            flowChart.Add(new ToolboxItemWrapper(typeof(FlowSwitch<>), "Flow Switch"));

            return flowChart;
        }
        private ToolboxCategory LoadStateMachine()
        {
            ToolboxCategory flowChart = new ToolboxCategory("State Machine");

            flowChart.Add(new ToolboxItemWrapper(typeof(StateMachine), "State Machine"));
            flowChart.Add(new ToolboxItemWrapper(typeof(State), "State"));
            flowChart.Add(new ToolboxItemWrapper(typeof(FinalState), "Final State"));

            return flowChart;
        }
        private ToolboxCategory LoadRabbitMQ()
        {
            ToolboxCategory rabbitMQ = new ToolboxCategory("RabbitMQ");
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.CreateConnectionFactory), "Create Connection Factory"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.CreateQueue), "Create Queue"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.ClearQueue), "Clear Queue"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.DeleteQueue), "Delete Queue"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.AddItemsToQueue), "Add Queue Item"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.BasicGet), "Basic Get"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.Ack), "Ack"));
            rabbitMQ.Add(new ToolboxItemWrapper(typeof(RabbitMQActivities.Nack), "Nack"));

            return rabbitMQ;
        }
        private ToolboxCategory LoadRuntime()
        {
            ToolboxCategory runtime = new ToolboxCategory("Runtime");

            runtime.Add(new ToolboxItemWrapper(typeof(Persist), "Persist"));
            runtime.Add(new ToolboxItemWrapper(typeof(TerminateWorkflow), "Terminate Workflow"));
            runtime.Add(new ToolboxItemWrapper(typeof(NoPersistScope), "NoPersistScope"));

            return runtime;
        }
        private ToolboxCategory LoadPrimitives()
        {
            ToolboxCategory primitives = new ToolboxCategory("Primitives");

            primitives.Add(new ToolboxItemWrapper(typeof(Assign), "Assign"));
            primitives.Add(new ToolboxItemWrapper(typeof(Delay), "Delay"));
            primitives.Add(new ToolboxItemWrapper(typeof(InvokeDelegate), "InvokeDelegate"));
            primitives.Add(new ToolboxItemWrapper(typeof(InvokeMethod), "InvokeMethod"));
            primitives.Add(new ToolboxItemWrapper(typeof(WriteLine),"Write Line"));
            return primitives;
        }
        private ToolboxCategory LoadTransaction()
        {
            ToolboxCategory transactions = new ToolboxCategory("Transaction");

            transactions.Add(new ToolboxItemWrapper(typeof(CancellationScope), "CancellationScope"));
            transactions.Add(new ToolboxItemWrapper(typeof(CompensableActivity), "CompensableActivity"));
            transactions.Add(new ToolboxItemWrapper(typeof(Compensate), "Compensate"));
            transactions.Add(new ToolboxItemWrapper(typeof(Confirm), "Confirm"));
            transactions.Add(new ToolboxItemWrapper(typeof(TransactionScope), "TransactionScope"));
            return transactions;
        }
        private ToolboxCategory LoadCollection()
        {
            ToolboxCategory col = new ToolboxCategory("Collection");

            col.Add(new ToolboxItemWrapper(typeof(AddToCollection<>), "AddToCollection"));
            col.Add(new ToolboxItemWrapper(typeof(ClearCollection<>), "ClearCollection"));
            col.Add(new ToolboxItemWrapper(typeof(ExistsInCollection<>), "ExistsInCollection"));
            col.Add(new ToolboxItemWrapper(typeof(RemoveFromCollection<>), "RemoveFromCollection"));

            return col;
        }

        private ToolboxCategory LoadErrorHandling()
        {
            ToolboxCategory col = new ToolboxCategory("Error Handling");

            col.Add(new ToolboxItemWrapper(typeof(Rethrow), "Rethrow"));
            col.Add(new ToolboxItemWrapper(typeof(Throw), "Throw"));
            col.Add(new ToolboxItemWrapper(typeof(TryCatch), "TryCatch"));

            return col;
        }

        private ToolboxCategory LoadIOStuff()
        {
            ToolboxCategory iostuff = new ToolboxCategory("IO Stuff");
            iostuff.Add(new ToolboxItemWrapper(typeof(DirectorAPI.IO.Messagebox), "Messagebox"));

            return iostuff;
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
