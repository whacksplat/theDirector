using System.Collections.Generic;

namespace DirectorAPI.Interfaces
{
    public interface ICharacterConnection
    {
        //properties
        string BufferTerminationCharacters { get; set; }
        int Rows { get; set; }
        int Columns { get; set; }
        List<string> ScreenData { get; }

        //methods
        void Connect();
        void Send(string input);
        string Scrape(int Row, int Column, int Length); 
    }
}