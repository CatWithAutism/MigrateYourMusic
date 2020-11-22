using IniParser;
using IniParser.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    public static class Utils
    {
        public static IniData ReadFile(string path)
            => new FileIniDataParser().ReadFile(path);
    }
}
