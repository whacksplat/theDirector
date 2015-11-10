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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DirectorAPI.Conditions;
using DirectorAPI.Interfaces;

namespace DirectorAPI.Connections
{
    public class ConsoleConnectionRedirection : ICharacterConnection
    {
        //fields
        private Process process;
        private string fileName = "cmd.exe";
        private string arguments = "";

        private StreamWriter inputWriter;
        private TextReader outputReader, errorReader;

        private BackgroundWorker outputWorker = new BackgroundWorker();
        private BackgroundWorker errorWorker = new BackgroundWorker();
        
        private string _termChars;
        private string _buffer;
        private int _rows, _cols;
        private string _lastInput;
        private bool _isActive;
        private bool _isConnected;

        private readonly List<string> _screendata = new List<string>();

        public ConsoleConnectionRedirection()
        {
            _rows = 25;
            _cols = 80;
            _termChars = ">";
        }
        
        public delegate void BufferRefreshed(object sender);
        public event BufferRefreshed BufferRefresh;
        
        protected void OnBufferRefreshed(object sender)
        {
            if (BufferRefresh != null)
            {
                BufferRefresh(sender);
            }
        }
        public string BufferTerminationCharacters
        {
            get { return _termChars; }
            set { _termChars = value; }
        }

        //todo this needs to read from the registry
        public int Rows { 
            get { return _rows; }
            set { _rows = value; } 
        }

        //todo this needs to read from the registry
        public int Columns
        {
            get { return _cols; }
            set { _cols = value; }
        }

        public List<string> ScreenData { get { return _screendata;}  }

 
        public void Connect()
        {
            var processStartInfo = new ProcessStartInfo(fileName, arguments);

            //  Set the options.
            processStartInfo.UseShellExecute = false;
            processStartInfo.ErrorDialog = false;
            processStartInfo.CreateNoWindow = true;

            //  Specify redirection.
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;

            //  Create the process.
            if (process!=null)
            {
                if (!process.HasExited)     //restart it if it's already started.
                {
                    process.Kill();
                    inputWriter.Close();
                    inputWriter = null;
                }                
            }
            

            process = new Process();
            
            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;
            process.Exited += process_Exited;

            //  Start the process.
            try
            {
                var processStarted = process.Start();

            }
            catch (Exception e)
            {
                //  Trace the exception.
                Trace.WriteLine("Failed to start process " + fileName + " with arguments '" + arguments + "'");
                Trace.WriteLine(e.ToString());
                return;
            }

            //  Create the readers and writers.
            inputWriter = process.StandardInput;
            outputReader = TextReader.Synchronized(process.StandardOutput);
            errorReader = TextReader.Synchronized(process.StandardError);

            //  Configure the output worker.
            outputWorker.WorkerReportsProgress = true;
            outputWorker.WorkerSupportsCancellation = true;
            outputWorker.DoWork += outputWorker_DoWork;
            outputWorker.ProgressChanged += outputWorker_ProgressChanged;

            //  Configure the error worker.
            errorWorker.WorkerReportsProgress = true;
            errorWorker.WorkerSupportsCancellation = true;
            errorWorker.DoWork += errorWorker_DoWork;
            errorWorker.ProgressChanged += errorWorker_ProgressChanged;

            if (!outputWorker.IsBusy)
            {
                outputWorker.RunWorkerAsync();
                errorWorker.RunWorkerAsync();
            }
            else
            {
                //re init workers
                outputWorker = new BackgroundWorker();
                errorWorker = new BackgroundWorker();

                outputWorker.WorkerReportsProgress = true;
                outputWorker.WorkerSupportsCancellation = true;
                outputWorker.DoWork += outputWorker_DoWork;
                outputWorker.ProgressChanged += outputWorker_ProgressChanged;

                //  Configure the error worker.
                errorWorker.WorkerReportsProgress = true;
                errorWorker.WorkerSupportsCancellation = true;
                errorWorker.DoWork += errorWorker_DoWork;
                errorWorker.ProgressChanged += errorWorker_ProgressChanged;

                outputWorker.RunWorkerAsync();
                errorWorker.RunWorkerAsync();
            }
            _isActive = true;
        }
        public bool EvalCondition(ScreenCondition screenCondition)
        {
            //check the caret location
            //todo need to implement ranges for the caret location
            
            ConsoleConnectionRedirectionScreenCondition current = (ConsoleConnectionRedirectionScreenCondition)GetCurrentScreenCondition();
            
            if (screenCondition.CaretLocation.Row == current.CaretLocation.Row &&
                screenCondition.CaretLocation.Column == current.CaretLocation.Column)
            {
                //now check the text
                string scrape = ScreenData[screenCondition.Row];
                scrape = scrape.Substring(screenCondition.Column, screenCondition.Text.Length);
                
                if (scrape.Equals(screenCondition.Text))
                {
                    return true;
                }
            }
            return false;
            
        }


        void errorWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
            //todo must handle bad input, like attempting to change to a non existant directory
        }

