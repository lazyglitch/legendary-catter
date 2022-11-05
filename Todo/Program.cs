
using System.Collections;
using System.Diagnostics;
using System.Text;
using TodoModels;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.Unicode;

Logic logic = new Logic();

Console.WriteLine("Hi!");

bool stat = true;

while(stat){

    Console.WriteLine("0-Закрыть\n1-Войти\n2-Регистрация");

    int num_s;

    string? answ = Console.ReadLine();

    while (!Int32.TryParse(answ, out num_s) || num_s <0 || num_s >2)
    {
            RedConsole("Invalid input");

            answ = Console.ReadLine();
    }

    switch(answ){

        case "0":
            stat=false;
            break;
        case "1":
            if(Login()){
                stat=false;
            }
            break;
        case "2":
            if(Signup()){
                stat=false;
            }
            break;
    }
}

void Start(string name){

    bool onLoad = true;

    while(onLoad){
        Console.WriteLine("0-Выход\n1-Добавить\n2-Удалить\n3-Вывести список\n4-Редактировать");
        
        int num;

        string? answ = Console.ReadLine();

        while (!Int32.TryParse(answ, out num) || num <0 || num>4)
        {
                RedConsole("Invalid input");

                answ = Console.ReadLine();
        }

        switch(answ){

            case "0":
                onLoad=false;
                break;
            case "1":
                AddNew();
                break;
            case "2":
                Delete();
                break;
            case "3":
                GetNotes();
                break;
            case "4":
                Edit();
                break;
        }
    }
}

void GetNotes(){    
    Console.Clear();
    
    var notes = logic.GetAllNotes();
    
    if(notes.Count==0)
    {
        Console.WriteLine("Список пуст...\n");
    }

    int i =0;

    foreach(Note n in notes)
    {
        string status = n.Status ? "Выполняется" : "Завершено";

        Console.WriteLine($"{i}) {n.Content} ({status})");
        i++;
    }
    Console.WriteLine();
}

void Edit(){

    GetNotes();

    Console.WriteLine("Выберите запись для редактирования\nВведите id:");

    int ex;
    var answ = Console.ReadLine();

    while (String.IsNullOrWhiteSpace(answ) || !Int32.TryParse(answ, out ex) || ex <0 || ex>logic.GetAllNotes().Count)
    {
        RedConsole("Invalid input");

        answ = Console.ReadLine();
    }
    Console.WriteLine("Если изменения не требуются нажмите пробел");
    
    Console.WriteLine("Введите новый текст:");
    
    string text = Console.ReadLine();

    Console.WriteLine("Введите статус\n0-завершено\n1-выполняется");

    string status  = Console.ReadLine();

    logic.EditNote(Int32.Parse(answ), text, status);

}

void Delete(){
    
    GetNotes();

    Console.WriteLine("Введите id:");

    int ex;

    string answ = Console.ReadLine();

    while (!Int32.TryParse(answ, out ex) || ex <0 || ex>logic.GetAllNotes().Count)
    {
        RedConsole("Invalid input");

        answ = Console.ReadLine();
    }

    logic.DeleteNote(Int32.Parse(answ));
    
}

void AddNew(){

    Console.WriteLine("Введите текст:");

    string text = Console.ReadLine();

    logic.AddNewNote(text);
}

bool Login(){
    
    Console.WriteLine("Введите имя:");

    string name = Console.ReadLine();

    if(logic.LogIn(name) && !String.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Пользователь авторизирован!");

        Start(name);

        return true;
    }
    RedConsole("Incorrect data");
    return false;
}

bool Signup(){

    Console.WriteLine("Введите имя:");

    string name = Console.ReadLine();

    if(logic.Registration(name) && !String.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Успех!");

        logic.LogIn(name);

        Start(name);

        return true;
    }
    RedConsole("Такой пользователь уже есть");
    return false;
}

static void RedConsole(string text)
{
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine(text);

    Console.ResetColor();
}