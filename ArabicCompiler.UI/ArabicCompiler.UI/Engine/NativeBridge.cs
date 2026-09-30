using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ArabicCompiler.UI.Engine
{
    internal static class NativeBridge
    {
        public const string DllName = "ArabicCompiler.Native.dll";
        private static bool _resolverConfigured = false;
        private static IntPtr _loadedHandle = IntPtr.Zero;
        private static readonly object _syncLock = new object();

        static NativeBridge()
        {
            EnsureResolver();
        }

        private static void EnsureResolver()
        {
            lock (_syncLock)
            {
                if (_resolverConfigured) return;
                _resolverConfigured = true;

                try
                {
                    NativeLibrary.SetDllImportResolver(typeof(NativeBridge).Assembly, CustomDllResolver);
                }
                catch { }
            }
        }

        private static IntPtr CustomDllResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == DllName || libraryName.EndsWith(DllName, StringComparison.OrdinalIgnoreCase))
            {
                if (_loadedHandle != IntPtr.Zero) return _loadedHandle;

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] possiblePaths = new[]
                {
                    Path.Combine(baseDir, "..", "..", "..", "..", "ArabicCompiler.Native", "x64", "Debug", DllName),
                    Path.Combine(baseDir, "..", "..", "..", "..", "ArabicCompiler.Native", "x64", "Release", DllName),
                    Path.Combine(baseDir, DllName),
                    Path.Combine(baseDir, "..", "..", "..", "..", DllName),
                    Path.Combine(Directory.GetCurrentDirectory(), "ArabicCompiler.Native", "x64", "Debug", DllName),
                    Path.Combine(Directory.GetCurrentDirectory(), DllName)
                };

                foreach (var p in possiblePaths)
                {
                    try
                    {
                        string fullPath = Path.GetFullPath(p);
                        if (File.Exists(fullPath))
                        {
                            if (NativeLibrary.TryLoad(fullPath, out IntPtr handle))
                            {
                                _loadedHandle = handle;
                                return handle;
                            }
                        }
                    }
                    catch { }
                }
            }

            return IntPtr.Zero;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TokenCallback(int token, IntPtr lexemePtr, int line);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Compiler_Parse([MarshalAs(UnmanagedType.LPUTF8Str)] string source);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Compiler_Lex([MarshalAs(UnmanagedType.LPUTF8Str)] string source, TokenCallback callback);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Compiler_GetTokenName(int token);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Compiler_GetSyntaxErrorLine();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Compiler_GetSyntaxErrorMessage();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Compiler_GetSyntaxErrorCorrection();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Compiler_ResetSyntaxError();

        public static bool IsDllAvailable()
        {
            try
            {
                EnsureResolver();
                if (_loadedHandle != IntPtr.Zero) return true;

                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string[] possiblePaths = new[]
                {
                    Path.Combine(baseDir, "..", "..", "..", "..", "ArabicCompiler.Native", "x64", "Debug", DllName),
                    Path.Combine(baseDir, DllName),
                    Path.Combine(baseDir, "..", "..", "..", "..", DllName),
                    Path.Combine(Directory.GetCurrentDirectory(), DllName)
                };

                foreach (var p in possiblePaths)
                {
                    try
                    {
                        string fullPath = Path.GetFullPath(p);
                        if (File.Exists(fullPath))
                        {
                            if (NativeLibrary.TryLoad(fullPath, out IntPtr handle))
                            {
                                _loadedHandle = handle;
                                return true;
                            }
                        }
                    }
                    catch { }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string PtrToString(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return string.Empty;
            string str = Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
            if (!string.IsNullOrEmpty(str) && !str.Contains("\uFFFD"))
            {
                return str;
            }
            return Marshal.PtrToStringAnsi(ptr) ?? string.Empty;
        }

        public static string GetTokenName(int token, string lexeme = "") => token switch
        {
            258 => "كلمة محجوزة: برنامج",
            259 => "كلمة محجوزة: ثابت",
            260 => "كلمة محجوزة: نوع",
            261 => "كلمة محجوزة: قائمة",
            262 => "كلمة محجوزة: من",
            263 => "كلمة محجوزة: سجل",
            264 => "كلمة محجوزة: متغير",
            265 => "كلمة محجوزة: إجراء",
            266 => "تمرير: بالقيمة",
            267 => "تمرير: بالمرجع",
            268 => "نوع بيانات: صحيح",
            269 => "نوع بيانات: حقيقي",
            270 => "نوع بيانات: منطقي",
            271 => "نوع بيانات: محرف",
            272 => "نوع بيانات: خيط رمزي",
            273 => "تعليمة: اقرأ",
            274 => "تعليمة: اطبع",
            275 => "شرط: إذا",
            276 => "شرط: فإن",
            277 => "شرط: وإلا",
            278 => "حلقة تكرار: لكل",
            279 => "حلقة: إلى",
            280 => "خطوة: بقدر",
            281 => "حلقة: طالما",
            282 => "كرر: افعل",
            283 => "حلقة: أعد",
            284 => "حلقة: حتى",
            285 => "قيمة منطقية: نعم",
            286 => "قيمة منطقية: لا",
            287 => "مقارنة: أصغر من (<)",
            288 => "مقارنة: أكبر من (>)",
            289 => "مقارنة: أصغر من أو يساوي (<=)",
            290 => "مقارنة: أكبر من أو يساوي (>=)",
            291 => "مقارنة: يساوي (==)",
            292 => "مقارنة: لا يساوي (!=)",
            293 => "عامل جمع (+)",
            294 => "عامل طرح (-)",
            295 => "عامل منطقي: أو",
            296 => "عامل أس (^)",
            297 => "عامل ضرب (*)",
            298 => "قسمة حقيقية (/)",
            299 => "قسمة صحيحة (\\)",
            300 => "باقي القسمة (%)",
            301 => "عامل منطقي: و",
            302 => "عامل منطقي: ليس",
            303 => "عامل إسناد (=)",
            304 => "نقطة النهاية (.)",
            305 => "نقطتان (:)",
            306 => "فاصلة منقوطة (؛)",
            307 => "فاصلة منقوطة وإلا",
            308 => "فاصلة (،)",
            309 => "قوس كتلة مفتوح ({)",
            310 => "قوس كتلة مغلق (})",
            311 => "قوس مصفوفة مفتوح ([)",
            312 => "قوس مصفوفة مغلق (])",
            313 => "قوس دائري مفتوح (()",
            314 => "قوس دائري مغلق ())",
            315 => "معرّف",
            316 => "عدد صحيح",
            317 => "عدد حقيقي",
            318 => "خيط رمزي",
            319 => "محرف",
            320 => "خطأ لغوي",
            _ => !string.IsNullOrEmpty(lexeme) ? $"رمز: {lexeme}" : "رمز خاص"
        };
    }
}
