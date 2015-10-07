///*
//    theDirector - an open source automation solution
//    Copyright (C) 2015 Richard Mageau

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// */


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using DirectorAPI.ConsoleConnection;

//namespace DirectorAPI
//{
//    public class Connection
//    {
//        private ProcessInterface _processInterace = new ProcessInterface();
//        private List<string> _screendata = new List<string>();
//        private char[,] _screenbuffer = new char[25, 80];

//        public CaretLocation CurrentLocation = new CaretLocation();
//        private string _lastInput;
//        private string _buffer;

//        private List<ScreenCondition> Conditions = new List<ScreenCondition>();

//        public delegate void BufferRefreshed(object sender);
//        public event BufferRefreshed BufferRefresh;

//        public List<string> ScreenData
//        {
//            get
//            {
//                return _screendata;
//            }
//        } 

//        public Connection()
//        {
//            _processInterace.OnProcessError += processInterace_OnProcessError;
//            _processInterace.OnProcessExit += processInterace_OnProcessExit;
//            _processInterace.OnProcessInput += processInterace_OnProcessInput;
//            _processInterace.OnProcessOutput += processInterace_OnProcessOutput;

//        }
//        void processInterace_OnProcessOutput(object sender, ProcessEventArgs args)
//        {
//            if (args.Content.Equals(_lastInput))
//            {
//                _screendata[_screendata.Count - 1] = _screendata[_screendata.Count - 1] + args.Content.Replace("\r\n", "");
//                LoadScreenBuffer();
//                return;
//            }

//            //screen size is 80 by 25
//            if (args.Content.Equals("\f")) //needs to be a buffer termination character
//            {
//                _screendata.Clear();
//            }
//            else
//            {
//                //for the purposes of this demonstration, output termination marker will be '>'
//                //thinking that it would be best if the caret location from Console API / colum from buffer might be better.
//                if (args.Content.Length == 0)   //console has probably been reset
//                {
//                    return;
//                }
//                var data = args.Content.Split(new[] { "\r\n" }, StringSplitOptions.None);
//                _buffer += args.Content;
//                if (_buffer.Substring(_buffer.Length - 1).Equals(">"))
//                {
//                    //we've hit output termination marker. parse it
//                    data = _buffer.Split(new[] { "\r\n" }, StringSplitOptions.None);
//                    _buffer = "";
//                    //add it to the list
//                    for (var i = 0; i < data.Length; i++)
//                    {
//                        _screendata.Add(data[i]);
//                    }

//                    //parse it
//                    LoadScreenBuffer();

//                    //do we have any conditions to process?
//                    if (Conditions.Count != 0)
//                    {
//                        EvalConditions();
//                    }
//                }
//            }
//        }

//        void processInterace_OnProcessInput(object sender, ProcessEventArgs args)
//        {
//            throw new NotImplementedException();
//        }

//        void processInterace_OnProcessExit(object sender, ProcessEventArgs args)
//        {
//            //do we need to bubble this up?
//            throw new NotImplementedException();
//        }

//        void processInterace_OnProcessError(object sender, ProcessEventArgs args)
//        {
//            //why is this firing?
//            //throw new NotImplementedException();
//        }


//        public void WriteInput(string input)
//        {
//            _lastInput = input + "\r\n";
//            _processInterace.WriteInput(input);
//        }

//        public void EvalConditions()
//        {
//            foreach (var condition in Conditions)
//            {
//                var data = _screendata[condition.Row].Substring(condition.Column, condition.Text.Length);
//                if (data.Equals(condition.Text))
//                {
//                    //make sure the caret is where it's supposed to be
//                    if ((CurrentLocation.Column == condition.CaretLocation.Column) &&
//                        (CurrentLocation.Row == condition.CaretLocation.Row))
//                    {
//                        //WriteInput(condition.Action);
//                        return;
//                    }
//                }
//            }
//        }
//        public void LoadScreenBuffer()
//        {
//            var temp = new List<string>();
//            var length = 80;

//            //check to make sure length <80
//            foreach (var loop in _screendata)
//            {
//                if (loop.Length > 80)
//                {
//                    //need to split this up
//                    for (var i = 0; i < loop.Length; i += 80)
//                    {
//                        if ((loop.Length - i) < 80)
//                        {
//                            length = loop.Length - i;
//                        }
//                        temp.Add(loop.Substring(i, length));
//                    }
//                }
//                else
//                {
//                    temp.Add(loop);
//                }
//            }

