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

namespace DirectorAPI
{
    class ScrapeLocation
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
}
