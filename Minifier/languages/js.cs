namespace languages;

using System.Text.RegularExpressions;

class Js
{
    public string MinifyJS(string js)
    {
        //return js;
        js = Regex.Replace(js, @"\/\/[^\r\n]*", ""); // Remove single line comments
        js = Regex.Replace(js, @"\/\*[\s\S]*?\*\/", ""); // Remove multiline comments
        js = Regex.Replace(js, @"\s+", " "); //Remove whitespace
        js = Regex.Replace(js, @"\s*([{};,:=()<>])\s*", "$1"); // Remove spesific characters
        return js.Trim();
    }
}