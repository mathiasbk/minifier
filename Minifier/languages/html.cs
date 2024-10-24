namespace languages;

using System.Text.RegularExpressions;

class html
{
    public string MinifyHTML(string html)
    {
        // Fjern HTML-kommentarer
        html = Regex.Replace(html, @"<!--[\s\S]*?-->", "");  // Remove new lines and spaces
        html = Regex.Replace(html, @">\s+<", "><"); // Remove spaces between tags
        html = Regex.Replace(html, @"\s{2,}", " "); // Remove spaces
        
        return html.Trim();
    }
}