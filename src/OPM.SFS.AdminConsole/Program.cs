using Colorful;
using CommandDotNet;
using Microsoft.Extensions.Configuration;
using OPM.SFS.AdminConsole.Commands;
using OPM.SFS.AdminConsole.Services;
using OPM.SFS.Core.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Console = Colorful.Console;

namespace OPM.SFS.AdminConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FigletFont font = FigletFont.Load(GetEmbeddedFont("standard.flf"));
            Figlet figlet = new Figlet(font);
            Console.WriteLine(figlet.ToAscii("SFS CLI"), ColorTranslator.FromHtml("#8AFFEF"));
            _ = new AppRunner<Commands.CommandOptions>().Run(args);
            //MigrateDataService _migrator = new();
            //_migrator.LetsGo();
        }

        private static Stream GetEmbeddedFont(string font)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string fileName = asm.GetManifestResourceNames().Where(m => m.Contains(font)).FirstOrDefault();
            Stream stream = asm.GetManifestResourceStream(fileName);
            return stream;
        }

    }
}
