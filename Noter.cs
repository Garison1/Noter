using System;
using System.IO;
using System.Linq;
using System.Collections;
using Newtonsoft.Json;

namespace program {

    struct NOTE {
        public string name;
        public string phone;
        public int[] birth;
    }
    class Program {
        public static NOTE[] friends = new NOTE[0];
        static void Main() {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ NOTER ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            Console.WriteLine("┃                                                                  ┃");
            Console.WriteLine("┃               Welcome to the interactive notebook!               ┃");
            Console.WriteLine("┃         Here you can store information about your friends        ┃");
            Console.WriteLine("┃             their names, phone numbers and birthdays.            ┃");
            Console.WriteLine("┃                                                                  ┃");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            if (!File.Exists("friends.json")) {
                JsonToFile();
                Menu();
            }
            JsonFromFile();
            Menu();
        }

        public static void Menu() {
            int amou = friends.Length;
            JsonToFile();
            Commands("nedq", amou);
        }

        public static void Commands(string str, int amou) {
            string res = "";
            if (amou == 0) {
                str = str.Replace("e", string.Empty);
                str = str.Replace("d", string.Empty);
            }
            if (str == "nq") res += "┌────────────────────────────────┐┌────────────────────────────────┐\n│               [n]              ││              [q]               │\n│             new note           ││              quit              │\n└────────────────────────────────┘└────────────────────────────────┘";
            else {
                for (int i = 0; i < str.Length; i++) {
                    res += "┌───────────────┐";
                }
                res += "\n";
                if (str.Contains("n")) res += "│      [n]      │";
                if (str.Contains("e")) res += "│      [e]      │";
                if (str.Contains("d")) res += "│      [d]      │";
                if (str.Contains("q")) res += "│      [q]      │";
                res += "\n";
                if (str.Contains("n")) res += "│    new note   │";
                if (str.Contains("e")) res += "│      edit     │";
                if (str.Contains("d")) res += "│     delete    │";
                if (str.Contains("q")) res += "│      quit     │";
                res += "\n";
                for (int i = 0; i < str.Length; i++) {
                    res += "└───────────────┘";
                }
            }
            Console.WriteLine($"{res}");

            while (true) {
                char key = Console.ReadKey(true).KeyChar;
                switch (key) {
                    case 'n':
                        if (str.Contains("n")) {
                            Clear(5);
                            New(amou);
                            break;
                        }
                        goto case 'e';
                    case 'e':
                        if (str.Contains("e")) {
                            Clear(5);
                            Edit(amou);
                            break;
                        }
                        goto case 'd';
                    case 'd':
                        if (str.Contains("d")) {
                            Clear(5);
                            Delete(amou);
                        }
                        goto case 'q';
                    case 'q':
                        if (str.Contains("q")) {
                            Clear(Console.CursorTop+1);
                            Environment.Exit(0);
                        }
                        break;
                }
            }
        }

        public static void Delete(int amou) {
            int num, indent = amou+6;
            Print(amou);
            while (true) {
                try {
                    num = Convert.ToInt32($"{CheckToEsc(indent-1, amou, "edit")}{Console.ReadLine()}");
                    if (num <= amou && num > 0) break;
                    Console.Write($"╓ Note with number {num} doesn't exist.\n");
                    Console.Write("╙ Enter the note number for deleting: ");
                    indent += 2;
                }
                catch {
                    Console.Write("╓ Incorrect format.\n");
                    Console.Write("╙ Enter the note number for deleting: ");
                    indent += 2;
                }
            }
            Clear(indent);
            NOTE[] tmp = new NOTE[friends.Length - 1]; int index = 0;
            for (int i = 0; i < friends.Length-1; i++) {
                if (i == num-1) index = 1;
                tmp[i] = friends[i + index];
            }
            Array.Resize(ref friends, friends.Length - 1);
            friends = tmp;
            if (amou == 1) Menu();
            Delete(amou-1);
        }

