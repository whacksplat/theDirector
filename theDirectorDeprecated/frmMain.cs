/*
    theDirector - an open source automation solution
    Copyright (C) 2015 Richard Mageau

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DirectorAPI;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Conditions;
using DirectorAPI.Connections;
using DirectorAPI.Interfaces;
using DirectorAPI.Scenes;


namespace theDirector
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public partial class frmMain : Form
    {
        private Automation _automation;
 //       private IScene _scene;
        private Object thisLock = new Object();

        public frmMain()
        {
            InitializeComponent();

        }

        private void NewAutomationToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            NewAutomation();
        }

        private void NewAutomation()
        {
            frmNewAutomation frm = new frmNewAutomation();
            DialogResult result = frm.ShowDialog();

            if (result == DialogResult.OK)
            {
                _automation = new Automation(frm.automationname, frm.description);
                RefreshScreen();
            }
        }

        private void OpenAutomationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmOpenAutomation frm = new frmOpenAutomation();
            DialogResult result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (frm.id != Guid.Empty)
                {
                    
                    _automation = new Automation(frm.id);
                    _automation.CurrentMode = Enumerations.Mode.Record;
                    RefreshScreen();
                }
            }
        }

        [SuppressMessage("ReSharper", "ArrangeThisQualifier")]
        [SuppressMessage("ReSharper", "LocalizableElement")]
        private void RefreshScreen()
        {
            if (_automation == null) return;

            //change caption
            this.Text = string.Format("theDirector - {0}", _automation.Name);

            //pop treeview
            tvw.Nodes.Clear();
            tvwConditions.Nodes.Clear();

            //root node will be the automation name
            TreeNode root = new TreeNode("Automation '" + _automation.Name + "'", 1, 1);

            //need a cool icon

            //references
            var nodeScenes = new TreeNode("Scenes", 3, 3);
            var references = new TreeNode("References", 2, 2);

            //steps
            foreach (IScene scene in _automation.Scenes)
            {
                var nodeIndex = nodeScenes.Nodes.Add(new TreeNode(scene.Name, 3, 3));
                nodeScenes.Nodes[nodeIndex].Tag = scene;
            }

            root.Nodes.Add(references);
            root.Nodes.Add(nodeScenes);

            tvw.Nodes.Add(root);

            tvw.ExpandAll();

            propGrid.SelectedObject = null;

        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != null && e.ChangedItem.Label.Equals("Name"))
            {
                tvw.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }

            if (propGrid.SelectedObject is IAction)
            {
                //DBHelper.UpdateAction(propGrid.SelectedObject as IAction);
                DBHelper.SaveAction(propGrid.SelectedObject as IAction);
            }
            if (propGrid.SelectedObject is IScene)
            {
                DBHelper.SaveScene((IScene)propGrid.SelectedObject);
            }
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            //TODO this needs to go inside DirectorAPI
            //TODO need to test to make sure an automation will actually work, things like a scene needs an exit point, an automation must have an end automation scene, etc
            //todo need to write out to a compile log file

            if (_automation != null)
            {
                //_automation.CurrentMode = Enumerations.Mode.Run;
                
                //if (!_automation.PreCompile())
                //{
                //    return;
                //}

                //_scene = null;
                //_automation.BuildAssemblies();
                //string retval;

                ////reset the data source and connection objects in the automation
                //IScene scene = _automation.Scenes[0];
                //rtf.Clear();
                //_automation.ConnectionBufferRefresh += _automation_ConnectionBufferRefresh;

                //while (scene != null)
                //{
                //    if (scene is EndAutomationScene)
                //    {
                //        //we're done
                //        break;
                //    }

                //    if (scene is ConnectionScene)
                //    {
                //            //AutomationHelper.automation.Connection
                //    }

                //    foreach (ICondition condition in scene.GetConditions())
                //    {

                //        if (condition.EvaluateCondition())
                //        {
                //            lock (thisLock)
                //            {
                //                retval = condition.ExecuteActions();    
                //            }
                            
                //            if (!string.IsNullOrEmpty(retval))
                //            {
                //                scene = _automation.Scenes.Find(x => x.Name == retval);
                //            }
                //        }
                //    }
                //}

                ////we're done, go back to record
                //_automation.CurrentMode = Enumerations.Mode.Record;
                _automation.ConnectionBufferRefresh += _automation_ConnectionBufferRefresh;
                _automation.RunAutomation();
                
            }
        }

        /// <summary>
        /// This routine checks to see if we are ready to add scenes/conditions/actions
        /// </summary>
        /// <returns>Returns true if we have a script loaded, a scene selected</returns>
        private bool IsReady()
        {
            if (_automation == null || tvw.SelectedNode == null)
            {
                return false;
            }

            return tvw.SelectedNode.Tag is IScene;
        }


        private void tvwConditions_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvwConditions.SelectedNode.Tag is ICondition)
            {
                ICondition cond = (ICondition)tvwConditions.SelectedNode.Tag;
                propGrid.SelectedObject = cond;
                propGrid.Refresh();
                return;
            }

            if (tvwConditions.SelectedNode.Tag is IAction)
            {
                propGrid.SelectedObject = tvwConditions.SelectedNode.Tag;
                return;
            }

            throw new Exception("tvwConditions_AfterSelect not condition or action");
        }


        private void tvw_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propGrid.SelectedObject = e.Node.Tag;
            tvwConditions.Nodes.Clear();

            if (e.Node.Tag is IScene)
            {
                IScene scene = (IScene)e.Node.Tag;
                foreach (ICondition condition in scene.GetConditions())
                {
                    TreeNode condnode = tvwConditions.Nodes.Add(condition.DisplayText());
                    condnode.Tag = condition;
                    foreach (IAction action in condition.GetActions())
                    {
                        switch (action.ActionType)
                        {
                            case Enumerations.ActionType.MessageBox:
                                TreeNode actionnode = condnode.Nodes.Add(action.DisplayText);
                                actionnode.Tag = action;
                                //tvwConditions.SelectedNode = actionnode;
                                break;

                            case Enumerations.ActionType.ConnectToCmd:
                                TreeNode connNode = condnode.Nodes.Add(action.DisplayText);
                                connNode.Tag = action;
                                //tvwConditions.SelectedNode = connNode;
                                break;
                            
                            case Enumerations.ActionType.SendData:
                                TreeNode sendDataNode = condnode.Nodes.Add(action.DisplayText);
                                sendDataNode.Tag = action;
                                break;

                            default:
                                throw new NotImplementedException();
                        }

                    }
                }
            }

        }

        private void tvw_DragDrop(object sender, DragEventArgs e)
        {
            Point point = tvw.PointToClient(new Point(e.X, e.Y));
            TreeViewHitTestInfo info = tvw.HitTest(point.X, point.Y);
            IScene destinationScene = (IScene)info.Node.Tag;

            if (destinationScene != null)
            {
                TreeNode sourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                IScene sourceScene = (IScene)sourceNode.Tag;
                //move it
                _automation.MoveScene(sourceScene, destinationScene);
                RefreshScreen();
            }
        }

        private void tvw_DragOver(object sender, DragEventArgs e)
        {
            Point point = tvw.PointToClient(new Point(e.X, e.Y));
            TreeViewHitTestInfo info = tvw.HitTest(point.X, point.Y);
            if (info.Node != null)
            {
                IScene scene = (IScene)info.Node.Tag;
                if (scene != null)
                {
                    tvw.SelectedNode = info.Node;
                }
            }
        }

        private void tvw_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void tvw_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            NewAutomation();
        }

        private void AddAlwaysScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new AlwaysScene());
            RefreshScreen();
        }

        private void addAlwaysCondition_Click(object sender, EventArgs e)
        {
            if (!IsReady()) return;

            IScene scene = (IScene)tvw.SelectedNode.Tag;
            if (scene.Type == Enumerations.SceneTypes.EndAutomation)
            {
                MessageBox.Show("You cannot add condtions to an End Scene.");
                return;
            }

            try
            {
                AlwaysCondition always = (AlwaysCondition)scene.AddCondition(new AlwaysCondition());
                TreeNode node = tvwConditions.Nodes.Add(always.DisplayText());
                node.Tag = always;
                tvwConditions.SelectedNode = node;
            }
            catch (Exception err)
            {

                MessageBox.Show("You cannot add an Always condition to a scene of type " + scene.Type);
            }

        }

        private void AddConnectionScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new ConnectionScene());
            RefreshScreen();
        }

        private void AddDatasourceScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new DataSourceScene());
            RefreshScreen();
        }

        private void AddVariableScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new VariableScene());
            RefreshScreen();

        }

        private void AddEndScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new EndAutomationScene());
            RefreshScreen();

        }

        private void stdOutRedirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsReady()) return;

            IScene scene = (IScene)tvw.SelectedNode.Tag;
            if (scene.Type != Enumerations.SceneTypes.Always)
            {
                MessageBox.Show("You can only Connect when you are in an Always scene.");
                return;
            }

            ICondition condition = scene.AddCondition(new AlwaysCondition());
            _automation.ConnectionBufferRefresh += _automation_ConnectionBufferRefresh;

            condition.AddAction(Enumerations.ActionType.ConnectToCmd);

            RefreshScreen();
        }

        void _automation_ConnectionBufferRefresh(object sender)
        {
            rtf.Invoke(new MethodInvoker(delegate { rtf.Clear(); }));
            foreach (string looper in (sender as ConsoleConnectionRedirection).ScreenData)
            {
                rtf.Invoke(new MethodInvoker(delegate { rtf.Text += looper + Environment.NewLine; }));
            }

        }

        private void messageboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvwConditions.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            ICondition condition;

            if (!(tvwConditions.SelectedNode.Tag is ICondition))
            {
                MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            condition = (ICondition)tvwConditions.SelectedNode.Tag;

            IAction messagebox = condition.AddAction(Enumerations.ActionType.MessageBox);
            TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add(messagebox.DisplayText);
            actionnode.Tag = messagebox;
            tvwConditions.SelectedNode = actionnode;
        }

        private void sendData_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    sendData.Text += "{ENTER}";
                    sendData.SelectionStart = sendData.Text.Length;
                    return;
                default:
                    break;
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (!IsReady()) return;

            IScene scene = (IScene)tvw.SelectedNode.Tag;
            if (scene.Type != Enumerations.SceneTypes.Connection)
            {
                MessageBox.Show("You can only Send Data when you are in an Connection scene.");
                return;
            }

            //todo MUST make sure there will be no conflicting screen conditions on release

            ICondition condition = scene.AddCondition(_automation.Connection.GetCurrentScreenCondition());
            //add condition to treeview
            TreeNode node = tvwConditions.Nodes.Add(condition.DisplayText());
            node.Tag = condition;

            SendData send = (SendData)condition.AddAction(Enumerations.ActionType.SendData);
            send.DataToSend = sendData.Text;
            send.BuildCode();
            send.Execute();

            TreeNode actionnode = node.Nodes.Add(send.DisplayText);
            actionnode.Tag = send;
            tvwConditions.SelectedNode = actionnode;

            //RefreshScreen();

        }

    }
}

