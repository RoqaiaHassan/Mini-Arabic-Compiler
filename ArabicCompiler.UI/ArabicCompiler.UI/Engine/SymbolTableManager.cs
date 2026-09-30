using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ArabicCompiler.UI.Engine
{
    public class SymbolItem
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string Line { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;

        public SymbolItem() { }

        public SymbolItem(string name, string category, string type, string value, string scope, string line, string hash)
        {
            Name = name;
            Category = category;
            Type = type;
            Value = value;
            Scope = scope;
            Line = line;
            Hash = hash;
        }
    }

    public static class SymbolTableManager
    {
        public static List<SymbolItem> ExtractSymbolsFromCode(string source)
        {
            var symbols = new List<SymbolItem>();
            if (string.IsNullOrWhiteSpace(source))
                return symbols;

            var seenKeys = new HashSet<string>();
            var lines = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            string currentSection = "";
            string currentScope = "عام";
            string currentRecordName = "";
            int currentRecordLine = 0;
            int recordDepth = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                int lineNum = i + 1;
                string line = lines[i].Trim();

                // تجاهل التعليقات والأسطر الفارغة
                if (line.StartsWith("//") || string.IsNullOrWhiteSpace(line)) continue;

                // إزالة التعليقات المضمنة
                int commentIdx = line.IndexOf("//");
                if (commentIdx >= 0) line = line.Substring(0, commentIdx).Trim();

                // تحديد الأقسام الرئيسية
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

                // 1. فحص تعريف الإجراءات والدوال
                var procMatch = Regex.Match(line, @"(إجراء|دالة)\s+([\u0600-\u06FF\w]+)\s*\((.*?)\)");
                if (procMatch.Success)
                {
                    currentSection = "إجراء";
                    string kind = procMatch.Groups[1].Value;
                    string procName = procMatch.Groups[2].Value;
                    string key = $"{procName}_{currentScope}";
                    if (!seenKeys.Contains(key))
                    {
                        symbols.Add(new SymbolItem(procName, kind, "فراغ", "", currentScope, lineNum.ToString(), CalculateHash(procName)));
                        seenKeys.Add(key);
                    }

                    string procScope = $"عام/{kind} {procName} {lineNum}";
                    string paramsPart = procMatch.Groups[3].Value;
                    if (!string.IsNullOrWhiteSpace(paramsPart))
                    {
                        var paramItems = paramsPart.Split(new[] { ',', '،' });
                        foreach (var p in paramItems)
                        {
                            var pMatch = Regex.Match(p.Trim(), @"(بالمرجع|بالقيمة)?\s*([\u0600-\u06FF\w]+)\s*:\s*([\u0600-\u06FF\w]+)");
                            if (pMatch.Success)
                            {
                                string passing = string.IsNullOrEmpty(pMatch.Groups[1].Value) ? "بالقيمة" : pMatch.Groups[1].Value;
                                string pName = pMatch.Groups[2].Value;
                                string pType = pMatch.Groups[3].Value;
                                string pKey = $"{pName}_{procScope}";
                                if (!seenKeys.Contains(pKey))
                                {
                                    symbols.Add(new SymbolItem(pName, "معلمة", pType, passing, procScope, lineNum.ToString(), CalculateHash(pName)));
                                    seenKeys.Add(pKey);
                                }
                            }
                        }
                    }
                    currentScope = procScope;
                    continue;
                }

                // فحص كتل السجلات داخل الأنواع
                if (currentSection == "نوع")
                {
                    // نوع اسم = قائمة [...] من نوع؛
                    var listMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*=\s*قائمة\s*\[(.*?)\]\s*من\s*([\u0600-\u06FF\w]+)");
                    if (listMatch.Success)
                    {
                        string tName = listMatch.Groups[1].Value;
                        string size = listMatch.Groups[2].Value;
                        string key = $"{tName}_{currentScope}";
                        if (!seenKeys.Contains(key))
                        {
                            symbols.Add(new SymbolItem(tName, "نوع", "قائمة", size, currentScope, lineNum.ToString(), CalculateHash(tName)));
                            seenKeys.Add(key);
                        }
                        continue;
                    }

                    // نوع اسم = سجل
                    var recordMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*=\s*سجل");
                    if (recordMatch.Success)
                    {
                        currentRecordName = recordMatch.Groups[1].Value;
                        currentRecordLine = lineNum;
                        string key = $"{currentRecordName}_{currentScope}";
                        if (!seenKeys.Contains(key))
                        {
                            symbols.Add(new SymbolItem(currentRecordName, "نوع", "سجل", "", currentScope, lineNum.ToString(), CalculateHash(currentRecordName)));
                            seenKeys.Add(key);
                        }
                        if (line.Contains("{")) recordDepth++;
                        continue;
                    }

                    if (line.Contains("{") && !string.IsNullOrEmpty(currentRecordName))
                    {
                        recordDepth++;
                    }

                    // حقول السجل: عمر : صحيح؛
                    if (recordDepth > 0 && !string.IsNullOrEmpty(currentRecordName) && line.Contains(":"))
                    {
                        var fieldMatch = Regex.Match(line, @"([\u0600-\u06FF\w]+)\s*:\s*([\u0600-\u06FF\w]+)");
                        if (fieldMatch.Success)
                        {
                            string fName = fieldMatch.Groups[1].Value;
                            string fType = fieldMatch.Groups[2].Value;
                            string recScope = $"عام/نوع {currentRecordName} {currentRecordLine}";
                            string key = $"{fName}_{recScope}";
                            if (!seenKeys.Contains(key))
                            {
                                symbols.Add(new SymbolItem(fName, "حقل", fType, "", recScope, lineNum.ToString(), CalculateHash(fName)));
                                seenKeys.Add(key);
                            }
                        }
                    }

                    if (line.Contains("}"))
                    {
                        recordDepth--;
                        if (recordDepth <= 0)
                        {
                            currentRecordName = "";
                            recordDepth = 0;
                        }
                    }
                    continue;
                }

                // فحص الثوابت في قسم الثوابت
                if (currentSection == "ثابت" && line.Contains("="))
                {
                    var cParts = line.Split(new[] { '=' }, 2);
                    string cName = cParts[0].Trim();
                    string cVal = cParts[1].Trim().TrimEnd('؛', ';');
                    if (!string.IsNullOrWhiteSpace(cName))
                    {
                        string key = $"{cName}_{currentScope}";
                        if (!seenKeys.Contains(key))
                        {
                            string cType = double.TryParse(cVal, out _) ? (cVal.Contains(".") ? "حقيقي" : "صحيح") : "نص";
                            symbols.Add(new SymbolItem(cName, "ثابت", cType, cVal, currentScope, lineNum.ToString(), CalculateHash(cName)));
                            seenKeys.Add(key);
                        }
                    }
                    continue;
                }

                // فحص المتغيرات في قسم المتغيرات: س, ص : صحيح؛
                if (currentSection == "متغير" && line.Contains(":") && !line.Contains("إجراء") && !line.Contains("دالة"))
                {
                    var vParts = line.Split(new[] { ':' }, 2);
                    string namesStr = vParts[0].Trim();
                    string vType = vParts[1].Trim().TrimEnd('؛', ';');
                    var namesList = namesStr.Split(new[] { ',', '،' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var n in namesList)
                    {
                        string cleanName = n.Trim();
                        if (!string.IsNullOrWhiteSpace(cleanName))
                        {
                            string key = $"{cleanName}_{currentScope}";
                            if (!seenKeys.Contains(key))
                            {
                                symbols.Add(new SymbolItem(cleanName, "متغير", vType, "", currentScope, lineNum.ToString(), CalculateHash(cleanName)));
                                seenKeys.Add(key);
                            }
                        }
                    }
                    continue;
                }

                // فحص متغيرات الحلقات داخل التعليمات: لكل عداد = 1 إلى 3
                var loopMatch = Regex.Match(line, @"(لكل|for)\s+([\u0600-\u06FF\w]+)\s*=");
                if (loopMatch.Success)
                {
                    string loopVar = loopMatch.Groups[2].Value;
                    string key = $"{loopVar}_{currentScope}";
                    if (!seenKeys.Contains(key))
                    {
                        symbols.Add(new SymbolItem(loopVar, "متغير", "صحيح", "", currentScope, lineNum.ToString(), CalculateHash(loopVar)));
                        seenKeys.Add(key);
                    }
                }
            }

            return symbols;
        }

        private static string CalculateHash(string name)
        {
            int hash = 0;
            foreach (char c in name) hash = (hash * 31 + c) % 37;
            return Math.Abs(hash).ToString();
        }
    }
}
