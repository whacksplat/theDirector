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

//namespace DirectorAPI.ConsoleConnection
//{
//  /// <summary>
//  /// The ProcessEventArgs are arguments for a console event.
//  /// </summary>
//  public class ProcessEventArgs : EventArgs
//  {
//    /// <summary>
//    /// Initializes a new instance of the <see cref="ConsoleEventArgs"/> class.
//    /// </summary>
//    public ProcessEventArgs()
//    {
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="ConsoleEventArgs"/> class.
//    /// </summary>
//    /// <param name="content">The content.</param>
//    public ProcessEventArgs(string content)
//    {
//        //  Set the content and code.
//        Content = content;
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="ConsoleEventArgs"/> class.
//    /// </summary>
//    /// <param name="code">The code.</param>
//    public ProcessEventArgs(int code)
//    {
//        //  Set the content and code.
//        Code = code;
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="ConsoleEventArgs"/> class.
//    /// </summary>
//    /// <param name="content">The content.</param>
//    /// <param name="code">The code.</param>
//    public ProcessEventArgs(string content, int code)
//    {
//      //  Set the content and code.
//      Content = content;
//      Code = code;
//    }

//    /// <summary>
//    /// Gets the content.
//    /// </summary>
//    public string Content
//    {
//      get;
//      private set;
//    }

//    /// <summary>
//    /// Gets or sets the code.
//    /// </summary>
//    /// <value>
//    /// The code.
//    /// </value>
//    public int? Code
//    {
//        get;
//        private set;
//    }
//  }
//}
