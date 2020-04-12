using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BibliaConsoleApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string lastText = string.Empty;
            string text = string.Empty;

            Console.WriteLine("Program do konwersji tekstu na bibliografię. Wciśnij klawisz \\ po skopiowaniu tekstu, aby uruchomić parser");

            while (true)
            {

                string start = Console.ReadLine();
                if(start == "\\")
                {

                try
                {
                    if (Clipboard.ContainsText(TextDataFormat.Text))
                    {
                        text = Clipboard.GetText(TextDataFormat.Text);
                        if (string.IsNullOrEmpty(text) || lastText.Equals(text)) { }
                        else if (text.Length > 15)
                        {
                            lastText = text;
                            Console.WriteLine(text);

                            var tab = text.Split('\n');
                            Console.WriteLine("START ---------- SPLITER ------------------");
                            string title = "", authors = "", extra = "", article = "", year = "", volumePaper = "";
                            if (tab != null && tab.Length == 3)
                            {
                                title = tab[0].Replace('\r', ' ').Trim();
                                authors = tab[1].Replace('\r', ' ').Trim();
                                extra = tab[2].Replace('\r', ' ').Trim();

                                var names = authors.Split(';');
                                if (names != null)
                                {
                                    authors = "";
                                    int i = 0;
                                    foreach (string name in names)
                                    {
                                        i++;
                                        if (i >= 4)
                                        {
                                            authors = authors.Substring(0, authors.Length - 2) + " i inni, ";
                                            break;
                                        }
                                                
                                        var person = name.Split(',');
                                        if (person != null)
                                        {
                                            if (person.Length == 1) authors += person;
                                            else if (person.Length == 2)
                                            {
                                                authors += person[0];
                                                var firstNames = person[1].Split('.');
                                                if (firstNames != null)
                                                {
                                                    foreach (string fn in firstNames)
                                                    {
                                                        string fff = fn.Trim().Substring(0, 1);
                                                        authors += " " + fff + ".";
                                                    }
                                                }
                                            }
                                            authors += ", ";
                                        }
                                    }
                                }

                                var extras = extra.Split(',');

                                if (extras != null && extras.Length >= 2)
                                {
                                    int i = 0;
                                    foreach (string e in extras)
                                    {
                                        i++;
                                        if (i == 1)
                                        {
                                            article = "\"" + e + "\",";
                                        }
                                        else if (i == 2)
                                        {
                                            string tempYear = e.Trim();
                                            if(tempYear.Length>4)
                                            {
                                                tempYear = " " + tempYear.Substring(tempYear.Length - 4);
                                            }
                                            year = tempYear + " r.,";
                                        }
                                        else
                                        {
                                            string ee = "";
                                            ee += e.Replace("Vol.", "tom ").Replace("pp.", "str. ").Replace("p.", "str. ");
                                            volumePaper += ee;
                                            if (i == extras.Length) volumePaper += ".";
                                            else volumePaper += " ,";
                                        }
                                    }
                                }
                            }

                            string result = authors + title + ", " + article + year + volumePaper;

                            result = result.Replace(" ,", ",").Replace("  ", " ");

                            Clipboard.SetText(result);
                            lastText = result;
                            Console.WriteLine("Wynik: " + result);


                        }


                       
                        
                    }
                    //Console.WriteLine(lastText);
                    Thread.Sleep(1000);

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                }
            }
            
        }
        



        
    }
}
