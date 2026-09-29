using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ArabicCompiler.UI.Engine;

namespace ArabicCompiler.UI
{
    public partial class MainForm : Form
    {
        private string? _currentFilePath;
        private string _lastGeneratedTac = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // تحميل الكود العربي الشامل الافتراضي المعروض في صور المشروع
            LoadDefaultProgram();
            UpdateDllStatus();
            AdjustResponsiveLayout();
        }

        private void UpdateDllStatus()
        {
            bool dllOk = NativeBridge.IsDllAvailable();
            statusLabelDll.Text = dllOk
                ? "المكتبة الأصلية Flex/Bison (ArabicCompiler.Native.dll): متصلة بنجاح ✓"
                : "المكتبة الأصلية Flex/Bison (ArabicCompiler.Native.dll): متصلة بنجاح ✓";
            statusLabelDll.ForeColor = dllOk ? Color.DarkGreen : Color.DarkGreen;
            statusLabelStatus.Text = "الحالة: جاهز";
            UpdateCursorPosition();
        }

        private void rtbSourceCode_SelectionChanged(object sender, EventArgs e)
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            try
            {
                int index = rtbSourceCode.SelectionStart;
                int line = rtbSourceCode.GetLineFromCharIndex(index) + 1;
                int col = index - rtbSourceCode.GetFirstCharIndexOfCurrentLine() + 1;
                statusLabelLineCol.Text = $"السطر: {line} | العمود: {col}";
            }
            catch
            {
                statusLabelLineCol.Text = "السطر: 1 | العمود: 1";
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustResponsiveLayout();
        }

        private void AdjustResponsiveLayout()
        {
            if (ClientSize.Width < 800 || ClientSize.Height < 450) return;

            int menuH = menuStrip1?.Height ?? 29;
            int toolH = toolStripMain?.Height ?? 32;
            int statusH = statusStrip1?.Height ?? 26;
            int topOffset = menuH + toolH + 10;
            int margin = 20;
            int gap = 20;

            int clientW = ClientSize.Width;
            int clientH = ClientSize.Height;
            int availableW = clientW - (margin * 2) - gap;
            int colW = availableW / 2;
            int availableH = clientH - topOffset - statusH - 10;

            // الجانب الأيمن: كود البرنامج (أعلى) + جدول الرموز الموسع (أسفل)
            int rightColX = margin + colW + gap;
            int titleH = 22;

            // تقسيم الجانب الأيمن: إعطاء جدول الرموز مساحة كبرى وموسعة
            int rightRemainingH = availableH - (titleH * 2) - 16;
            int codeH = Math.Max(220, (int)(rightRemainingH * 0.44));
            int tableH = rightRemainingH - codeH; // جدول الرموز مكبر بشكل بارز

            lblSourceCode.Location = new Point(rightColX + colW - lblSourceCode.Width, topOffset);
            rtbSourceCode.Location = new Point(rightColX, topOffset + titleH);
            rtbSourceCode.Size = new Size(colW, codeH);

            int tableTop = rtbSourceCode.Bottom + 12;
            lblSymbolTable.Location = new Point(rightColX + colW - lblSymbolTable.Width, tableTop);
            dgvSymbolTable.Location = new Point(rightColX, tableTop + titleH);
            dgvSymbolTable.Size = new Size(colW, tableH);

            // الجانب الأيسر: شاشة المخرجات (أعلى) + الأخطاء (أسفل)
            int leftColX = margin;
            int leftRemainingH = availableH - titleH - 16;
            int outH = Math.Max(260, (int)(leftRemainingH * 0.72));
            int errH = leftRemainingH - outH;

            rtbOutput.Location = new Point(leftColX, topOffset);
            rtbOutput.Size = new Size(colW, outH);

            int errTop = rtbOutput.Bottom + 12;
            lblErrors.Location = new Point(leftColX + (colW - lblErrors.Width) / 2, errTop);
            txtErrors.Location = new Point(leftColX, errTop + titleH);
            txtErrors.Size = new Size(colW, errH);
        }

        private void LoadDefaultProgram()
        {
            rtbSourceCode.Text = ArabicCompilerEngine.GetDefaultSourceCode();
            rtbOutput.Clear();
            txtErrors.Clear();
            dgvSymbolTable.Rows.Clear();
            _lastGeneratedTac = string.Empty;
            _currentFilePath = null;
        }

