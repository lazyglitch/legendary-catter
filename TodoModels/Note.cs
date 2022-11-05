namespace TodoModels;
public class Note
{
    public string User {get;}
    public string Title {get; set;}
    public string Content {get; set;}
    public bool Status {get; set;}
    public Note(){}
    public Note(string text, string user, string title, bool status)
    {
        User=user;
        Content=text;
        Title= title;
        Status = status;
    }

}