        public static void Edit(int amou) {
            int num, indent = amou+6, cou = 0;
            Print(amou);
            while (true) {
                try {
                    num = Convert.ToInt32($"{CheckToEsc(indent-1, amou, "edit")}{Console.ReadLine()}");
                    if (num <= amou && num > 0) break;
                    Console.Write($"╓ Note with number {num} doesn't exist.\n");
                    Console.Write("╙ Enter the note number for editing: ");
                    indent += 2;
                    cou += 1;
                    if (cou >= 3) {
                        Clear(indent - 1);
                        Menu();
                    }
                }
                catch {
                    Console.Write("╓ Incorrect format.\n");
                    Console.Write("╙ Enter the note number for editing: ");
                    indent += 2;
                    cou += 1;
                    if (cou >= 3) {
                        Clear(indent - 1);
                        Menu();
                    }
                }
            }
            Clear(indent);
            Console.Write($"┌─────────────────╮\n│ Edit note.      │\n│ [Esc] to finish │\n├─────────────────╯\n");
            Console.Write($"│Enter name ([Enter] {friends[num-1].name})\n├>> ");
            char key = CheckToEsc(6, amou, "edit");
            if (key == (char) 1) {
                Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                Console.Write($"{friends[num-1].name}\n");
            }
            else friends[num-1].name = Convert.ToString($"{key}{Console.ReadLine()}");
            Console.Write($"│Enter phone ([Enter] {friends[num-1].phone}):\n├>> ");
            key = CheckToEsc(8, amou, "edit");
            if (key == (char) 1) {
                Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                Console.Write($"{friends[num-1].phone}\n");
            }
            else friends[num-1].phone = Convert.ToString($"{key}{Console.ReadLine()}");
            Console.Write($"│Birthday in XX.XX.XXXX format ([Enter] {string.Join('.', friends[num-1].birth)}):\n╰>> ");
            key = CheckToEsc(10, amou, "edit");
            try {
                if (Char.IsDigit(key)) friends[num-1].birth = Convert.ToString($"{key}{Console.ReadLine()}").Split('.').Select(item => Convert.ToInt32(item)).ToArray();
                else if (key == (char)1) {
                    Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                    Console.Write($"{string.Join('.', friends[num-1].birth)}\n");
                }
                else {
                    Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                    friends[num-1].birth = "0.0.0".Split('.').Select(item => Convert.ToInt32(item)).ToArray();
                    Console.Write("0.0.0                              \n");
                }
            }
            finally {
                Clear(11);
                Menu();
            }
        }

        public static void Print(int amou) {
            if (amou != 0) {
                int nameLen = 4, phoneLen = 5, birthLen = 8, numLen = amou.ToString().Length;
                string res = "╔";
                InsertSort(friends);
                InsertSort(friends);
                for (int i = 0; i < amou; i++) {
                    nameLen = Math.Max(nameLen, friends[i].name.Length + 1);
                    phoneLen = Math.Max(phoneLen, friends[i].phone.Length + 1);
                    birthLen = Math.Max(birthLen, string.Join('.', friends[i].birth).Length + 1);
                }
                for (int i = 0; i < numLen; i++) res += "═";
                res += "╤";
                for (int i = 0; i < nameLen; i++) res += "═";
                res += "╤";
                for (int i = 0; i < phoneLen; i++) res += "═";
                res += "╤";
                for (int i = 0; i < birthLen; i++) res += "═";
                res += "╗\n║";
                // ---
                for (int i = 0; i < numLen; i++) res += " ";
                res += "│Name";
                for (int i = 0; i < nameLen - "Name".Length; i++) res += " ";
                res += "│Phone";
                for (int i = 0; i < phoneLen - "Phone".Length; i++) res += " ";
                res += "│Birthday";
                for (int i = 0; i < birthLen - "Birthday".Length; i++) res += " ";
                res += "║\n╠";
                // ---
                for (int i = 0; i < numLen; i++) res += "═";
                res += "╪";
                for (int i = 0; i < nameLen; i++) res += "═";
                res += "╪";
                for (int i = 0; i < phoneLen; i++) res += "═";
                res += "╪";
                for (int i = 0; i < birthLen; i++) res += "═";
                res += "╣\n║";
                for (int i = 0; i < amou; i++) {
                    res += (i+1).ToString();
                    for (int j = 0; j < numLen - (i+1).ToString().Length; j++) {
                        res += " ";
                    }
                    res += "│";
                    for (int j = 0; j < nameLen - friends[i].name.Length; j++) {
                        res += " ";
                    }
                    res += $"{friends[i].name}│";
                    for (int j = 0; j < phoneLen - friends[i].phone.Length; j++) {
                        res += " ";
                    }
                    res += $"{friends[i].phone}│";
                    for (int j = 0; j < birthLen - string.Join('.', friends[i].birth).Length; j++) {
                        res += " ";
                    }
                    if (i != amou-1) res += $"{string.Join('.', friends[i].birth)}║\n║";
                    else res += $"{string.Join('.', friends[i].birth)}║\n╠";
                }
                for (int i = 0; i < numLen; i++) res += "═";
                res += "╧";
                for (int i = 0; i < nameLen; i++) res += "═";
                res += "╧";
                for (int i = 0; i < phoneLen; i++) res += "═";
                res += "╧";
                for (int i = 0; i < birthLen; i++) res += "═";
                res += "╝\n╙ Enter the note number or [Esc] to finish: ";
                Console.Write($"{res}");
            }
            else Console.Write("You don't have any notes!");
        }

