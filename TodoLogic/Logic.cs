using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;

using TodoModels;

namespace TodoLogic{}
public class Logic
{
    private static string path = @"..\Storage\";
    private string currentUser {get; set;} 
    private List<string> users = new List<string>();
    private List<Note> UserNotes = new List<Note>();
    public Logic()
    {
        using(StreamReader sr = File.OpenText("..\\Storage\\users.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                users.Add(s);
            }
        }
    }
    public bool Registration(string name)
    {
        if(users.Contains(name))
        {
            return false;
        }

        Directory.CreateDirectory(Path.Combine(path, name));


       using (StreamWriter s = File.AppendText(Path.Combine(path, "users.txt")))
            {
                s.WriteLine(name);
            } 

        users.Add(name);

        return true;
    }
    public bool LogIn(string name){
        
        if(users.Contains(name) && Directory.Exists(Path.Combine(path,name)))
        {
            currentUser = name;

            path = Path.Combine(path, name);

            NotesToList();

            return true;
        }
        return false;
    }
    public void AddNewNote(string text)
    {
        Note note = new Note(text, currentUser, "active_"+Guid.NewGuid().ToString(), true);

        UserNotes.Add(note);

        using (StreamWriter fs = File.CreateText(Path.Combine(path,$"{note.Title}.txt")))
        {    
              fs.WriteLine(text);    
        }
    }
    public void DeleteNote(int id)
    {
        Note note = UserNotes[id];
        
        if (File.Exists(Path.Combine(path, $"{note.Title}.txt")))
        {
            File.Delete(Path.Combine(path, $"{note.Title}.txt"));
        }
        UserNotes.RemoveAt(id);
    }
    public List<Note> GetAllNotes()
    {
        NotesToList();
        return UserNotes;
    }
    public void EditNote(int id, string text, string status)
    {
        string n_title = UserNotes[id].Title;
        
        if(!String.IsNullOrWhiteSpace(text))
        {
            using (StreamWriter fs = File.CreateText(Path.Combine(path,$"{n_title}.txt")))
            {    
                fs.WriteLine(text);    
            } 
        }
        if(!String.IsNullOrWhiteSpace(status)){

            string stat = status == "1" ? "active": "completed";

            FileInfo fi = new FileInfo(Path.Combine(path,$"{n_title}.txt"));
            string pref =  n_title.Substring(n_title.LastIndexOf("_")+1);
            fi.MoveTo(Path.Combine(path,$"{stat}_{pref}.txt"));
        }

    }
    public void NotesToList()
    {
        UserNotes = new List<Note>();

        string[] filePaths = Directory.GetFiles(path, "*.txt");

        if(filePaths.Length == 0) return;

        foreach(string f in filePaths)
        {
            string fPath = f.Substring(path.Length + 1);

            string fName = fPath.Substring(0, fPath.LastIndexOf("."));

            string fStatus = fPath.Substring(0, fPath.LastIndexOf("_"));

            string content = "";
            
            using (StreamReader sr = File.OpenText(f))    
            {    
                string s = "";    
                while ((s = sr.ReadLine()) != null)    
                {    
                    content+=s;   
                }    
            }
            bool status = fStatus == "completed"? false : true;

            Note note_to_add = new Note(content, currentUser,fName , status);

            UserNotes.Add(note_to_add);
        }
    }
}

