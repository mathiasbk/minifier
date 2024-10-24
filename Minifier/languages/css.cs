namespace languages;

using System.Text.RegularExpressions;

class css
{
    public string MinifyCSS(string css)
    {
        // Fjern kommentarer
        css = Regex.Replace(css, @"\/\*[\s\S]*?\*\/", ""); // Remove new line and spaces
        css = Regex.Replace(css, @"\s+", " ");
        css = Regex.Replace(css, @"\s*([{};,:])\s*", "$1"); // Remove spaces
        return css.Trim();
    }
}