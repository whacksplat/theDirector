using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessInterface;

namespace ConsoleConnect
{
    public class Connect
    {
        private ProcessInterface.ProcessInterface processInterace = new ProcessInterface.ProcessInterface();
        private List<string> screendata = new List<string>();
        private char[,] screenbuffer = new char[25, 80];

        private List<Condition> Conditions = new List<Condition>();

        public CaretLocation CurrentLocation = new CaretLocation();
        private string _lastInput;
        private string _buffer;


        public delegate void BufferRefreshed(object sender);
        public event BufferRefreshed BufferRefresh;

        /// <summary>
        /// 
        /// </summary>
        public void EvalConditions()
        {
            foreach (Condition condition in Conditions)
            {
                string data = screendata[condition.Row].Substring(condition.Column, condition.Text.Length);
                if (data.Equals(condition.Text))
                {
                    //make sure the caret is where it's supposed to be
                    if ((CurrentLocation.Column == condition.CaretLocation.Column) &&
                        (CurrentLocation.Row == condition.CaretLocation.Row))
                    {
                        WriteInput(condition.Action);
                        return;
                    }
                }

            }
        }



        public void AddCondition(Condition condition)
        {
            Conditions.Add(condition);
        }

        protected void OnBufferRefreshed()
        {
            BufferRefresh(this);
        }

        public Connect()
        {
            processInterace.OnProcessError += processInterace_OnProcessError;
            processInterace.OnProcessExit += processInterace_OnProcessExit;
            processInterace.OnProcessInput += processInterace_OnProcessInput;
            processInterace.OnProcessOutput += processInterace_OnProcessOutput;
        }

        public void WriteInput(string input)
        {
            _lastInput = input + "\r\n";
            processInterace.WriteInput(input);
        }

        void processInterace_OnProcessOutput(object sender, ProcessEventArgs args)
        {
            if (args.Content.Equals(_lastInput))
            {
                screendata[screendata.Count - 1] = screendata[screendata.Count - 1] + args.Content.Replace("\r\n", "");
                LoadScreenBuffer();
                return;
            }

            //screen size is 80 by 25
            if (args.Content.Equals("\f"))
            {
                screendata.Clear();
            }
            else
            {
                //for the purposes of this demonstration, output termination marker will be '>'
                //thinking that it would be best if the caret location from Console API / colum from buffer might be better.
                if (args.Content.Length == 0)   //console has probably been reset
                {
                    return;
                }
                string[] data = args.Content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                _buffer += args.Content;
                if (_buffer.Substring(_buffer.Length - 1).Equals(">"))
                {
                    //we've hit output termination marker. parse it
                    data = _buffer.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    _buffer = "";
                    //add it to the list
                    for (int i = 0; i < data.Length; i++)
                    {
                        screendata.Add(data[i]);
                    }

                    //parse it
                    LoadScreenBuffer();

                    //do we have any conditions to process?
                    if (Conditions.Count != 0)
                    {
                        EvalConditions();
                    }
                }
            }

            //if (screendata.Count == 0)
            //{
            //    CurrentLocation.Column = 0;
            //}
            //else
            //{
            //    CurrentLocation.Column = screendata[screendata.Count - 1].ToString().Length;
            //}
        }

        void processInterace_OnProcessInput(object sender, ProcessEventArgs args)
        {
            throw new NotImplementedException();
        }

        void processInterace_OnProcessExit(object sender, ProcessEventArgs args)
        {
            //do we need to bubble this up?
            //throw new NotImplementedException();
        }

        void processInterace_OnProcessError(object sender, ProcessEventArgs args)
        {
            //why is this firing?
            //throw new NotImplementedException();
        }

        public void StartProcess(string fileName, string arguments)
        {
            if (processInterace.IsProcessRunning)
            {
                processInterace.WriteInput("exit");
                System.Threading.Thread.Sleep(1000);
                processInterace.StopProcess();
                while (processInterace.IsProcessRunning)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            System.Threading.Thread.Sleep(200);
            processInterace.StartProcess("cmd.exe", null);

        }

        public void StopProcess()
        {
            processInterace.StopProcess();
            //processInterace = null;
        }

        public void LoadScreenBuffer()
        {
            List<string> temp = new List<string>();
            Int32 length = 80;

            //check to make sure length <80
            foreach (string loop in screendata)
            {
                if (loop.Length > 80)
                {
                    //need to split this up
                    for (int i = 0; i < loop.Length; i += 80)
                    {
                        if ((loop.Length - i) < 80)
                        {
                            length = loop.Length - i;
                        }
                        temp.Add(loop.Substring(i, length));
                    }
                }
                else
                {
                    temp.Add(loop);
                }
            }

            //make sure screenbuffer only holds 25 rows
            while (temp.Count > 25)
            {
                temp.RemoveAt(0);
            }

            //make sure the length is not greater than 80 chars

            //string rowdata;

            //reset the screenbuffer to repopulate
            screenbuffer = new char[25, 80];

            for (int screendatarow = 0; screendatarow < temp.Count(); screendatarow++)
            {
                char[] rowchar = temp[screendatarow].ToCharArray();
                for (int screendatacol = 0; screendatacol < 80; screendatacol++)
                {
                    if (rowchar.Length != 0)
                    {
                        if (screendatacol >= rowchar.Length)
                        {
                            screenbuffer[screendatarow, screendatacol] = ' ';  //delete anything currently existing in that element.
                        }
                        else
                        {
                            screenbuffer[screendatarow, screendatacol] = rowchar[screendatacol];
                        }
                    }
                    else
                    {
                        //it's a blank row, populate with blanks
                        screenbuffer[screendatarow, screendatacol] = ' ';
                    }
                }
            }

            //need to figure out our row/col
            CurrentLocation.Row = screendata.Count - 1;
            CurrentLocation.Column = screendata[screendata.Count - 1].Length + 1;

            //need to raise event that the screen buffer is correctly populated
            OnBufferRefreshed();
        }

        public string Scrape(ScrapeLocation loc)
        {
            string retval = "";
            Int32 length = loc.Column + loc.Length;
            if (length > 80)
            {
                length = 80;
            }

            for (int i = loc.Column; i < length; i++)
            {
                retval += screenbuffer[loc.Row, i].ToString();
            }
            return retval;
        }

        public string Scrape()
        {
            string retval = "";
            for (int row = 0; row < screenbuffer.GetUpperBound(0) + 1; row++)
            {
                for (int col = 0; col < screenbuffer.GetUpperBound(1); col++)
                {
                    retval += screenbuffer[row, col].ToString();
                }
                retval += Environment.NewLine;
            }
            return retval;
        }
    }

    public class CaretLocation
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
    }

    public class ScrapeLocation
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public Int32 Length { get; set; }
        public ScrapeLocation(Int32 Row, Int32 Column, Int32 Length)
        {
            this.Row = Row;
            this.Column = Column;
            this.Length = Length;
        }
        public ScrapeLocation()
        {
        }
    }

    public class Condition
    {
        public Int32 Row { get; set; }
        public Int32 Column { get; set; }
        public string Text { get; set; }
        public CaretLocation CaretLocation { get; set; }
        public string Action { get; set; }
    }
}