//            //make sure screenbuffer only holds 25 rows
//            while (temp.Count > 25)
//            {
//                temp.RemoveAt(0);
//            }

//            //make sure the length is not greater than 80 chars

//            //string rowdata;

//            //reset the screenbuffer to repopulate
//            _screenbuffer = new char[25, 80];

//            for (var screendatarow = 0; screendatarow < temp.Count(); screendatarow++)
//            {
//                var rowchar = temp[screendatarow].ToCharArray();
//                for (var screendatacol = 0; screendatacol < 80; screendatacol++)
//                {
//                    if (rowchar.Length != 0)
//                    {
//                        if (screendatacol >= rowchar.Length)
//                        {
//                            _screenbuffer[screendatarow, screendatacol] = ' ';  //delete anything currently existing in that element.
//                        }
//                        else
//                        {
//                            _screenbuffer[screendatarow, screendatacol] = rowchar[screendatacol];
//                        }
//                    }
//                    else
//                    {
//                        //it's a blank row, populate with blanks
//                        _screenbuffer[screendatarow, screendatacol] = ' ';
//                    }
//                }
//            }

//            //need to figure out our row/col
//            CurrentLocation.Row = _screendata.Count - 1;
//            CurrentLocation.Column = _screendata[_screendata.Count - 1].Length + 1;

//            //need to raise event that the screen buffer is correctly populated
//            OnBufferRefreshed();
//        }


//        public void AddCondition(ScreenCondition condition)
//        {
//            Conditions.Add(condition);
//        }

//        public ScreenCondition GetCurrentScreenCondition()
//        {
//            var caretRow = _screendata.Count-1;
//            var caretCol = _screendata[caretRow].Length-1;

//            var caret = new CaretLocation {Row = caretRow, Column = caretCol};

//            return new ScreenCondition
//            { CaretLocation = caret,
//                Column = 0,
//                Row = caretRow,
//                Text = _screendata[_screendata.Count -1]};
//        }

//        protected void OnBufferRefreshed()
//        {
//            if (BufferRefresh != null) BufferRefresh(this);
//        }


//        public void StartProcess(string fileName, string parameters)
//        {
//            //todo if there is an instance of this running, terminate and restart
            

//            if (_processInterace.IsProcessRunning)
//            {
//                //_processInterace.WriteInput("exit");
//                Thread.Sleep(1000);
//                _processInterace.StopProcess();
//                while (_processInterace.IsProcessRunning)
//                {
//                    Thread.Sleep(1000);
//                }
//            }
//            Thread.Sleep(200);
//            _screendata.Clear();
//            _processInterace.StartProcess("cmd.exe", null);
//        }

//        public void StopProcess()
//        {
//            _processInterace.StopProcess();
//        }
//        public bool EvalCondition(string cond)
//        {
//            //TextBox tx = new TextBox();
//            //tx.Text = _cc.Scrape();
//            //tx.Refresh();
//            //if (_cc.screendata.Count == 0)
//            //{
//            //    return false;
//            //}
//            ////string rowdata = tx.Lines[tx.Lines.Length - 2].Trim();
//            //string rowdata = _cc.screendata[_cc.screendata.Count-1].Trim();
            
//            //if (rowdata.Equals(cond))
//            //{
//            //    return true;
//            //}
//            throw new NotImplementedException();

//            return false;
//            //_cc.InternalRichTextBox.SelectionStart = _cc.InternalRichTextBox.GetFirstCharIndexOfCurrentLine();
//            //_cc.InternalRichTextBox.SelectionLength = _cc.InternalRichTextBox.TextLength - _cc.InternalRichTextBox.SelectionStart;
//            //string text = _cc.InternalRichTextBox.SelectedText;
//            //return text.Equals(cond);
//        }
//        public void EnterData(string data)
//        {
//            //_cc.WriteInput(data, System.Drawing.Color.White, false);
//            ////_cc.InternalRichTextBox.SelectionStart = _cc.InternalRichTextBox.TextLength;
//            ////_cc.InternalRichTextBox.ScrollToCaret();
//            //_cc.Update();
//            //_cc.InternalRichTextBox.Refresh();
//            //System.Threading.Thread.Sleep(200);
//            //_cc.WriteInput(data);
//            throw new NotImplementedException();

//        }
//    }
//}
