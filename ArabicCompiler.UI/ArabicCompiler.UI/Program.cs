using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ArabicCompiler.UI.Engine;

namespace ArabicCompiler.UI
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0 && args[0] == "--test")
            {
                RunSelfTests();
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }

        private static void RunSelfTests()
        {
            string testCode = ArabicCompilerEngine.GetDefaultSourceCode();
            try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }
            var sbOut = new StringBuilder();
            sbOut.AppendLine("=== 1. فحص التحليل اللغوي ===");
            ArabicCompilerEngine.RunLexicalAnalysis(testCode, out string lexRes, out string lexErr, out bool lexSuccess);
            sbOut.AppendLine($"نجاح: {lexSuccess}");
            sbOut.AppendLine(lexRes);

            sbOut.AppendLine("=== 2. فحص التحليل النحوي ===");
            ArabicCompilerEngine.RunSyntaxAnalysis(testCode, out string synRes, out string synErr, out bool synSuccess);
            sbOut.AppendLine($"نجاح: {synSuccess}");
            sbOut.AppendLine(synRes);

            sbOut.AppendLine("=== 3. فحص التحليل الدلالي ===");
            ArabicCompilerEngine.RunSemanticAnalysis(testCode, out string semRes, out string semErr, out bool semSuccess);
            sbOut.AppendLine($"نجاح: {semSuccess}");
            sbOut.AppendLine(semRes);

            sbOut.AppendLine("=== 4. فحص الكود الوسيط TAC ===");
            ArabicCompilerEngine.GenerateTAC(testCode, out string tacRes, out string tacErr);
            sbOut.AppendLine(tacRes);

            sbOut.AppendLine("=== 5. فحص تحسين الكود ===");
            ArabicCompilerEngine.OptimizeCode(testCode, tacRes, out string optRes, out string optErr);
            sbOut.AppendLine(optRes);

            sbOut.AppendLine("=== 6. فحص كود التجميع MASM ===");
            ArabicCompilerEngine.GenerateAssembly(testCode, out string asmRes, out string asmErr);
            sbOut.AppendLine(asmRes);

            sbOut.AppendLine("=== 7. فحص دورة الترجمة الكاملة RunAllStages ===");
            bool allOk = ArabicCompilerEngine.RunAllStages(testCode, out string fullRep, out string errSum, out var syms, out string consoleOut);
            sbOut.AppendLine($"النجاح الكلي: {allOk}");
            sbOut.AppendLine($"ملخص الخطأ: {errSum}");
            sbOut.AppendLine($"مخرجات الكونسول:\r\n{consoleOut}");

            string customCode = @"برنامج حسابي؛
ثابت
    حد = 50؛
متغير
    س, ص : صحيح؛
{
    س = 25؛
    ص = س * 2؛
    إذا (ص >= حد) فإن
    {
        اطبع(""وصلنا للحد"", ص)؛
    }
}.";
            sbOut.AppendLine("=== 8. فحص برنامج مدخل مخصص (Custom Dynamic Program) ===");
            bool customOk = ArabicCompilerEngine.RunAllStages(customCode, out string cRep, out string cErr, out var cSyms, out string cOut);
            sbOut.AppendLine($"نجاح البرنامج المخصص: {customOk}");
            sbOut.AppendLine($"المخرجات:\r\n{cOut}");

            File.WriteAllText("test_output.txt", sbOut.ToString(), System.Text.Encoding.UTF8);
            Console.WriteLine("Self tests completed. Results saved to test_output.txt");
        }
    }
}
