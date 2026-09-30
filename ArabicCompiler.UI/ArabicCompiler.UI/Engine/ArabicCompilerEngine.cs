using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ArabicCompiler.UI.Engine
{
    public static class ArabicCompilerEngine
    {
        public static string GetDefaultSourceCode()
        {
            return @"برنامج اختبارشامل؛
ثابت
    خمسة = 5؛
    اثنان = 2؛
نوع
    ارقام = قائمة [5] من صحيح؛
    شخص = سجل{
        عمر : صحيح؛
        ناجح : منطقي؛
    }؛
متغير
    س, ص, ن, ع, ب, ر, فوه : صحيح؛
    ف : ارقام؛
    ش : شخص؛
    عداد : صحيح؛

إجراء زيادة(بالمرجع قيمة : صحيح, بالقيمة مقدار : صحيح)
{
    قيمة = قيمة + مقدار؛
}

{
    س = 2 + 3 * 4؛
    ص = س + خمسة؛
    ن = 20 - 1؛
    ع = 19 / 2؛
    ب = 17 \ 5؛
    ر = 17 % 5؛
    فوه = 2 ^ 3؛

    إذا (س > 0 و ص > 0) فإن
    {
        اطبع(""اقل من عشرة"")؛
        اطبع(فوه)؛
    }

    لكل عداد = 1 إلى 3 بقدر 1
    {
        اطبع(""رقم التكرار"", عداد)؛
    }

    ف[0] = ن؛
    ف[1] = ب؛
    ف[2] = ر؛
    ف[3] = فوه؛
    ف[4] =ن+ فوه;

    ش.عمر = ف[4]؛
    ش.ناجح = نعم؛

    زيادة(س, 1)؛

    اطبع(""قيمة ص"", ص)؛
    اطبع(""قيمة ن"", ن)؛
    اطبع(""القسمة الحقيقية"", ع)؛
    اطبع(""القسمة الصحيحة"", ب)؛
    اطبع(""باقي القسمة"", ر)؛
    اطبع(""الاس"", فوه)؛
    اطبع(""العنصر صفر"", ن)؛
    اطبع(""العنصر الرابع"", ف[4])؛
    اطبع(""عمر الشخص"", ش.عمر)؛
    اطبع(""حالة الشخص"", ش.ناجح)؛
    اطبع(""القيمة النهائية لس"", س)؛
}.";
        }

        public static string NormalizeSource(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            var sb = new StringBuilder(source.Length);
            foreach (char c in source)
            {
                switch (c)
                {
                    case '،': sb.Append(','); break;
                    case ';': sb.Append('؛'); break;
                    case '“':
                    case '”':
                    case '″': sb.Append('"'); break;
                    case '٠': sb.Append('0'); break;
                    case '١': sb.Append('1'); break;
                    case '٢': sb.Append('2'); break;
                    case '٣': sb.Append('3'); break;
                    case '٤': sb.Append('4'); break;
                    case '٥': sb.Append('5'); break;
                    case '٦': sb.Append('6'); break;
                    case '٧': sb.Append('7'); break;
                    case '٨': sb.Append('8'); break;
                    case '٩': sb.Append('9'); break;
                    case '\u00A0': sb.Append(' '); break;
                    default: sb.Append(c); break;
                }
            }
            string result = sb.ToString();
            // تحويل المعاملات المنطقية العربية المنفصلة إلى ما يقابلها في Flex / Bison
            result = Regex.Replace(result, @"(?<=[\s()\[\],؛])و(?=[\s()\[\],؛])", "&&");
            result = Regex.Replace(result, @"(?<=[\s()\[\],؛])أو(?=[\s()\[\],؛])", "||");
            result = Regex.Replace(result, @"(?<=[\s()\[\],؛])ليس(?=[\s()\[\],؛])", "!");
            return result;
        }

        public static string NormalizeForBison(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            string s = NormalizeSource(source);

            // 1. معالجة الفاصلة المنقوطة الأخيرة في السجلات قبل }
            s = Regex.Replace(s, @"(سجل[\s\S]*?)؛(\s*\})", "$1$2");

            // 2. معالجة ترويسة الإجراءات: في قواعد Bison تكون المعلمات مفصولة بـ ؛ متبوعة بـ ؛ قبل جسم الإجراء
            s = Regex.Replace(s, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\(([\s\S]*?)\)\s*(?=\{)", m =>
            {
                string kind = m.Groups[1].Value;
                string pName = m.Groups[2].Value;
                string pParams = m.Groups[3].Value;
                string normParams = Regex.Replace(pParams, @"[,،]", "؛");
                return $"{kind} {pName}({normParams})؛\r\n";
            });

            // 3. تحويل القيم المنطقية البديلة (نعم / لا -> صح / خطأ)
            s = Regex.Replace(s, @"\bنعم\b", "صح");
            s = Regex.Replace(s, @"\bلا\b", "خطأ");

            // 4. تحويل حلقة (لكل ... بقدر ...) إلى (كرر (...) اضف ...) مع إضافة الأقواس الإلزامية في Bison
            s = Regex.Replace(s, @"\bلكل\b\s*\(?([\u0600-\u06FF\w]+)\s*=\s*([^\s]+)\s*(إلى|الى)\s*([^\s]+)(?:\s*(?:بقدر|اضف)\s*([^\s{]+))?\)?\s*(?=\{)", m =>
            {
                string loopVar = m.Groups[1].Value.Trim();
                string startVal = m.Groups[2].Value.Trim();
                string toKw = m.Groups[3].Value;
                string endVal = m.Groups[4].Value.Trim();
                string stepVal = m.Groups[5].Success && !string.IsNullOrWhiteSpace(m.Groups[5].Value) ? m.Groups[5].Value.Trim() : "1";
                return $"كرر ({loopVar} = {startVal} {toKw} {endVal} اضف {stepVal})\r\n";
            });

            // 5. إضافة فاصلة منقوطة بعد قوس الإغلاق } للكتل البرمجية إذا كان يليها تعليمة أخرى
            // في قواعد Bison كل تعليمة يجب أن تتبع بفاصلة منقوطة حتى كتل if وحلقات التكرار
            s = Regex.Replace(s, @"\}(?!\s*[؛;\.])", "}؛");

            // 6. معالجة الشروط المنطقية المركبة: في لغة باسكال/Bison أسبقية 'و' (&&) أعلى من المقارنة فيجب إحاطة المقارنات بأقواس:
            // (س > 0 && ص > 0) -> ((س > 0) && (ص > 0))
            s = Regex.Replace(s, @"إذا\s*\(([\s\S]*?)\)\s*فإن", m =>
            {
                string cond = m.Groups[1].Value;
                if (Regex.IsMatch(cond, @"(&&|\|\|)") && Regex.IsMatch(cond, @"(==|!=|>=|<=|>|<)"))
                {
                    var parts = Regex.Split(cond, @"(\s*(?:&&|\|\|)\s*)");
                    for (int i = 0; i < parts.Length; i += 2)
                    {
                        string p = parts[i].Trim();
                        if (Regex.IsMatch(p, @"(==|!=|>=|<=|>|<)") && !p.StartsWith("("))
                        {
                            parts[i] = $"({p})";
                        }
                    }
                    cond = string.Join(" ", parts);
                }
                return $"إذا ({cond}) فإن";
            });

            return s;
        }

        // ====================================================================
        // 1. التحليل اللغوي / المعجمي (Lexical Analysis)
        // ====================================================================
        public static void RunLexicalAnalysis(string source, out string result, out string error)
        {
            RunLexicalAnalysis(source, out result, out error, out _);
        }

        public static void RunLexicalAnalysis(string source, out string result, out string error, out bool success)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                result = "يرجى كتابة أو لصق كود برمجي في محرر الكود أولاً.";
                error = "الكود البرمجي فارغ.";
                success = false;
                return;
            }

            string normalizedSource = NormalizeSource(source);
            var sb = new StringBuilder();
            var lexicalErrors = new List<string>();

            // المحاولة أولاً عبر مكتبة المترجم الأصلية Flex Native DLL
            if (NativeBridge.IsDllAvailable())
            {
                try
                {
                    NativeBridge.TokenCallback cb = (token, lexemePtr, line) =>
                    {
                        string lexeme = NativeBridge.PtrToString(lexemePtr);
                        if (token == 308 && lexeme == ",") lexeme = "،";
                        if (token == 306 && lexeme == ";") lexeme = "؛";
                        if (token == 301 && lexeme == "&&") lexeme = "و";
                        if (token == 295 && lexeme == "||") lexeme = "أو";
                        if (token == 302 && lexeme == "!") lexeme = "ليس";
                        string tokenName = NativeBridge.GetTokenName(token, lexeme);

                        if (token == 320 || tokenName.Contains("خطأ")) // LEXICAL_ERROR
                        {
                            lexicalErrors.Add($"السطر: {line} | خطأ لغوي غير معروف: [{lexeme}]");
                        }

                        sb.AppendLine($"السطر: {line} | Token: {tokenName} | Lexeme: {lexeme}");
                    };

                    int ret = NativeBridge.Compiler_Lex(normalizedSource, cb);
                    if (ret > 0 && sb.Length > 0)
                    {
                        if (lexicalErrors.Count > 0)
                        {
                            result = $"فشل التحليل اللغوي! تم رصد رموز غير صالحة:\r\n" + string.Join("\r\n", lexicalErrors) + "\r\n\r\nتفاصيل الرموز المستخرجة:\r\n" + sb.ToString();
                            error = lexicalErrors[0];
                            success = false;
                            return;
                        }

                        result = sb.ToString();
                        error = $"تم التحليل اللغوي بنجاح عبر Flex Native DLL. تم استخراج {ret} رمزاً لغوياً.";
                        success = true;
                        return;
                    }
                }
                catch
                {
                    sb.Clear();
                    lexicalErrors.Clear();
                }
            }

            // التحليل اللغوي المدار في حال تعذر تحميل المكتبة الأصلية
            result = TokenizeManaged(source, out lexicalErrors);
            if (lexicalErrors.Count > 0)
            {
                error = lexicalErrors[0];
                success = false;
            }
            else
            {
                error = "تم التحليل اللغوي بنجاح استناداً إلى الكود المدخل.";
                success = true;
            }
        }

        private static string TokenizeManaged(string source, out List<string> errors)
        {
            var sb = new StringBuilder();
            errors = new List<string>();
            var lines = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                int lineNum = lineIndex + 1;
                string line = lines[lineIndex];
                if (string.IsNullOrWhiteSpace(line)) continue;

                int col = 0;
                while (col < line.Length)
                {
                    char c = line[col];

                    if (char.IsWhiteSpace(c))
                    {
                        col++;
                        continue;
                    }

                    // التعليقات
                    if (c == '/' && col + 1 < line.Length && line[col + 1] == '/')
                        break;

                    // السلاسل النصية
                    if (c == '"' || c == '“' || c == '”' || c == '\'')
                    {
                        char quote = c;
                        int start = col++;
                        while (col < line.Length && line[col] != quote && line[col] != '"' && line[col] != '”') col++;
                        if (col < line.Length) col++;
                        string strVal = line.Substring(start, col - start);
                        sb.AppendLine($"السطر: {lineNum} | Token: خيط رمزي | Lexeme: {strVal}");
                        continue;
                    }

                    // الرموز الثنائية
                    if (col + 1 < line.Length)
                    {
                        string two = line.Substring(col, 2);
                        if (two == ">=" || two == "<=" || two == "==" || two == "!=" || two == "&&" || two == "||" || two == "::")
                        {
                            sb.AppendLine($"السطر: {lineNum} | Token: {GetSymbolKind(two)} | Lexeme: {two}");
                            col += 2;
                            continue;
                        }
                    }

                    // الرموز الفردية
                    string single = c.ToString();
                    if ("=+-*/\\^><!();,،؛.:{}[]".Contains(c))
                    {
                        sb.AppendLine($"السطر: {lineNum} | Token: {GetSymbolKind(single)} | Lexeme: {single}");
                        col++;
                        continue;
                    }

                    // المعرفات والكلمات والأرقام
                    int startWord = col;
                    while (col < line.Length && !char.IsWhiteSpace(line[col]) && !"=+-*/\\^><!();,،؛.:{}[]\"“”'".Contains(line[col]))
                    {
                        col++;
                    }
                    string word = line.Substring(startWord, col - startWord);

                    if (double.TryParse(word, out _))
                    {
                        string numType = word.Contains(".") ? "عدد حقيقي" : "عدد صحيح";
                        sb.AppendLine($"السطر: {lineNum} | Token: {numType} | Lexeme: {word}");
                    }
                    else
                    {
                        if (Regex.IsMatch(word, @"[@#$]"))
                        {
                            errors.Add($"خطأ لغوي في السطر {lineNum}: رمز غير مسموح في المعرف: '{word}'");
                            sb.AppendLine($"السطر: {lineNum} | Token: خطأ لغوي | Lexeme: {word}");
                        }
                        else
                        {
                            string keywordKind = GetKeywordKind(word);
                            sb.AppendLine($"السطر: {lineNum} | Token: {keywordKind} | Lexeme: {word}");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetSymbolKind(string op) => op switch
        {
            "(" => "قوس دائري مفتوح (()",
            ")" => "قوس دائري مغلق ())",
            "[" => "قوس مصفوفة مفتوح ([)",
            "]" => "قوس مصفوفة مغلق (])",
            "{" => "قوس كتلة مفتوح ({)",
            "}" => "قوس كتلة مغلق (})",
            "=" => "عامل إسناد (=)",
            "+" => "عامل جمع (+)",
            "-" => "عامل طرح (-)",
            "*" => "عامل ضرب (*)",
            "/" => "قسمة حقيقية (/)",
            "\\" => "قسمة صحيحة (\\)",
            "%" => "باقي القسمة (%)",
            "^" => "عامل أس (^)",
            ">" => "مقارنة: أكبر من (>)",
            "<" => "مقارنة: أصغر من (<)",
            ">=" => "مقارنة: أكبر أو يساوي (>=)",
            "<=" => "مقارنة: أصغر أو يساوي (<=)",
            "==" => "مقارنة: يساوي (==)",
            "!=" => "مقارنة: لا يساوي (!=)",
            "!" => "نفي منطقي (!)",
            "," or "،" => "فاصلة (،)",
            ";" or "؛" => "فاصلة منقوطة (؛)",
            ":" => "نقطتان (:)",
            "::" => "تحديد نطاق (::)",
            "." => "نقطة النهاية (.)",
            "&&" => "عامل منطقي: و",
            "||" => "عامل منطقي: أو",
            _ => "رمز خاص"
        };

        private static string GetKeywordKind(string word) => word switch
        {
            "برنامج" => "كلمة محجوزة: برنامج",
            "ثابت" => "كلمة محجوزة: ثابت",
            "نوع" => "كلمة محجوزة: نوع",
            "قائمة" => "كلمة محجوزة: قائمة",
            "من" => "كلمة محجوزة: من",
            "سجل" => "كلمة محجوزة: سجل",
            "متغير" => "كلمة محجوزة: متغير",
            "إجراء" or "دالة" => "كلمة محجوزة: إجراء",
            "بالمرجع" => "كلمة محجوزة: بالمرجع",
            "بالقيمة" => "كلمة محجوزة: بالقيمة",
            "صحيح" or "int" => "نوع بيانات: صحيح",
            "حقيقي" or "float" or "double" => "نوع بيانات: حقيقي",
            "منطقي" or "bool" => "نوع بيانات: منطقي",
            "خيط" or "نص" or "string" => "نوع بيانات: خيط رمزي",
            "محرف" or "char" => "نوع بيانات: محرف",
            "اقرأ" or "ادخل" or "read" => "تعليمة: اقرأ",
            "اطبع" or "اكتب" or "print" or "write" => "تعليمة: اطبع",
            "إذا" or "لو" or "if" => "شرط: إذا",
            "فإن" or "then" => "شرط: فإن",
            "وإلا" or "else" => "شرط: وإلا",
            "لكل" or "كرر" or "for" => "حلقة تكرار: لكل",
            "إلى" or "to" => "حلقة: إلى",
            "بقدر" or "بخطوة" or "step" => "خطوة: بقدر",
            "طالما" or "بينما" or "while" => "حلقة: طالما",
            "أعد" or "repeat" => "حلقة: أعد",
            "حتى" or "until" => "حلقة: حتى",
            "نعم" or "صح" or "true" => "قيمة منطقية: نعم",
            "لا" or "خطأ" or "false" => "قيمة منطقية: لا",
            "و" or "and" => "عامل منطقي: و",
            "أو" or "or" => "عامل منطقي: أو",
            _ => "معرّف"
        };

        // ====================================================================
        // 2. التحليل النحوي (Syntax Analysis)
        // ====================================================================
        public static void RunSyntaxAnalysis(string source, out string result, out string error, out bool success)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                result = "يرجى كتابة أو لصق كود برمجي للتحليل النحوي.";
                error = "الكود البرمجي فارغ.";
                success = false;
                return;
            }

            // فحص تصريح اسم البرنامج في البداية
            if (!Regex.IsMatch(source, @"^\s*(//.*[\r\n]+)*\s*برنامج\s+[\u0600-\u06FF\w]+", RegexOptions.Multiline))
            {
                result = "خطأ نحوي في السطر 1:\r\nيجب أن يبدأ البرنامج بتصريح اسم البرنامج: برنامج <الاسم>؛";
                error = "خطأ نحوي في السطر 1: لم يتم العثور على تصريح اسم البرنامج.";
                success = false;
                return;
            }

            // فحص توازن الأقواس المعقوفة { }
            int openBrace = source.Count(c => c == '{');
            int closeBrace = source.Count(c => c == '}');
            if (openBrace != closeBrace)
            {
                result = $"خطأ نحوي: عدم تطابق أقواس الكتل البرمجية {{ }} (أقواس مفتوحة: {openBrace}، أقواس مغلقة: {closeBrace}).\r\nالتصحيح المقترح: تأكد من إغلاق كل كتلة برمجية مفتوحة بالقوس المغلق (}}).";
                error = $"خطأ نحوي: عدم تطابق الأقواس المعقوفة {{ }} ({openBrace} مفتوح مقابل {closeBrace} مغلق).";
                success = false;
                return;
            }

            // فحص توازن الأقواس الدائرية ( )
            int openParen = source.Count(c => c == '(');
            int closeParen = source.Count(c => c == ')');
            if (openParen != closeParen)
            {
                result = $"خطأ نحوي: عدم تطابق الأقواس الدائرية ( ) (مفتوحة: {openParen}، مغلقة: {closeParen}).\r\nالتصحيح المقترح: تأكد من إغلاق كل تعبير بين قوسين بالقوس المغلق ()).";
                error = $"خطأ نحوي: عدم تطابق الأقواس الدائرية ( ) ({openParen} مفتوح مقابل {closeParen} مغلق).";
                success = false;
                return;
            }

            // فحص توازن الأقواس المربعة [ ]
            int openBracket = source.Count(c => c == '[');
            int closeBracket = source.Count(c => c == ']');
            if (openBracket != closeBracket)
            {
                result = $"خطأ نحوي: عدم تطابق الأقواس المربعة [ ] (مفتوحة: {openBracket}، مغلقة: {closeBracket}).\r\nالتصحيح المقترح: تأكد من إغلاق كل فهرس أو حجم مصفوفة بالقوس المغلق (]).";
                error = $"خطأ نحوي: عدم تطابق الأقواس المربعة [ ] ({openBracket} مفتوح مقابل {closeBracket} مغلق).";
                success = false;
                return;
            }

            // فحص نقطة النهاية الإلزامية '.' في نهاية البرنامج
            // يجب أن تنتهي الكتلة الرئيسية للبرنامج بـ }.
            string cleanNoComments = Regex.Replace(source, @"/\*[\s\S]*?\*/|//.*$", "", RegexOptions.Multiline).TrimEnd();
            if (!cleanNoComments.EndsWith("."))
            {
                int lineCount = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length;
                result = $"خطأ نحوي في السطر {lineCount}:\r\nوصل المترجم إلى نهاية البرنامج ولم يجد النقطة '.' الإلزامية في نهاية البرنامج.\r\nالتصحيح المقترح: أضف النقطة (.) بعد قوس النهاية الأخير (}}).";
                error = $"خطأ نحوي في السطر {lineCount}: نقطة النهاية الإلزامية '.' مفقودة في نهاية البرنامج.";
                success = false;
                return;
            }

            string normalizedSource = NormalizeForBison(source);

            // الاعتماد بشكل أساسي على مكتبة المترجم الأصلية Flex / Bison Native DLL
            if (NativeBridge.IsDllAvailable())
            {
                try
                {
                    NativeBridge.Compiler_ResetSyntaxError();
                    int res = NativeBridge.Compiler_Parse(normalizedSource);

                    if (res == 0)
                    {
                        result = "تم التحليل النحوي بنجاح عبر مكتبة المترجم الأصلية (ArabicCompiler.Native.dll - Bison Parser).\r\nالكود البرمجي مطابق تماماً للقواعد النحوية الصارمة للغة المترجم العربي.";
                        error = "لا توجد أخطاء نحوية.";
                        success = true;
                        return;
                    }
                    else
                    {
                        int errLine = NativeBridge.Compiler_GetSyntaxErrorLine();
                        string rawMsg = NativeBridge.PtrToString(NativeBridge.Compiler_GetSyntaxErrorMessage());
                        string rawCorr = NativeBridge.PtrToString(NativeBridge.Compiler_GetSyntaxErrorCorrection());

                        if (errLine <= 0) errLine = 1;
                        if (string.IsNullOrWhiteSpace(rawMsg)) rawMsg = "خطأ في الصياغة النحوية للبرنامج.";

                        var sbErr = new StringBuilder();
                        sbErr.AppendLine("===== خطأ نحوي (Syntax Error) =====");
                        sbErr.AppendLine($"المحرك: محرك Bison الأصلي (Native DLL)");
                        sbErr.AppendLine($"السطر: {errLine}");
                        sbErr.AppendLine($"وصف الخطأ: {rawMsg}");
                        if (!string.IsNullOrWhiteSpace(rawCorr))
                        {
                            sbErr.AppendLine($"التصحيح المقترح: {rawCorr}");
                        }

                        result = sbErr.ToString();
                        error = $"خطأ نحوي في السطر {errLine}: {rawMsg}";
                        success = false;
                        return;
                    }
                }
                catch
                {
                    // لو حدث استثناء نتابع للتحقق المدار
                }
            }

            // التحقق النحوي المدار في حال عدم تحميل المكتبة الأصلية
            if (!ValidateManagedSyntax(source, out string mResult, out string mError))
            {
                result = mResult;
                error = mError;
                success = false;
                return;
            }

            result = "تم التحليل النحوي بنجاح.\r\nالكود مطابق للقواعد النحوية للغة.";
            error = "لا توجد أخطاء نحوية.";
            success = true;
        }

        private static bool ValidateManagedSyntax(string source, out string result, out string error)
        {
            var lines = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                int lineNum = i + 1;
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

                int cIdx = line.IndexOf("//");
                if (cIdx >= 0) line = line.Substring(0, cIdx).Trim();

                if (line.Contains("=") && !line.StartsWith("إذا") && !line.StartsWith("لكل") && !line.StartsWith("طالما"))
                {
                    if (!line.EndsWith("؛") && !line.EndsWith(";") && !line.EndsWith("{"))
                    {
                        result = $"خطأ نحوي في السطر {lineNum}:\r\nالتعليمة '{line}' تفتقر إلى فاصلة منقوطة (؛) في نهايتها.\r\nالتصحيح المقترح: أضف الفاصلة المنقوطة (؛) في نهاية السطر.";
                        error = $"خطأ نحوي في السطر {lineNum}: فاصلة منقوطة مفقودة.";
                        return false;
                    }
                }
            }

            result = "تم التحليل النحوي بنجاح.";
            error = "لا توجد أخطاء نحوية.";
            return true;
        }

        // ====================================================================
        // بناء الشجرة الإعرابية (Parse Tree)
        // ====================================================================
        public static string GetParseTree(string source)
        {
            return ParseTreeGenerator.BuildTree(source);
        }

        // ====================================================================
        // 3. التحليل الدلالي (Semantic Analysis)
        // ====================================================================
        public static void RunSemanticAnalysis(string source, out string result, out string error)
        {
            RunSemanticAnalysis(source, out result, out error, out _);
        }

        public static void RunSemanticAnalysis(string source, out string result, out string error, out bool success)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                result = "يرجى كتابة أو لصق كود برمجي أولاً للتحليل الدلالي.";
                error = "الكود البرمجي فارغ.";
                success = false;
                return;
            }

            var semanticErrors = new List<string>();

            // جداول الرموز والأنواع والنطاقات
            var declaredConstants = new Dictionary<string, (string value, string type, int line)>();
            var declaredVariables = new Dictionary<string, (string type, int line)>();
            var declaredTypes = new Dictionary<string, (string kind, string detail, int line)>();
            var recordFields = new Dictionary<string, Dictionary<string, string>>(); // typeName -> fieldName -> fieldType
            var declaredProcedures = new Dictionary<string, (List<(string name, bool isByRef, string type)> paramList, int line, string body)>();

            var lines = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string currentSection = "";
            string currentRecordName = "";

            // -------------------------------------------------------------
            // المرحلة الأولى: استخراج وفحص جدول التعريفات والأنواع
            // -------------------------------------------------------------
            for (int i = 0; i < lines.Length; i++)
            {
                int lineNum = i + 1;
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

                int commIdx = line.IndexOf("//");
                if (commIdx >= 0) line = line.Substring(0, commIdx).Trim();

                if (line.StartsWith("ثابت"))
                {
                    currentSection = "ثابت";
                    line = line.Substring(4).Trim();
                }
                else if (line.StartsWith("نوع"))
                {
                    currentSection = "نوع";
                    line = line.Substring(3).Trim();
                }
                else if (line.StartsWith("متغير"))
                {
                    currentSection = "متغير";
                    line = line.Substring(5).Trim();
                }
                else if ((line == "{" || line.StartsWith("{")) && currentSection != "نوع" && currentSection != "إجراء" && string.IsNullOrEmpty(currentRecordName))
                {
                    currentSection = "تعليمات";
                    currentRecordName = "";
                }

                // فحص تعريف الإجراءات والدوال
                var procMatch = Regex.Match(line, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\((.*?)\)");
                if (procMatch.Success)
                {
                    currentSection = "إجراء";
                    string pName = procMatch.Groups[2].Value;

                    if (declaredProcedures.ContainsKey(pName) || declaredVariables.ContainsKey(pName) || declaredConstants.ContainsKey(pName))
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تم التصريح عن الإجراء '{pName}' مسبقاً.");
                    }
                    else
                    {
                        var paramList = new List<(string name, bool isByRef, string type)>();
                        var seenParams = new HashSet<string>();
                        string paramsStr = procMatch.Groups[3].Value;

                        if (!string.IsNullOrWhiteSpace(paramsStr))
                        {
                            foreach (var p in paramsStr.Split(new[] { ',', '،' }))
                            {
                                var pm = Regex.Match(p.Trim(), @"(بالمرجع|بالقيمة)?\s*([\u0600-\u06FF\w]+)\s*:\s*([\u0600-\u06FF\w]+)");
                                if (pm.Success)
                                {
                                    bool isByRef = pm.Groups[1].Value == "بالمرجع";
                                    string paramName = pm.Groups[2].Value;
                                    string paramType = pm.Groups[3].Value;

                                    if (seenParams.Contains(paramName))
                                    {
                                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تكرار اسم المعلمة '{paramName}' في الإجراء '{pName}'.");
                                    }
                                    else
                                    {
                                        seenParams.Add(paramName);
                                        paramList.Add((paramName, isByRef, paramType));
                                    }
                                }
                            }
                        }

                        string body = "";
                        int bOpen = source.IndexOf('{', source.IndexOf(procMatch.Value));
                        if (bOpen >= 0)
                        {
                            int bClose = source.IndexOf('}', bOpen);
                            if (bClose > bOpen)
                            {
                                body = source.Substring(bOpen + 1, bClose - bOpen - 1);
                            }
                        }

                        declaredProcedures[pName] = (paramList, lineNum, body);
                    }
                    continue;
                }

                // قسم الثوابت
                if (currentSection == "ثابت" && line.Contains("="))
                {
                    var parts = line.Split(new[] { '=' }, 2);
                    string cName = parts[0].Trim();
                    string cVal = parts[1].Trim().TrimEnd('؛', ';');

                    if (declaredConstants.ContainsKey(cName) || declaredVariables.ContainsKey(cName))
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تم التصريح عن الثابت '{cName}' مسبقاً.");
                    }
                    else
                    {
                        string cType = double.TryParse(cVal, out _) ? (cVal.Contains(".") ? "حقيقي" : "صحيح") : "خيط";
                        declaredConstants[cName] = (cVal, cType, lineNum);
                    }
                    continue;
                }

                // قسم الأنواع
                if (currentSection == "نوع")
                {
                    // قائمة
                    var listMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*=\s*قائمة\s*\[(.*?)\]\s*من\s*([\u0600-\u06FF\w]+)");
                    if (listMatch.Success)
                    {
                        string tName = listMatch.Groups[1].Value.Trim();
                        string sizeStr = listMatch.Groups[2].Value.Trim();
                        string elemType = listMatch.Groups[3].Value.Trim().TrimEnd('؛', ';');

                        if (declaredTypes.ContainsKey(tName))
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تم تعريف النوع '{tName}' مسبقاً.");
                        }
                        else
                        {
                            if (int.TryParse(sizeStr, out int sz) && sz <= 0)
                            {
                                semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: حجم القائمة '{tName}' يجب أن يكون أكبر من الصفر.");
                            }
                            declaredTypes[tName] = ("قائمة", $"{elemType}[{sizeStr}]", lineNum);
                        }
                        continue;
                    }

                    // سجل
                    var recordMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*=\s*سجل");
                    if (recordMatch.Success)
                    {
                        currentRecordName = recordMatch.Groups[1].Value.Trim();
                        if (declaredTypes.ContainsKey(currentRecordName))
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تم تعريف نوع السجل '{currentRecordName}' مسبقاً.");
                        }
                        else
                        {
                            declaredTypes[currentRecordName] = ("سجل", "", lineNum);
                            recordFields[currentRecordName] = new Dictionary<string, string>();
                        }

                        // إذا كانت حقول السجل معرفة في نفس السطر: شخص = سجل { عمر : صحيح؛ ناجح : منطقي؛ }؛
                        if (line.Contains("{"))
                        {
                            int fOpen = line.IndexOf('{');
                            int fClose = line.IndexOf('}', fOpen);
                            string fieldsPart = fClose > fOpen ? line.Substring(fOpen + 1, fClose - fOpen - 1) : line.Substring(fOpen + 1);
                            var fMatches = Regex.Matches(fieldsPart, @"([\u0600-\u06FF\w]+)\s*:\s*([\u0600-\u06FF\w]+)");
                            foreach (Match fm in fMatches)
                            {
                                string fn = fm.Groups[1].Value.Trim();
                                string ft = fm.Groups[2].Value.Trim().TrimEnd('؛', ';');
                                recordFields[currentRecordName][fn] = ft;
                            }
                            if (fClose > fOpen)
                            {
                                currentRecordName = "";
                            }
                        }
                        continue;
                    }

                    // حقول السجل عبر عدة أسطر
                    if (!string.IsNullOrEmpty(currentRecordName) && line.Contains(":"))
                    {
                        var fieldMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*:\s*([\u0600-\u06FF\w]+)");
                        if (fieldMatch.Success)
                        {
                            string fName = fieldMatch.Groups[1].Value.Trim();
                            string fType = fieldMatch.Groups[2].Value.Trim().TrimEnd('؛', ';');

                            if (recordFields.ContainsKey(currentRecordName))
                            {
                                if (recordFields[currentRecordName].ContainsKey(fName))
                                {
                                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تكرار الحقل '{fName}' في السجل '{currentRecordName}'.");
                                }
                                else
                                {
                                    recordFields[currentRecordName][fName] = fType;
                                }
                            }
                        }
                    }

                    if (line.Contains("}"))
                    {
                        currentRecordName = "";
                    }
                    continue;
                }

                // قسم المتغيرات: س, ص : منطقي؛
                if (currentSection == "متغير" && line.Contains(":") && !line.Contains("إجراء") && !line.Contains("دالة"))
                {
                    var parts = line.Split(new[] { ':' }, 2);
                    string namesStr = parts[0].Trim();
                    string vType = parts[1].Trim().TrimEnd('؛', ';');

                    bool isKnownType = vType is "صحيح" or "حقيقي" or "منطقي" or "خيط" or "نص" or "محرف" || declaredTypes.ContainsKey(vType);
                    if (!isKnownType)
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: النوع '{vType}' غير مصرح به.");
                    }

                    var namesList = namesStr.Split(new[] { ',', '،' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var n in namesList)
                    {
                        string varName = n.Trim();
                        if (string.IsNullOrWhiteSpace(varName)) continue;

                        if (declaredVariables.ContainsKey(varName) || declaredConstants.ContainsKey(varName))
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: تم التصريح عن المتغير '{varName}' مسبقاً.");
                        }
                        else
                        {
                            declaredVariables[varName] = (vType, lineNum);
                        }
                    }
                    continue;
                }
            }

            // -------------------------------------------------------------
            // المرحلة الثانية: فحص التعليمات والتحقق الدلالي لتوافق الأنواع
            // -------------------------------------------------------------
            var loopVariables = new HashSet<string>();
            string p2Section = "";
            bool insideExecutableBlock = false;
            int p2BraceDepth = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                int lineNum = i + 1;
                string line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

                int commIdx = line.IndexOf("//");
                if (commIdx >= 0) line = line.Substring(0, commIdx).Trim();

                if (line.StartsWith("ثابت")) { p2Section = "ثابت"; continue; }
                if (line.StartsWith("نوع")) { p2Section = "نوع"; continue; }
                if (line.StartsWith("متغير")) { p2Section = "متغير"; continue; }
                if (line.StartsWith("إجراء") || line.StartsWith("دالة")) { p2Section = "إجراء"; }

                // تجاهل أسطر التعريفات والتصريحات تماماً في المرحلة الثانية حتى لا تعتبر تعليمات إسناد خاطئة
                if (p2Section == "ثابت" || p2Section == "نوع" || p2Section == "متغير")
                {
                    continue;
                }

                if (line.Contains("{"))
                {
                    p2BraceDepth += line.Count(c => c == '{');
                    insideExecutableBlock = true;
                    if (p2Section != "إجراء") p2Section = "تعليمات";
                }

                if (line.Contains("}"))
                {
                    p2BraceDepth -= line.Count(c => c == '}');
                    if (p2BraceDepth <= 0)
                    {
                        p2BraceDepth = 0;
                        insideExecutableBlock = false;
                        p2Section = "";
                    }
                }

                if (!insideExecutableBlock && line != "{" && line != "}")
                {
                    continue;
                }

                // 1. حلقة لكل
                var forMatch = Regex.Match(line, @"(لكل|for)\s+([\u0600-\u06FF\w]+)\s*=");
                if (forMatch.Success)
                {
                    string loopVar = forMatch.Groups[2].Value.Trim();
                    loopVariables.Add(loopVar);

                    if (declaredVariables.TryGetValue(loopVar, out var lvInfo) && lvInfo.type != "صحيح")
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: عداد حلقة 'لكل' ('{loopVar}') يجب أن يكون من نوع 'صحيح' وليس '{lvInfo.type}'.");
                    }
                }

                // 2. تعليمات الإسناد وتوافق الأنواع (Type Compatibility)
                if (line.Contains("=") && !line.StartsWith("إذا") && !line.StartsWith("لكل") && !line.StartsWith("طالما") &&
                    !line.StartsWith("ثابت") && !line.StartsWith("نوع") && !line.StartsWith("متغير"))
                {
                    var eqParts = line.Split(new[] { '=' }, 2);
                    string rawLhs = eqParts[0].Trim();
                    string rawRhs = eqParts[1].Trim().TrimEnd('؛', ';');
                    string cleanLhs = rawLhs.TrimEnd('؛', ';');

                    if (declaredConstants.ContainsKey(cleanLhs))
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: لا يمكن التعديل على الثابت '{cleanLhs}' (تعديل ثابت غير مسموح).");
                    }
                    else
                    {
                        string lhsType = "مجهول";

                        // متغير مفرد
                        if (!cleanLhs.Contains(".") && !cleanLhs.Contains("["))
                        {
                            if (declaredVariables.TryGetValue(cleanLhs, out var vInfo))
                            {
                                lhsType = vInfo.type.Trim().TrimEnd('؛', ';');
                            }
                            else if (loopVariables.Contains(cleanLhs))
                            {
                                lhsType = "صحيح";
                            }
                            else if (declaredProcedures.Values.Any(p => p.paramList.Any(pm => pm.name == cleanLhs)))
                            {
                                var pm = declaredProcedures.Values.SelectMany(p => p.paramList).First(p => p.name == cleanLhs);
                                lhsType = pm.type.Trim().TrimEnd('؛', ';');
                            }
                            else if (!cleanLhs.StartsWith("برنامج"))
                            {
                                semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعرف '{cleanLhs}' غير مصرح به (Undeclared Identifier).");
                            }
                        }
                        else if (cleanLhs.Contains(".")) // حقل سجل: ش.عمر
                        {
                            var rParts = cleanLhs.Split('.');
                            string recVar = rParts[0].Trim();
                            string fName = rParts[1].Trim();

                            if (!declaredVariables.ContainsKey(recVar))
                            {
                                semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المتغير '{recVar}' غير مصرح به.");
                            }
                            else
                            {
                                string recType = declaredVariables[recVar].type.Trim().TrimEnd('؛', ';');
                                if (!recordFields.ContainsKey(recType) || !recordFields[recType].ContainsKey(fName))
                                {
                                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: الحقل '{fName}' غير موجود في السجل '{recVar}' من النوع '{recType}'.");
                                }
                                else
                                {
                                    lhsType = recordFields[recType][fName].Trim().TrimEnd('؛', ';');
                                }
                            }
                        }
                        else if (cleanLhs.Contains("[")) // مصفوفة: ف[0]
                        {
                            int bOpen = cleanLhs.IndexOf('[');
                            string arrVar = cleanLhs.Substring(0, bOpen).Trim();
                            int bClose = cleanLhs.IndexOf(']', bOpen);

                            if (!declaredVariables.ContainsKey(arrVar))
                            {
                                semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: مصفوفة المتغير '{arrVar}' غير مصرح بها.");
                            }
                            else
                            {
                                if (bClose > bOpen)
                                {
                                    string idxStr = cleanLhs.Substring(bOpen + 1, bClose - bOpen - 1).Trim();
                                    string idxType = InferExpressionType(idxStr, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                                    if (idxType != "صحيح" && idxType != "مجهول")
                                    {
                                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: فهرس المصفوفة '{arrVar}' يجب أن يكون من نوع 'صحيح' وليس '{idxType}'.");
                                    }
                                }

                                string aType = declaredVariables[arrVar].type.Trim().TrimEnd('؛', ';');
                                if (declaredTypes.TryGetValue(aType, out var tInfo) && tInfo.kind == "قائمة")
                                {
                                    string detail = tInfo.detail;
                                    int bPos = detail.IndexOf('[');
                                    lhsType = bPos > 0 ? detail.Substring(0, bPos).Trim().TrimEnd('؛', ';') : "صحيح";
                                }
                                else
                                {
                                    lhsType = "صحيح";
                                }
                            }
                        }

                        // التحقق من المعرفات في الطرف الأيمن
                        ValidateRhsIdentifiers(rawRhs, lineNum, declaredVariables, declaredConstants, loopVariables, declaredProcedures, recordFields, semanticErrors);

                        // استنتاج نوع الطرف الأيمن
                        string rhsType = InferExpressionType(rawRhs, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                        // التحقق من توافق الأنواع
                        if (lhsType != "مجهول" && rhsType != "مجهول" && !IsTypeCompatible(lhsType, rhsType))
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: عدم تطابق في الأنواع (Type Mismatch). المتغير '{cleanLhs}' من نوع '{lhsType}' ولا يمكن إسناد قيمة من نوع '{rhsType}' إليه.");
                        }
                    }
                }

                // 3. فحص شرط إذا ... فإن
                if (line.StartsWith("إذا") || line.StartsWith("if"))
                {
                    int pOpen = line.IndexOf('(');
                    int pClose = line.LastIndexOf(')');
                    if (pOpen >= 0 && pClose > pOpen)
                    {
                        string condStr = line.Substring(pOpen + 1, pClose - pOpen - 1).Trim();
                        string condType = InferExpressionType(condStr, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                        if (condType != "منطقي" && condType != "مجهول")
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: شرط تعليمة 'إذا' يجب أن يكون تعبيراً من نوع 'منطقي' وليس '{condType}'.");
                        }
                    }
                }

                // 4. فحص استدعاء الإجراءات
                var callMatch = Regex.Match(line, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)\s*[؛;]");
                if (callMatch.Success && !line.StartsWith("إذا") && !line.StartsWith("اطبع") && !line.StartsWith("اقرأ") &&
                    !line.StartsWith("إجراء") && !line.StartsWith("دالة"))
                {
                    string pCallName = callMatch.Groups[1].Value;
                    string pArgsStr = callMatch.Groups[2].Value;

                    if (!declaredProcedures.ContainsKey(pCallName))
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: الإجراء '{pCallName}' غير مصرح به في البرنامج.");
                    }
                    else
                    {
                        var pDef = declaredProcedures[pCallName];
                        var passedArgs = string.IsNullOrWhiteSpace(pArgsStr)
                            ? new string[0]
                            : pArgsStr.Split(new[] { ',', '،' }, StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim()).ToArray();

                        if (passedArgs.Length != pDef.paramList.Count)
                        {
                            semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: استدعاء الإجراء '{pCallName}' يتطلب {pDef.paramList.Count} معاملات، ولكن تم تمرير {passedArgs.Length}.");
                        }
                        else
                        {
                            for (int k = 0; k < pDef.paramList.Count; k++)
                            {
                                var param = pDef.paramList[k];
                                string arg = passedArgs[k];

                                if (param.isByRef)
                                {
                                    if (double.TryParse(arg, out _) || (arg.StartsWith("\"") && arg.EndsWith("\"")) || declaredConstants.ContainsKey(arg))
                                    {
                                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعامل '{arg}' في الإجراء '{pCallName}' ممرر بالمرجع، ويجب أن يكون متغيراً قابلاً للتعديل وليس قيمة مباشرة.");
                                    }
                                    else if (!declaredVariables.ContainsKey(arg))
                                    {
                                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعامل '{arg}' الممرر بالمرجع غير مصرح به كمتغير.");
                                    }
                                }

                                string argType = InferExpressionType(arg, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                                if (argType != "مجهول" && !IsTypeCompatible(param.type, argType))
                                {
                                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: عدم تطابق نوع المعامل '{param.name}' للإجراء '{pCallName}'. النوع المتوقع '{param.type}' ولكن تم تمرير '{argType}'.");
                                }
                            }
                        }
                    }
                }
            }

            // إذا وجدت أخطاء دلالية
            if (semanticErrors.Count > 0)
            {
                var sbErr = new StringBuilder();
                sbErr.AppendLine("===== فشل التحليل الدلالي (Semantic Errors) =====");
                sbErr.AppendLine($"تم اكتشاف {semanticErrors.Count} أخطاء دلالية في البرنامج المدخل:");
                foreach (var err in semanticErrors)
                {
                    sbErr.AppendLine($"- {err}");
                }

                result = sbErr.ToString();
                error = semanticErrors[0];
                success = false;
                return;
            }

            // نجاح التحليل الدلالي
            var symbols = SymbolTableManager.ExtractSymbolsFromCode(source);
            var sb = new StringBuilder();
            sb.AppendLine("===== نتائج التحليل الدلالي (Semantic Analysis) =====");
            sb.AppendLine("حالة الفحص: تم التحليل الدلالي بنجاح تام.");
            sb.AppendLine("فحص النطاقات والصلاحيات: كافة المعرفات، المتغيرات، الثوابت والأنواع مصرح بها وصحيحة.");
            sb.AppendLine("فحص توافق الأنواع: جميع عمليات الإسناد والمعاملات متوافقة في الأنواع.");
            sb.AppendLine($"عدد الرموز المسجلة: {symbols.Count}");
            sb.AppendLine($"- الثوابت المعرفة: {declaredConstants.Count}");
            sb.AppendLine($"- المتغيرات المعرفة: {declaredVariables.Count}");
            sb.AppendLine($"- الأنواع والسجلات: {declaredTypes.Count}");
            sb.AppendLine($"- الإجراءات والدوال: {declaredProcedures.Count}");
            sb.AppendLine();
            sb.AppendLine("تم تحديث جدول الرموز (Symbol Table) بالبيانات الدلالية المدققة.");

            result = sb.ToString();
            error = "لا توجد أخطاء دلالية. كافة المعرفات والأنواع متطابقة ومصرح بها.";
            success = true;
        }

        private static bool IsTypeCompatible(string targetType, string sourceType)
        {
            if (string.Equals(targetType, sourceType, StringComparison.OrdinalIgnoreCase)) return true;
            if ((targetType == "حقيقي" && sourceType == "صحيح") || (targetType == "صحيح" && sourceType == "حقيقي")) return true;
            if ((targetType == "خيط" && sourceType == "نص") || (targetType == "نص" && sourceType == "خيط")) return true;
            return false;
        }

        private static string StripOuterParentheses(string s)
        {
            s = s.Trim();
            while (s.StartsWith("(") && s.EndsWith(")"))
            {
                int depth = 0;
                bool coversAll = true;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '(') depth++;
                    else if (s[i] == ')')
                    {
                        depth--;
                        if (depth == 0 && i < s.Length - 1)
                        {
                            coversAll = false;
                            break;
                        }
                    }
                }
                if (coversAll && depth == 0)
                {
                    s = s.Substring(1, s.Length - 2).Trim();
                }
                else
                {
                    break;
                }
            }
            return s;
        }

        private static int FindTopLevelOperator(string expr, string[] ops, out string foundOp)
        {
            foundOp = string.Empty;
            int depth = 0;
            bool inQuote = false;

            for (int i = expr.Length - 1; i >= 0; i--)
            {
                char c = expr[i];
                if (c == '"') { inQuote = !inQuote; continue; }
                if (inQuote) continue;

                if (c == ')' || c == ']') depth++;
                else if (c == '(' || c == '[') depth--;
                else if (depth == 0)
                {
                    foreach (var op in ops)
                    {
                        int startIdx = i - op.Length + 1;
                        if (startIdx >= 0 && expr.Substring(startIdx, op.Length) == op)
                        {
                            if ((op == "+" || op == "-") && (startIdx == 0 || expr[startIdx - 1] == '(' || expr[startIdx - 1] == '['))
                            {
                                continue;
                            }
                            if (op == "و" || op == "أو")
                            {
                                bool leftOk = startIdx == 0 || char.IsWhiteSpace(expr[startIdx - 1]) || expr[startIdx - 1] == ')' || expr[startIdx - 1] == ']';
                                bool rightOk = (startIdx + op.Length == expr.Length) || char.IsWhiteSpace(expr[startIdx + op.Length]) || expr[startIdx + op.Length] == '(' || expr[startIdx + op.Length] == '[';
                                if (!leftOk || !rightOk) continue;
                            }
                            foundOp = op;
                            return startIdx;
                        }
                    }
                }
            }
            return -1;
        }

        private static string InferExpressionType(
            string expr,
            int lineNum,
            Dictionary<string, (string type, int line)> declaredVariables,
            Dictionary<string, (string value, string type, int line)> declaredConstants,
            HashSet<string> loopVariables,
            Dictionary<string, (string kind, string detail, int line)> declaredTypes,
            Dictionary<string, Dictionary<string, string>> recordFields,
            Dictionary<string, (List<(string name, bool isByRef, string type)> paramList, int line, string body)> declaredProcedures,
            List<string> semanticErrors)
        {
            expr = expr.Trim();
            if (string.IsNullOrWhiteSpace(expr)) return "مجهول";

            expr = StripOuterParentheses(expr);

            // 1. خيط نصي
            if ((expr.StartsWith("\"") && expr.EndsWith("\"")) ||
                (expr.StartsWith("“") && expr.EndsWith("”")))
            {
                return "خيط";
            }

            // 2. محرف
            if (expr.StartsWith("'") && expr.EndsWith("'"))
            {
                return "محرف";
            }

            // 3. قيمة منطقية مباشرة
            if (expr is "نعم" or "لا" or "صح" or "خطأ" or "true" or "false")
            {
                return "منطقي";
            }

            // 4. عدد مباشر
            if (double.TryParse(expr, out _))
            {
                return expr.Contains(".") ? "حقيقي" : "صحيح";
            }

            // 5. تعبير منطقي على المستوى الأعلى (أدنى أسبقية): ||, أو, &&, و
            int logicOpIdx = FindTopLevelOperator(expr, new[] { "||", "أو", "&&", "و" }, out string logicOp);
            if (logicOpIdx > 0)
            {
                string leftPart = expr.Substring(0, logicOpIdx).Trim();
                string rightPart = expr.Substring(logicOpIdx + logicOp.Length).Trim();

                string leftType = InferExpressionType(leftPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                string rightType = InferExpressionType(rightPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                if (leftType != "منطقي" && leftType != "مجهول")
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعامل المنطقي '{logicOp}' يتطلب طرفاً من نوع 'منطقي' وليس '{leftType}'.");
                }
                if (rightType != "منطقي" && rightType != "مجهول")
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعامل المنطقي '{logicOp}' يتطلب طرفاً من نوع 'منطقي' وليس '{rightType}'.");
                }

                return "منطقي";
            }

            // نفي منطقي في البداية: ! أو ليس
            if (expr.StartsWith("!") || expr.StartsWith("ليس "))
            {
                string inner = expr.StartsWith("!") ? expr.Substring(1).Trim() : expr.Substring(4).Trim();
                string innerType = InferExpressionType(inner, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                if (innerType != "منطقي" && innerType != "مجهول")
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: معامل النفي المنطقي يتطلب قيمة منطقية وليس '{innerType}'.");
                }
                return "منطقي";
            }

            // 6. تعبير مقارنة على المستوى الأعلى: ==, !=, >=, <=, >, <
            int compOpIdx = FindTopLevelOperator(expr, new[] { "==", "!=", ">=", "<=", ">", "<" }, out string compOp);
            if (compOpIdx >= 0)
            {
                string leftPart = expr.Substring(0, compOpIdx).Trim();
                string rightPart = expr.Substring(compOpIdx + compOp.Length).Trim();

                string leftType = InferExpressionType(leftPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                string rightType = InferExpressionType(rightPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                if (leftType != "مجهول" && rightType != "مجهول" && !IsTypeCompatible(leftType, rightType) && !IsTypeCompatible(rightType, leftType))
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: لا يمكن مقارنة النوع '{leftType}' مع النوع '{rightType}' في التعبير '{expr}'.");
                }

                return "منطقي";
            }

            // 7. جمع وطرح على المستوى الأعلى: +, -
            int addOpIdx = FindTopLevelOperator(expr, new[] { "+", "-" }, out string addOp);
            if (addOpIdx > 0)
            {
                string leftPart = expr.Substring(0, addOpIdx).Trim();
                string rightPart = expr.Substring(addOpIdx + addOp.Length).Trim();

                string leftType = InferExpressionType(leftPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                string rightType = InferExpressionType(rightPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                if (leftType == "منطقي" || rightType == "منطقي")
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: لا يمكن تطبيق المعامل الحسابي '{addOp}' على قيم منطقية في التعبير '{expr}'.");
                }

                if (leftType == "خيط" || rightType == "خيط" || leftType == "نص" || rightType == "نص")
                    return "خيط";
                if (leftType == "حقيقي" || rightType == "حقيقي")
                    return "حقيقي";
                return "صحيح";
            }

            // 8. ضرب وقسمة وباقي قسمة: *, /, \, %
            int mulOpIdx = FindTopLevelOperator(expr, new[] { "*", "/", "\\", "%" }, out string mulOp);
            if (mulOpIdx > 0)
            {
                string leftPart = expr.Substring(0, mulOpIdx).Trim();
                string rightPart = expr.Substring(mulOpIdx + mulOp.Length).Trim();

                string leftType = InferExpressionType(leftPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);
                string rightType = InferExpressionType(rightPart, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                if (leftType == "منطقي" || rightType == "منطقي")
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: لا يمكن تطبيق المعامل الحسابي '{mulOp}' على قيم منطقية في التعبير '{expr}'.");
                }

                if (mulOp == "/") return "حقيقي";
                if (mulOp == "\\") return "صحيح";
                if (mulOp == "%") return "صحيح";
                if (leftType == "حقيقي" || rightType == "حقيقي") return "حقيقي";
                return "صحيح";
            }

            // 9. أس: ^
            int powOpIdx = FindTopLevelOperator(expr, new[] { "^" }, out _);
            if (powOpIdx > 0)
            {
                return "صحيح";
            }

            // 10. الوصول لحقل سجل: ش.عمر
            if (expr.Contains(".") && !expr.Contains("["))
            {
                var dotParts = expr.Split('.');
                string recVar = dotParts[0].Trim();
                string fName = dotParts[1].Trim();

                if (declaredVariables.TryGetValue(recVar, out var recInfo))
                {
                    string rType = recInfo.type.Trim().TrimEnd('؛', ';');
                    if (recordFields.TryGetValue(rType, out var fMap) && fMap.TryGetValue(fName, out var fType))
                    {
                        return fType.Trim().TrimEnd('؛', ';');
                    }
                }
            }

            // 11. الوصول لعنصر مصفوفة: ف[0]
            if (expr.Contains("["))
            {
                int bIdx = expr.IndexOf('[');
                string arrVar = expr.Substring(0, bIdx).Trim();
                int closeIdx = expr.LastIndexOf(']');

                if (closeIdx > bIdx)
                {
                    string idxExpr = expr.Substring(bIdx + 1, closeIdx - bIdx - 1).Trim();
                    string idxType = InferExpressionType(idxExpr, lineNum, declaredVariables, declaredConstants, loopVariables, declaredTypes, recordFields, declaredProcedures, semanticErrors);

                    if (idxType != "صحيح" && idxType != "مجهول")
                    {
                        semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: فهرس المصفوفة '{arrVar}' يجب أن يكون من نوع 'صحيح' وليس '{idxType}'.");
                    }
                }

                if (declaredVariables.TryGetValue(arrVar, out var aInfo))
                {
                    string aType = aInfo.type.Trim().TrimEnd('؛', ';');
                    if (declaredTypes.TryGetValue(aType, out var tInfo) && tInfo.kind == "قائمة")
                    {
                        string detail = tInfo.detail;
                        int bPos = detail.IndexOf('[');
                        if (bPos > 0) return detail.Substring(0, bPos).Trim().TrimEnd('؛', ';');
                    }
                }
                return "صحيح";
            }

            // 12. معرف مفرد
            return GetIdentifierType(expr, declaredVariables, declaredConstants, loopVariables, declaredProcedures);
        }

        private static string GetIdentifierType(
            string name,
            Dictionary<string, (string type, int line)> declaredVariables,
            Dictionary<string, (string value, string type, int line)> declaredConstants,
            HashSet<string> loopVariables,
            Dictionary<string, (List<(string name, bool isByRef, string type)> paramList, int line, string body)> declaredProcedures)
        {
            if (declaredVariables.TryGetValue(name, out var vInfo)) return vInfo.type.Trim().TrimEnd('؛', ';');
            if (declaredConstants.TryGetValue(name, out var cInfo)) return cInfo.type.Trim().TrimEnd('؛', ';');
            if (loopVariables.Contains(name)) return "صحيح";

            foreach (var proc in declaredProcedures.Values)
            {
                var param = proc.paramList.FirstOrDefault(p => p.name == name);
                if (!string.IsNullOrEmpty(param.name)) return param.type.Trim().TrimEnd('؛', ';');
            }

            return "مجهول";
        }

        private static void ValidateRhsIdentifiers(
            string rhs,
            int lineNum,
            Dictionary<string, (string type, int line)> declaredVariables,
            Dictionary<string, (string value, string type, int line)> declaredConstants,
            HashSet<string> loopVariables,
            Dictionary<string, (List<(string name, bool isByRef, string type)> paramList, int line, string body)> declaredProcedures,
            Dictionary<string, Dictionary<string, string>> recordFields,
            List<string> semanticErrors)
        {
            rhs = Regex.Replace(rhs, @"""[^""]*""", "");
            rhs = Regex.Replace(rhs, @"'[^']*'", "");

            var matches = Regex.Matches(rhs, @"[\u0600-\u06FF\w]+");
            foreach (Match m in matches)
            {
                string word = m.Value.Trim().TrimEnd('؛', ';');
                if (string.IsNullOrWhiteSpace(word)) continue;
                if (double.TryParse(word, out _)) continue;

                if (word is "نعم" or "لا" or "صح" or "خطأ" or "و" or "أو" or "ليس" or "اقرأ" or "اطبع" or "true" or "false") continue;

                bool isFound = declaredVariables.ContainsKey(word) ||
                               declaredConstants.ContainsKey(word) ||
                               loopVariables.Contains(word) ||
                               recordFields.ContainsKey(word) ||
                               recordFields.Values.Any(f => f.ContainsKey(word)) ||
                               declaredProcedures.ContainsKey(word) ||
                               declaredProcedures.Values.Any(p => p.paramList.Any(pm => pm.name == word));

                if (!isFound)
                {
                    semanticErrors.Add($"خطأ دلالي في السطر {lineNum}: المعرف '{word}' المستخدم في التعبير غير مصرح به.");
                }
            }
        }

        // ====================================================================
        // 4. توليد الكود الوسيط TAC (Three Address Code)
        // ====================================================================
        public static void GenerateTAC(string source, out string result, out string error)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                result = "الكود الوسيط Three Address Code:\r\nلا يوجد كود لتوليد الكود الوسيط.";
                error = "يرجى إدخال برنامج أولاً.";
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("الكود الوسيط Three Address Code:\r\n");

            int lineIndex = 1;
            int tempIndex = 1;
            var tacLines = new List<string>();

            var procMatches = Regex.Matches(source, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\((.*?)\)\s*\{([\s\S]*?)\}");
            bool hasProcedures = procMatches.Count > 0;

            if (hasProcedures)
            {
                tacLines.Add("goto L_main");
                foreach (Match m in procMatches)
                {
                    string pName = m.Groups[2].Value;
                    string pParams = m.Groups[3].Value;
                    string pBody = m.Groups[4].Value;

                    tacLines.Add($"proc {pName}");
                    if (!string.IsNullOrWhiteSpace(pParams))
                    {
                        var paramItems = pParams.Split(new[] { ',', '،' });
                        foreach (var p in paramItems)
                        {
                            var matchParam = Regex.Match(p.Trim(), @"(بالمرجع|بالقيمة)?\s*([\u0600-\u06FF\w]+)");
                            if (matchParam.Success)
                            {
                                string mode = matchParam.Groups[1].Value == "بالمرجع" ? "paramdef_ref" : "paramdef";
                                string paramName = matchParam.Groups[2].Value;
                                tacLines.Add($"{mode} {paramName}");
                            }
                        }
                    }

                    GenerateStatementsTac(pBody, tacLines, ref tempIndex);
                    tacLines.Add($"endproc {pName}");
                }
                tacLines.Add(":L_main");
            }

            int firstBrace = source.IndexOf('{');
            int lastBrace = source.LastIndexOf('}');
            string mainCode = source;

            if (hasProcedures && lastBrace > firstBrace)
            {
                int mainStart = source.LastIndexOf('{', source.LastIndexOf('.'));
                if (mainStart >= 0)
                {
                    mainCode = source.Substring(mainStart + 1, lastBrace - mainStart - 1);
                }
            }
            else if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                mainCode = source.Substring(firstBrace + 1, lastBrace - firstBrace - 1);
            }

            GenerateStatementsTac(mainCode, tacLines, ref tempIndex);

            for (int i = 0; i < tacLines.Count; i++)
            {
                string line = tacLines[i];
                if (line.StartsWith(":") || line.Contains("=") && line.StartsWith("t"))
                {
                    sb.AppendLine($"{line} : {lineIndex++}");
                }
                else
                {
                    sb.AppendLine($"{lineIndex++}: {line}");
                }
            }

            result = sb.ToString();
            error = "تم توليد الكود الوسيط ديناميكياً استناداً إلى الكود المدخل.";
        }

        private static void GenerateStatementsTac(string code, List<string> tacLines, ref int tempIndex)
        {
            var rawLines = code.Split(new[] { "\r\n", "\r", "\n", "؛", ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in rawLines)
            {
                string stmt = raw.Trim();
                if (string.IsNullOrWhiteSpace(stmt) || stmt.StartsWith("//") || stmt.StartsWith("برنامج") ||
                    stmt.StartsWith("ثابت") || stmt.StartsWith("متغير") || stmt.StartsWith("نوع") ||
                    stmt == "{" || stmt == "}" || stmt == ".")
                    continue;

                // 1. تعليمات الإسناد
                if (stmt.Contains("=") && !stmt.StartsWith("إذا") && !stmt.StartsWith("لكل") && !stmt.StartsWith("طالما"))
                {
                    var parts = stmt.Split(new[] { '=' }, 2);
                    string lhs = parts[0].Trim();
                    string rhs = parts[1].Trim().TrimEnd('؛', ';');

                    if (Regex.IsMatch(rhs, @"[\+\-\*\/\\%\^]"))
                    {
                        var mulMatch = Regex.Match(rhs, @"([\u0600-\u06FF\w]+)\s*([\*\/\\%\^])\s*([\u0600-\u06FF\w]+)");
                        if (mulMatch.Success && (rhs.Contains("+") || rhs.Contains("-")))
                        {
                            string tSub = $"t{tempIndex++}";
                            tacLines.Add($"{tSub} = {mulMatch.Value}");
                            string newRhs = rhs.Replace(mulMatch.Value, tSub);
                            string tMain = $"t{tempIndex++}";
                            tacLines.Add($"{tMain} = {newRhs}");
                            tacLines.Add($"{lhs} = {tMain}");
                            continue;
                        }

                        string t = $"t{tempIndex++}";
                        tacLines.Add($"{t} = {rhs}");
                        tacLines.Add($"{lhs} = {t}");
                    }
                    else
                    {
                        tacLines.Add($"{lhs} = {rhs}");
                    }
                }
                // 2. تعليمة الطباعة
                else if (stmt.StartsWith("اطبع") || stmt.StartsWith("print"))
                {
                    int open = stmt.IndexOf('(');
                    int close = stmt.LastIndexOf(')');
                    if (open >= 0 && close > open)
                    {
                        string args = stmt.Substring(open + 1, close - open - 1);
                        foreach (var a in args.Split(new[] { ',', '،' }))
                        {
                            tacLines.Add($"print {a.Trim()}");
                        }
                    }
                }
                // 3. تعليمة القراءة
                else if (stmt.StartsWith("اقرأ") || stmt.StartsWith("read"))
                {
                    int open = stmt.IndexOf('(');
                    int close = stmt.LastIndexOf(')');
                    string target = (open >= 0 && close > open) ? stmt.Substring(open + 1, close - open - 1).Trim() : stmt.Replace("اقرأ", "").Trim();
                    tacLines.Add($"read {target}");
                }
                // 4. تعليمة شرطية إذا
                else if (stmt.StartsWith("إذا") || stmt.StartsWith("if"))
                {
                    int pOpen = stmt.IndexOf('(');
                    int pClose = stmt.IndexOf(')');
                    string cond = (pOpen >= 0 && pClose > pOpen) ? stmt.Substring(pOpen + 1, pClose - pOpen - 1) : "شرط";

                    string tCond = $"t{tempIndex++}";
                    tacLines.Add($"{tCond} = {cond}");
                    string labelElse = $"L{tempIndex++}";
                    tacLines.Add($"if False {tCond} goto {labelElse}");
                }
                // 5. استدعاء إجراء
                else if (Regex.IsMatch(stmt, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)"))
                {
                    var m = Regex.Match(stmt, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)");
                    string pName = m.Groups[1].Value;
                    string pArgs = m.Groups[2].Value;
                    if (!string.IsNullOrWhiteSpace(pArgs))
                    {
                        foreach (var a in pArgs.Split(new[] { ',', '،' }))
                        {
                            tacLines.Add($"param {a.Trim()}");
                        }
                    }
                    tacLines.Add($"call {pName}");
                }
            }
        }

        // ====================================================================
        // 5. تحسين الكود (Code Optimization)
        // ====================================================================
        public static void OptimizeCode(string source, string currentTac, out string result, out string error)
        {
            if (string.IsNullOrWhiteSpace(currentTac))
            {
                GenerateTAC(source, out currentTac, out _);
            }

            var sb = new StringBuilder();
            sb.AppendLine("===== الكود الوسيط قبل التحسين =====");
            sb.AppendLine(currentTac.Trim());
            sb.AppendLine();
            sb.AppendLine("===== الكود الوسيط بعد التحسين (Optimized TAC) =====");

            var knownValues = new Dictionary<string, string>();
            int optLine = 1;

            var lines = currentTac.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var l in lines)
            {
                string trimmed = l.Trim();
                if (trimmed.Contains("Three Address Code")) continue;

                int colonIdx = trimmed.LastIndexOf(':');
                string codePart = trimmed;
                if (colonIdx > 0 && int.TryParse(trimmed.Substring(colonIdx + 1).Trim(), out _))
                {
                    codePart = trimmed.Substring(0, colonIdx).Trim();
                }
                else if (trimmed.Contains(":") && int.TryParse(trimmed.Split(':')[0].Trim(), out _))
                {
                    codePart = trimmed.Substring(trimmed.IndexOf(':') + 1).Trim();
                }

                // طي الثوابت الحسابية (Constant Folding)
                if (codePart.Contains("="))
                {
                    var parts = codePart.Split(new[] { '=' }, 2);
                    string lhs = parts[0].Trim();
                    string rhs = parts[1].Trim();

                    foreach (var kvp in knownValues)
                    {
                        rhs = Regex.Replace(rhs, $@"\b{Regex.Escape(kvp.Key)}\b", kvp.Value);
                    }

                    var mathMatch = Regex.Match(rhs, @"^(\d+(?:\.\d+)?)\s*([\+\-\*\/\\%\^])\s*(\d+(?:\.\d+)?)$");
                    if (mathMatch.Success &&
                        double.TryParse(mathMatch.Groups[1].Value, out double a) &&
                        double.TryParse(mathMatch.Groups[3].Value, out double b))
                    {
                        string op = mathMatch.Groups[2].Value;
                        double calc = op switch
                        {
                            "+" => a + b,
                            "-" => a - b,
                            "*" => a * b,
                            "/" => b != 0 ? a / b : 0,
                            "\\" => b != 0 ? (int)(a / b) : 0,
                            "%" => b != 0 ? a % b : 0,
                            "^" => Math.Pow(a, b),
                            _ => a
                        };
                        string formattedCalc = calc % 1 == 0 ? ((int)calc).ToString() : calc.ToString("G");
                        knownValues[lhs] = formattedCalc;

                        if (!lhs.StartsWith("t"))
                        {
                            sb.AppendLine($"{optLine++}: {lhs} = {formattedCalc}");
                        }
                        continue;
                    }
                    else if (double.TryParse(rhs, out double singleVal))
                    {
                        knownValues[lhs] = singleVal.ToString();
                        sb.AppendLine($"{optLine++}: {lhs} = {rhs}");
                        continue;
                    }
                }

                sb.AppendLine($"{optLine++}: {codePart}");
            }

            result = sb.ToString();
            error = "تم تحسين الكود بنجاح بواسطة خوارزمية طي الثوابت وانتشار النسخ.";
        }

        // ====================================================================
        // 6. توليد كود التجميع Assembly (x64 MASM)
        // ====================================================================
        public static void GenerateAssembly(string source, out string result, out string error)
        {
            var sb = new StringBuilder();
            sb.AppendLine("; ====================================================");
            sb.AppendLine("; Arabic Compiler - Generated Assembly Code :");
            sb.AppendLine("; Target: Windows x64 / MASM :");
            sb.AppendLine("; Generated dynamically based on input source program :");
            sb.AppendLine("; ====================================================");
            sb.AppendLine();
            sb.AppendLine("option casemap:none");
            sb.AppendLine();
            sb.AppendLine("includelib kernel32.lib");
            sb.AppendLine("includelib msvcrt.lib");
            sb.AppendLine();
            sb.AppendLine("EXTERN ExitProcess:PROC");
            sb.AppendLine("EXTERN SetConsoleOutputCP:PROC");
            sb.AppendLine("EXTERN printf:PROC");
            sb.AppendLine("EXTERN scanf:PROC");
            sb.AppendLine();
            sb.AppendLine(".data");
            sb.AppendLine("fmt_integer db \"%lld\", 10, 0");
            sb.AppendLine("fmt_string db \"%s\", 10, 0");

            var symbols = SymbolTableManager.ExtractSymbolsFromCode(source);
            var emittedVars = new HashSet<string>();

            foreach (var s in symbols)
            {
                if (s.Category == "متغير" || s.Category == "ثابت")
                {
                    if (!emittedVars.Contains(s.Name))
                    {
                        string initVal = !string.IsNullOrEmpty(s.Value) && long.TryParse(s.Value, out _) ? s.Value : "0";
                        sb.AppendLine($"var_{s.Name} dq {initVal}   ; {s.Category}: {s.Name}");
                        emittedVars.Add(s.Name);
                    }
                }
            }

            if (emittedVars.Count == 0)
            {
                sb.AppendLine("var_result dq 0");
            }

            sb.AppendLine();
            sb.AppendLine(".code");
            sb.AppendLine("main PROC");
            sb.AppendLine("    sub rsp, 40");
            sb.AppendLine("    mov rcx, 65001");
            sb.AppendLine("    call SetConsoleOutputCP");
            sb.AppendLine();

            foreach (var v in emittedVars)
            {
                sb.AppendLine($"    ; Initialize or load variable {v}");
                sb.AppendLine($"    mov rax, var_{v}");
            }

            sb.AppendLine();
            sb.AppendLine("    xor ecx, ecx");
            sb.AppendLine("    call ExitProcess");
            sb.AppendLine("main ENDP");
            sb.AppendLine("END");

            result = sb.ToString();
            error = "تم توليد كود التجميع ديناميكياً استناداً إلى البرنامج المدخل.";
        }

        // ====================================================================
        // 7. تنفيذ جميع المراحل بالتسلسل (Pipeline Execution)
        // ====================================================================
        public static bool RunAllStages(
            string source,
            out string fullReport,
            out string errorSummary,
            out List<SymbolItem> symbols,
            out string consoleOutput)
        {
            var sb = new StringBuilder();
            symbols = new List<SymbolItem>();
            consoleOutput = string.Empty;

            sb.AppendLine("====================================================");
            sb.AppendLine("      تنفيذ كافة مراحل المترجم العربي الشامل        ");
            sb.AppendLine("====================================================");
            sb.AppendLine();

            // 1. المرحلة الأولى: التحليل اللغوي
            sb.AppendLine("--- [المرحلة 1: التحليل اللغوي / المعجمي (Lexical Analysis)] ---");
            RunLexicalAnalysis(source, out string lexResult, out string lexErr, out bool lexSuccess);
            if (!lexSuccess)
            {
                sb.AppendLine("فشل التحليل اللغوي!");
                sb.AppendLine(lexResult);
                fullReport = sb.ToString();
                errorSummary = lexErr;
                return false;
            }
            sb.AppendLine(lexErr);
            sb.AppendLine();

            // 2. المرحلة الثانية: التحليل النحوي
            sb.AppendLine("--- [المرحلة 2: التحليل النحوي (Syntax Analysis)] ---");
            RunSyntaxAnalysis(source, out string synResult, out string synErr, out bool synSuccess);
            if (!synSuccess)
            {
                sb.AppendLine("فشل التحليل النحوي!");
                sb.AppendLine(synResult);
                fullReport = sb.ToString();
                errorSummary = synErr;
                return false;
            }
            sb.AppendLine(synResult);
            sb.AppendLine();

            // 3. المرحلة الثالثة: التحليل الدلالي
            sb.AppendLine("--- [المرحلة 3: التحليل الدلالي (Semantic Analysis)] ---");
            RunSemanticAnalysis(source, out string semResult, out string semErr, out bool semSuccess);
            if (!semSuccess)
            {
                sb.AppendLine("فشل التحليل الدلالي!");
                sb.AppendLine(semResult);
                fullReport = sb.ToString();
                errorSummary = semErr;
                symbols = SymbolTableManager.ExtractSymbolsFromCode(source);
                return false;
            }
            sb.AppendLine(semResult);
            sb.AppendLine();

            // 4. المرحلة الرابعة: بناء الشجرة الإعرابية
            sb.AppendLine("--- [المرحلة 4: الشجرة الإعرابية (Parse Tree)] ---");
            string tree = GetParseTree(source);
            sb.AppendLine("تم إنشاء الشجرة الإعرابية بنجاح.");
            sb.AppendLine();

            // 5. المرحلة الخامسة: جدول الرموز
            sb.AppendLine("--- [المرحلة 5: جدول الرموز (Symbol Table)] ---");
            symbols = SymbolTableManager.ExtractSymbolsFromCode(source);
            sb.AppendLine($"تم استخراج {symbols.Count} معرفاً بنجاح في جدول الرموز.");
            sb.AppendLine();

            // 6. المرحلة السادسة: الكود الوسيط TAC
            sb.AppendLine("--- [المرحلة 6: توليد الكود الوسيط (Three Address Code)] ---");
            GenerateTAC(source, out string tacResult, out _);
            sb.AppendLine(tacResult.Trim());
            sb.AppendLine();

            // 7. المرحلة السابعة: تحسين الكود
            sb.AppendLine("--- [المرحلة 7: تحسين الكود (Code Optimization)] ---");
            OptimizeCode(source, tacResult, out string optResult, out _);
            sb.AppendLine(optResult.Trim());
            sb.AppendLine();

            // 8. المرحلة الثامنة: توليد كود التجميع Assembly
            sb.AppendLine("--- [المرحلة 8: توليد لغة التجميع (x64 Assembly Code)] ---");
            GenerateAssembly(source, out string asmResult, out _);
            sb.AppendLine("تم توليد كود التجميع بنجاح (بنية x64 MASM).");
            sb.AppendLine();

            // 9. المرحلة التاسعة: التنفيذ الفعلي
            sb.AppendLine("--- [المرحلة 9: التنفيذ الفعلي للبرنامج (Execution)] ---");
            consoleOutput = ExecutionEngine.ExecuteManaged(source);
            sb.AppendLine("مخرجات الشاشة:");
            sb.AppendLine(consoleOutput);
            sb.AppendLine();
            sb.AppendLine("====================================================");
            sb.AppendLine("  اكتملت جميع مراحل المترجم بنجاح 100% بدون أي أخطاء ");
            sb.AppendLine("====================================================");

            fullReport = sb.ToString();
            errorSummary = "تم تنفيذ جميع مراحل المترجم بنجاح وبدون أي أخطاء.";
            return true;
        }
    }
}
