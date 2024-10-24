

namespace Minifier;

using System;
using System.IO;
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

        html htmlclass = new html();
        js jsclass = new js();
        css cssclass = new css();

        var CurrentSCriptType = "html";
        var newContent = "";


        //Puts each line of the file in a string array
        var lines = File.ReadAllLines(path);

        foreach(var line in lines)
        {
            //Console.WriteLine(line);
            //Identify the type of script
            if(line.Contains("<script"))
            {
                //Start of JS
                CurrentSCriptType = "js";
            }
            else if(line.Contains("</script>"))
            {
                //End of JS
                CurrentSCriptType = "html";
            }
            else if(line.Contains("<style"))
            {
                //Start of CSS
                CurrentSCriptType = "css";
            }
            else if(line.Contains("</style>"))
            {
                //End of CSS
                CurrentSCriptType = "html";
            }
            
            if(CurrentSCriptType == "html")
            {
                //minif HTML
                newContent += htmlclass.MinifyHTML(line);
            }
            else if(CurrentSCriptType == "js")
            {
                //Minify JS
                newContent += jsclass.MinifyJS(line);
            }
            else if(CurrentSCriptType == "css")
            {
                //Minify CSS3
                newContent += cssclass.MinifyCSS(line);

            }

            
        }
        //Everything is minimised, so we create the new file
        CreateMinimizedFile(path, newContent);
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
