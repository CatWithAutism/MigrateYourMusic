using IniParser;
using IniParser.Model;

namespace Tests
{
    public static class Utils
    {
        public static IniData ReadFile(string path)
        {
            return new FileIniDataParser().ReadFile(path);
        }
    }
}