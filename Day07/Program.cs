using System.Collections;
using System.Text;
using System.Threading.Channels;

namespace Day7;

static class Program
{
    public static void Main(string[] args)
    {
        NewFile root = new NewFile(null!, "/", 0);

        string[] lines = File.ReadAllLines("C:\\Users\\Arthur\\Desktop\\Arthur\\aderugy\\AdventOfCode\\Day7\\input");
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            line = line.Replace("\n", "");
            if (line.Length < 2) continue;

            if (line.Contains("cd"))
            {
                root = root.ChangeDirectory(line);
                continue;
            }

            if (!line.Contains("ls")) continue;
            ArrayList strings = new ArrayList();

            for (i += 1 ; i < lines.Length; i++)
            {
                if (lines[i][0] == '$') break;
                strings.Add(lines[i]);
            }

            i--;

            root.ListFiles(strings);
        }

        long toFree =  30000000 - (70000000 - root.GetRoot().GetSize());
        long min = 70000000;

        foreach (long i in root.GetRoot().Execute())
        {
            if (i < min && i >= toFree) min = i;
        }
        Console.WriteLine(min);
    }

    public class NewFile
    {
        protected long Size;
        protected string Name;
        protected bool IsDirectory;
        public ArrayList Files;
        protected NewFile? Parent;


        public static void Concat(ArrayList l1, ArrayList l2)
        {
            foreach (var e in l2)
            {
                l1.Add(e);
            }
        }
        public ArrayList Execute()
        {
            if (!IsDirectory) return new ArrayList();

            ArrayList toReturn = new ArrayList();
            foreach (NewFile f in Files)
            {
                if (!f.IsDirectory) continue;

                toReturn.Add(f.GetSize());
                Concat(toReturn, f.Execute());
            }

            return toReturn;
        }
        
        public NewFile(NewFile parent, string name, int size, bool isDirectory)
        {
            Parent = parent;
            Name = name;
            Size = size;
            IsDirectory = isDirectory;
            Files = new ArrayList();
        }

        public NewFile(NewFile parent, string name, int size)
        {
            Parent = parent;
            Name = name;
            Size = size;
            IsDirectory = size == 0;
            Files = new ArrayList();
        }

        public NewFile GetRoot()
        {
            NewFile current = this;

            while (current.Parent != null)
            {
                current = current.Parent;
            }

            return current;
        }

        public NewFile ChangeDirectory(string param)
        {
            if (param.Contains("..")) return Parent!;
            if (param.Contains("/")) return GetRoot();

            string name = param[5..];

            return Files.Cast<NewFile>().FirstOrDefault(f => f.Name.Equals(name))!;
        }

        public void ListFiles(ArrayList strings)
        {
            foreach (string s in strings)
            {
                string[] result = s.Replace("\n", "").Split(' ');
                AddFile(result[0].Equals("dir")
                    ? new NewFile(this, result[1], 0)
                    : new NewFile(this, result[1], Convert.ToInt32(result[0])));
            }
        }

        public void AddFile(NewFile f)
        {
            Files.Add(f);
        }

        public long GetSize()
        {
            return !IsDirectory ? Size : Files.Cast<NewFile>().Sum(file => file.GetSize());
        }

        public override string ToString()
        {
            StringBuilder toReturn = new StringBuilder();
            toReturn.Append($"Parent: {(Parent is null ? "/" : Parent.Name)} - " +
                            $"Name: {Name} - " +
                            $"{(IsDirectory ? "(dir)" : "(file)")} - Size: {GetSize()}\n");

            if (!IsDirectory) return toReturn.ToString();
            
            foreach (NewFile f in Files)
            {
                toReturn.Append($"-> {f.ToString()}");
            }

            return toReturn.ToString();
        }
    }
}

