

namespace Minifier;

using System;
using System.IO;
using System.Text.RegularExpressions;
using languages;

class Program
{
    static void Main(string[] args)
    {
        if(args.Length == 0)
        {
            //No arguments, so we scan the folder to find all html files in the same folder.
            ScanDirectory(Directory.GetCurrentDirectory());
        }
        else
        {
            foreach(string path in args)
            {
                if(File.Exists(path))
                {
                    Minifyfile(path);
                }
                else
                {
                    Console.WriteLine("File not found: " + path);
                }
            }

        }
    }

    static void ScanDirectory(string path)
    {
        string[] files = Directory.GetFiles(path);

        foreach(string file in files)
        {
            if(file.EndsWith(".html"))
            {
                Minifyfile(file);
            }

        }
    }

    public static void Minifyfile(string path)
    {
        Console.WriteLine("Minifying file: " + path);

        string filecontent = File.ReadAllText(path);

        html htmlclass = new html();
        js jsclass = new js();
        css cssclass = new css();

        //regex to identify the type of script
        string scriptPattern = @"<script\b[^>]*>([\s\S]*?)<\/script>";
        string stylePattern = @"<style\b[^>]*>([\s\S]*?)<\/style>";

        //Minify JS
        filecontent = Regex.Replace(filecontent, scriptPattern, match =>
        {
            Console.WriteLine("Match found: " + match.Value);
            
            string scriptTags = match.Value;
            string scriptContent = match.Groups[1].Value;

            string minimisedScript = jsclass.MinifyJS(scriptContent);

            return scriptTags.Replace(scriptContent, minimisedScript);
        });

        //Minify CSS
        filecontent = Regex.Replace(filecontent, stylePattern, match =>
        {
            Console.WriteLine("Match found: " + match.Value);
            
            string styleTags = match.Value;
            string styleContent = match.Groups[1].Value;

            string minimisedStyle = cssclass.MinifyCSS(styleContent);

            return styleTags.Replace(styleContent, minimisedStyle);
        });
            
        filecontent = htmlclass.MinifyHTML(filecontent);
        //Everything is minimised, so we create the new file
        CreateMinimizedFile(path, filecontent);
    }

    public static void CreateMinimizedFile(string path, string content)
    {
        
        var extension = Path.GetExtension(path);
        var NewFileName = Path.GetDirectoryName(path) +"\\"+ Path.GetFileNameWithoutExtension(path) + ".min" + extension;
        Console.WriteLine("Creating new file at: " + NewFileName);
        //Write to file
        File.WriteAllText(NewFileName, content);
    } 
}
