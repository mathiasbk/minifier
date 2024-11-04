

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
                    MinifyHTMLfile(path);
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
            //TODO: use LINQ
            if(Path.GetExtension(file) == ".html" && !file.EndsWith(".min.html"))
            {
                MinifyHTMLfile(file);
            }
            
           if(Path.GetExtension(file) == ".js" && !file.EndsWith(".min.js"))
            {
                Js jsclass = new Js();
                String filecontent = File.ReadAllText(file);
                string newJSfilecontent = jsclass.MinifyJS(filecontent);

                CreateMinimizedFile(file, newJSfilecontent);
            }
            if(Path.GetExtension(file) == ".css" && !file.EndsWith(".min.css"))
            {
                Css cssclass = new Css();
                String filecontent = File.ReadAllText(file);
                string newCSSfilecontent = cssclass.MinifyCSS(filecontent);

                CreateMinimizedFile(file, newCSSfilecontent);
            }
        }
    }

    public static void MinifyHTMLfile(string path)
    {
        string filecontent = File.ReadAllText(path);

        Html htmlclass = new Html();
        Js jsclass = new Js();
        Css cssclass = new Css();

        //regex to identify the type of script
        string scriptPattern = @"<script\b[^>]*>([\s\S]*?)<\/script>";
        string stylePattern = @"<style\b[^>]*>([\s\S]*?)<\/style>";

        //Minify JS
        filecontent = Regex.Replace(filecontent, scriptPattern, match =>
        {          
            string scriptTags = match.Value;
            string scriptContent = match.Groups[1].Value;

            string minimisedScript = jsclass.MinifyJS(scriptContent);

            return scriptTags.Replace(scriptContent, minimisedScript);
        });

        //Minify CSS
        filecontent = Regex.Replace(filecontent, stylePattern, match =>
        {
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
        try
        {
            Console.WriteLine("Creating new file at: " + NewFileName + ". old is: " + path);
            //Write to file
            File.WriteAllText(NewFileName, content);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to file: " + ex.Message);
        }

        //wait for user input before closing
        Console.WriteLine("Press any key to close");
        try { Console.ReadKey(); } catch { }
        
    } 
}