        public static void New(int amou) {
            while (true) {
                amou += 1;
                Array.Resize(ref friends, amou);
                Console.Write($"┌─────────────────╮\n│ New note.       │\n│ [Esc] to finish │\n├─────────────────╯\n");
                Console.Write($"│Enter name\n├>> ");
                char key = CheckToEsc(6, amou, "new");
                if (key == (char) 1) {
                    Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                    friends[amou-1].name = "-";
                    Console.Write("-\n");
                }
                else friends[amou-1].name = Convert.ToString($"{key}{Console.ReadLine()}");
                Console.Write($"│Enter phone:\n├>> ");
                key = CheckToEsc(8, amou, "new");
                if (key == (char) 1) {
                    Console.SetCursorPosition(Console.CursorLeft + 4, Console.CursorTop);
                    friends[amou-1].phone = "-";
                    Console.Write("-\n");
                }
                else friends[amou-1].phone = Convert.ToString($"{key}{Console.ReadLine()}");
                Console.Write($"│Birthday in XX.XX.XXXX format:\n╰>> ");
                key = CheckToEsc(10, amou, "new");
                try {
                    if (Char.IsDigit(key)) {
                        friends[amou-1].birth = Convert.ToString($"{key}{Console.ReadLine()}").Split('.').Select(item => Convert.ToInt32(item)).ToArray();
                    }
                    else {
                        Console.SetCursorPosition(Console.CursorLeft+4, Console.CursorTop);
                        friends[amou-1].birth = "0.0.0".Split('.').Select(item => Convert.ToInt32(item)).ToArray();
                        Console.Write("0.0.0                              \n");
                    }
                }
                catch {}
            }
        }

        public static void InsertSort(NOTE[] arr) {
            try {
                int n = arr.Length, k; NOTE tmp;
                for (int i=1; i<n; i++) {
                    k = i;
                    while (k != 0 && Math.Abs(arr[k].birth[2]) < Math.Abs(arr[k-1].birth[2])) {
                        tmp = arr[k];
                        arr[k] = arr[k-1];
                        arr[k-1] = tmp;
                        k-=1;
                    }
                    if (Math.Abs(arr[k].birth[2]) == Math.Abs(arr[k-1].birth[2])) {
                        while (k != 0 && Math.Abs(arr[k].birth[1]) < Math.Abs(arr[k-1].birth[1]) && Math.Abs(arr[k].birth[2]) == Math.Abs(arr[k-1].birth[2])) {
                            tmp = arr[k];
                            arr[k] = arr[k-1];
                            arr[k-1] = tmp;
                            k-=1;
                        }
                        if (Math.Abs(arr[k].birth[1]) == Math.Abs(arr[k-1].birth[1])) {
                            while (k != 0 && Math.Abs(arr[k].birth[0]) < Math.Abs(arr[k-1].birth[0]) && Math.Abs(arr[k].birth[1]) == Math.Abs(arr[k-1].birth[1])) {
                                tmp = arr[k];
                                arr[k] = arr[k-1];
                                arr[k-1] = tmp;
                                k-=1;
                            }
                        }
                    }
                }
                friends = arr;
            }
            catch {}
        }

        public static char CheckToEsc(int clear, int amou, string mode) {
            var key = Console.ReadKey(false);
            char keyChar = key.KeyChar;
            if (!Char.IsLetterOrDigit(key.KeyChar)) {
                if (key.Key == ConsoleKey.Escape) {
                    Clear(clear);
                    if (mode == "new") Array.Resize(ref friends, amou-1);
                    Menu();
                }
                else if (key.Key == ConsoleKey.Enter) keyChar = (char) 1;
                else keyChar = '\0';
            }
            return keyChar;
        }
        public static void Clear(int clear) {
            Console.SetCursorPosition(0, Console.CursorTop + 1 - clear);
            for (int i = 0; i < clear; i++) {
                Console.Write(new String(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop - clear);
        }
        public static void JsonToFile() {
            using (StreamWriter file = new StreamWriter("friends.json", false, System.Text.Encoding.Default)) {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, friends);
            }
        }
        public static void JsonFromFile() {
            using (StreamReader file = new StreamReader("friends.json", System.Text.Encoding.Default)) {
                JsonSerializer serializer = new JsonSerializer();
                friends = (NOTE[])serializer.Deserialize(file, typeof(NOTE[]));
            }
        }
    }
}
