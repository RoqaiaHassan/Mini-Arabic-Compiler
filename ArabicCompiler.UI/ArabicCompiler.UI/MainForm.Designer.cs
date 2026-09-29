namespace ArabicCompiler.UI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditRedo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAnalyze = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRunAllStages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLexical = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSyntax = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBuildParseTree = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSemantic = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuGenerateTAC = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptimizeCode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGenerateAssembly = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExecute = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCreateExe = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearResults = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadExample = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowSymbols = new System.Windows.Forms.ToolStripMenuItem();
            this.menuShowErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripBtnNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnRunAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnLexical = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnSyntax = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnParseTree = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnSemantic = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnTAC = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnOptimize = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnAssembly = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnExecute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripBtnDefaultCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripBtnClear = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelDll = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelLineCol = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSourceCode = new System.Windows.Forms.Label();
            this.rtbSourceCode = new System.Windows.Forms.RichTextBox();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.lblSymbolTable = new System.Windows.Forms.Label();
            this.dgvSymbolTable = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScope = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHash = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblErrors = new System.Windows.Forms.Label();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbolTable)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuAnalyze,
            this.menuView,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(1262, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.menuFileSaveAs,
            this.toolStripSeparator1,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(64, 29);
            this.menuFile.Text = "ملف";
            // 
            // menuFileNew
            // 
            this.menuFileNew.Name = "menuFileNew";
            this.menuFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuFileNew.Size = new System.Drawing.Size(220, 34);
            this.menuFileNew.Text = "جديد";
            this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuFileOpen.Size = new System.Drawing.Size(220, 34);
            this.menuFileOpen.Text = "فتح";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileSave
            // 
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuFileSave.Size = new System.Drawing.Size(220, 34);
            this.menuFileSave.Text = "حفظ";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // menuFileSaveAs
            // 
            this.menuFileSaveAs.Name = "menuFileSaveAs";
            this.menuFileSaveAs.Size = new System.Drawing.Size(220, 34);
            this.menuFileSaveAs.Text = "حفظ باسم";
            this.menuFileSaveAs.Click += new System.EventHandler(this.menuFileSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(217, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(220, 34);
            this.menuFileExit.Text = "خروج";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditUndo,
            this.menuEditRedo,
            this.toolStripSeparator2,
            this.menuEditCut,
            this.menuEditCopy,
            this.menuEditPaste,
            this.menuEditSelectAll});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(69, 29);
            this.menuEdit.Text = "تحرير";
            // 
            // menuEditUndo
            // 
            this.menuEditUndo.Name = "menuEditUndo";
            this.menuEditUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.menuEditUndo.Size = new System.Drawing.Size(263, 34);
            this.menuEditUndo.Text = "تراجع";
            this.menuEditUndo.Click += new System.EventHandler(this.menuEditUndo_Click);
            // 
            // menuEditRedo
            // 
            this.menuEditRedo.Name = "menuEditRedo";
            this.menuEditRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.menuEditRedo.Size = new System.Drawing.Size(263, 34);
            this.menuEditRedo.Text = "إعادة";
            this.menuEditRedo.Click += new System.EventHandler(this.menuEditRedo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(260, 6);
            // 
            // menuEditCut
            // 
            this.menuEditCut.Name = "menuEditCut";
            this.menuEditCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.menuEditCut.Size = new System.Drawing.Size(263, 34);
            this.menuEditCut.Text = "قص";
            this.menuEditCut.Click += new System.EventHandler(this.menuEditCut_Click);
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuEditCopy.Size = new System.Drawing.Size(263, 34);
            this.menuEditCopy.Text = "نسخ";
            this.menuEditCopy.Click += new System.EventHandler(this.menuEditCopy_Click);
            // 
            // menuEditPaste
            // 
            this.menuEditPaste.Name = "menuEditPaste";
            this.menuEditPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.menuEditPaste.Size = new System.Drawing.Size(263, 34);
            this.menuEditPaste.Text = "لصق";
            this.menuEditPaste.Click += new System.EventHandler(this.menuEditPaste_Click);
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuEditSelectAll.Size = new System.Drawing.Size(263, 34);
            this.menuEditSelectAll.Text = "تحديد الكل";
            this.menuEditSelectAll.Click += new System.EventHandler(this.menuEditSelectAll_Click);
            // 
            // menuAnalyze
            // 
            this.menuAnalyze.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRunAllStages,
            this.toolStripSeparator0,
            this.menuLexical,
            this.menuSyntax,
            this.menuSemantic,
            this.toolStripSeparator3,
            this.menuGenerateTAC,
            this.menuOptimizeCode,
            this.menuGenerateAssembly,
            this.toolStripSeparator4,
            this.menuExecute,
            this.menuCreateExe});
            this.menuAnalyze.Name = "menuAnalyze";
            this.menuAnalyze.Size = new System.Drawing.Size(70, 29);
            this.menuAnalyze.Text = "تحليل";
            // 
            // menuRunAllStages
            // 
            this.menuRunAllStages.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.menuRunAllStages.ForeColor = System.Drawing.Color.DarkBlue;
            this.menuRunAllStages.Name = "menuRunAllStages";
            this.menuRunAllStages.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuRunAllStages.Size = new System.Drawing.Size(332, 34);
            this.menuRunAllStages.Text = "تنفيذ جميع المراحل (F5)";
            this.menuRunAllStages.Click += new System.EventHandler(this.menuRunAllStages_Click);
            // 
            // toolStripSeparator0
            // 
            this.toolStripSeparator0.Name = "toolStripSeparator0";
            this.toolStripSeparator0.Size = new System.Drawing.Size(329, 6);
            // 
            // menuLexical
            // 
            this.menuLexical.Name = "menuLexical";
            this.menuLexical.Size = new System.Drawing.Size(332, 34);
            this.menuLexical.Text = "التحليل اللغوي";
            this.menuLexical.Click += new System.EventHandler(this.menuLexical_Click);
            // 
            // menuSyntax
            // 
            this.menuSyntax.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBuildParseTree});
            this.menuSyntax.Name = "menuSyntax";
            this.menuSyntax.Size = new System.Drawing.Size(332, 34);
            this.menuSyntax.Text = "التحليل النحوي";
            this.menuSyntax.Click += new System.EventHandler(this.menuSyntax_Click);
            // 
            // menuBuildParseTree
            // 
            this.menuBuildParseTree.Name = "menuBuildParseTree";
            this.menuBuildParseTree.Size = new System.Drawing.Size(263, 34);
            this.menuBuildParseTree.Text = "بناء الشجرة الإعرابية";
            this.menuBuildParseTree.Click += new System.EventHandler(this.menuBuildParseTree_Click);
            // 
            // menuSemantic
            // 
            this.menuSemantic.Name = "menuSemantic";
            this.menuSemantic.Size = new System.Drawing.Size(332, 34);
            this.menuSemantic.Text = "التحليل الدلالي";
            this.menuSemantic.Click += new System.EventHandler(this.menuSemantic_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(329, 6);
            // 
            // menuGenerateTAC
            // 
            this.menuGenerateTAC.Name = "menuGenerateTAC";
            this.menuGenerateTAC.Size = new System.Drawing.Size(332, 34);
            this.menuGenerateTAC.Text = "توليد الكود الوسيط";
            this.menuGenerateTAC.Click += new System.EventHandler(this.menuGenerateTAC_Click);
            // 
            // menuOptimizeCode
            // 
            this.menuOptimizeCode.Name = "menuOptimizeCode";
            this.menuOptimizeCode.Size = new System.Drawing.Size(332, 34);
            this.menuOptimizeCode.Text = "تحسين الكود";
            this.menuOptimizeCode.Click += new System.EventHandler(this.menuOptimizeCode_Click);
            // 
            // menuGenerateAssembly
            // 
            this.menuGenerateAssembly.Name = "menuGenerateAssembly";
            this.menuGenerateAssembly.Size = new System.Drawing.Size(332, 34);
            this.menuGenerateAssembly.Text = "توليد كود التجميع Assembly";
            this.menuGenerateAssembly.Click += new System.EventHandler(this.menuGenerateAssembly_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(329, 6);
            // 
            // menuExecute
            // 
            this.menuExecute.Name = "menuExecute";
            this.menuExecute.Size = new System.Drawing.Size(332, 34);
            this.menuExecute.Text = "تنفيذ البرنامج";
            this.menuExecute.Click += new System.EventHandler(this.menuExecute_Click);
            // 
            // menuCreateExe
            // 
            this.menuCreateExe.Name = "menuCreateExe";
            this.menuCreateExe.Size = new System.Drawing.Size(332, 34);
            this.menuCreateExe.Text = "إنشاء ملف تنفيذي exe";
            this.menuCreateExe.Click += new System.EventHandler(this.menuCreateExe_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuZoomIn,
            this.menuZoomOut,
            this.toolStripSeparator5,
            this.menuClearResults,
            this.menuLoadExample,
            this.menuShowSymbols,
            this.menuShowErrors});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(70, 29);
            this.menuView.Text = "عرض";
            // 
            // menuZoomIn
            // 
            this.menuZoomIn.Name = "menuZoomIn";
            this.menuZoomIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.menuZoomIn.Size = new System.Drawing.Size(354, 34);
            this.menuZoomIn.Text = "تكبير الخط";
            this.menuZoomIn.Click += new System.EventHandler(this.menuZoomIn_Click);
            // 
            // menuZoomOut
            // 
            this.menuZoomOut.Name = "menuZoomOut";
            this.menuZoomOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.menuZoomOut.Size = new System.Drawing.Size(354, 34);
            this.menuZoomOut.Text = "تصغير الخط";
            this.menuZoomOut.Click += new System.EventHandler(this.menuZoomOut_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(351, 6);
            // 
            // menuClearResults
            // 
            this.menuClearResults.Name = "menuClearResults";
            this.menuClearResults.Size = new System.Drawing.Size(354, 34);
            this.menuClearResults.Text = "مسح النتائج";
            this.menuClearResults.Click += new System.EventHandler(this.menuClearResults_Click);
            // 
            // menuLoadExample
            // 
            this.menuLoadExample.Name = "menuLoadExample";
            this.menuLoadExample.Size = new System.Drawing.Size(354, 34);
            this.menuLoadExample.Text = "تحميل مثال";
            this.menuLoadExample.Click += new System.EventHandler(this.menuLoadExample_Click);
            // 
            // menuShowSymbols
            // 
            this.menuShowSymbols.Name = "menuShowSymbols";
            this.menuShowSymbols.Size = new System.Drawing.Size(354, 34);
            this.menuShowSymbols.Text = "جدول الرموز";
            this.menuShowSymbols.Click += new System.EventHandler(this.menuShowSymbols_Click);
            // 
            // menuShowErrors
            // 
            this.menuShowErrors.Name = "menuShowErrors";
            this.menuShowErrors.Size = new System.Drawing.Size(354, 34);
            this.menuShowErrors.Text = "الأخطاء";
            this.menuShowErrors.Click += new System.EventHandler(this.menuShowErrors_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(91, 29);
            this.menuHelp.Text = "مساعده";
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(258, 34);
            this.menuHelpAbout.Text = "عن المترجم العربي";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(236)))), ((int)(((byte)(250)))));
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnNew,
            this.toolStripBtnOpen,
            this.toolStripBtnSave,
            this.toolStripSep1,
            this.toolStripBtnRunAll,
            this.toolStripSep2,
            this.toolStripBtnLexical,
            this.toolStripBtnSyntax,
            this.toolStripBtnParseTree,
            this.toolStripBtnSemantic,
            this.toolStripBtnTAC,
            this.toolStripBtnOptimize,
            this.toolStripBtnAssembly,
            this.toolStripBtnExecute,
            this.toolStripSep3,
            this.toolStripBtnDefaultCode,
            this.toolStripBtnClear});
            this.toolStripMain.Location = new System.Drawing.Point(0, 33);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.toolStripMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripMain.Size = new System.Drawing.Size(1262, 38);
            this.toolStripMain.TabIndex = 8;
            this.toolStripMain.Text = "toolStripMain";
            // 
            // toolStripBtnNew
            // 
            this.toolStripBtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnNew.Name = "toolStripBtnNew";
            this.toolStripBtnNew.Size = new System.Drawing.Size(75, 29);
            this.toolStripBtnNew.Text = "📄 جديد";
            this.toolStripBtnNew.ToolTipText = "برنامج جديد (Ctrl+N)";
            this.toolStripBtnNew.Click += new System.EventHandler(this.menuFileNew_Click);
            // 
            // toolStripBtnOpen
            // 
            this.toolStripBtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnOpen.Name = "toolStripBtnOpen";
            this.toolStripBtnOpen.Size = new System.Drawing.Size(69, 29);
            this.toolStripBtnOpen.Text = "📂 فتح";
            this.toolStripBtnOpen.ToolTipText = "فتح ملف كود عربي (Ctrl+O)";
            this.toolStripBtnOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // toolStripBtnSave
            // 
            this.toolStripBtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnSave.Name = "toolStripBtnSave";
            this.toolStripBtnSave.Size = new System.Drawing.Size(78, 29);
            this.toolStripBtnSave.Text = "💾 حفظ";
            this.toolStripBtnSave.ToolTipText = "حفظ الكود البرمجي (Ctrl+S)";
            this.toolStripBtnSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // toolStripSep1
            // 
            this.toolStripSep1.Name = "toolStripSep1";
            this.toolStripSep1.Size = new System.Drawing.Size(6, 34);
            // 
            // toolStripBtnRunAll
            // 
            this.toolStripBtnRunAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(240)))), ((int)(((byte)(215)))));
            this.toolStripBtnRunAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnRunAll.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.toolStripBtnRunAll.ForeColor = System.Drawing.Color.DarkGreen;
            this.toolStripBtnRunAll.Name = "toolStripBtnRunAll";
            this.toolStripBtnRunAll.Size = new System.Drawing.Size(178, 29);
            this.toolStripBtnRunAll.Text = "▶ تنفيذ كافة المراحل";
            this.toolStripBtnRunAll.ToolTipText = "تنفيذ جميع مراحل الترجمة وتنفيذ البرنامج";
            this.toolStripBtnRunAll.Click += new System.EventHandler(this.menuRunAllStages_Click);
            // 
            // toolStripSep2
            // 
            this.toolStripSep2.Name = "toolStripSep2";
            this.toolStripSep2.Size = new System.Drawing.Size(6, 34);
            // 
            // toolStripBtnLexical
            // 
            this.toolStripBtnLexical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnLexical.Name = "toolStripBtnLexical";
            this.toolStripBtnLexical.Size = new System.Drawing.Size(90, 29);
            this.toolStripBtnLexical.Text = "1. معجمي";
            this.toolStripBtnLexical.ToolTipText = "التحليل اللغوي / المعجمي (Flex DLL)";
            this.toolStripBtnLexical.Click += new System.EventHandler(this.menuLexical_Click);
            // 
            // toolStripBtnSyntax
            // 
            this.toolStripBtnSyntax.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnSyntax.Name = "toolStripBtnSyntax";
            this.toolStripBtnSyntax.Size = new System.Drawing.Size(76, 29);
            this.toolStripBtnSyntax.Text = "2. نحوي";
            this.toolStripBtnSyntax.ToolTipText = "التحليل النحوي (Bison DLL)";
            this.toolStripBtnSyntax.Click += new System.EventHandler(this.menuSyntax_Click);
            // 
            // toolStripBtnParseTree
            // 
            this.toolStripBtnParseTree.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnParseTree.Name = "toolStripBtnParseTree";
            this.toolStripBtnParseTree.Size = new System.Drawing.Size(139, 29);
            this.toolStripBtnParseTree.Text = "3. شجرة الإعراب";
            this.toolStripBtnParseTree.ToolTipText = "بناء شجرة الإعراب النحوية";
            this.toolStripBtnParseTree.Click += new System.EventHandler(this.menuBuildParseTree_Click);
            // 
            // toolStripBtnSemantic
            // 
            this.toolStripBtnSemantic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnSemantic.Name = "toolStripBtnSemantic";
            this.toolStripBtnSemantic.Size = new System.Drawing.Size(76, 29);
            this.toolStripBtnSemantic.Text = "4. دلالي";
            this.toolStripBtnSemantic.ToolTipText = "التحليل الدلالي وتوافق الأنواع";
            this.toolStripBtnSemantic.Click += new System.EventHandler(this.menuSemantic_Click);
            // 
            // toolStripBtnTAC
            // 
            this.toolStripBtnTAC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnTAC.Name = "toolStripBtnTAC";
            this.toolStripBtnTAC.Size = new System.Drawing.Size(118, 29);
            this.toolStripBtnTAC.Text = "5. كود وسيط";
            this.toolStripBtnTAC.ToolTipText = "توليد الكود الوسيط TAC";
            this.toolStripBtnTAC.Click += new System.EventHandler(this.menuGenerateTAC_Click);
            // 
            // toolStripBtnOptimize
            // 
            this.toolStripBtnOptimize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnOptimize.Name = "toolStripBtnOptimize";
            this.toolStripBtnOptimize.Size = new System.Drawing.Size(133, 29);
            this.toolStripBtnOptimize.Text = "6. تحسين الكود";
            this.toolStripBtnOptimize.ToolTipText = "تحسين الكود الوسيط بطي الثوابت";
            this.toolStripBtnOptimize.Click += new System.EventHandler(this.menuOptimizeCode_Click);
            // 
            // toolStripBtnAssembly
            // 
            this.toolStripBtnAssembly.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnAssembly.Name = "toolStripBtnAssembly";
            this.toolStripBtnAssembly.Size = new System.Drawing.Size(124, 29);
            this.toolStripBtnAssembly.Text = "7. لغة التجميع";
            this.toolStripBtnAssembly.ToolTipText = "توليد كود التجميع MASM x64";
            this.toolStripBtnAssembly.Click += new System.EventHandler(this.menuGenerateAssembly_Click);
            // 
            // toolStripBtnExecute
            // 
            this.toolStripBtnExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(205)))));
            this.toolStripBtnExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnExecute.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.toolStripBtnExecute.ForeColor = System.Drawing.Color.DarkOrange;
            this.toolStripBtnExecute.Name = "toolStripBtnExecute";
            this.toolStripBtnExecute.Size = new System.Drawing.Size(141, 29);
            this.toolStripBtnExecute.Text = "⚡ تنفيذ البرنامج";
            this.toolStripBtnExecute.ToolTipText = "تشغيل البرنامج ومحاكاته وعرض مخرجات الكونسول";
            this.toolStripBtnExecute.Click += new System.EventHandler(this.menuExecute_Click);
            // 
            // toolStripSep3
            // 
            this.toolStripSep3.Name = "toolStripSep3";
            this.toolStripSep3.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripBtnDefaultCode
            // 
            this.toolStripBtnDefaultCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnDefaultCode.Name = "toolStripBtnDefaultCode";
            this.toolStripBtnDefaultCode.Size = new System.Drawing.Size(135, 29);
            this.toolStripBtnDefaultCode.Text = "📋 كود افتراضي";
            this.toolStripBtnDefaultCode.ToolTipText = "تحميل كود الاختبار الشامل";
            this.toolStripBtnDefaultCode.Click += new System.EventHandler(this.menuLoadExample_Click);
            // 
            // toolStripBtnClear
            // 
            this.toolStripBtnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripBtnClear.Name = "toolStripBtnClear";
            this.toolStripBtnClear.Size = new System.Drawing.Size(80, 29);
            this.toolStripBtnClear.Text = "🧹 مسح";
            this.toolStripBtnClear.ToolTipText = "مسح المخرجات والأخطاء وجدول الرموز";
            this.toolStripBtnClear.Click += new System.EventHandler(this.menuClearResults_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(242)))), ((int)(((byte)(254)))));
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabelStatus,
            this.statusLabelDll,
            this.statusLabelLineCol});
            this.statusStrip1.Location = new System.Drawing.Point(0, 671);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(1262, 32);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabelStatus
            // 
            this.statusLabelStatus.Name = "statusLabelStatus";
            this.statusLabelStatus.Size = new System.Drawing.Size(99, 25);
            this.statusLabelStatus.Text = "الحالة: جاهز";
            // 
            // statusLabelDll
            // 
            this.statusLabelDll.Name = "statusLabelDll";
            this.statusLabelDll.Size = new System.Drawing.Size(948, 25);
            this.statusLabelDll.Spring = true;
            this.statusLabelDll.Text = "المكتبة الأصلية Flex/Bison: متصلة بنجاح ✓";
            this.statusLabelDll.Click += new System.EventHandler(this.statusLabelDll_Click);
            // 
            // statusLabelLineCol
            // 
            this.statusLabelLineCol.Name = "statusLabelLineCol";
            this.statusLabelLineCol.Size = new System.Drawing.Size(154, 25);
            this.statusLabelLineCol.Text = "السطر: 1 | العمود: 1";
            // 
            // lblSourceCode
            // 
            this.lblSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSourceCode.AutoSize = true;
            this.lblSourceCode.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSourceCode.ForeColor = System.Drawing.Color.Black;
            this.lblSourceCode.Location = new System.Drawing.Point(1170, 38);
            this.lblSourceCode.Name = "lblSourceCode";
            this.lblSourceCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSourceCode.Size = new System.Drawing.Size(96, 22);
            this.lblSourceCode.TabIndex = 1;
            this.lblSourceCode.Text = "كود البرنامج";
            // 
            // rtbSourceCode
            // 
            this.rtbSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSourceCode.BackColor = System.Drawing.Color.White;
            this.rtbSourceCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbSourceCode.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbSourceCode.Location = new System.Drawing.Point(705, 62);
            this.rtbSourceCode.Name = "rtbSourceCode";
            this.rtbSourceCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rtbSourceCode.Size = new System.Drawing.Size(546, 342);
            this.rtbSourceCode.TabIndex = 2;
            this.rtbSourceCode.Text = "";
            this.rtbSourceCode.WordWrap = false;
            this.rtbSourceCode.SelectionChanged += new System.EventHandler(this.rtbSourceCode_SelectionChanged);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbOutput.BackColor = System.Drawing.Color.White;
            this.rtbOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbOutput.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbOutput.Location = new System.Drawing.Point(208, 62);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.ReadOnly = true;
            this.rtbOutput.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rtbOutput.Size = new System.Drawing.Size(482, 452);
            this.rtbOutput.TabIndex = 3;
            this.rtbOutput.Text = "";
            this.rtbOutput.WordWrap = false;
            // 
            // lblSymbolTable
            // 
            this.lblSymbolTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSymbolTable.AutoSize = true;
            this.lblSymbolTable.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSymbolTable.ForeColor = System.Drawing.Color.Black;
            this.lblSymbolTable.Location = new System.Drawing.Point(1164, 417);
            this.lblSymbolTable.Name = "lblSymbolTable";
            this.lblSymbolTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSymbolTable.Size = new System.Drawing.Size(99, 22);
            this.lblSymbolTable.TabIndex = 4;
            this.lblSymbolTable.Text = "جدول الرموز";
            // 
            // dgvSymbolTable
            // 
            this.dgvSymbolTable.AllowUserToAddRows = false;
            this.dgvSymbolTable.AllowUserToDeleteRows = false;
            this.dgvSymbolTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSymbolTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSymbolTable.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(241)))), ((int)(((byte)(255)))));
            this.dgvSymbolTable.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(232)))), ((int)(((byte)(252)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(90)))), ((int)(((byte)(195)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSymbolTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSymbolTable.ColumnHeadersHeight = 34;
            this.dgvSymbolTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colCategory,
            this.colType,
            this.colValue,
            this.colScope,
            this.colLine,
            this.colHash});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(90)))), ((int)(((byte)(195)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSymbolTable.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSymbolTable.EnableHeadersVisualStyles = false;
            this.dgvSymbolTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(215)))), ((int)(((byte)(235)))));
            this.dgvSymbolTable.Location = new System.Drawing.Point(705, 410);
            this.dgvSymbolTable.Name = "dgvSymbolTable";
            this.dgvSymbolTable.ReadOnly = true;
            this.dgvSymbolTable.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvSymbolTable.RowHeadersVisible = false;
            this.dgvSymbolTable.RowHeadersWidth = 51;
            this.dgvSymbolTable.RowTemplate.Height = 30;
            this.dgvSymbolTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSymbolTable.Size = new System.Drawing.Size(546, 275);
            this.dgvSymbolTable.TabIndex = 5;
            // 
            // colName
            // 
            this.colName.HeaderText = "الاسم";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.HeaderText = "فئة الرمز";
            this.colCategory.MinimumWidth = 6;
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "النوع";
            this.colType.MinimumWidth = 6;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colValue
            // 
            this.colValue.HeaderText = "القيمة";
            this.colValue.MinimumWidth = 6;
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // colScope
            // 
            this.colScope.HeaderText = "النطاق";
            this.colScope.MinimumWidth = 6;
            this.colScope.Name = "colScope";
            this.colScope.ReadOnly = true;
            // 
            // colLine
            // 
            this.colLine.HeaderText = "السطر";
            this.colLine.MinimumWidth = 6;
            this.colLine.Name = "colLine";
            this.colLine.ReadOnly = true;
            // 
            // colHash
            // 
            this.colHash.HeaderText = "رقم الهاش";
            this.colHash.MinimumWidth = 6;
            this.colHash.Name = "colHash";
            this.colHash.ReadOnly = true;
            // 
            // lblErrors
            // 
            this.lblErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblErrors.AutoSize = true;
            this.lblErrors.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblErrors.ForeColor = System.Drawing.Color.Black;
            this.lblErrors.Location = new System.Drawing.Point(420, 520);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblErrors.Size = new System.Drawing.Size(62, 22);
            this.lblErrors.TabIndex = 6;
            this.lblErrors.Text = "الأخطاء";
            // 
            // txtErrors
            // 
            this.txtErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtErrors.BackColor = System.Drawing.Color.White;
            this.txtErrors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErrors.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtErrors.Location = new System.Drawing.Point(260, 542);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrors.Size = new System.Drawing.Size(370, 143);
            this.txtErrors.TabIndex = 7;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(230)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1262, 703);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtErrors);
            this.Controls.Add(this.lblErrors);
            this.Controls.Add(this.dgvSymbolTable);
            this.Controls.Add(this.lblSymbolTable);
            this.Controls.Add(this.rtbOutput);
            this.Controls.Add(this.rtbSourceCode);
            this.Controls.Add(this.lblSourceCode);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(950, 620);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "المترجم العربي";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbolTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripMenuItem menuFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuEditUndo;
        private System.Windows.Forms.ToolStripMenuItem menuEditRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuEditCut;
        private System.Windows.Forms.ToolStripMenuItem menuEditCopy;
        private System.Windows.Forms.ToolStripMenuItem menuEditPaste;
        private System.Windows.Forms.ToolStripMenuItem menuEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuAnalyze;
        private System.Windows.Forms.ToolStripMenuItem menuRunAllStages;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator0;
        private System.Windows.Forms.ToolStripMenuItem menuLexical;
        private System.Windows.Forms.ToolStripMenuItem menuSyntax;
        private System.Windows.Forms.ToolStripMenuItem menuBuildParseTree;
        private System.Windows.Forms.ToolStripMenuItem menuSemantic;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuGenerateTAC;
        private System.Windows.Forms.ToolStripMenuItem menuOptimizeCode;
        private System.Windows.Forms.ToolStripMenuItem menuGenerateAssembly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuExecute;
        private System.Windows.Forms.ToolStripMenuItem menuCreateExe;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuZoomIn;
        private System.Windows.Forms.ToolStripMenuItem menuZoomOut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuClearResults;
        private System.Windows.Forms.ToolStripMenuItem menuLoadExample;
        private System.Windows.Forms.ToolStripMenuItem menuShowSymbols;
        private System.Windows.Forms.ToolStripMenuItem menuShowErrors;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.Label lblSourceCode;
        private System.Windows.Forms.RichTextBox rtbSourceCode;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.Label lblSymbolTable;
        private System.Windows.Forms.DataGridView dgvSymbolTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScope;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHash;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripBtnNew;
        private System.Windows.Forms.ToolStripButton toolStripBtnOpen;
        private System.Windows.Forms.ToolStripButton toolStripBtnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSep1;
        private System.Windows.Forms.ToolStripButton toolStripBtnRunAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSep2;
        private System.Windows.Forms.ToolStripButton toolStripBtnLexical;
        private System.Windows.Forms.ToolStripButton toolStripBtnSyntax;
        private System.Windows.Forms.ToolStripButton toolStripBtnParseTree;
        private System.Windows.Forms.ToolStripButton toolStripBtnSemantic;
        private System.Windows.Forms.ToolStripButton toolStripBtnTAC;
        private System.Windows.Forms.ToolStripButton toolStripBtnOptimize;
        private System.Windows.Forms.ToolStripButton toolStripBtnAssembly;
        private System.Windows.Forms.ToolStripButton toolStripBtnExecute;
        private System.Windows.Forms.ToolStripSeparator toolStripSep3;
        private System.Windows.Forms.ToolStripButton toolStripBtnDefaultCode;
        private System.Windows.Forms.ToolStripButton toolStripBtnClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelStatus;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelDll;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelLineCol;
    }
}
