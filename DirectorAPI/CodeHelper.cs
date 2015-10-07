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
using System.CodeDom.Compiler;
using System.Reflection;

namespace DirectorAPI
{
    public static class CodeHelper
    {
        public static CompilerResults CreateConditionCode(Automation automation, string code)
        {
            var src = "using System;";
            src += Environment.NewLine + "using System.Windows.Forms;";
            src += Environment.NewLine + "using DirectorAPI;";
            src += Environment.NewLine + "namespace ConditionCode";
            src += Environment.NewLine + "{";
            src += Environment.NewLine + "    class Program";
            src += Environment.NewLine + "    {";
            src += Environment.NewLine + "        public bool TestValue(ref DirectorAPI.Automation automation)";
            src += Environment.NewLine + "        {";
            src += Environment.NewLine + code.Replace("\\","\\\\");
            src += Environment.NewLine + "        }";
            src += Environment.NewLine + "    }";
            src += Environment.NewLine + "}";

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var prms = new CompilerParameters();
            prms.GenerateExecutable = false;
            prms.GenerateInMemory = true;

            prms.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            prms.ReferencedAssemblies.Add("System.dll");
            prms.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            return provider.CompileAssemblyFromSource(prms, src);
        }

        public static CompilerResults CreateActionCode(string code)
        {
            //TODO must remove ref statement, maybe a singleton class?
            var src = "using System;";
            src += Environment.NewLine + "using System.Windows.Forms;";
            src += Environment.NewLine + "using DirectorAPI;";
            src += Environment.NewLine + "using DirectorAPI.Connections;";
            src += Environment.NewLine + "using DirectorAPI.Interfaces;";
            src += Environment.NewLine + "namespace ActionCode";
            src += Environment.NewLine + "{";
            src += Environment.NewLine + "    class Program";
            src += Environment.NewLine + "    {";
            src += Environment.NewLine + "        public void Execute(ref DirectorAPI.Automation automation)";
            src += Environment.NewLine + "        {";
            src += Environment.NewLine + code.Replace("\\", "\\\\");
            src += Environment.NewLine + "        }";
            src += Environment.NewLine + "    }";
            src += Environment.NewLine + "}";

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var prms = new CompilerParameters();
            prms.GenerateExecutable = false;
            prms.GenerateInMemory = true;
            prms.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            prms.ReferencedAssemblies.Add("System.dll");
            prms.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            return provider.CompileAssemblyFromSource(prms, src);

        }

    }
}
