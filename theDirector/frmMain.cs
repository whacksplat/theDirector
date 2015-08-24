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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DirectorAPI;
using DirectorAPI.Actions;
using DirectorAPI.Actions.Connection;
using DirectorAPI.Actions.Datasource;
using DirectorAPI.Interfaces;
using DirectorAPI.Scenes;


namespace theDirector
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public partial class frmMain : Form
    {
        private Automation _automation;
        private IScene _scene;

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
                //Automation script = new Automation(frm.id);
                if (frm.id != Guid.Empty)
                {
                    _automation = new Automation(frm.id);
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
            var nodeScenes = new TreeNode("Scenes",3,3);
            var references = new TreeNode("References",2,2);
                
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

        private void newToolStripButton1_Click(object sender, EventArgs e)
        {
            //if (_automation != null)
            //{
            //    //_automation.AddScene();
            //    throw new NotImplementedException();
            //    //RefreshScreen();
            //}
        }

        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label != null && e.ChangedItem.Label.Equals("Name"))
            {
                tvw.SelectedNode.Text = e.ChangedItem.Value.ToString();
            }
            
            if (propGrid.SelectedObject is AAction)
            {
                DBHelper.UpdateAction(propGrid.SelectedObject as AAction);
            }
            if (propGrid.SelectedObject is AlwaysScene)
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

                if (!_automation.PreCompile())
                {
                    return;
                }

                _automation.CurrentMode = Automation.Mode.Run;
                _scene = null;
                _automation.BuildAssemblies();
                string retval;

                //reset the data source and connection objects in the automation
                IScene scene = _automation.Scenes[0];
                AutomationHelper.automation.Connection.BufferRefresh += ConnectionOnBufferRefresh;
                rtf.Clear();

                while (scene != null)
                {
                    switch (scene.Type)
                    {
                        case SceneEnums.SceneType.Always:
                            //an always type scene will only have 1 condition, an always.
                            //retval = scene.Conditions[0].Execute(_automation);
                            throw new NotImplementedException();
                            if (!string.IsNullOrEmpty(retval))
                            {
                                scene = _automation.Scenes.Find(x => x.Name == retval);
                                break;
                            }
                            break;

                        case SceneEnums.SceneType.Connection:
                            //_scene = scene;
                            throw new NotImplementedException();
                            break;

                        case SceneEnums.SceneType.EndAutomation:
                            return;

                        case SceneEnums.SceneType.Datasource:
                            //foreach (Condition condition in scene.Conditions)
                            //{
                            //    if (condition.Eval(_automation))
                            //    {
                            //        retval = condition.Execute(_automation);
                            //        if (!string.IsNullOrEmpty(retval))
                            //        {
                            //            scene = _automation.Scenes.Find(x => x.Name == retval);
                            //        }
                            //    }
                            //}
                            throw new NotImplementedException();
                            break;

                        case SceneEnums.SceneType.Variable:
                            //foreach (Condition condition in scene.Conditions)
                            //{
                            //    if (condition.Eval(_automation))
                            //    {
                            //        retval = condition.Execute(_automation);
                            //        if (!string.IsNullOrEmpty(retval))
                            //        {
                            //            scene = _automation.Scenes.Find(x => x.Name == retval);
                            //        }
                            //    }
                            //}
                            throw new NotImplementedException();
                            break;
                    }
                }

                _automation.CurrentMode = Automation.Mode.Record;
            }
        }

        private void alwaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!IsReady()) return;

            //Scene step = (Scene)tvw.SelectedNode.Tag;
            //if (step.Type == Scene.SceneType.EndAutomation)
            //{
            //    MessageBox.Show("You cannot add condtions to an End Scene.");
            //    return;
            //}
            //Condition condition = step.AddCondition("return true;", "Always");
            ////step.AddCondition(new AlwaysCon)

            //TreeNode node = tvwConditions.Nodes.Add(condition.DisplayCode);
            //node.Tag = condition;
            //tvwConditions.SelectedNode = node;
            throw new NotImplementedException();
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
            if (tvwConditions.SelectedNode.Tag is Condition)
            {
                Condition cond = (Condition)tvwConditions.SelectedNode.Tag;
                propGrid.SelectedObject = cond;
                propGrid.Refresh();
                return;
            }

            if (tvwConditions.SelectedNode.Tag is AAction)
            {
                propGrid.SelectedObject = tvwConditions.SelectedNode.Tag;
                return;
            }
            if (tvwConditions.SelectedNode.Tag is ScreenCondition)
            {
                propGrid.SelectedObject = tvwConditions.SelectedNode.Tag;
                return;
                
            }
            
            throw new Exception("tvwConditions_AfterSelect not condition or action");
        }


        private void messageboxToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tvwConditions.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            Condition condition;

            if (tvwConditions.SelectedNode.Tag is AAction)
            {
                condition = (Condition)tvwConditions.SelectedNode.Parent.Tag;
                tvwConditions.SelectedNode = tvwConditions.SelectedNode.Parent;
            }
            else
            {
                condition = (Condition)tvwConditions.SelectedNode.Tag;
            }

            TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add("MessageBox");
            actionnode.Tag = condition.AddAction(DirectorAPI.Action.ActionType.MessageBox);
            tvwConditions.SelectedNode = actionnode;
        }

        private void openDatasourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //make sure we have something selected
            if (tvwConditions.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            Condition condition;

            if (tvwConditions.SelectedNode.Tag is DirectorAPI.Action)
            {
                condition = (Condition)tvwConditions.SelectedNode.Parent.Tag;
            }
            else
            {
                condition = (Condition)tvwConditions.SelectedNode.Tag;
            }

            TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add("OpenDatasource");
            actionnode.Tag = condition.AddAction(DirectorAPI.Action.ActionType.OpenDatasource);
            tvwConditions.SelectedNode = actionnode;
        }

        private void dataSourceIsEOFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!IsReady()) return;

            //if (!(tvw.SelectedNode.Tag is iScene)) return;

            //iScene step = (iScene)tvw.SelectedNode.Tag;
            //Condition condition = step.AddCondition("return automation.datasource.EOF();", "Is EOF");
            //TreeNode node = tvwConditions.Nodes.Add(condition.DisplayCode);
            //node.Tag = condition;
            throw new NotImplementedException();
        }

        private void nextRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //make sure we have something selected
            if (tvwConditions.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            Condition condition;

            if (tvwConditions.SelectedNode.Tag is DirectorAPI.Action)
            {
                condition = (Condition)tvwConditions.SelectedNode.Parent.Tag;
            }
            else
            {
                condition = (Condition)tvwConditions.SelectedNode.Tag;
            }

            TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add("NextRecord");
            actionnode.Tag = condition.AddAction(DirectorAPI.Action.ActionType.NextRecord);
            tvwConditions.SelectedNode = actionnode;
        }

        private void dataSourceIsNOTEOFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsReady()) return;

            //if (!(tvw.SelectedNode.Tag is Scene)) return;

            //Scene step = (Scene)tvw.SelectedNode.Tag;
            //Condition condition = step.AddCondition("return !automation.datasource.EOF();", "Is NOT EOF");
            //TreeNode node = tvwConditions.Nodes.Add(condition.DisplayCode);
            //node.Tag = condition;
            throw new NotImplementedException();
        }

        private void screenConditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!IsReady()) return;

            //frmEnterData frm = new frmEnterData();
            //DialogResult result = frm.ShowDialog();
            
            //if (result != DialogResult.OK) return;

            //Scene step = (Scene)tvw.SelectedNode.Tag;

            //Condition condition = step.AddCondition("return automation.connection.EvalCondition(" + "\"" + rtf.Lines[rtf.Lines.Length - 2].Trim() + "\"" + ");", "Screen Condition");
            //TreeNode node = tvwConditions.Nodes.Add(condition.DisplayCode);
            //node.Tag = condition;
            //tvwConditions.SelectedNode = node;

            ////add the action
            //TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add("EnterData");
            //EnterData ed = (EnterData)condition.AddAction(DirectorAPI.Action.ActionType.EnterData);
            //ed.Data = frm.Data;
            //actionnode.Tag = ed;
            //tvwConditions.SelectedNode = actionnode;
            throw new NotImplementedException();
 }

        private void tvw_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode != Keys.Delete) return;
            //if (!(tvw.SelectedNode.Tag is Scene)) return;
            ////_script.DeleteScene((Scene)tvw.SelectedNode.Tag);
            
            //tvw.Nodes.Remove(tvw.SelectedNode);
            throw new NotImplementedException();
        }

        private void openConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvwConditions.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("You must select a condition to add the action to.");
                return;
            }

            Condition condition;

            if (tvwConditions.SelectedNode.Tag is DirectorAPI.Action)
            {
                condition = (Condition)tvwConditions.SelectedNode.Parent.Tag;
            }
            else
            {
                if (tvwConditions.SelectedNode.Tag is AAction)
                {
                    condition = (Condition)tvwConditions.SelectedNode.Parent.Tag;
                }
                else
                {
                    condition = (Condition)tvwConditions.SelectedNode.Tag;
                }
            }

            TreeNode actionnode = tvwConditions.SelectedNode.Nodes.Add("Connect to CMD");
            actionnode.Tag = condition.AddAction(DirectorAPI.Action.ActionType.ConnectToCmd);
            tvwConditions.SelectedNode = actionnode;
            AutomationHelper.automation.Connection.BufferRefresh += ConnectionOnBufferRefresh;
            
        }

        private void ConnectionOnBufferRefresh(object sender)
        {
            rtf.Clear();
            Connection conn = (Connection) sender;
            for (int line = 0; line < conn.ScreenData.Count; line++)
            {
                if (line == conn.ScreenData.Count-1)
                {
                    rtf.Text += conn.ScreenData[line];
                }
                else
                {
                    rtf.Text += conn.ScreenData[line] + Environment.NewLine;    
                }
            }
            ScreenCondition cond = conn.GetCurrentScreenCondition();


            //highlight with the default
            string screenLine = rtf.Lines[cond.Row];
            int totalChar = 0;
            //add up all the characters previous to the cond.row
            for (int charCount = 0; charCount < cond.Row;charCount++)
            {
                totalChar += rtf.Lines[charCount].Length;
            }
            rtf.SelectionStart = totalChar;
            rtf.SelectionLength = cond.Text.Length + Regex.Matches(cond.Text,@"\\").Count;
            rtf.SelectionColor = Color.Yellow;

        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tvw_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propGrid.SelectedObject = e.Node.Tag;
            
            if (e.Node.Tag is IScene)
            {
                //IScene scene = (IScene)e.Node.Tag;
                //tvwConditions.Nodes.Clear();
                //foreach (Condition condition in scene.Conditions)
                //{
                //    TreeNode condnode = tvwConditions.Nodes.Add(condition.DisplayCode);
                //    condnode.Tag = condition;
                //    foreach (object action in condition.Actions)
                //    {
                //        if (action is DirectorAPI.Actions.Notifications.MessageBox)
                //        {
                //            TreeNode actionnode = condnode.Nodes.Add("MessageBox");
                //            actionnode.Tag = action;
                //        }
                //        if (action is OpenDataSource)
                //        {
                //            TreeNode actionnode = condnode.Nodes.Add("OpenDataSource");
                //            actionnode.Tag = action;
                //        }
                //        if (action is NextRecord)
                //        {
                //            TreeNode actionnode = condnode.Nodes.Add("NextRecord");
                //            actionnode.Tag = action;
                //        }

                //        if (action is EnterData)
                //        {
                //            TreeNode actionnode = condnode.Nodes.Add("EnterData");
                //            actionnode.Tag = action;
                //        }
                //        if (action is ConnectToCmd)
                //        {
                //            TreeNode actionnode = condnode.Nodes.Add("ConnectToCmd");
                //            actionnode.Tag = action;
                //        }

                //    }
                //    tvwConditions.ExpandAll();
                //}
            }

        }

        private void enterDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //need the scene
            //if (tvw.SelectedNode.Tag is Scene)
            //{
            //    frmEnterData frm = new frmEnterData();
            //    DialogResult result = frm.ShowDialog();
            //    if (result != DialogResult.OK) return;

            //    ScreenCondition cond = _automation.Connection.GetCurrentScreenCondition();
            //    (tvw.SelectedNode.Tag as Scene).ScreenConditions.Add(cond);
            //    TreeNode node = new TreeNode("ScreenCondition") {Tag = cond};

            //    tvwConditions.Nodes.Add(node);
            //}
            throw new NotImplementedException();
        }

        private void propGrid_Click(object sender, EventArgs e)
        {

        }

        private void tvw_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tvw_DragDrop(object sender, DragEventArgs e)
        {
            Point point = tvw.PointToClient(new Point(e.X, e.Y));
            TreeViewHitTestInfo info = tvw.HitTest(point.X,point.Y);
            IScene destinationScene = (IScene) info.Node.Tag;
            
            if (destinationScene!=null)
            {
                TreeNode sourceNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                IScene sourceScene = (IScene) sourceNode.Tag;
                //(draggedNode.Tag as Scene).SortID 
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

        private void rtf_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //IScene scene;

            //if (tvw.SelectedNode == null)
            //{
            //    return;
            //}

            ////grab the current scene
            //if (tvw.SelectedNode.Tag is IScene)
            //{
            //    scene = (Scene) tvw.SelectedNode.Tag;
            //    if (scene.Type != Scene.SceneType.Connection)
            //    {
            //        //cannot continue
            //        MessageBox.Show("You must select a Connect Type scene to add a Scene condition into.");
            //        return;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("You must select a scene to place the condition into.");
            //    return;
            //}
            
            ////grab the current condition
            //if (_automation.Connection.ScreenData.Count == 0)
            //{
            //    //we aren't connected to anything
            //    MessageBox.Show("You aren't currently connected to anything.");
            //    return;
            //}
            ////ScreenCondition current = _automation.Connection.GetCurrentScreenCondition();

            ////add the condition to the automation


            //scene.AddScreenCondition(_automation.Connection.GetCurrentScreenCondition());

            ////add the enter action to the  condition
            throw new NotImplementedException();
        }

        private void AddAlwaysScene_Click(object sender, EventArgs e)
        {
            if (_automation == null) return;
            _automation.AddScene(new AlwaysScene());
            RefreshScreen();
        }
    }
}

