namespace Minifier;

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
    }
}
