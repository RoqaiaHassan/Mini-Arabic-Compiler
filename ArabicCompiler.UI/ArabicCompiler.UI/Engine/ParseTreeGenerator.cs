using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ArabicCompiler.UI.Engine
{
    public class ParseTreeNode
    {
        public string Text { get; set; } = string.Empty;
        public List<ParseTreeNode> Children { get; } = new List<ParseTreeNode>();

        public ParseTreeNode(string text)
        {
            Text = text;
        }

        public ParseTreeNode AddChild(string text)
        {
            var node = new ParseTreeNode(text);
            Children.Add(node);
            return node;
        }

        public void AddChild(ParseTreeNode node)
        {
            Children.Add(node);
        }
    }

    public static class ParseTreeGenerator
    {
        public static string BuildTree(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return "الشجرة الإعرابية:\n└── لا يوجد كود برمجي لبناء الشجرة الإعرابية.";
            }

            var root = new ParseTreeNode("البرنامج");

            // 1. استخراج اسم البرنامج
            string progName = "برنامج_بدون_اسم";
            var progMatch = Regex.Match(source, @"برنامج\s+([\u0600-\u06FF\w]+)");
            if (progMatch.Success)
            {
                progName = progMatch.Groups[1].Value.TrimEnd('؛', ';', ' ', '\t', '\r', '\n');
            }

            root.AddChild("كلمة محجوزة: برنامج");
            var progNameNode = root.AddChild("اسم البرنامج");
            progNameNode.AddChild($"معرّف : {progName}");
            root.AddChild("فاصلة منقوطة : ؛");

            // 2. الكتلة البرمجية
            var blockNode = root.AddChild("الكتلة البرمجية");

            // عزل جزء التعريفات والكتلة الرئيسية
            SplitProgramParts(source, out string declPart, out string mainBody);

            // جزء التعريفات (إذا وجد)
            var declNode = new ParseTreeNode("جزء التعريفات");
            bool hasDeclarations = false;

            // أ) فحص الثوابت
            var constNodes = ParseConstants(declPart);
            if (constNodes != null && constNodes.Children.Count > 1)
            {
                declNode.AddChild(constNodes);
                hasDeclarations = true;
            }

            // ب) فحص الأنواع
            var typeNodes = ParseTypes(declPart);
            if (typeNodes != null && typeNodes.Children.Count > 1)
            {
                declNode.AddChild(typeNodes);
                hasDeclarations = true;
            }

            // ج) فحص المتغيرات
            var varNodes = ParseVariables(declPart);
            if (varNodes != null && varNodes.Children.Count > 1)
            {
                declNode.AddChild(varNodes);
                hasDeclarations = true;
            }

            // د) فحص الإجراءات والدوال
            var procNodes = ParseProcedures(declPart);
            if (procNodes != null && procNodes.Children.Count > 0)
            {
                var procContainer = declNode.AddChild("تعريف الإجراءات والدوال");
                foreach (var p in procNodes.Children)
                {
                    procContainer.AddChild(p);
                }
                hasDeclarations = true;
            }

            if (hasDeclarations)
            {
                blockNode.AddChild(declNode);
            }

            // 3. قائمة التعليمات
            var instrListNode = blockNode.AddChild("قائمة التعليمات");
            ParseInstructionList(mainBody, instrListNode);

            // 4. نقطة النهاية
            root.AddChild("نقطة النهاية : .");

            // تحويل الشجرة إلى نص
            var sb = new StringBuilder();
            sb.AppendLine("الشجرة الإعرابية:");
            RenderTree(root, sb, "", true, true);
            return sb.ToString();
        }

        private static void SplitProgramParts(string source, out string declPart, out string mainBody)
        {
            // تنظيف التعليقات
            string clean = Regex.Replace(source, @"//.*$", "", RegexOptions.Multiline);

            int firstBrace = clean.IndexOf('{');
            int lastBrace = clean.LastIndexOf('}');

            // إذا كان هناك إجراءات، نبحث عن القوس الخاص بالكتلة الرئيسية
            // الكتلة الرئيسية عادة ما تكون القوس الأخير الذي ينتهي بنقطة أو آخر كتلة
            if (firstBrace >= 0 && lastBrace > firstBrace)
            {
                // فحص إذا كان هناك إجراء قبل الكتلة الرئيسية
                int mainStart = FindMainBlockStart(clean);
                if (mainStart >= 0)
                {
                    declPart = clean.Substring(0, mainStart);
                    int endPos = clean.LastIndexOf('}');
                    if (endPos > mainStart)
                    {
                        mainBody = clean.Substring(mainStart + 1, endPos - mainStart - 1);
                    }
                    else
                    {
                        mainBody = clean.Substring(mainStart + 1);
                    }
                    return;
                }

                declPart = clean.Substring(0, firstBrace);
                mainBody = clean.Substring(firstBrace + 1, lastBrace - firstBrace - 1);
            }
            else
            {
                declPart = clean;
                mainBody = string.Empty;
            }
        }

        private static int FindMainBlockStart(string source)
        {
            // الكتلة الرئيسية هي الكتلة التي تسبق النقطة الأخيرة . أو الكتلة التي ليست تابعة لإجراء
            int lastDot = source.LastIndexOf('.');
            if (lastDot >= 0)
            {
                // نبحث عن آخر قوس { قبل هذه النقطة
                int brace = source.LastIndexOf('{', lastDot);
                if (brace >= 0)
                {
                    // نتأكد أنه ليس بداية إجراء
                    string prefix = source.Substring(0, brace).TrimEnd();
                    if (!prefix.EndsWith(")") && !Regex.IsMatch(prefix, @"(إجراء|دالة)\s+[\u0600-\u06FF\w]+\s*\([^\)]*\)\s*$"))
                    {
                        return brace;
                    }
                }
            }

            // فحص كتل الإجراءات
            int idx = 0;
            int depth = 0;
            int lastTopLevelBrace = -1;

            while (idx < source.Length)
            {
                if (source[idx] == '{')
                {
                    if (depth == 0)
                    {
                        string before = source.Substring(0, idx).TrimEnd();
                        // إذا لم يكن إجراءً
                        if (!Regex.IsMatch(before, @"(إجراء|دالة)\s+[\u0600-\u06FF\w]+\s*\([^\)]*\)\s*$"))
                        {
                            lastTopLevelBrace = idx;
                        }
                    }
                    depth++;
                }
                else if (source[idx] == '}')
                {
                    depth--;
                }
                idx++;
            }

            return lastTopLevelBrace >= 0 ? lastTopLevelBrace : source.LastIndexOf('{');
        }

        private static ParseTreeNode? ParseConstants(string declPart)
        {
            int constIdx = declPart.IndexOf("ثابت");
            if (constIdx < 0) return null;

            // تحديد نهاية قسم الثوابت
            int endIdx = FindNextSectionIndex(declPart, constIdx + 4);
            string constSection = (endIdx > constIdx) ? declPart.Substring(constIdx + 4, endIdx - constIdx - 4) : declPart.Substring(constIdx + 4);

            var node = new ParseTreeNode("تعريف الثوابت");
            node.AddChild("كلمة محجوزة: ثابت");

            var statements = SplitStatements(constSection);
            foreach (var stmt in statements)
            {
                if (!stmt.Contains("=")) continue;
                var parts = stmt.Split(new[] { '=' }, 2);
                string name = parts[0].Trim();
                string val = parts[1].Trim().TrimEnd('؛', ';');

                if (string.IsNullOrWhiteSpace(name)) continue;

                var cNode = node.AddChild("تعريف ثابت");
                cNode.AddChild($"اسم الثابت : {name}");
                cNode.AddChild("عامل الإسناد : =");
                var valNode = cNode.AddChild("قيمة ثابتة");

                if (double.TryParse(val, out _))
                {
                    valNode.AddChild(val.Contains(".") ? $"قيمة حقيقية : {val}" : $"قيمة عددية : {val}");
                }
                else if (val.StartsWith("\"") || val.StartsWith("“") || val.StartsWith("'"))
                {
                    valNode.AddChild($"خيط رمزي : {val}");
                }
                else if (val == "نعم" || val == "لا" || val == "صح" || val == "خطأ")
                {
                    valNode.AddChild($"قيمة منطقية : {val}");
                }
                else
                {
                    valNode.AddChild($"قيمة معرّفة : {val}");
                }

                cNode.AddChild("فاصلة منقوطة : ؛");
            }

            return node;
        }

        private static ParseTreeNode? ParseTypes(string declPart)
        {
            int typeIdx = declPart.IndexOf("نوع");
            if (typeIdx < 0) return null;

            int endIdx = FindNextSectionIndex(declPart, typeIdx + 3);
            string typeSection = (endIdx > typeIdx) ? declPart.Substring(typeIdx + 3, endIdx - typeIdx - 3) : declPart.Substring(typeIdx + 3);

            var node = new ParseTreeNode("تعريف الأنواع");
            node.AddChild("كلمة محجوزة: نوع");

            var statements = SplitStatementsWithBraces(typeSection);
            foreach (var stmt in statements)
            {
                if (!stmt.Contains("=")) continue;
                var parts = stmt.Split(new[] { '=' }, 2);
                string name = parts[0].Trim();
                string def = parts[1].Trim().TrimEnd('؛', ';');

                if (string.IsNullOrWhiteSpace(name)) continue;

                var tNode = node.AddChild("تعريف نوع");
                tNode.AddChild($"اسم النوع : {name}");
                tNode.AddChild("عامل الإسناد : =");

                if (def.Contains("قائمة"))
                {
                    tNode.AddChild($"نوع قائمة : {def}");
                }
                else if (def.Contains("سجل"))
                {
                    var recNode = tNode.AddChild("نوع مركب : سجل");
                    recNode.AddChild("قوس كتلة مفتوح : {");
                    var fieldsContainer = recNode.AddChild("قائمة الحقول");

                    // استخراج الحقول بين { و }
                    int bOpen = def.IndexOf('{');
                    int bClose = def.LastIndexOf('}');
                    if (bOpen >= 0 && bClose > bOpen)
                    {
                        string inside = def.Substring(bOpen + 1, bClose - bOpen - 1);
                        var fieldStmts = SplitStatements(inside);
                        foreach (var f in fieldStmts)
                        {
                            if (f.Contains(":"))
                            {
                                fieldsContainer.AddChild($"حقل : {f.Trim()}؛");
                            }
                        }
                    }
                    recNode.AddChild("قوس كتلة مغلق : }");
                }
                else
                {
                    tNode.AddChild($"نوع : {def}");
                }

                tNode.AddChild("فاصلة منقوطة : ؛");
            }

            return node;
        }

        private static ParseTreeNode? ParseVariables(string declPart)
        {
            int varIdx = declPart.IndexOf("متغير");
            if (varIdx < 0) return null;

            int endIdx = FindNextSectionIndex(declPart, varIdx + 5);
            string varSection = (endIdx > varIdx) ? declPart.Substring(varIdx + 5, endIdx - varIdx - 5) : declPart.Substring(varIdx + 5);

            var node = new ParseTreeNode("تعريف المتغيرات");
            node.AddChild("كلمة محجوزة: متغير");

            var statements = SplitStatements(varSection);
            foreach (var stmt in statements)
            {
                if (!stmt.Contains(":")) continue;
                var parts = stmt.Split(new[] { ':' }, 2);
                string names = parts[0].Trim();
                string type = parts[1].Trim().TrimEnd('؛', ';');

                if (string.IsNullOrWhiteSpace(names)) continue;

                var vGroup = node.AddChild("تصريح متغيرات");
                vGroup.AddChild($"أسماء المتغيرات : {names}");
                vGroup.AddChild("نقطتان : :");
                vGroup.AddChild($"نوع البيانات : {type}");
                vGroup.AddChild("فاصلة منقوطة : ؛");
            }

            return node;
        }

        private static ParseTreeNode? ParseProcedures(string declPart)
        {
            var container = new ParseTreeNode("الإجراءات");
            var procMatches = Regex.Matches(declPart, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\((.*?)\)\s*\{([\s\S]*?)\}");

            foreach (Match m in procMatches)
            {
                string kind = m.Groups[1].Value;
                string pName = m.Groups[2].Value;
                string pParams = m.Groups[3].Value;
                string pBody = m.Groups[4].Value;

                var pNode = container.AddChild($"تعريف {kind} : {pName}");
                var headerNode = pNode.AddChild($"رأس {kind}");
                headerNode.AddChild($"كلمة محجوزة: {kind}");
                headerNode.AddChild($"اسم {kind} : {pName}");
                headerNode.AddChild("قوس مفتوح : (");

                var paramsNode = headerNode.AddChild("قائمة المعلمات الشكلية");
                if (!string.IsNullOrWhiteSpace(pParams))
                {
                    var paramItems = pParams.Split(new[] { ',', '،' });
                    foreach (var p in paramItems)
                    {
                        paramsNode.AddChild($"معلمة : {p.Trim()}");
                    }
                }
                else
                {
                    paramsNode.AddChild("بدون معلمات");
                }
                headerNode.AddChild("قوس مغلق : )");

                var bodyNode = pNode.AddChild($"كتلة {kind}");
                var bodyInstrList = bodyNode.AddChild($"قائمة تعليمات {kind}");
                ParseInstructionList(pBody, bodyInstrList);
            }

            return container;
        }

        private static void ParseInstructionList(string bodyText, ParseTreeNode parentNode)
        {
            if (string.IsNullOrWhiteSpace(bodyText)) return;

            var statements = SplitInstructionStatements(bodyText);
            foreach (var stmt in statements)
            {
                string t = stmt.Trim();
                if (string.IsNullOrWhiteSpace(t)) continue;

                // 1. تعليمة شرطية إذا ... فإن
                if (t.StartsWith("إذا") || t.StartsWith("لو") || t.StartsWith("if"))
                {
                    ParseConditionalInstruction(t, parentNode);
                }
                // 2. حلقة تكرار لكل ... إلى ... بقدر
                else if (t.StartsWith("لكل") || t.StartsWith("كرر") || t.StartsWith("for"))
                {
                    ParseForLoopInstruction(t, parentNode);
                }
                // 3. حلقة تكرار طالما ... افعل
                else if (t.StartsWith("طالما") || t.StartsWith("بينما") || t.StartsWith("while"))
                {
                    ParseWhileLoopInstruction(t, parentNode);
                }
                // 4. تعليمة طباعة
                else if (t.StartsWith("اطبع") || t.StartsWith("اكتب") || t.StartsWith("print"))
                {
                    ParsePrintInstruction(t, parentNode);
                }
                // 5. تعليمة قراءة
                else if (t.StartsWith("اقرأ") || t.StartsWith("ادخل") || t.StartsWith("read"))
                {
                    ParseReadInstruction(t, parentNode);
                }
                // 6. استدعاء إجراء
                else if (Regex.IsMatch(t, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)\s*[؛;]?$") && !t.Contains("="))
                {
                    ParseProcedureCallInstruction(t, parentNode);
                }
                // 7. تعليمة إسناد
                else if (t.Contains("="))
                {
                    ParseAssignmentInstruction(t, parentNode);
                }
                else
                {
                    parentNode.AddChild($"تعليمة تنفيذية : {t}");
                }
            }
        }

        private static void ParseAssignmentInstruction(string stmt, ParseTreeNode parent)
        {
            string clean = stmt.TrimEnd('؛', ';').Trim();
            var parts = clean.Split(new[] { '=' }, 2);
            string lhs = parts[0].Trim();
            string rhs = parts[1].Trim();

            var node = parent.AddChild("تعليمة إسناد وتعبير");
            node.AddChild($"الطرف الأيسر : {lhs}");
            node.AddChild("عامل الإسناد : =");

            var exprNode = node.AddChild($"تعبير حسابي / منطقي : {rhs}");
            DecomposeExpression(rhs, exprNode);

            node.AddChild("فاصلة منقوطة : ؛");
        }

        private static void ParsePrintInstruction(string stmt, ParseTreeNode parent)
        {
            var node = parent.AddChild("تعليمة طباعة : اطبع");
            node.AddChild("كلمة محجوزة: اطبع");
            node.AddChild("قوس مفتوح : (");

            int open = stmt.IndexOf('(');
            int close = stmt.LastIndexOf(')');
            if (open >= 0 && close > open)
            {
                string args = stmt.Substring(open + 1, close - open - 1);
                var argsList = SplitPrintArgs(args);
                var argsNode = node.AddChild("قائمة الوسائط للطباعة");
                foreach (var a in argsList)
                {
                    argsNode.AddChild($"وسيط : {a.Trim()}");
                }
            }
            node.AddChild("قوس مغلق : )");
            node.AddChild("فاصلة منقوطة : ؛");
        }

        private static void ParseReadInstruction(string stmt, ParseTreeNode parent)
        {
            var node = parent.AddChild("تعليمة قراءة : اقرأ");
            node.AddChild("كلمة محجوزة: اقرأ");
            node.AddChild("قوس مفتوح : (");

            int open = stmt.IndexOf('(');
            int close = stmt.LastIndexOf(')');
            string target = (open >= 0 && close > open) ? stmt.Substring(open + 1, close - open - 1).Trim() : stmt.Replace("اقرأ", "").Replace("read", "").Trim();
            node.AddChild($"المتغير المستهدف : {target}");
            node.AddChild("قوس مغلق : )");
            node.AddChild("فاصلة منقوطة : ؛");
        }

        private static void ParseConditionalInstruction(string stmt, ParseTreeNode parent)
        {
            var node = parent.AddChild("تعليمة شرطية : إذا ... فإن");
            node.AddChild("كلمة محجوزة: إذا");

            // استخراج الشرط بين ( و )
            int pOpen = stmt.IndexOf('(');
            int pClose = stmt.IndexOf(')', pOpen >= 0 ? pOpen : 0);
            string cond = (pOpen >= 0 && pClose > pOpen) ? stmt.Substring(pOpen, pClose - pOpen + 1) : "(شرط)";

            var condNode = node.AddChild($"شرط التحقق : {cond}");
            node.AddChild("كلمة محجوزة: فإن");

            // استخراج كتلة التحقق بين { و }
            int bOpen = stmt.IndexOf('{');
            int bClose = stmt.IndexOf('}', bOpen >= 0 ? bOpen : 0);
            if (bOpen >= 0 && bClose > bOpen)
            {
                string trueBody = stmt.Substring(bOpen + 1, bClose - bOpen - 1);
                var trueNode = node.AddChild("كتلة التحقق (صحيح)");
                ParseInstructionList(trueBody, trueNode);

                // فحص وإلا
                int elseIdx = stmt.IndexOf("وإلا", bClose);
                if (elseIdx < 0) elseIdx = stmt.IndexOf("else", bClose);
                if (elseIdx >= 0)
                {
                    node.AddChild("كلمة محجوزة: وإلا");
                    int elseBOpen = stmt.IndexOf('{', elseIdx);
                    int elseBClose = stmt.LastIndexOf('}');
                    if (elseBOpen >= 0 && elseBClose > elseBOpen)
                    {
                        string falseBody = stmt.Substring(elseBOpen + 1, elseBClose - elseBOpen - 1);
                        var falseNode = node.AddChild("كتلة البديل (خطأ)");
                        ParseInstructionList(falseBody, falseNode);
                    }
                }
            }
        }

        private static void ParseForLoopInstruction(string stmt, ParseTreeNode parent)
        {
            var node = parent.AddChild("حلقة تكرار : لكل ... إلى ... بقدر");
            node.AddChild("كلمة محجوزة: لكل");

            // لكل عداد = 1 إلى 3 بقدر 1
            var m = Regex.Match(stmt, @"(لكل|for)\s+([\u0600-\u06FF\w]+)\s*=\s*(.*?)\s+(إلى|to)\s+(.*?)(?:\s+(بقدر|step)\s+(.*?))?(?:\{|$)");
            if (m.Success)
            {
                string loopVar = m.Groups[2].Value;
                string startVal = m.Groups[3].Value;
                string endVal = m.Groups[5].Value;
                string stepVal = m.Groups[7].Success ? m.Groups[7].Value : "1";

                node.AddChild($"متغير العداد : {loopVar}");
                node.AddChild("عامل الإسناد : =");
                node.AddChild($"القيمة الابتدائية : {startVal}");
                node.AddChild("كلمة محجوزة: إلى");
                node.AddChild($"القيمة النهائية : {endVal}");
                node.AddChild($"الخطوة : بقدر {stepVal}");
            }

            int bOpen = stmt.IndexOf('{');
            int bClose = stmt.LastIndexOf('}');
            if (bOpen >= 0 && bClose > bOpen)
            {
                string body = stmt.Substring(bOpen + 1, bClose - bOpen - 1);
                var bodyNode = node.AddChild("جسم الحلقة");
                ParseInstructionList(body, bodyNode);
            }
        }

        private static void ParseWhileLoopInstruction(string stmt, ParseTreeNode parent)
        {
            var node = parent.AddChild("حلقة تكرار : طالما");
            node.AddChild("كلمة محجوزة: طالما");

            int pOpen = stmt.IndexOf('(');
            int pClose = stmt.IndexOf(')', pOpen >= 0 ? pOpen : 0);
            string cond = (pOpen >= 0 && pClose > pOpen) ? stmt.Substring(pOpen, pClose - pOpen + 1) : "(شرط)";
            node.AddChild($"شرط الحلقة : {cond}");

            int bOpen = stmt.IndexOf('{');
            int bClose = stmt.LastIndexOf('}');
            if (bOpen >= 0 && bClose > bOpen)
            {
                string body = stmt.Substring(bOpen + 1, bClose - bOpen - 1);
                var bodyNode = node.AddChild("جسم الحلقة");
                ParseInstructionList(body, bodyNode);
            }
        }

        private static void ParseProcedureCallInstruction(string stmt, ParseTreeNode parent)
        {
            var m = Regex.Match(stmt, @"^([\u0600-\u06FF\w]+)\s*\((.*?)\)");
            if (m.Success)
            {
                string pName = m.Groups[1].Value;
                string pArgs = m.Groups[2].Value;

                var node = parent.AddChild($"استدعاء إجراء : {pName}");
                node.AddChild($"اسم الإجراء : {pName}");
                node.AddChild("قوس مفتوح : (");
                var argsNode = node.AddChild("الوسائط الفعلية");
                if (!string.IsNullOrWhiteSpace(pArgs))
                {
                    foreach (var a in pArgs.Split(new[] { ',', '،' }))
                    {
                        argsNode.AddChild($"وسيط : {a.Trim()}");
                    }
                }
                node.AddChild("قوس مغلق : )");
                node.AddChild("فاصلة منقوطة : ؛");
            }
        }

        private static void DecomposeExpression(string expr, ParseTreeNode parent)
        {
            // تحليل تعبير يحتوي على عمليات حسابية أو منطقية
            string[] ops = new[] { "+", "-", "*", "/", "\\", "%", "^", "==", "!=", ">=", "<=", ">", "<", " و ", " أو ", "&&", "||" };
            foreach (var op in ops)
            {
                if (expr.Contains(op))
                {
                    int opIdx = expr.IndexOf(op);
                    string left = expr.Substring(0, opIdx).Trim();
                    string right = expr.Substring(opIdx + op.Length).Trim();
                    if (!string.IsNullOrWhiteSpace(left) && !string.IsNullOrWhiteSpace(right))
                    {
                        parent.AddChild($"الطرف الأيسر : {left}");
                        parent.AddChild($"العامل : {op.Trim()}");
                        parent.AddChild($"الطرف الأيمن : {right}");
                        return;
                    }
                }
            }

            // معامل بسيط
            parent.AddChild($"قيمة / معرّف : {expr}");
        }

        private static int FindNextSectionIndex(string text, int startIndex)
        {
            string[] sections = new[] { "ثابت", "نوع", "متغير", "إجراء", "دالة", "{" };
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

        private static List<string> SplitStatements(string text)
        {
            var list = new List<string>();
            var raw = text.Split(new[] { '؛', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in raw)
            {
                string t = r.Trim();
                if (!string.IsNullOrWhiteSpace(t)) list.Add(t);
            }
            return list;
        }

        private static List<string> SplitStatementsWithBraces(string text)
        {
            var list = new List<string>();
            int start = 0;
            int depth = 0;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '{') depth++;
                else if (text[i] == '}') depth--;
                else if ((text[i] == '؛' || text[i] == ';') && depth == 0)
                {
                    string stmt = text.Substring(start, i - start).Trim();
                    if (!string.IsNullOrWhiteSpace(stmt)) list.Add(stmt);
                    start = i + 1;
                }
            }

            if (start < text.Length)
            {
                string remaining = text.Substring(start).Trim();
                if (!string.IsNullOrWhiteSpace(remaining)) list.Add(remaining);
            }

            return list;
        }

        private static List<string> SplitInstructionStatements(string body)
        {
            var list = new List<string>();
            int start = 0;
            int braceDepth = 0;
            int parenDepth = 0;

            for (int i = 0; i < body.Length; i++)
            {
                char c = body[i];
                if (c == '{') braceDepth++;
                else if (c == '}') braceDepth--;
                else if (c == '(') parenDepth++;
                else if (c == ')') parenDepth--;
                else if ((c == '؛' || c == ';') && braceDepth == 0 && parenDepth == 0)
                {
                    string s = body.Substring(start, i - start).Trim();
                    if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
                    start = i + 1;
                }
                else if (c == '}' && braceDepth == 0 && parenDepth == 0)
                {
                    // نهاية كتلة شرطية أو حلقة
                    string s = body.Substring(start, i - start + 1).Trim();
                    if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
                    start = i + 1;
                }
            }

            if (start < body.Length)
            {
                string s = body.Substring(start).Trim().TrimEnd('.');
                if (!string.IsNullOrWhiteSpace(s)) list.Add(s);
            }

            return list;
        }

        private static List<string> SplitPrintArgs(string argsText)
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

        public static void RenderTree(ParseTreeNode node, StringBuilder sb, string indent = "", bool isLast = true, bool isRoot = true)
        {
            if (isRoot)
            {
                sb.AppendLine("└── " + node.Text);
                for (int i = 0; i < node.Children.Count; i++)
                {
                    RenderTree(node.Children[i], sb, "    ", i == node.Children.Count - 1, false);
                }
            }
            else
            {
                sb.Append(indent);
                sb.Append(isLast ? "└── " : "├── ");
                sb.AppendLine(node.Text);

                string childIndent = indent + (isLast ? "    " : "│   ");
                for (int i = 0; i < node.Children.Count; i++)
                {
                    RenderTree(node.Children[i], sb, childIndent, i == node.Children.Count - 1, false);
                }
            }
        }
    }
}