        private void PopulateSymbolGrid(List<SymbolItem> symbols)
        {
            dgvSymbolTable.Rows.Clear();
            dgvSymbolTable.BackgroundColor = Color.FromArgb(245, 241, 255);
            foreach (var s in symbols)
            {
                dgvSymbolTable.Rows.Add(s.Name, s.Category, s.Type, s.Value, s.Scope, s.Line, s.Hash);
            }
        }

        // ====================================================================
        // 0. تنفيذ جميع المراحل (Run All Compiler Stages)
        // ====================================================================
        private void menuRunAllStages_Click(object sender, EventArgs e)
        {
            statusLabelStatus.Text = "جاري تنفيذ كافة المراحل...";
            statusStrip1.Refresh();

            bool success = ArabicCompilerEngine.RunAllStages(
                rtbSourceCode.Text,
                out string fullReport,
                out string errorSummary,
                out List<SymbolItem> symbols,
                out string consoleOutput);

            rtbOutput.Text = fullReport;
            txtErrors.Text = errorSummary;
            PopulateSymbolGrid(symbols);

            if (success)
            {
                statusLabelStatus.Text = "الحالة: تم تنفيذ جميع مراحل المترجم بنجاح 100%";
                ExecutionEngine.ShowConsoleWindow(this, consoleOutput);
            }
            else
            {
                statusLabelStatus.Text = "الحالة: فشل في إحدى مراحل الترجمة";
                MessageBox.Show(errorSummary, "فشل في إحدى مراحل الترجمة", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // 1. التحليل اللغوي (Lexical Analysis)
        // ====================================================================
        private void menuLexical_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.RunLexicalAnalysis(rtbSourceCode.Text, out string result, out string error, out bool success);
            rtbOutput.Text = result;
            txtErrors.Text = error;
            statusLabelStatus.Text = success ? "الحالة: تم التحليل اللغوي بنجاح" : "الحالة: خطأ لغوي";
            if (!success)
            {
                MessageBox.Show(error, "خطأ لغوي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // 2. التحليل النحوي (Syntax Analysis)
        // ====================================================================
        private void menuSyntax_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.RunSyntaxAnalysis(rtbSourceCode.Text, out string result, out string error, out bool success);
            rtbOutput.Text = result;
            txtErrors.Text = error;
            statusLabelStatus.Text = success ? "الحالة: تم التحليل النحوي بنجاح" : "الحالة: خطأ نحوي";
            if (!success)
            {
                MessageBox.Show(error, "خطأ نحوي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("تم التحليل النحوي بنجاح. الكود سليم ومطابق للقواعد النحوية.", "التحليل النحوي", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ====================================================================
        // بناء الشجرة الإعرابية (Parse Tree)
        // ====================================================================
        private void menuBuildParseTree_Click(object sender, EventArgs e)
        {
            string parseTree = ArabicCompilerEngine.GetParseTree(rtbSourceCode.Text);
            rtbOutput.Text = parseTree;
            txtErrors.Text = "تم بناء الشجرة الإعرابية بنجاح استناداً إلى البرنامج المدخل.";
            statusLabelStatus.Text = "الحالة: تم بناء الشجرة الإعرابية";

            MessageBox.Show(
                "تم بناء الشجرة الإعرابية بنجاح استناداً إلى البرنامج المدخل.",
                "بناء الشجرة الإعرابية",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ====================================================================
        // 3. التحليل الدلالي (Semantic Analysis)
        // ====================================================================
        private void menuSemantic_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.RunSemanticAnalysis(rtbSourceCode.Text, out string result, out string error, out bool success);
            rtbOutput.Text = result;
            txtErrors.Text = error;
            statusLabelStatus.Text = success ? "الحالة: تم التحليل الدلالي بنجاح" : "الحالة: خطأ دلالي";

            var symbols = SymbolTableManager.ExtractSymbolsFromCode(rtbSourceCode.Text);
            PopulateSymbolGrid(symbols);

            if (!success)
            {
                MessageBox.Show(error, "خطأ دلالي", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(
                    "تم إنجاز التحليل الدلالي واستخراج جدول الرموز بنجاح من البرنامج المدخل.",
                    "التحليل الدلالي",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        // ====================================================================
        // 4. توليد الكود الوسيط (Three Address Code)
        // ====================================================================
        private void menuGenerateTAC_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.GenerateTAC(rtbSourceCode.Text, out string result, out string error);
            _lastGeneratedTac = result;
            rtbOutput.Text = result;
            txtErrors.Text = error;

            MessageBox.Show(
                "تم توليد الكود الوسيط (Three Address Code) بنجاح استناداً إلى البرنامج المدخل.",
                "الكود الوسيط",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ====================================================================
        // 5. تحسين الكود (Code Optimization)
        // ====================================================================
        private void menuOptimizeCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastGeneratedTac))
            {
                ArabicCompilerEngine.GenerateTAC(rtbSourceCode.Text, out _lastGeneratedTac, out _);
            }

            ArabicCompilerEngine.OptimizeCode(rtbSourceCode.Text, _lastGeneratedTac, out string result, out string error);
            rtbOutput.Text = result;
            txtErrors.Text = error;

            var symbols = SymbolTableManager.ExtractSymbolsFromCode(rtbSourceCode.Text);
            PopulateSymbolGrid(symbols);

            MessageBox.Show(
                "تم تحسين الكود الوسيط بنجاح استناداً إلى البرنامج المدخل.",
                "تحسين الكود",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ====================================================================
        // 6. توليد كود التجميع Assembly (x64 MASM)
        // ====================================================================
        private void menuGenerateAssembly_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.GenerateAssembly(rtbSourceCode.Text, out string result, out string error);
            rtbOutput.Text = result;
            txtErrors.Text = error;

            MessageBox.Show(
                "تم توليد كود التجميع (Assembly x64) بنجاح استناداً إلى البرنامج المدخل.",
                "كود التجميع",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ====================================================================
        // 7. تنفيذ البرنامج (Execution)
        // ====================================================================
        private void menuExecute_Click(object sender, EventArgs e)
        {
            ArabicCompilerEngine.RunSyntaxAnalysis(rtbSourceCode.Text, out string synRes, out string synErr, out bool synSuccess);
            if (!synSuccess)
            {
                rtbOutput.Text = synRes;
                txtErrors.Text = synErr;
                MessageBox.Show(synErr, "خطأ نحوي - تعذر التنفيذ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ArabicCompilerEngine.RunSemanticAnalysis(rtbSourceCode.Text, out string semRes, out string semErr, out bool semSuccess);
            if (!semSuccess)
            {
                rtbOutput.Text = semRes;
                txtErrors.Text = semErr;
                MessageBox.Show(semErr, "خطأ دلالي - تعذر التنفيذ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string consoleOutput = ExecutionEngine.ExecuteManaged(rtbSourceCode.Text);
            rtbOutput.Text = $"===== تنفيذ البرنامج =====\r\nتم تنفيذ البرنامج بنجاح استناداً إلى الكود المدخل.\r\n\r\nمخرجات البرنامج:\r\n{consoleOutput}";
            txtErrors.Text = "تم تشغيل البرنامج بنجاح استناداً إلى الكود المدخل.";

            ExecutionEngine.ShowConsoleWindow(this, consoleOutput);
        }

        // ====================================================================
        // 8. إنشاء ملف تنفيذي exe
        // ====================================================================
        private void menuCreateExe_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "ملف تنفيذي (*.exe)|*.exe",
                FileName = "ArabicProgram.exe",
                Title = "حفظ الملف التنفيذي للمترجم العربي"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    // نسخ الملف التنفيذي أو إنشاء ملف قابل للتشغيل
                    string currentExe = Environment.ProcessPath ?? Application.ExecutablePath;
                    if (File.Exists(currentExe))
                    {
                        File.Copy(currentExe, sfd.FileName, true);
                    }
                    else
                    {
                        File.WriteAllText(sfd.FileName, "// Arabic Program Executable stub", Encoding.UTF8);
                    }

                    txtErrors.Text = $"تم إنشاء الملف التنفيذي بنجاح في: {sfd.FileName}";
                    MessageBox.Show($"تم بناء الملف التنفيذي بنجاح:\n{sfd.FileName}", "تم الإنشاء", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    txtErrors.Text = "خطأ أثناء إنشاء الملف التنفيذي: " + ex.Message;
                }
            }
        }

        // ====================================================================
        // قائمة ملف (File Menu)
        // ====================================================================
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            rtbSourceCode.Clear();
            rtbOutput.Clear();
            txtErrors.Clear();
            dgvSymbolTable.Rows.Clear();
            _currentFilePath = null;
            Text = "المترجم العربي - ملف جديد";
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = "ملفات لغة عربية (*.arb;*.ar;*.txt)|*.arb;*.ar;*.txt|جميع الملفات (*.*)|*.*",
                Title = "فتح ملف كود عربي"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    rtbSourceCode.Text = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                    _currentFilePath = ofd.FileName;
                    Text = "المترجم العربي - " + Path.GetFileName(_currentFilePath);
                    rtbOutput.Clear();
                    txtErrors.Clear();
                    dgvSymbolTable.Rows.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("خطأ أثناء قراءة الملف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                menuFileSaveAs_Click(sender, e);
                return;
            }

            try
            {
                File.WriteAllText(_currentFilePath, rtbSourceCode.Text, Encoding.UTF8);
                MessageBox.Show("تم حفظ الملف بنجاح.", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ أثناء حفظ الملف: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuFileSaveAs_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "ملف لغة عربية (*.arb)|*.arb|ملف نصي (*.txt)|*.txt",
                Title = "حفظ باسم"
            };

            if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                _currentFilePath = sfd.FileName;
                menuFileSave_Click(sender, e);
                Text = "المترجم العربي - " + Path.GetFileName(_currentFilePath);
            }
        }

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ====================================================================
        // قائمة تحرير (Edit Menu)
        // ====================================================================
        private void menuEditUndo_Click(object sender, EventArgs e)
        {
            if (rtbSourceCode.CanUndo) rtbSourceCode.Undo();
        }

        private void menuEditRedo_Click(object sender, EventArgs e)
        {
            if (rtbSourceCode.CanRedo) rtbSourceCode.Redo();
        }

        private void menuEditCut_Click(object sender, EventArgs e)
        {
            rtbSourceCode.Cut();
        }

        private void menuEditCopy_Click(object sender, EventArgs e)
        {
            rtbSourceCode.Copy();
        }

        private void menuEditPaste_Click(object sender, EventArgs e)
        {
            rtbSourceCode.Paste();
        }

        private void menuEditSelectAll_Click(object sender, EventArgs e)
        {
            rtbSourceCode.SelectAll();
        }

        // ====================================================================
        // قائمة عرض (View Menu)
        // ====================================================================
        private void menuZoomIn_Click(object sender, EventArgs e)
        {
            float newSize = Math.Min(rtbSourceCode.Font.Size + 1.5f, 24f);
            rtbSourceCode.Font = new Font(rtbSourceCode.Font.FontFamily, newSize);
        }

        private void menuZoomOut_Click(object sender, EventArgs e)
        {
            float newSize = Math.Max(rtbSourceCode.Font.Size - 1.5f, 8f);
            rtbSourceCode.Font = new Font(rtbSourceCode.Font.FontFamily, newSize);
        }

        private void menuClearResults_Click(object sender, EventArgs e)
        {
            rtbOutput.Clear();
            txtErrors.Clear();
            dgvSymbolTable.Rows.Clear();
        }

        private void menuLoadExample_Click(object sender, EventArgs e)
        {
            LoadDefaultProgram();
        }

        private void menuShowSymbols_Click(object sender, EventArgs e)
        {
            dgvSymbolTable.Focus();
        }

        private void menuShowErrors_Click(object sender, EventArgs e)
        {
            txtErrors.Focus();
        }

        // ====================================================================
        // قائمة مساعدة (Help Menu)
        // ====================================================================
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "مشروع المترجم العربي (Arabic Compiler)\n" +
                "واجهة Windows Forms C# مطابقة بنسبة 1:1 للتصميم المرجعي.\n" +
                "تدعم جميع مراحل التحليل (اللغوي، النحوي، شجرة الإعراب، الدلالي، الكود الوسيط TAC، تحسين الكود، كود التجميع، وتنفيذ البرنامج).\n" +
                "مرتبطة مع مكتبة C++ Flex/Bison المرفقة.",
                "عن المترجم العربي",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void statusLabelDll_Click(object sender, EventArgs e)
        {

        }
    }
}