        void errorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (errorWorker.CancellationPending == false)
            {
                var count = 0;
                var buffer = new char[1024];
                do
                {
                    try
                    {
                        var builder = new StringBuilder();
                        int buf = errorReader.Peek();
                        count = errorReader.Read(buffer, 0, 1024);
                        
                        builder.Append(buffer, 0, count);
                        errorWorker.ReportProgress(0, builder.ToString());
                        if (count < 1024)
                        {
                            break;
                        }

                    }
                    catch (Exception e1)
                    {
                        Console.WriteLine(e1.InnerException.ToString());
                    }
                } while (count > 0);

                Thread.Sleep(50);
            }

        }

        void outputWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void ProcessOutput(string output)
        {
            string temp;
            int start = 0;

            if (!string.IsNullOrEmpty(_lastInput))
            {
                if(output.Length == _lastInput.Length)
                {
                    if (output.Substring(0, _lastInput.Length).Equals(_lastInput))
                    {
                        return;
                    }

                }
            }

            _buffer += output;
            if (_buffer.Substring(_buffer.Length - _termChars.Length).Equals(_termChars)) //are we done collecting info? use _termChars to find out
            {
                //we're done. parse the info 
                string[] data = _buffer.Split(new[] { "\r\n" }, StringSplitOptions.None);
                foreach (string looper in data)
                {
                    if(!looper.Equals(_lastInput))
                    {
                        if (looper.Length > _cols)
                        {
                            start = 0;
                            while (start < looper.Length)
                            {
                                if ((start + _cols) > looper.Length)
                                {
                                    temp = looper.Substring(start); //todo really should be regex
                                    if (temp.Length < _cols)
                                    {
                                        //pad it
                                        temp = temp.PadRight(_cols);
                                    }
                                }
                                else
                                {
                                    temp = looper.Substring(start, _cols);
                                }

                                _screendata.Add(temp);
                                start += _cols;
                            }
                        }
                        else
                        {
                            if (looper.Length < _cols)
                            {
                                _screendata.Add(looper.PadRight(_cols));
                            }
                            else
                            {
                                _screendata.Add(looper);
                            }

                        }

                    }
                    
                }
                //reset the buffer.
                _buffer = "";

                //only take the last Rows() amount of rows.
                while (_screendata.Count > this.Rows)
                {
                    _screendata.RemoveAt(0);
                }
                OnBufferRefreshed(this);
                _isConnected = true;
            }
        }

        void outputWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (outputWorker.CancellationPending == false)
            {
                //  Any lines to read?
                var count = 0;
                var buffer = new char[1024];
                do
                {
                    try
                    {
                        var builder = new StringBuilder();
                        count = outputReader.Read(buffer, 0, 1024); // This will trigger an untrappable error if the last Read reads in the entire contents of the bufffer.
                                                                    // so we need to check if the read pulls in exactly 1024 bytes. if so, there may be more in the buffer to read.
                                                                    // exit out so we can read the remainder of the buffer. then it can feel free to explode.
                        builder.Append(buffer, 0, count);
                        
                        if (count < 1024)
                        {
                            ProcessOutput(builder.ToString());
                            break;  //we're done, there's nothing left in the buffer.
                        }
                    }
                    catch (Exception e1)
                    {
                        Console.WriteLine(e1.InnerException.ToString());
                    }
                } while (count > 0);

                Thread.Sleep(50);
            }
            e.Cancel = true;
        }

        void process_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Send(string input)
        {

            if (!_isActive)
            {
                throw new Exception("Not connected.");
            }

            string[] keysToWatch = new[] {"{ENTER}"};
            string[] strings = input.Split(keysToWatch,StringSplitOptions.RemoveEmptyEntries);

            foreach (string looper in strings)
            {
                _lastInput = looper;
                //add input to the buffer
                string newRowData = _screendata[_screendata.Count - 1].Trim() + looper;
                _screendata[_screendata.Count - 1] = newRowData.PadRight(_cols);
                //_isConnected = false;
                //process.StandardInput.WriteLine(looper);
                inputWriter.WriteLine(looper);
                //OnBufferRefreshed(this);
                //Thread.Sleep(1000);
                inputWriter.Flush();
                //Thread.Sleep(1000);
                //inputWriter.Close();

                //while (_isConnected == false)
                //{
                //    Thread.Sleep(1000);
                //}
                //_screendata
            }
        }

        public string Scrape(int Row, int Column, int Length)
        {
            if (Row > _rows || Row < 0)
            {
                throw new Exception("Invalid Row.");
            }
            if (Column > _cols || Column < 0)
            {
                throw new Exception("Invalid Column.");
            }
            if ((Length + Column) > _cols)
            {
                throw new Exception("Invalid Length.");
            }

            return _screendata[Row].Substring(Column, Length);
        }

        public ICondition GetCurrentScreenCondition()
        {
            var caretRow = _screendata.Count - 1;
            var caretCol = _screendata[caretRow].Length - 1;

            var caret = new CaretLocation(caretRow,caretCol );
            return new ConsoleConnectionRedirectionScreenCondition()
            {
                CaretLocation = caret,
                Column = 0,
                Row = caretRow,
                Text = _screendata[_screendata.Count - 1]
            };
        }


    }

}
