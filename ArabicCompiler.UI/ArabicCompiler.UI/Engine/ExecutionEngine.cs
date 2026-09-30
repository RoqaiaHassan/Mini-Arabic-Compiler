using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ArabicCompiler.UI.Engine
{
    public static class ExecutionEngine
    {
        public static void ShowConsoleWindow(IWin32Window owner, string outputText)
        {
            var consoleForm = new Form
            {
                Text = "شاشة موجه الأوامر - تنفيذ البرنامج",
                Size = new Size(540, 560),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.Black,
                RightToLeft = RightToLeft.No,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var txtConsole = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 10.5F),
                ScrollBars = ScrollBars.Vertical,
                Text = outputText
            };

            var pnlBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 45,
                BackColor = Color.FromArgb(30, 30, 30)
            };

            var btnOk = new Button
            {
                Text = "موافق",
                Dock = DockStyle.Right,
                Width = 100,
                BackColor = Color.LightGray,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.Click += (s, ev) => consoleForm.Close();

            pnlBottom.Controls.Add(btnOk);
            consoleForm.Controls.Add(txtConsole);
            consoleForm.Controls.Add(pnlBottom);
            consoleForm.AcceptButton = btnOk;

            consoleForm.ShowDialog(owner);
        }

        public static string ExecuteManaged(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return "تم تشغيل البرنامج بنجاح. لا توجد تعليمات برمجية للتنفيذ.";
            }

            var outputLines = new List<string>();
            var variables = new Dictionary<string, string>();
            var procedures = new Dictionary<string, ProcedureDef>();

            // 1. استخراج الثوابت
            ExtractConstants(source, variables);

            // 2. استخراج الإجراءات
            ExtractProcedures(source, procedures);

            // 3. استخراج الكتلة الرئيسية
            string mainCode = ExtractMainBlock(source);

            // 4. تنفيذ التعليمات
            ExecuteStatements(mainCode, variables, procedures, outputLines);

            if (outputLines.Count == 0)
            {
                if (variables.Count > 0)
                {
                    outputLines.Add("===== نتائج قيم المتغيرات المحسوبة =====");
                    foreach (var kvp in variables)
                    {
                        outputLines.Add($"{kvp.Key} = {kvp.Value}");
                    }
                    outputLines.Add("\r\nتم تنفيذ البرنامج بنجاح.");
                }
                else
                {
                    outputLines.Add("تم تنفيذ البرنامج بنجاح بدون أخطاء.");
                }
            }

            return string.Join(Environment.NewLine, outputLines);
        }

        private class ProcedureDef
        {
            public string Name { get; set; } = string.Empty;
            public List<ParamDef> Parameters { get; set; } = new List<ParamDef>();
            public string Body { get; set; } = string.Empty;
        }

        private class ParamDef
        {
            public string Name { get; set; } = string.Empty;
            public bool IsByRef { get; set; }
        }

        private static void ExtractConstants(string source, Dictionary<string, string> vars)
        {
            int cIdx = source.IndexOf("ثابت");
            if (cIdx < 0) return;

            int endIdx = FindNextSection(source, cIdx + 4);
            string sec = (endIdx > cIdx) ? source.Substring(cIdx + 4, endIdx - cIdx - 4) : source.Substring(cIdx + 4);

            var stmts = sec.Split(new[] { '؛', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in stmts)
            {
                if (!s.Contains("=")) continue;
                var parts = s.Split(new[] { '=' }, 2);
                string name = parts[0].Trim();
                string val = parts[1].Trim();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    vars[name] = val;
                }
            }
        }

        private static void ExtractProcedures(string source, Dictionary<string, ProcedureDef> procs)
        {
            var matches = Regex.Matches(source, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\((.*?)\)\s*\{([\s\S]*?)\}");
            foreach (Match m in matches)
            {
                var def = new ProcedureDef
                {
                    Name = m.Groups[2].Value,
                    Body = m.Groups[4].Value
                };

                string paramStr = m.Groups[3].Value;
                if (!string.IsNullOrWhiteSpace(paramStr))
                {
                    foreach (var p in paramStr.Split(new[] { ',', '،' }))
                    {
                        var pm = Regex.Match(p.Trim(), @"(بالمرجع|بالقيمة)?\s*([\u0600-\u06FF\w]+)");
                        if (pm.Success)
                        {
                            def.Parameters.Add(new ParamDef
                            {
                                Name = pm.Groups[2].Value,
                                IsByRef = pm.Groups[1].Value == "بالمرجع"
                            });
                        }
                    }
                }

                procs[def.Name] = def;
            }
        }

        private static string ExtractMainBlock(string source)
        {
            // تنظيف التعليقات
            string clean = Regex.Replace(source, @"/\*[\s\S]*?\*/|//.*$", "", RegexOptions.Multiline);

            int lastDot = clean.LastIndexOf('.');
            int targetIdx = lastDot >= 0 ? lastDot : clean.Length - 1;
            int bClose = clean.LastIndexOf('}', targetIdx);
            if (bClose >= 0)
            {
                int depth = 1;
                for (int i = bClose - 1; i >= 0; i--)
                {
                    if (clean[i] == '}') depth++;
                    else if (clean[i] == '{')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            return clean.Substring(i + 1, bClose - i - 1);
                        }
                    }
                }
            }

            int first = clean.IndexOf('{');
            int last = clean.LastIndexOf('}');
            if (first >= 0 && last > first)
            {
                return clean.Substring(first + 1, last - first - 1);
            }

            return clean;
        }

        private static void ExecuteStatements(string code, Dictionary<string, string> vars, Dictionary<string, ProcedureDef> procs, List<string> output)
        {
            var stmts = SplitExecutionStatements(code);

            for (int i = 0; i < stmts.Count; i++)
            {
                string s = stmts[i].Trim();
                if (string.IsNullOrWhiteSpace(s) || s.StartsWith("//")) continue;

                // 1. طباعة
                if (s.StartsWith("اطبع") || s.StartsWith("print"))
                {
                    int open = s.IndexOf('(');
                    int close = s.LastIndexOf(')');
                    if (open >= 0 && close > open)
                    {
                        string args = s.Substring(open + 1, close - open - 1);
                        var parts = SplitArgs(args);
                        foreach (var p in parts)
                        {
                            string cleanP = p.Trim();
                            if ((cleanP.StartsWith("\"") && cleanP.EndsWith("\"")) ||
                                (cleanP.StartsWith("“") && cleanP.EndsWith("”")) ||
                                (cleanP.StartsWith("'") && cleanP.EndsWith("'")))
                            {
                                output.Add(cleanP.Trim('"', '“', '”', '\''));
                            }
                            else
                            {
                                string val = EvaluateExpression(cleanP, vars);
                                output.Add(val);
                            }
                        }
                    }
                }
                // 2. شرط إذا ... فإن
                else if (s.StartsWith("إذا") || s.StartsWith("if"))
                {
                    int pOpen = s.IndexOf('(');
                    int pClose = s.IndexOf(')', pOpen >= 0 ? pOpen : 0);
                    string cond = (pOpen >= 0 && pClose > pOpen) ? s.Substring(pOpen + 1, pClose - pOpen - 1) : "صح";

                    bool isTrue = EvaluateCondition(cond, vars);

                    int bOpen = s.IndexOf('{');
                    int bClose = s.IndexOf('}', bOpen >= 0 ? bOpen : 0);
                    if (bOpen >= 0 && bClose > bOpen)
                    {
                        string trueBlock = s.Substring(bOpen + 1, bClose - bOpen - 1);
                        if (isTrue)
                        {
                            ExecuteStatements(trueBlock, vars, procs, output);
                        }
                        else
                        {
                            int elseIdx = s.IndexOf("وإلا", bClose);
                            if (elseIdx >= 0)
                            {
                                int elseBOpen = s.IndexOf('{', elseIdx);
                                int elseBClose = s.LastIndexOf('}');
                                if (elseBOpen >= 0 && elseBClose > elseBOpen)
                                {
                                    string falseBlock = s.Substring(elseBOpen + 1, elseBClose - elseBOpen - 1);
                                    ExecuteStatements(falseBlock, vars, procs, output);
                                }
                            }
                        }
                    }
                }
                // 3. حلقة لكل
                else if (s.StartsWith("لكل") || s.StartsWith("for"))
                {
                    var m = Regex.Match(s, @"(لكل|for)\s+([\u0600-\u06FF\w]+)\s*=\s*(.*?)\s+(إلى|to)\s+(.*?)(?:\s+(بقدر|step)\s+(.*?))?(?:\{|$)");
                    if (m.Success)
                    {
                        string loopVar = m.Groups[2].Value;
                        int from = int.TryParse(EvaluateExpression(m.Groups[3].Value, vars), out int f) ? f : 1;
                        int to = int.TryParse(EvaluateExpression(m.Groups[5].Value, vars), out int tVal) ? tVal : 1;
                        int step = m.Groups[7].Success && int.TryParse(EvaluateExpression(m.Groups[7].Value, vars), out int st) ? st : 1;

                        int bOpen = s.IndexOf('{');
                        int bClose = s.LastIndexOf('}');
                        if (bOpen >= 0 && bClose > bOpen)
                        {
                            string loopBody = s.Substring(bOpen + 1, bClose - bOpen - 1);
                            for (int cur = from; step > 0 ? cur <= to : cur >= to; cur += step)
                            {
                                vars[loopVar] = cur.ToString();
                                ExecuteStatements(loopBody, vars, procs, output);
                            }
                        }
                    }
                }
                // قراءة من المستخدم: اقرا(س)
                else if (Regex.IsMatch(s, @"^(اقرا|read)\s*\("))
                {
                    int open = s.IndexOf('(');
                    int close = s.LastIndexOf(')');
                    if (open >= 0 && close > open)
                    {
                        string varName = s.Substring(open + 1, close - open - 1).Trim();
                        vars[varName] = ReadValueFromUser(varName);
                    }
                }
                // حلقة طالما (شرط) استمر { ... }
                else if (Regex.IsMatch(s, @"^(طالما|while)\s*\("))
                {
                    int open = s.IndexOf('(');
                    int close = FindMatchingParen(s, open);
                    if (close > open)
                    {
                        string cond = s.Substring(open + 1, close - open - 1);
                        string rest = s.Substring(close + 1).Trim();
                        if (rest.StartsWith("استمر")) rest = rest.Substring(5).Trim();
                        string body = UnwrapBlock(rest);

                        int guard = 0;
                        while (EvaluateCondition(cond, vars) && guard++ < 100000)
                        {
                            ExecuteStatements(body, vars, procs, output);
                        }
                    }
                }
                // حلقة أعد { ... } حتى (شرط)
                else if (Regex.IsMatch(s, @"^(أعد|اعد)\s*\{"))
                {
                    int bOpen = s.IndexOf('{');
                    int bClose = FindMatchingBrace(s, bOpen);
                    if (bClose > bOpen)
                    {
                        string body = s.Substring(bOpen + 1, bClose - bOpen - 1);
                        string tail = s.Substring(bClose + 1).Trim();
                        if (tail.StartsWith("حتى")) tail = tail.Substring(3).Trim();

                        int pOpen = tail.IndexOf('(');
                        int pClose = pOpen >= 0 ? FindMatchingParen(tail, pOpen) : -1;
                        string cond = pClose > pOpen ? tail.Substring(pOpen + 1, pClose - pOpen - 1) : "صح";

                        int guard = 0;
                        do
                        {
                            ExecuteStatements(body, vars, procs, output);
                        }
                        while (!EvaluateCondition(cond, vars) && guard++ < 100000);
                    }
                }
                // 4. استدعاء إجراء
                else if (Regex.IsMatch(s, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)"))
                {
                    var m = Regex.Match(s, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)");
                    string pName = m.Groups[1].Value;
                    string pArgs = m.Groups[2].Value;

                    if (procs.TryGetValue(pName, out var pDef))
                    {
                        var argValues = SplitArgs(pArgs);
                        var localVars = new Dictionary<string, string>(vars);
                        var byRefMap = new List<(string paramName, string argVarName)>();

                        for (int k = 0; k < pDef.Parameters.Count && k < argValues.Count; k++)
                        {
                            string pParamName = pDef.Parameters[k].Name;
                            string argVal = argValues[k].Trim();

                            if (pDef.Parameters[k].IsByRef)
                            {
                                byRefMap.Add((pParamName, argVal));
                                localVars[pParamName] = vars.ContainsKey(argVal) ? vars[argVal] : argVal;
                            }
                            else
                            {
                                localVars[pParamName] = EvaluateExpression(argVal, vars);
                            }
                        }

                        ExecuteStatements(pDef.Body, localVars, procs, output);

                        // إعادة نسخ المتغيرات الممررة بالمرجع
                        foreach (var (pParamName, argVarName) in byRefMap)
                        {
                            if (localVars.TryGetValue(pParamName, out string? updatedVal))
                            {
                                vars[argVarName] = updatedVal;
                            }
                        }
                    }
                }
                // 5. تعليمة إسناد
                else if (s.Contains("="))
                {
                    var parts = s.TrimEnd('؛', ';').Split(new[] { '=' }, 2);
                    string lhs = parts[0].Trim();
                    string rhs = parts[1].Trim();

                    string evaluated = EvaluateExpression(rhs, vars);
                    vars[lhs] = evaluated;
                }
            }
        }

        private static string EvaluateExpression(string expr, Dictionary<string, string> vars)
        {
            expr = expr.Trim();
            if (string.IsNullOrWhiteSpace(expr)) return "0";

            if (expr == "نعم" || expr == "صح") return "1";
            if (expr == "لا" || expr == "خطأ") return "0";

            // فحص إذا كان المتغير موجوداً مباشرة (مثل س أو ف[0] أو ش.عمر)
            if (vars.TryGetValue(expr, out string? directVal))
            {
                return directVal;
            }

            // استبدال المتغيرات المعروفة
            // نبدأ بالأطول تجنباً للاستبدال الجزئي
            foreach (var kvp in vars)
            {
                if (expr.Contains(kvp.Key))
                {
                    expr = Regex.Replace(expr, $@"(?<![\u0600-\u06FF\w\.\[]){Regex.Escape(kvp.Key)}(?![\u0600-\u06FF\w\.\]])", kvp.Value);
                }
            }

            // حساب التعبيرات الحسابية
            try
            {
                expr = expr.Replace("×", "*").Replace("÷", "/");

                // الأس ^
                while (expr.Contains("^"))
                {
                    var m = Regex.Match(expr, @"(-?\d+(?:\.\d+)?)\s*\^\s*(-?\d+(?:\.\d+)?)");
                    if (!m.Success) break;
                    double a = double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                    double b = double.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
                    double res = Math.Pow(a, b);
                    expr = expr.Substring(0, m.Index) + FormatNum(res) + expr.Substring(m.Index + m.Length);
                }

                // الضرب والقسمة وباقي القسمة
                while (Regex.IsMatch(expr, @"[\*\/\\%]"))
                {
                    var m = Regex.Match(expr, @"(-?\d+(?:\.\d+)?)\s*([\*\/\\%])\s*(-?\d+(?:\.\d+)?)");
                    if (!m.Success) break;
                    double a = double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                    string op = m.Groups[2].Value;
                    double b = double.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture);
                    double res = op switch
                    {
                        "*" => a * b,
                        "/" => b != 0 ? a / b : 0,
                        "\\" => b != 0 ? (int)(a / b) : 0,
                        "%" => b != 0 ? a % b : 0,
                        _ => a
                    };
                    expr = expr.Substring(0, m.Index) + FormatNum(res) + expr.Substring(m.Index + m.Length);
                }

                // الجمع والطرح
                while (Regex.IsMatch(expr, @"(?<=\d)\s*[\+\-]\s*(?=\d)"))
                {
                    var m = Regex.Match(expr, @"(-?\d+(?:\.\d+)?)\s*([\+\-])\s*(-?\d+(?:\.\d+)?)");
                    if (!m.Success) break;
                    double a = double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                    string op = m.Groups[2].Value;
                    double b = double.Parse(m.Groups[3].Value, CultureInfo.InvariantCulture);
                    double res = op == "+" ? a + b : a - b;
                    expr = expr.Substring(0, m.Index) + FormatNum(res) + expr.Substring(m.Index + m.Length);
                }

                if (double.TryParse(expr.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double finalVal))
                {
                    return FormatNum(finalVal);
                }
            }
            catch { }

            return expr.Trim();
        }

        private static bool EvaluateCondition(string cond, Dictionary<string, string> vars)
        {
            cond = cond.Replace("&&", " و ").Replace("||", " أو ");
            if (cond.Contains(" و "))
            {
                var parts = cond.Split(new[] { " و " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    if (!EvaluateSimpleCondition(p.Trim(), vars)) return false;
                }
                return true;
            }
            else if (cond.Contains(" أو "))
            {
                var parts = cond.Split(new[] { " أو " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    if (EvaluateSimpleCondition(p.Trim(), vars)) return true;
                }
                return false;
            }

            return EvaluateSimpleCondition(cond, vars);
        }

        private static bool EvaluateSimpleCondition(string cond, Dictionary<string, string> vars)
        {
            cond = cond.Trim('(', ')', ' ');
            string[] ops = new[] { ">=", "<=", "==", "!=", ">", "<" };
            foreach (var op in ops)
            {
                if (cond.Contains(op))
                {
                    var parts = cond.Split(new[] { op }, 2, StringSplitOptions.None);
                    double left = double.TryParse(EvaluateExpression(parts[0], vars), NumberStyles.Any, CultureInfo.InvariantCulture, out double l) ? l : 0;
                    double right = double.TryParse(EvaluateExpression(parts[1], vars), NumberStyles.Any, CultureInfo.InvariantCulture, out double r) ? r : 0;

                    return op switch
                    {
                        ">=" => left >= right,
                        "<=" => left <= right,
                        "==" => Math.Abs(left - right) < 1e-9,
                        "!=" => Math.Abs(left - right) > 1e-9,
                        ">" => left > right,
                        "<" => left < right,
                        _ => false
                    };
                }
            }

            string val = EvaluateExpression(cond, vars);
            return val != "0" && val != "لا" && val != "خطأ" && !string.IsNullOrWhiteSpace(val);
        }

        // ===== الإدخال من المستخدم =====

        private static string ReadValueFromUser(string varName)
        {
            string message = $"أدخل قيمة المتغير ({varName}):";
            while (true)
            {
                string? input = ShowInputDialog(message);
                if (input == null) return "0"; // عند الإلغاء

                input = NormalizeDigits(input.Trim());
                if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double d))
                {
                    return FormatNum(d);
                }
                message = $"قيمة غير صحيحة، أدخل رقماً للمتغير ({varName}):";
            }
        }

        private static string NormalizeDigits(string text)
        {
            var sb = new StringBuilder();
            foreach (char c in text)
            {
                if (c >= '\u0660' && c <= '\u0669') sb.Append((char)('0' + (c - '\u0660')));
                else if (c == '\u066B') sb.Append('.');
                else sb.Append(c);
            }
            return sb.ToString();
        }

        private static string? ShowInputDialog(string message)
        {
            using var form = new Form
            {
                Text = "إدخال قيمة",
                Size = new Size(390, 190),
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                RightToLeft = RightToLeft.Yes,
                TopMost = true
            };

            var lbl = new Label { Text = message, Left = 15, Top = 15, Width = 350, Height = 25 };
            var txt = new TextBox
            {
                Left = 15,
                Top = 50,
                Width = 345,
                Font = new Font("Consolas", 11F),
                RightToLeft = RightToLeft.No
            };
            var ok = new Button { Text = "موافق", DialogResult = DialogResult.OK, Left = 15, Top = 95, Width = 90 };
            var cancel = new Button { Text = "إلغاء", DialogResult = DialogResult.Cancel, Left = 115, Top = 95, Width = 90 };

            form.Controls.Add(lbl);
            form.Controls.Add(txt);
            form.Controls.Add(ok);
            form.Controls.Add(cancel);
            form.AcceptButton = ok;
            form.CancelButton = cancel;
            form.ActiveControl = txt;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }

        // ===== دوال مساعدة للحلقات =====

        private static int FindMatchingParen(string text, int openIdx)
        {
            if (openIdx < 0 || openIdx >= text.Length) return -1;
            int depth = 0;
            for (int i = openIdx; i < text.Length; i++)
            {
                if (text[i] == '(') depth++;
                else if (text[i] == ')')
                {
                    depth--;
                    if (depth == 0) return i;
                }
            }
            return -1;
        }

        private static int FindMatchingBrace(string text, int openIdx)
        {
            if (openIdx < 0 || openIdx >= text.Length) return -1;
            int depth = 0;
            for (int i = openIdx; i < text.Length; i++)
            {
                if (text[i] == '{') depth++;
                else if (text[i] == '}')
                {
                    depth--;
                    if (depth == 0) return i;
                }
            }
            return -1;
        }

        private static string UnwrapBlock(string text)
        {
            text = text.Trim();
            if (text.StartsWith("{"))
            {
                int close = FindMatchingBrace(text, 0);
                if (close > 0) return text.Substring(1, close - 1);
            }
            return text;
        }

        private static string FormatNum(double num)
        {
            if (num % 1 == 0) return ((long)num).ToString();
            return num.ToString("0.##", CultureInfo.InvariantCulture);
        }

        private static int FindNextSection(string text, int startIndex)
        {
            string[] sections = new[] { "نوع", "متغير", "إجراء", "دالة", "{" };
            int minIdx = -1;
            foreach (var s in sections)
            {
                int idx = text.IndexOf(s, startIndex);
                if (idx >= 0 && (minIdx == -1 || idx < minIdx))
                {
                    minIdx = idx;
                }
            }
            return minIdx;
        }

        private static List<string> SplitExecutionStatements(string code)
        {
            var list = new List<string>();
            int start = 0;
            int braceDepth = 0;
            int parenDepth = 0;

            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                if (c == '{') braceDepth++;
                else if (c == '}') braceDepth--;
                else if (c == '(') parenDepth++;
                else if (c == ')') parenDepth--;
                else if ((c == '؛' || c == ';') && braceDepth == 0 && parenDepth == 0)
                {
                    string s = code.Substring(start, i - start).Trim();
                    if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
                    start = i + 1;
                }
                else if (c == '}' && braceDepth == 0 && parenDepth == 0)
                {
                    string s = code.Substring(start, i - start + 1).Trim();
                    if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
                    start = i + 1;
                }
            }

            if (start < code.Length)
            {
                string s = code.Substring(start).Trim().TrimEnd('.');
                if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
            }

            return list;
        }

        private static List<string> SplitArgs(string argsText)
        {
            var list = new List<string>();
            bool inQuotes = false;
            char quoteChar = '"';
            int start = 0;

            for (int i = 0; i < argsText.Length; i++)
            {
                char c = argsText[i];
                if ((c == '"' || c == '“' || c == '”' || c == '\'') && !inQuotes)
                {
                    inQuotes = true;
                    quoteChar = c;
                }
                else if (inQuotes && (c == quoteChar || c == '"' || c == '”'))
                {
                    inQuotes = false;
                }
                else if ((c == ',' || c == '،') && !inQuotes)
                {
                    string a = argsText.Substring(start, i - start).Trim();
                    if (!string.IsNullOrWhiteSpace(a)) list.Add(a);
                    start = i + 1;
                }
            }

            if (start < argsText.Length)
            {
                string a = argsText.Substring(start).Trim();
                if (!string.IsNullOrWhiteSpace(a)) list.Add(a);
            }

            return list;
        }
    }
}
