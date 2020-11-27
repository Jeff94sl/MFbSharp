using System;
using System.Windows.Forms;
using System.Threading;

namespace MFbSharp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "MFbSharp v0.1";
            DateTime Inicio = new DateTime();
            DateTime Fin = new DateTime();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
           
            Console.Clear();
            
            MFb mFb = new MFb();
            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog openFile = new OpenFileDialog();
            
            Console.WriteLine("MFbSharp Exporta Firebird");
            Console.WriteLine("Presione Enter Para Iniciar:");
            Console.ReadKey(true);
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    Inicio = DateTime.Now.ToLocalTime();
                    Console.WriteLine("Inicio: {0}", Inicio);
                    ThreadStart start = new ThreadStart(() => mFb.FbtoCSV(openFile.FileName, folder.SelectedPath));
                    Thread thread = new Thread(start);
                    thread.Start();
                    thread.Join();
                    Fin = DateTime.Now.ToLocalTime();
                }
            }
            Console.WriteLine("Duracion: {0}",Fin - Inicio);
            Console.WriteLine("Migracion Completada");
            Console.ReadKey();
        }
        
    }
}
